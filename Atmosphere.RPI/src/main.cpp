#include "config.h"

#include <Arduino.h>
#include <DallasTemperature.h>
#include <OneWire.h>
#include <WebSockets.h>
#include <WebSocketsClient.h>
#include <WiFi.h>

#include <iomanip>
#include <sstream>

#include <bourne/json.hpp>

#include "CreateReading.h"
#include "AuthApi.h"
#include "ReadingApi.h"
#include "OneWireSensor.h"

std::list<Tiny::CreateReading> convert(const std::vector<Reading> &reading) {
  std::list<Tiny::CreateReading> readings;
  for (auto &r : reading) {
    Tiny::CreateReading reading;
    reading.setTimestamp(r.timestamp);
    reading.setValue(r.value);
    reading.setUnit(r.unit);
    reading.setType(Tiny::ReadingTypeFromString(r.type));

    readings.push_back(reading);
  }
  return readings;
}

Tiny::ReadingApi readingApi;
Tiny::AuthApi authApi;
WebSocketsClient webSocket;
std::string token;
unsigned int send_interval = 1000;
constexpr unsigned int ping_interval = 20000;
unsigned int last_ping = 0;

std::vector<Sensor *> sensors;

void onEvent(WStype_t type, uint8_t *payload, size_t length) {
  switch (type) {
  case WStype_TEXT:
    try {
      auto json = bourne::json::parse(std::string((char *)payload, length));
      if (json.has_key("type")) {
        auto type = json["type"].to_string();
        if (type == "pollingRate") {
          auto rate = json["payload"].to_int();
          send_interval = rate;
        }
      }
    } catch (const std::exception &e) {
      Serial.printf("Error parsing json: %s\r\n", e.what());
    }
    break;
  }
}

void init_wifi() {
  while (WiFi.begin(WIFI_SSID, WIFI_PASSWD) != WL_CONNECTED) {
    Serial.printf("Connecting... Status code: %d\r\n", WiFi.status());
    delay(500);
  }
  Serial.println("Wifi connected.");
  Serial.printf("IP: %s\r\n", WiFi.localIP().toString().c_str());
}

void init_ntp() {
  NTP.begin(NTP_SERVER0, NTP_SERVER1);
  Serial.print("Waiting for NTP time sync: ");
  time_t now = time(nullptr);
  while (now < 8 * 3600 * 2) {
    delay(500);
    Serial.print(".");
    now = time(nullptr);
  }
  Serial.println("");

  struct tm timeinfo;
  gmtime_r(&now, &timeinfo);
  Serial.print("Current time: ");
  Serial.print(asctime(&timeinfo));
}

void init_server_connection() {
  Serial.println("Setup api");
  const char *mac = WiFi.macAddress().c_str();
  Tiny::Authenticate authPayload;
  authPayload.setUsername(mac);
  authPayload.setPassword(PASSWD);
  auto response = authApi.apiAuthAuthenticatePost(authPayload);
  if (response.code != 200) {
    Serial.println("Device not registered. Registering...");
    Tiny::RegisterDevice registerDevicePayload;
    registerDevicePayload.setIdentifier(authPayload.getUsername());
    registerDevicePayload.setUsername(authPayload.getUsername());
    registerDevicePayload.setPassword(PASSWD);
    auto response = authApi.apiAuthRegisterDevicePost(registerDevicePayload);
    if (response.code != 200) {
      Serial.printf("Error: %d\r\n", response.code);
      return;
    }

    auto response2 = authApi.apiAuthAuthenticatePost(authPayload);
    if (response2.code != 200) {
      Serial.printf("Error: %d\r\n", response2.code);
      return;
    }

    token = response2.obj.getToken();
  } else {
    token = response.obj.getToken();
  }

  Serial.println("Setup websocket");
  std::string wsPath = BASE_WS_PATH + token;
  webSocket.begin(API_HOST, HOST_PORT, wsPath.c_str(), "");
  webSocket.onEvent(onEvent);
  webSocket.setReconnectInterval(5000);
}

void setup() {
  Serial.begin();
  Serial.println("Begin setup");
  init_wifi();
  init_ntp();
  init_server_connection();

  Serial.println("Setup sensors");
  sensors.push_back(new OneWireSensor(28));
  for (auto sensor : sensors) {
    sensor->setup();
  }
  Serial.println("End setup");
}

static auto end = 0;
static auto diff = 0;
void loop() {
  while (!webSocket.isConnected()) {
    webSocket.loop();
  }

  auto begin = millis();
  webSocket.loop();

  if (last_ping > ping_interval) {
    if (webSocket.isConnected())
      webSocket.sendTXT("ping");
    last_ping = 0;
  }

  if (diff >= send_interval) {
    diff = 0;
    std::vector<Reading> readings;
    for (auto sensor : sensors) {
      auto sensorReadings = sensor->read();
      readings.insert(readings.end(), sensorReadings.begin(),
                      sensorReadings.end());
    }

    auto body = convert(readings);
    auto auth = "Bearer " + token;
    readingApi.addHeader("Authorization", auth);
    auto response = readingApi.apiReadingCreateReadingsPost(body);
  }

  end = millis();
  last_ping += end - begin;
  diff += end - begin;
}