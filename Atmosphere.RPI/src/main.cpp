#include <Arduino.h>
#include <DallasTemperature.h>
#include <OneWire.h>
#include <WebSockets.h>
#include <WebSocketsClient.h>
#include <WiFi.h>

#include <iomanip>
#include <sstream>

#include "AuthApi.h"
#include "ReadingApi.h"

#include "config.h"

OneWire onewire(28);
DallasTemperature sensors(&onewire);
Tiny::ReadingApi readingApi;
Tiny::AuthApi authApi;
WebSocketsClient webSocket;
std::string token;
unsigned int send_interval = 1000;

void onEvent(WStype_t type, uint8_t *payload, size_t length) {
  switch (type) {}
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
  webSocket.begin(API_HOST, HOST_PORT, wsPath.c_str());
  webSocket.sendPing();
  webSocket.onEvent(onEvent);
}

void setup() {
  Serial.begin();
  Serial.println("Begin setup");
  init_wifi();
  init_ntp();
  init_server_connection();

  Serial.println("Setup sensors");
  sensors.begin();

  Serial.println("End setup");
}

void loop() {
  auto begin = millis();
  sensors.requestTemperatures();
  uint8_t deviceCount = sensors.getDeviceCount();
  webSocket.loop();
  for (int i = 0; i < deviceCount; ++i) {
    DeviceAddress deviceAddress;
    if (sensors.getAddress(deviceAddress, i)) {
      float temp = sensors.getTempC(deviceAddress);

      Tiny::CreateReading body;
      std::stringstream stream;
      for (int i = 0; i < 8; ++i) {
        stream << std::setfill('0') << std::setw(2) << std::hex
               << (uint)deviceAddress[i];
      }

      auto now = time(nullptr);
      struct tm timeinfo;
      gmtime_r(&now, &timeinfo);
      char buf[sizeof("2011-10-08T07:07:09Z")];
      strftime(buf, sizeof buf, "%FT%TZ", &timeinfo);
      body.setTimestamp(buf);
      body.setValue(temp);
      body.setUnit("C");
      body.setType(Tiny::ReadingType::Temperature);

      auto auth = "Bearer " + token;
      readingApi.addHeader("Authorization", auth);
      auto response = readingApi.apiReadingCreateReadingPost(body);
    }
  }

  auto end = millis();
  if (end - begin < send_interval) {
    delay(send_interval - (end - begin));
  }
}