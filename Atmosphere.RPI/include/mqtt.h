#ifndef _MQTT_H
#define _MQTT_H

#include "lwip/apps/mqtt.h"

class MQTT {
    public:
      MQTT(const char* addr, const char* client_id);
      err_t subscribe(const char* topic);
      err_t publish(const char* topic, const char* msg);

    private:
      mqtt_client_t *_client;
      mqtt_connect_client_info_t _clientInfo;
};


#endif