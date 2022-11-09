#ifndef _MQTT_TASK_H
#define _MQTT_TASK_H

#include "thread.hpp"
#include "queue.hpp"

#include "mqtt.h"

class MQTTTask : public cpp_freertos::Thread {
public:
  MQTTTask(const std::string &name,
          cpp_freertos::Queue *connectionMsgQueue)
      : Thread(name, configMINIMAL_STACK_SIZE, tskIDLE_PRIORITY + 1),
        _connectionMsqQueue(connectionMsgQueue), _mqtt(nullptr) {}

protected:
  virtual void Run();

private:
  cpp_freertos::Queue *_connectionMsqQueue;
  cpp_freertos::Queue *_readingMsqQueue;
  MQTT *_mqtt;
};

#endif