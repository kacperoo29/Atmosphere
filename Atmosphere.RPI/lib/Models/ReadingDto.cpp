

#include "ReadingDto.h"

using namespace Tiny;

ReadingDto::ReadingDto() {
  id = std::string();
  device = DeviceDto();
  value = float(0);
  unit = std::string();
  timestamp = std::string();
  type = ReadingType();
}

ReadingDto::ReadingDto(std::string jsonString) { this->fromJson(jsonString); }

ReadingDto::~ReadingDto() {}

void ReadingDto::fromJson(std::string jsonObj) {
  bourne::json object = bourne::json::parse(jsonObj);

  const char *idKey = "id";

  if (object.has_key(idKey)) {
    bourne::json value = object[idKey];

    jsonToValue(&id, value, "std::string");
  }

  const char *deviceKey = "device";

  if (object.has_key(deviceKey)) {
    bourne::json value = object[deviceKey];

    DeviceDto *obj = &device;
    obj->fromJson(value.dump());
  }

  const char *valueKey = "value";

  if (object.has_key(valueKey)) {
    bourne::json value = object[valueKey];

    jsonToValue(&value, value, "double");
  }

  const char *unitKey = "unit";

  if (object.has_key(unitKey)) {
    bourne::json value = object[unitKey];

    jsonToValue(&unit, value, "std::string");
  }

  const char *timestampKey = "timestamp";

  if (object.has_key(timestampKey)) {
    bourne::json value = object[timestampKey];

    jsonToValue(&timestamp, value, "std::string");
  }

  const char *typeKey = "type";

  if (object.has_key(typeKey)) {
    bourne::json value = object[typeKey];

    auto typeString = value.is_string() ? value.to_string() : "";
    type = ReadingTypeFromString(typeString);
  }
}

bourne::json ReadingDto::toJson() {
  bourne::json object = bourne::json::object();

  object["id"] = getId();

  object["device"] = getDevice().toJson();

  object["value"] = getValue();

  object["unit"] = getUnit();

  object["timestamp"] = getTimestamp();

  object["type"] = Tiny::ReadingTypeToString(getType());

  return object;
}

std::string ReadingDto::getId() { return id; }

void ReadingDto::setId(std::string id) { this->id = id; }

DeviceDto ReadingDto::getDevice() { return device; }

void ReadingDto::setDevice(DeviceDto device) { this->device = device; }

double ReadingDto::getValue() { return value; }

void ReadingDto::setValue(double value) { this->value = value; }

std::string ReadingDto::getUnit() { return unit; }

void ReadingDto::setUnit(std::string unit) { this->unit = unit; }

std::string ReadingDto::getTimestamp() { return timestamp; }

void ReadingDto::setTimestamp(std::string timestamp) {
  this->timestamp = timestamp;
}

ReadingType ReadingDto::getType() { return type; }

void ReadingDto::setType(ReadingType type) { this->type = type; }
