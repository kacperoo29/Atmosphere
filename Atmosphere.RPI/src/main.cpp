#include "pico/cyw43_arch.h"
#include "pico/stdlib.h"

#include "FreeRTOS.h"
// #include "semphr.h"
// #include "task.h"

// #include "mqtt.h"
// #include "temp_reader.h"

#include "thread.hpp"
// #include "ticks.hpp"
// #include "condition_variable.hpp"
// #include "mutex.hpp"
#include "queue.hpp"

#include "tasks/connect_task.h"
#include "tasks/mqtt_task.h"

// constexpr int TEMP_PORT = 28;
//  static xSemaphoreHandle connectedMutex;
//  TaskHandle_t task;
//  TaskHandle_t listener;

// #define ms(x) x / portTICK_RATE_MS

// class ConnectThread : public cpp_freertos::Thread {
// public:
//   ConnectThread()
//       : Thread(CONNECT_THREAD, configMINIMAL_STACK_SIZE, tskIDLE_PRIORITY +
//       1) {
//   }

// protected:
//   virtual void Run() {
//     if (cyw43_arch_init()) {
//       printf("Failed to init.\n");
//       return;
//     }

//     cyw43_arch_enable_sta_mode();

//     int triesRemaining = RETRY_COUNT;
//     int status = 0;
//     while (triesRemaining) {
//       printf("Connecting to WIFI. Try %i out of %i\n",
//              (RETRY_COUNT - triesRemaining) + 1, RETRY_COUNT);
//       status = cyw43_arch_wifi_connect_blocking(WIFI_SSID, WIFI_PASSWORD,
//                                                 CYW43_AUTH_WPA2_AES_PSK);
//       if (!status) {
//         printf("Giving\n");
//         // xSemaphoreGive(connectedMutex);
//         //xTaskNotifyGive(listener);
//         printf("Given\n");
//         break;
//       }
//       printf("Failed to connect. Try %i out of %i\n",
//              (RETRY_COUNT - triesRemaining) + 1, RETRY_COUNT);
//       --triesRemaining;
//     }

//     //printf("out\n");
//     //printf("listener: %i", (int)listener);
//     //TaskStatus_t details;
//     //vTaskGetInfo(listener, &details, pdTRUE, eInvalid);
//     //printf("State: %i", details.eCurrentState);

//     while (true) {
//       Delay(cpp_freertos::Ticks::MsToTicks(200));
//       //vTaskDelay(ms(100));
//     }

//     cyw43_arch_deinit();
//   }
// };

// void background_task(__unused void *params) {
//   TempReader reader(TEMP_PORT);
//   while (true) {
//     for (auto &res : reader.pollResults()) {
//       printf("Dev address: %s\n", res.deviceAddress.c_str());
//       printf("Value: %f\n", res.value);
//     }
//     vTaskDelay(ms(1000));
//   }
// }

// void connect_task(__unused void *params) {
//   if (cyw43_arch_init()) {
//     printf("Failed to init.\n");
//     return;
//   }

//   cyw43_arch_enable_sta_mode();

//   int triesRemaining = RETRY_COUNT;
//   int status = 0;
//   while (triesRemaining) {
//     printf("Connecting to WIFI. Try %i out of %i\n",
//            (RETRY_COUNT - triesRemaining) + 1, RETRY_COUNT);
//     status = cyw43_arch_wifi_connect_blocking(WIFI_SSID, WIFI_PASSWORD,
//                                               CYW43_AUTH_WPA2_AES_PSK);
//     if (!status) {
//       printf("Giving\n");
//       // xSemaphoreGive(connectedMutex);
//       xTaskNotifyGive(listener);
//       printf("Given\n");
//       break;
//     }

//     printf("Failed to connect. Try %i out of %i\n",
//            (RETRY_COUNT - triesRemaining) + 1, RETRY_COUNT);
//     --triesRemaining;
//   }

//   printf("out\n");
//   printf("listener: %i", (int)listener);
//   TaskStatus_t details;
//   vTaskGetInfo(listener, &details, pdTRUE, eInvalid);
//   printf("State: %i", details.eCurrentState);

//   while (true) {
//     vTaskDelay(ms(100));
//   }

//   cyw43_arch_deinit();
// }

// void inner_listener(__unused void *params) {

//   // try {
//   //   printf("Try\n");
//   //   MQTT mqtt("192.168.0.50", "Pico-W-1");
//   //   printf("after con\n");
//   //   mqtt.subscribe("topic/test");
//   //   mqtt.publish("topic/test", "test msg");
//   // } catch (std::exception &exception) {
//   //   printf("MQTT thread exception: %s\n", exception.what());
//   // }
// }

// void listener_task(__unused void *params) {
//   printf("Listener\n");
//   // xSemaphoreTake(connectedMutex, portMAX_DELAY);
//   ulTaskNotifyTake(pdTRUE, portMAX_DELAY);
//   printf("Take\n");
//   MQTT mqtt("192.168.0.50", "Pico-W-1");

//   while (true) {
//     vTaskDelay(ms(100));
//   }
// }

// void vLaunch() {
//   // connectedMutex = xSemaphoreCreateMutex();
//   // xSemaphoreTake(connectedMutex, portMAX_DELAY);
//   xTaskCreate(connect_task, "connect_task", configMINIMAL_STACK_SIZE, NULL,
//               tskIDLE_PRIORITY + 1, &task);

//   // TaskHandle_t bck_task;
//   // xTaskCreate(background_task, "background_task",
//   configMINIMAL_STACK_SIZE,
//   //             NULL, tskIDLE_PRIORITY + 2, &bck_task);
//   xTaskCreate(listener_task, "listener_task", configMINIMAL_STACK_SIZE, NULL,
//               tskIDLE_PRIORITY + 1, &listener);

//   vTaskStartScheduler();
// }

int main() {
  stdio_init_all();
  sleep_ms(500);

  {
    using namespace cpp_freertos;
    Queue *connectionMsgQueue = new Queue(1, sizeof(bool));
    ConnectTask connectTask("connectTask", connectionMsgQueue);
    connectTask.Start();

    MQTTTask mqttTask("mqttTask", connectionMsgQueue);
    mqttTask.Start();

    Thread::StartScheduler();
  }

  printf("Scheduler exited.");
  
  return 0;
}