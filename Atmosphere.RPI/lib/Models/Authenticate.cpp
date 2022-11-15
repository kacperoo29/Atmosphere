

#include "Authenticate.h"

using namespace Tiny;

Authenticate::Authenticate()
{
	identifier = std::string();
	key = std::string();
}

Authenticate::Authenticate(std::string jsonString)
{
	this->fromJson(jsonString);
}

Authenticate::~Authenticate()
{

}

void
Authenticate::fromJson(std::string jsonObj)
{
    bourne::json object = bourne::json::parse(jsonObj);

    const char *identifierKey = "identifier";

    if(object.has_key(identifierKey))
    {
        bourne::json value = object[identifierKey];



        jsonToValue(&identifier, value, "std::string");


    }

    const char *keyKey = "key";

    if(object.has_key(keyKey))
    {
        bourne::json value = object[keyKey];



        jsonToValue(&key, value, "std::string");


    }


}

bourne::json
Authenticate::toJson()
{
    bourne::json object = bourne::json::object();





    object["identifier"] = getIdentifier();






    object["key"] = getKey();



    return object;

}

std::string
Authenticate::getIdentifier()
{
	return identifier;
}

void
Authenticate::setIdentifier(std::string  identifier)
{
	this->identifier = identifier;
}

std::string
Authenticate::getKey()
{
	return key;
}

void
Authenticate::setKey(std::string  key)
{
	this->key = key;
}



