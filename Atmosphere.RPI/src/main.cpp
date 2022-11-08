#include "pico/cyw43_arch.h"
#include "pico/stdlib.h"

#include "FreeRTOS.h"
#include "semphr.h"
#include "task.h"

#include "temp_reader.h"
#include "mqtt.h"

constexpr int TEMP_PORT = 28;
constexpr int RETRY_COUNT = 500;
static xSemaphoreHandle connectedMutex;

#define ms(x) x / portTICK_RATE_MS

void background_task(__unused void *params) {
  TempReader reader(TEMP_PORT);
  while (true) {
    for (auto &res : reader.pollResults()) {
      printf("Dev address: %s\n", res.deviceAddress.c_str());
      printf("Value: %f\n", res.value);
    }
    vTaskDelay(ms(1000));
  }
}

void connect_task(__unused void *params) {
  if (cyw43_arch_init()) {
    printf("Failed to init.\n");
    return;
  }

  cyw43_arch_enable_sta_mode();

  int triesRemaining = RETRY_COUNT;
  int status = 0;
  while (triesRemaining) {
    printf("Connecting to WIFI. Try %i out of %i\n",
           (RETRY_COUNT - triesRemaining) + 1, RETRY_COUNT);
    status = cyw43_arch_wifi_connect_blocking(WIFI_SSID, WIFI_PASSWORD,
                                              CYW43_AUTH_WPA2_AES_PSK);
    if (!status) {
      xSemaphoreGive(connectedMutex);
      break;
    }

    printf("Failed to connect. Try %i out of %i\n",
           (RETRY_COUNT - triesRemaining) + 1, RETRY_COUNT);
    --triesRemaining;
  }

  while (true) {
    vTaskDelay(100);
  }

  cyw43_arch_deinit();
}

void listener_task(__unused void *params) {
  xSemaphoreTake(connectedMutex, portMAX_DELAY);
  MQTT("192.168.0.50", "Pico-W-1");

  while (true) {
    vTaskDelay(100);
  }
}

void vLaunch() {
  connectedMutex = xSemaphoreCreateMutex();
  xSemaphoreTake(connectedMutex, portMAX_DELAY);
  TaskHandle_t task;
  xTaskCreate(connect_task, "connect_task", configMINIMAL_STACK_SIZE, NULL,
              tskIDLE_PRIORITY + 1, &task);

  // TaskHandle_t bck_task;
  // xTaskCreate(background_task, "background_task", configMINIMAL_STACK_SIZE,
  //             NULL, tskIDLE_PRIORITY + 2, &bck_task);

  TaskHandle_t listener;
  xTaskCreate(listener_task, "listener_task", configMINIMAL_STACK_SIZE, NULL,
              tskIDLE_PRIORITY + 3, &listener);

  vTaskStartScheduler();
}

int main() {
  stdio_init_all();

  vLaunch();
  while (true)
    ;

  return 0;
}