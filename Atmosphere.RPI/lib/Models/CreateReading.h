
/*
 * CreateReading.h
 *
 *
 */

#ifndef TINY_CPP_CLIENT_CreateReading_H_
#define TINY_CPP_CLIENT_CreateReading_H_

#include "Helpers.h"
#include "ReadingType.h"
#include "bourne/json.hpp"
#include <string>

namespace Tiny {

/*! \brief
 *
 *  \ingroup Models
 *
 */

class CreateReading {
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

  /*! \brief Fills in members of this class from bourne JSON object representing
   * it.
   */
  void fromJson(std::string jsonObj);

  /*! \brief Get
   */
  std::string getDeviceAddress();

  /*! \brief Set
   */
  void setDeviceAddress(std::string deviceAddress);
  /*! \brief Get
   */
  double getValue();

  /*! \brief Set
   */
  void setValue(double value);
  /*! \brief Get
   */
  ReadingType getType();

  /*! \brief Set
   */
  void setType(ReadingType type);
  /*! \brief Get
   */
  std::string getTimestamp();

  /*! \brief Set
   */
  void setTimestamp(std::string timestamp);

private:
  std::string deviceAddress{};
  double value{};
  ReadingType type;
  std::string timestamp{};
};
} // namespace Tiny

#endif /* TINY_CPP_CLIENT_CreateReading_H_ */
