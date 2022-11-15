
/*
 * RegisterDevice.h
 *
 * 
 */

#ifndef TINY_CPP_CLIENT_RegisterDevice_H_
#define TINY_CPP_CLIENT_RegisterDevice_H_


#include <string>
#include "bourne/json.hpp"
#include "Helpers.h"

namespace Tiny {


/*! \brief 
 *
 *  \ingroup Models
 *
 */

class RegisterDevice{
public:

    /*! \brief Constructor.
	 */
    RegisterDevice();
    RegisterDevice(std::string jsonString);


    /*! \brief Destructor.
	 */
    virtual ~RegisterDevice();


    /*! \brief Retrieve a bourne JSON representation of this class.
	 */
    bourne::json toJson();


    /*! \brief Fills in members of this class from bourne JSON object representing it.
	 */
    void fromJson(std::string jsonObj);

	/*! \brief Get 
	 */
	std::string getIdentifier();

	/*! \brief Set 
	 */
	void setIdentifier(std::string  identifier);
	/*! \brief Get 
	 */
	std::string getPassword();

	/*! \brief Set 
	 */
	void setPassword(std::string  password);


    private:
    std::string identifier{};
    std::string password{};
};
}

#endif /* TINY_CPP_CLIENT_RegisterDevice_H_ */
