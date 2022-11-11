#ifndef _SENSOR_TASK_H
#define _SENSOR_TASK_H

#include "queue.hpp"
#include "thread.hpp"

#include "mqtt.h"
#include "temp_reader.h"

class SensorTask : public cpp_freertos::Thread {
public:
  SensorTask(const std::string& name, cpp_freertos::Queue *connectionMsgQueue, uint port)
      : Thread(name, configMINIMAL_STACK_SIZE, tskIDLE_PRIORITY + 1),
        _connectionMsgQueue(connectionMsgQueue), tempReader(port), _mqtt(nullptr) {}

protected:
  virtual void Run();

private:
  cpp_freertos::Queue *_connectionMsgQueue;
  TempReader tempReader;
  MQTT *_mqtt;
};

#endif