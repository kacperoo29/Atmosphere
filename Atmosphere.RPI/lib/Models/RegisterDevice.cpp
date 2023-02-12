

#include "RegisterDevice.h"

using namespace Tiny;

RegisterDevice::RegisterDevice()
{
	username = std::string();
	identifier = std::string();
	password = std::string();
}

RegisterDevice::RegisterDevice(std::string jsonString)
{
	this->fromJson(jsonString);
}

RegisterDevice::~RegisterDevice()
{

}

void
RegisterDevice::fromJson(std::string jsonObj)
{
    bourne::json object = bourne::json::parse(jsonObj);

    const char *usernameKey = "username";

    if(object.has_key(usernameKey))
    {
        bourne::json value = object[usernameKey];



        jsonToValue(&username, value, "std::string");


    }

    const char *identifierKey = "identifier";

    if(object.has_key(identifierKey))
    {
        bourne::json value = object[identifierKey];



        jsonToValue(&identifier, value, "std::string");


    }

    const char *passwordKey = "password";

    if(object.has_key(passwordKey))
    {
        bourne::json value = object[passwordKey];



        jsonToValue(&password, value, "std::string");


    }


}

bourne::json
RegisterDevice::toJson()
{
    bourne::json object = bourne::json::object();





    object["username"] = getUsername();






    object["identifier"] = getIdentifier();






    object["password"] = getPassword();



    return object;

}

std::string
RegisterDevice::getUsername()
{
	return username;
}

void
RegisterDevice::setUsername(std::string  username)
{
	this->username = username;
}

std::string
RegisterDevice::getIdentifier()
{
	return identifier;
}

void
RegisterDevice::setIdentifier(std::string  identifier)
{
	this->identifier = identifier;
}

std::string
RegisterDevice::getPassword()
{
	return password;
}

void
RegisterDevice::setPassword(std::string  password)
{
	this->password = password;
}



