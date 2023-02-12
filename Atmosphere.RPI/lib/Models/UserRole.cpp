

#include "UserRole.h"

using namespace Tiny;

UserRole::UserRole()
{
}

UserRole::UserRole(std::string jsonString)
{
	this->fromJson(jsonString);
}

UserRole::~UserRole()
{

}

void
UserRole::fromJson(std::string jsonObj)
{
    bourne::json object = bourne::json::parse(jsonObj);


}

bourne::json
UserRole::toJson()
{
    bourne::json object = bourne::json::object();


    return object;

}



