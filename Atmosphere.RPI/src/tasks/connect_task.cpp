#include "tasks/connect_task.h"

#include "pico/cyw43_arch.h"
#include "pico/stdlib.h"

#include "queue.hpp"
#include "ticks.hpp"

constexpr int RETRY_COUNT = 500;

void ConnectTask::Run() {
  if (cyw43_arch_init()) {
    printf("Failed to init.\n");
    return;
  }

  cyw43_arch_enable_sta_mode();

  int triesRemaining = RETRY_COUNT;
  int status = 0;
  bool msg = false;
  while (triesRemaining) {
    printf("Connecting to WIFI. Try %i out of %i\n",
           (RETRY_COUNT - triesRemaining) + 1, RETRY_COUNT);
    status = cyw43_arch_wifi_connect_blocking(WIFI_SSID, WIFI_PASSWORD,
                                              CYW43_AUTH_WPA2_AES_PSK);
    if (!status) {
      msg = true;
      _connectionMsqQueue->Enqueue((void *)&msg);
      break;
    }

    printf("Failed to connect. Try %i out of %i\n",
           (RETRY_COUNT - triesRemaining) + 1, RETRY_COUNT);
    --triesRemaining;
  }

  if (status) {
    msg = false;
    _connectionMsqQueue->Enqueue((void *)&msg);
  }

  while (true) {
    Delay(cpp_freertos::Ticks::MsToTicks(200));
  }

  cyw43_arch_deinit();
}