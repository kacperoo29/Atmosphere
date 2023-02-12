
/*
 * ReadingDto.h
 *
 * 
 */

#ifndef TINY_CPP_CLIENT_ReadingDto_H_
#define TINY_CPP_CLIENT_ReadingDto_H_


#include <string>
#include "bourne/json.hpp"
#include "Helpers.h"
#include "DeviceDto.h"
#include "ReadingType.h"

namespace Tiny {


/*! \brief 
 *
 *  \ingroup Models
 *
 */

class ReadingDto{
public:

    /*! \brief Constructor.
	 */
    ReadingDto();
    ReadingDto(std::string jsonString);


    /*! \brief Destructor.
	 */
    virtual ~ReadingDto();


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
	DeviceDto getDevice();

	/*! \brief Set 
	 */
	void setDevice(DeviceDto  device);
	/*! \brief Get 
	 */
	double getValue();

	/*! \brief Set 
	 */
	void setValue(double  value);
	/*! \brief Get 
	 */
	std::string getUnit();

	/*! \brief Set 
	 */
	void setUnit(std::string  unit);
	/*! \brief Get 
	 */
	std::string getTimestamp();

	/*! \brief Set 
	 */
	void setTimestamp(std::string  timestamp);
	/*! \brief Get 
	 */
	ReadingType getType();

	/*! \brief Set 
	 */
	void setType(ReadingType  type);


    private:
    std::string id{};
    DeviceDto device;
    double value{};
    std::string unit{};
    std::string timestamp{};
    ReadingType type;
};
}

#endif /* TINY_CPP_CLIENT_ReadingDto_H_ */
