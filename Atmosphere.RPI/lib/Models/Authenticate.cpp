#include "Authenticate.h"
#include "Arduino.h"

using namespace Tiny;

Authenticate::Authenticate() {
  username = std::string();
  password = std::string();
}

Authenticate::Authenticate(std::string jsonString) {
  this->fromJson(jsonString);
}

Authenticate::~Authenticate() {}

void Authenticate::fromJson(std::string jsonObj) {
  bourne::json object = bourne::json::parse(jsonObj);

  const char *usernameKey = "username";

  if (object.has_key(usernameKey)) {
    bourne::json value = object[usernameKey];

    jsonToValue(&username, value, "std::string");
  }

  const char *passwordKey = "password";

  if (object.has_key(passwordKey)) {
    bourne::json value = object[passwordKey];

    jsonToValue(&password, value, "std::string");
  }
}

bourne::json Authenticate::toJson() {
  bourne::json object = bourne::json::object();

  object["username"] = getUsername();
  object["password"] = getPassword();

  return object;
}

std::string Authenticate::getUsername() { return username; }

void Authenticate::setUsername(std::string username) {
  this->username = username;
}

std::string Authenticate::getPassword() { return password; }

void Authenticate::setPassword(std::string password) {
  this->password = password;
}
