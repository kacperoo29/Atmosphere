
/*
 * UserRole.h
 *
 * 
 */

#ifndef TINY_CPP_CLIENT_UserRole_H_
#define TINY_CPP_CLIENT_UserRole_H_


#include <string>
#include "bourne/json.hpp"
#include "Helpers.h"

namespace Tiny {


/*! \brief 
 *
 *  \ingroup Models
 *
 */

class UserRole{
public:

    /*! \brief Constructor.
	 */
    UserRole();
    UserRole(std::string jsonString);


    /*! \brief Destructor.
	 */
    virtual ~UserRole();


    /*! \brief Retrieve a bourne JSON representation of this class.
	 */
    bourne::json toJson();


    /*! \brief Fills in members of this class from bourne JSON object representing it.
	 */
    void fromJson(std::string jsonObj);



    private:
};
}

#endif /* TINY_CPP_CLIENT_UserRole_H_ */
