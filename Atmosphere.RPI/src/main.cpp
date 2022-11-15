#include "ReadingApi.h"
#include <Arduino.h>
#include <DallasTemperature.h>
#include <OneWire.h>
#include <WiFi.h>

#include <iomanip>
#include <sstream>

OneWire onewire(28);
DallasTemperature sensors(&onewire);
Tiny::ReadingApi api;

void setup() {
  Serial.begin();

  Serial.println("Begin setup");
  while (WiFi.begin("Xiaomi_23E2", "HK8FJC59KKV73") != WL_CONNECTED) {
    Serial.printf("Connecting... Status code: %d\r\n", WiFi.status());
    delay(500);
  }
  Serial.println("Wifi connected.");

  NTP.begin("0.pl.pool.ntp.org", "1.pl.pool.ntp.org");
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

  Serial.println("Setup sensors");
  sensors.begin();

  Serial.println("End setup");
}

void loop() {
  auto begin = millis();
  sensors.requestTemperatures();
  uint8_t deviceCount = sensors.getDeviceCount();
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
      body.setDeviceAddress(stream.str());
      body.setValue(temp);
      body.setType(Tiny::ReadingType::Temperature);
      Serial.printf("Body: %s\r\n", body.toJson().dump().c_str());

      api.addHeader(
          "Authorization",
          "Bearer "
          "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9."
          "eyJ1c2VySWQiOiJjMDk0NmNiMS01YzQ5LTQ0MzktYTdiYi0yMzI0M2I3Yzc3YmEiLCJo"
          "dHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWlt"
          "cy9uYW1lIjoidGVzdCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAw"
          "OC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkRldmljZSIsImV4cCI6MTY2ODUzMDA4"
          "OCwiaXNzIjoiQXRtb3NwaGVyZSIsImF1ZCI6IkF0bW9zcGhlcmUifQ."
          "g494znAEw13mQ-uGT1am_rhKiIOyCnjTUUiR07qQwk8");
      auto response = api.apiReadingCreateReadingPost(body);
      Serial.printf("Response code: %d\r\n", response.code);
    }
  }

  auto end = millis();
  if (end - begin < 1000) {
    delay(1000 - (end - begin));
  }
}