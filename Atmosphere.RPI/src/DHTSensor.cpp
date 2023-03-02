#include "DHTSensor.h"

#include <iomanip>
#include <sstream>

DHTSensor::DHTSensor(uint8_t pin, uint8_t type) : dht(pin, type, 1) {}

void DHTSensor::setup() { dht.begin(); }

std::vector<Reading> DHTSensor::read() {
  auto now = time(nullptr);
  struct tm timeinfo;
  gmtime_r(&now, &timeinfo);
  char buf[sizeof("2011-10-08T07:07:09Z")];
  strftime(buf, sizeof(buf), "%FT%TZ", &timeinfo);
  std::vector<Reading> readings;
  auto h = dht.readHumidity();
  auto t = dht.readTemperature();
  if (!isnan(h)) {
    readings.push_back(Reading{h, "%", "humidity", "", buf});
  }

  if (!isnan(t)) {
    readings.push_back(Reading{t, "C", "temperature", "", buf});
  }

  return readings;
}
