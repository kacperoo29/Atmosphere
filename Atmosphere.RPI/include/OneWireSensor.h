#ifndef _ONEWIRE_SENSOR_H
#define _ONEWIRE_SENSOR_H

#include "Sensor.h"

#include <DallasTemperature.h>
#include <OneWire.h>

class OneWireSensor : public Sensor {
public:
    OneWireSensor(uint8_t pin);
    ~OneWireSensor() override = default;
    void setup() override;
    std::vector<Reading> read() override;

private:
    OneWire oneWire;
    DallasTemperature sensors;
};

#endif