
/*
 * BaseUserDto.h
 *
 * 
 */

#ifndef TINY_CPP_CLIENT_BaseUserDto_H_
#define TINY_CPP_CLIENT_BaseUserDto_H_


#include <string>
#include "bourne/json.hpp"
#include "Helpers.h"
#include "UserRole.h"

namespace Tiny {


/*! \brief 
 *
 *  \ingroup Models
 *
 */

class BaseUserDto{
public:

    /*! \brief Constructor.
	 */
    BaseUserDto();
    BaseUserDto(std::string jsonString);


    /*! \brief Destructor.
	 */
    virtual ~BaseUserDto();


    /*! \brief Retrieve a bourne JSON representation of this class.
	 */
    bourne::json toJson();


    /*! \brief Fills in members of this class from bourne JSON object representing it.
	 */
    void fromJson(std::string jsonObj);

	/*! \brief Get 
	 */
	std::string getId();

	/*! \brief Set 
	 */
	void setId(std::string  id);
	/*! \brief Get 
	 */
	std::string getUsername();

	/*! \brief Set 
	 */
	void setUsername(std::string  username);
	/*! \brief Get 
	 */
	UserRole getRole();

	/*! \brief Set 
	 */
	void setRole(UserRole  role);
	/*! \brief Get 
	 */
	bool isIsActive();

	/*! \brief Set 
	 */
	void setIsActive(bool  isActive);


    private:
    std::string id{};
    std::string username{};
    UserRole role;
    bool isActive{};
};
}

#endif /* TINY_CPP_CLIENT_BaseUserDto_H_ */
