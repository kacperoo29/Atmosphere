#include "mqtt.h"

#include <cstring>

static void mqtt_incoming_data_cb(void *arg, const u8_t *data, u16_t len,
                                  u8_t flags) {
  const struct mqtt_connect_client_info_t *client_info =
      (const struct mqtt_connect_client_info_t *)arg;
  LWIP_UNUSED_ARG(data);

  LWIP_PLATFORM_DIAG(("MQTT client \"%s\" data cb: len %d, flags %d\n",
                      client_info->client_id, (int)len, (int)flags));
}

static void mqtt_incoming_publish_cb(void *arg, const char *topic,
                                     u32_t tot_len) {
  const struct mqtt_connect_client_info_t *client_info =
      (const struct mqtt_connect_client_info_t *)arg;

  LWIP_PLATFORM_DIAG(("MQTT client \"%s\" publish cb: topic %s, len %d\n",
                      client_info->client_id, topic, (int)tot_len));
}

static void mqtt_request_cb(void *arg, err_t err) {
  const struct mqtt_connect_client_info_t *client_info =
      (const struct mqtt_connect_client_info_t *)arg;

  LWIP_PLATFORM_DIAG(("MQTT client \"%s\" request cb: err %d\n",
                      client_info->client_id, (int)err));
}

static void mqtt_connection_cb(mqtt_client_t *client, void *arg,
                               mqtt_connection_status_t status) {
  const struct mqtt_connect_client_info_t *client_info =
      (const struct mqtt_connect_client_info_t *)arg;
  LWIP_UNUSED_ARG(client);

  LWIP_PLATFORM_DIAG(("MQTT client \"%s\" connection cb: status %d\n",
                      client_info->client_id, (int)status));

  if (status == MQTT_CONNECT_ACCEPTED) {
    mqtt_sub_unsub(client, "topic_qos1", 1, mqtt_request_cb,
                   LWIP_CONST_CAST(void *, client_info), 1);
    mqtt_sub_unsub(client, "topic_qos0", 0, mqtt_request_cb,
                   LWIP_CONST_CAST(void *, client_info), 1);
  }
}

MQTT::MQTT(const char *addr, const char *client_id)
    : _client(mqtt_client_new()) {
  *_client_info = {
    client_id,
    NULL, /* user */
    NULL, /* pass */
    100,  /* keep alive */
    NULL, /* will_topic */
    NULL, /* will_msg */
    0,    /* will_qos */
    0     /* will_retain */
#if LWIP_ALTCP && LWIP_ALTCP_TLS
    ,
    NULL
#endif
  };

  ip_addr_t mqtt_ip;
  ip4_addr_set_u32(&mqtt_ip, ipaddr_addr(addr));

  mqtt_set_inpub_callback(_client, mqtt_incoming_publish_cb,
                          mqtt_incoming_data_cb,
                          LWIP_CONST_CAST(void *, _client_info));

  mqtt_client_connect(_client, &mqtt_ip, MQTT_PORT, mqtt_connection_cb,
                      LWIP_CONST_CAST(void *, _client_info), _client_info);
}

void MQTT::subscribe(const char *topic) {
  mqtt_sub_unsub(_client, topic, 0, mqtt_request_cb,
                 LWIP_CONST_CAST(void *, _client_info), 1);
}

void MQTT::publish(const char *topic, const char *msg) {
  mqtt_publish(_client, topic, msg, std::strlen(msg), 0, 0, mqtt_request_cb,
               LWIP_CONST_CAST(void *, _client_info));
}
