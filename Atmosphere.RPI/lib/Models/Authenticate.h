
/*
 * Authenticate.h
 *
 * 
 */

#ifndef TINY_CPP_CLIENT_Authenticate_H_
#define TINY_CPP_CLIENT_Authenticate_H_


#include <string>
#include "bourne/json.hpp"
#include "Helpers.h"

namespace Tiny {


/*! \brief 
 *
 *  \ingroup Models
 *
 */

class Authenticate{
public:

    /*! \brief Constructor.
	 */
    Authenticate();
    Authenticate(std::string jsonString);


    /*! \brief Destructor.
	 */
    virtual ~Authenticate();


    /*! \brief Retrieve a bourne JSON representation of this class.
	 */
    bourne::json toJson();


    /*! \brief Fills in members of this class from bourne JSON object representing it.
	 */
    void fromJson(std::string jsonObj);

	/*! \brief Get 
	 */
	std::string getUsername();

	/*! \brief Set 
	 */
	void setUsername(std::string  username);
	/*! \brief Get 
	 */
	std::string getPassword();

	/*! \brief Set 
	 */
	void setPassword(std::string  password);


    private:
    std::string username{};
    std::string password{};
};
}

#endif /* TINY_CPP_CLIENT_Authenticate_H_ */
