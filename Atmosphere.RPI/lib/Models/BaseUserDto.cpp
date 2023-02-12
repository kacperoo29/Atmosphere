

#include "BaseUserDto.h"

using namespace Tiny;

BaseUserDto::BaseUserDto()
{
	id = std::string();
	username = std::string();
	role = UserRole();
	isActive = bool(false);
}

BaseUserDto::BaseUserDto(std::string jsonString)
{
	this->fromJson(jsonString);
}

BaseUserDto::~BaseUserDto()
{

}

void
BaseUserDto::fromJson(std::string jsonObj)
{
    bourne::json object = bourne::json::parse(jsonObj);

    const char *idKey = "id";

    if(object.has_key(idKey))
    {
        bourne::json value = object[idKey];



        jsonToValue(&id, value, "std::string");


    }

    const char *usernameKey = "username";

    if(object.has_key(usernameKey))
    {
        bourne::json value = object[usernameKey];



        jsonToValue(&username, value, "std::string");


    }

    const char *roleKey = "role";

    if(object.has_key(roleKey))
    {
        bourne::json value = object[roleKey];




        UserRole* obj = &role;
		obj->fromJson(value.dump());

    }

    const char *isActiveKey = "isActive";

    if(object.has_key(isActiveKey))
    {
        bourne::json value = object[isActiveKey];



        jsonToValue(&isActive, value, "bool");


    }


}

bourne::json
BaseUserDto::toJson()
{
    bourne::json object = bourne::json::object();





    object["id"] = getId();






    object["username"] = getUsername();







	object["role"] = getRole().toJson();





    object["isActive"] = isIsActive();



    return object;

}

std::string
BaseUserDto::getId()
{
	return id;
}

void
BaseUserDto::setId(std::string  id)
{
	this->id = id;
}

std::string
BaseUserDto::getUsername()
{
	return username;
}

void
BaseUserDto::setUsername(std::string  username)
{
	this->username = username;
}

UserRole
BaseUserDto::getRole()
{
	return role;
}

void
BaseUserDto::setRole(UserRole  role)
{
	this->role = role;
}

bool
BaseUserDto::isIsActive()
{
	return isActive;
}

void
BaseUserDto::setIsActive(bool  isActive)
{
	this->isActive = isActive;
}



