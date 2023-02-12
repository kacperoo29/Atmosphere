

#include "AuthResponseDto.h"

using namespace Tiny;

AuthResponseDto::AuthResponseDto()
{
	token = std::string();
}

AuthResponseDto::AuthResponseDto(std::string jsonString)
{
	this->fromJson(jsonString);
}

AuthResponseDto::~AuthResponseDto()
{

}

void
AuthResponseDto::fromJson(std::string jsonObj)
{
    bourne::json object = bourne::json::parse(jsonObj);

    const char *tokenKey = "token";

    if(object.has_key(tokenKey))
    {
        bourne::json value = object[tokenKey];



        jsonToValue(&token, value, "std::string");


    }


}

bourne::json
AuthResponseDto::toJson()
{
    bourne::json object = bourne::json::object();





    object["token"] = getToken();



    return object;

}

std::string
AuthResponseDto::getToken()
{
	return token;
}

void
AuthResponseDto::setToken(std::string  token)
{
	this->token = token;
}



