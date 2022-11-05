#include "lwip/apps/http_client.h"
#include "pico/cyw43_arch.h"
#include "pico/stdlib.h"

#include "temp_reader.h"

constexpr int TEMP_PORT = 28;
constexpr int RETRY_COUNT = 5;

static int connect_wifi() {
  if (cyw43_arch_init()) {
    printf("Failed to init.\n");
    return 1;
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
      break;
    }

    printf("Failed to connect. Try %i out of %i\n",
           (RETRY_COUNT - triesRemaining) + 1, RETRY_COUNT);
    --triesRemaining;
  }

  return status;
}

int main() {
  stdio_init_all();
  if (connect_wifi()) {
    printf("Failed to connect.\n");
    return 1;
  }

  TempReader reader(TEMP_PORT);
  while (true) {
    for (auto &res : reader.pollResults()) {
      printf("Dev address: %s\n", res.deviceAddress.c_str());
      printf("Value: %f\n", res.value);
    }
    sleep_ms(1000);
  }

  return 0;
}