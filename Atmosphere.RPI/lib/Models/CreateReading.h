
/*
 * CreateReading.h
 *
 * 
 */

#ifndef TINY_CPP_CLIENT_CreateReading_H_
#define TINY_CPP_CLIENT_CreateReading_H_


#include <string>
#include "bourne/json.hpp"
#include "Helpers.h"
#include "ReadingType.h"

namespace Tiny {


/*! \brief 
 *
 *  \ingroup Models
 *
 */

class CreateReading{
public:

    /*! \brief Constructor.
	 */
    CreateReading();
    CreateReading(std::string jsonString);


    /*! \brief Destructor.
	 */
    virtual ~CreateReading();


    /*! \brief Retrieve a bourne JSON representation of this class.
	 */
    bourne::json toJson();


    /*! \brief Fills in members of this class from bourne JSON object representing it.
	 */
    void fromJson(std::string jsonObj);

	/*! \brief Get 
	 */
	double getValue();

	/*! \brief Set 
	 */
	void setValue(double  value);
	/*! \brief Get 
	 */
	ReadingType getType();

	/*! \brief Set 
	 */
	void setType(ReadingType  type);
	/*! \brief Get 
	 */
	std::string getTimestamp();

	/*! \brief Set 
	 */
	void setTimestamp(std::string  timestamp);
	/*! \brief Get 
	 */
	std::string getUnit();

	/*! \brief Set 
	 */
	void setUnit(std::string  unit);


    private:
    double value{};
    ReadingType type;
    std::string timestamp{};
    std::string unit{};
};
}

#endif /* TINY_CPP_CLIENT_CreateReading_H_ */
