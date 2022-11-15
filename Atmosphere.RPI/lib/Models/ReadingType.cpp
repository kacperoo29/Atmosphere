

#include "ReadingType.h"

using namespace Tiny;

std::string Tiny::ReadingTypeToString(ReadingType value) {
  switch (value) {
    case ReadingType::Unknown:
      return "unknown";
    case ReadingType::Temperature:
      return "temperature";
    case ReadingType::Humidity:
      return "humidity";
    case ReadingType::Pressure:
      return "pressure";
    default:
      return "Unknown";
  }
}

ReadingType Tiny::ReadingTypeFromString(std::string value) {
  if (value == "unknown") {
    return ReadingType::Unknown;
  }
  if (value == "temperature") {
    return ReadingType::Temperature;
  }
  if (value == "humidity") {
    return ReadingType::Humidity;
  }
  if (value == "pressure") {
    return ReadingType::Pressure;
  }
  return ReadingType::Unknown;
}
