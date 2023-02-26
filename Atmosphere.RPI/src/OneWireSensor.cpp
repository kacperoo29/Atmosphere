#include "OneWireSensor.h"

#include <iomanip>
#include <sstream>

OneWireSensor::OneWireSensor(uint8_t pin) : oneWire(pin), sensors(&oneWire) {}

void OneWireSensor::setup() { sensors.begin(); }

std::vector<Reading> OneWireSensor::read() {
  std::vector<Reading> readings;
  sensors.requestTemperatures();
  uint8_t deviceCount = sensors.getDeviceCount();
  float value = 0;
  for (uint8_t i = 0; i < deviceCount; i++) {
    DeviceAddress deviceAddress;
    if (sensors.getAddress(deviceAddress, i)) {
      std::stringstream stream;
      for (int i = 0; i < 8; ++i) {
        stream << std::setfill('0') << std::setw(2) << std::hex
               << (uint)deviceAddress[i];
      }
      float temp = sensors.getTempC(deviceAddress);
      value += temp;

      auto now = time(nullptr);
      struct tm timeinfo;
      gmtime_r(&now, &timeinfo);
      char buf[sizeof("2011-10-08T07:07:09Z")];
      strftime(buf, sizeof(buf), "%FT%TZ", &timeinfo);
      readings.push_back(Reading{temp, "C", "temperature", stream.str(), buf});
    }
  }

  return readings;
}
