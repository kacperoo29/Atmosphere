#ifndef _DHT11_SENSOR_H
#define _DHT11_SENSOR_H

#include "Sensor.h"

#include <DHT.h>

class DHTSensor : public Sensor {
public:
    DHTSensor(uint8_t pin, uint8_t type);
    ~DHTSensor() override = default;
    void setup() override;
    std::vector<Reading> read() override;

private:
    DHT dht;
};

#endif
