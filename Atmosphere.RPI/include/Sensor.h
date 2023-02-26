#ifndef _SENSOR_H
#define _SENSOR_H

#include <vector>

#include "Reading.h"

class Sensor {
public:
    Sensor() = default;
    virtual ~Sensor() = default;
    virtual void setup() = 0;
    virtual std::vector<Reading> read() = 0;
};

#endif