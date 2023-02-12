

#include "DeviceDto.h"

using namespace Tiny;

DeviceDto::DeviceDto() {
  id = std::string();
  identifier = std::string();
  isActive = bool(false);
  isConnected = bool(false);
}

DeviceDto::DeviceDto(std::string jsonString) { this->fromJson(jsonString); }

DeviceDto::~DeviceDto() {}

void DeviceDto::fromJson(std::string jsonObj) {
  bourne::json object = bourne::json::parse(jsonObj);

  const char *idKey = "id";

  if (object.has_key(idKey)) {
    bourne::json value = object[idKey];

    jsonToValue(&id, value, "std::string");
  }

  const char *identifierKey = "identifier";

  if (object.has_key(identifierKey)) {
    bourne::json value = object[identifierKey];

    jsonToValue(&identifier, value, "std::string");
  }

  const char *isActiveKey = "isActive";

  if (object.has_key(isActiveKey)) {
    bourne::json value = object[isActiveKey];

    jsonToValue(&isActive, value, "bool");
  }

  const char *isConnectedKey = "isConnected";

  if (object.has_key(isConnectedKey)) {
    bourne::json value = object[isConnectedKey];

    jsonToValue(&isConnected, value, "bool");
  }
}

bourne::json DeviceDto::toJson() {
  bourne::json object = bourne::json::object();

  object["id"] = getId();

  object["identifier"] = getIdentifier();

  object["isActive"] = isIsActive();

  object["isConnected"] = isIsConnected();

  return object;
}

std::string DeviceDto::getId() { return id; }

void DeviceDto::setId(std::string id) { this->id = id; }

std::string DeviceDto::getIdentifier() { return identifier; }

void DeviceDto::setIdentifier(std::string identifier) {
  this->identifier = identifier;
}

bool DeviceDto::isIsActive() { return isActive; }

void DeviceDto::setIsActive(bool isActive) { this->isActive = isActive; }

bool DeviceDto::isIsConnected() { return isConnected; }

void DeviceDto::setIsConnected(bool isConnected) {
  this->isConnected = isConnected;
}
