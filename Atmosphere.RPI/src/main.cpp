#include "pico/cyw43_arch.h"
#include "pico/stdlib.h"

#include "FreeRTOS.h"
#include "thread.hpp"
#include "queue.hpp"

#include "tasks/connect_task.h"
// #include "tasks/mqtt_task.h"
#include "tasks/sensor_task.h"


int main() {
  stdio_init_all();
  sleep_ms(500);

  {
    using namespace cpp_freertos;
    Queue *connectionMsgQueue = new Queue(1, sizeof(bool));
    ConnectTask connectTask("connectTask", connectionMsgQueue);
    connectTask.Start();

    SensorTask sensorTask("sensorTask", connectionMsgQueue, 28);
    sensorTask.Start();

    Thread::StartScheduler();

    delete connectionMsgQueue;
  }

  printf("Scheduler exited.");
  
  return 0;
}