

#include "CreateReading.h"

using namespace Tiny;

CreateReading::CreateReading() {
  value = float(0);
  type = ReadingType();
  timestamp = std::string();
  unit = std::string();
}

CreateReading::CreateReading(std::string jsonString) {
  this->fromJson(jsonString);
}

CreateReading::~CreateReading() {}

void CreateReading::fromJson(std::string jsonObj) {
  bourne::json object = bourne::json::parse(jsonObj);

  const char *valueKey = "value";

  if (object.has_key(valueKey)) {
    bourne::json value = object[valueKey];

    jsonToValue(&value, value, "double");
  }

  const char *typeKey = "type";

  if (object.has_key(typeKey)) {
    bourne::json value = object[typeKey];

    auto typeString = value.is_string() ? value.to_string() : "";
    type = ReadingTypeFromString(typeString);
  }

  const char *timestampKey = "timestamp";

  if (object.has_key(timestampKey)) {
    bourne::json value = object[timestampKey];

    jsonToValue(&timestamp, value, "std::string");
  }

  const char *unitKey = "unit";

  if (object.has_key(unitKey)) {
    bourne::json value = object[unitKey];

    jsonToValue(&unit, value, "std::string");
  }
}

bourne::json CreateReading::toJson() {
  bourne::json object = bourne::json::object();

  object["value"] = getValue();

  object["type"] = Tiny::ReadingTypeToString(getType());

  object["timestamp"] = getTimestamp();

  object["unit"] = getUnit();

  return object;
}

double CreateReading::getValue() { return value; }

void CreateReading::setValue(double value) { this->value = value; }

ReadingType CreateReading::getType() { return type; }

void CreateReading::setType(ReadingType type) { this->type = type; }

std::string CreateReading::getTimestamp() { return timestamp; }

void CreateReading::setTimestamp(std::string timestamp) {
  this->timestamp = timestamp;
}

std::string CreateReading::getUnit() { return unit; }

void CreateReading::setUnit(std::string unit) { this->unit = unit; }
