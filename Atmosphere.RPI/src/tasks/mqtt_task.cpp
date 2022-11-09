#include "tasks/mqtt_task.h"

#include "pico/stdlib.h"

#include "ticks.hpp"

void MQTTTask::Run() {
  bool item;
  _connectionMsqQueue->Dequeue((void *)&item);

  try {
    _mqtt = new MQTT("192.168.0.50", "Pico-W-1");
    _mqtt->subscribe("topic/test");
  } catch (std::exception &e) {
    printf("MQTT fail: %s", e.what());
  }

  while (true) {
    //_mqtt->publish("topic/test", "testmsg");
    Delay(cpp_freertos::Ticks::MsToTicks(200));
  }
}