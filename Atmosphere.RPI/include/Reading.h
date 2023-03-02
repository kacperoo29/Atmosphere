#ifndef _READING_H
#define _READING_H

#include <string>
#include <bourne/json.hpp>

struct Reading {
  double value;
  std::string unit;
  std::string type;
  std::string deviceAddress;
  std::string timestamp;

  bourne::json toJson() const {
    bourne::json json;
    json["value"] = value;
    json["unit"] = unit;
    json["type"] = type;
    json["deviceAddress"] = deviceAddress;
    json["timestamp"] = timestamp;
    
    return json;
  }
};

#endif