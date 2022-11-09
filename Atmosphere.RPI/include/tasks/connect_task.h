#ifndef _CONNECT_TASK_H
#define _CONNECT_TASK_H

#include "queue.hpp"
#include "thread.hpp"

class ConnectTask : public cpp_freertos::Thread {
public:
  ConnectTask(const std::string &name, cpp_freertos::Queue *msgQueue)
      : Thread(name, configMINIMAL_STACK_SIZE, tskIDLE_PRIORITY + 1),
        _connectionMsqQueue(msgQueue) {}

protected:
  virtual void Run();

private:
  cpp_freertos::Queue *_connectionMsqQueue;
};

#endif