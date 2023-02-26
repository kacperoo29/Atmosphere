#include "DHTSensor.h"

DHTSensor::DHTSensor(uint8_t pin, uint8_t type) : dht(pin, type) {}

void DHTSensor::setup() {
    dht.begin();
}

std::vector<Reading> DHTSensor::read() {
    std::vector<Reading> readings;
    auto h = dht.readHumidity();
    auto t = dht.readTemperature();
    if (isnan(h) || isnan(t)) {
        return readings;
    }
    readings.push_back(Reading{h, "%", "humidity", "dht11", ""});
    readings.push_back(Reading{t, "C", "temperature", "dht11", ""});
    return readings;
}
