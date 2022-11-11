#include "tasks/sensor_task.h"

#include "ticks.hpp"

void SensorTask::Run() {
  bool item;
  _connectionMsgQueue->Dequeue((void *)&item);
  Delay(cpp_freertos::Ticks::MsToTicks(200));

  try {
    _mqtt = new MQTT("192.168.0.50", "Pico-W-1");
    _mqtt->subscribe("topic/test");
  } catch (std::exception &e) {
    printf("MQTT fail: %s", e.what());
  }

  while (true) {
    auto results = tempReader.pollResults();
    while (!results.empty()) {
      auto result = results.front();
      results.pop_front();
      std::string json = result.toJson();
      _mqtt->publish("topic/test", json.c_str());
    }
  }
}
