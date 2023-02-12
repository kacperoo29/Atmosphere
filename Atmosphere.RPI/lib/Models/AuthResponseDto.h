
/*
 * AuthResponseDto.h
 *
 * 
 */

#ifndef TINY_CPP_CLIENT_AuthResponseDto_H_
#define TINY_CPP_CLIENT_AuthResponseDto_H_


#include <string>
#include "bourne/json.hpp"
#include "Helpers.h"

namespace Tiny {


/*! \brief 
 *
 *  \ingroup Models
 *
 */

class AuthResponseDto{
public:

    /*! \brief Constructor.
	 */
    AuthResponseDto();
    AuthResponseDto(std::string jsonString);


    /*! \brief Destructor.
	 */
    virtual ~AuthResponseDto();


    /*! \brief Retrieve a bourne JSON representation of this class.
	 */
    bourne::json toJson();


    /*! \brief Fills in members of this class from bourne JSON object representing it.
	 */
    void fromJson(std::string jsonObj);

	/*! \brief Get 
	 */
	std::string getToken();

	/*! \brief Set 
	 */
	void setToken(std::string  token);


    private:
    std::string token{};
};
}

#endif /* TINY_CPP_CLIENT_AuthResponseDto_H_ */
