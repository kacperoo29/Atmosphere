
/*
 * DeviceDto.h
 *
 *
 */

#ifndef TINY_CPP_CLIENT_DeviceDto_H_
#define TINY_CPP_CLIENT_DeviceDto_H_

#include "Helpers.h"
#include "bourne/json.hpp"
#include <string>

namespace Tiny {

/*! \brief
 *
 *  \ingroup Models
 *
 */

class DeviceDto {
public:
  /*! \brief Constructor.
   */
  DeviceDto();
  DeviceDto(std::string jsonString);

  /*! \brief Destructor.
   */
  virtual ~DeviceDto();

  /*! \brief Retrieve a bourne JSON representation of this class.
   */
  bourne::json toJson();

  /*! \brief Fills in members of this class from bourne JSON object representing
   * it.
   */
  void fromJson(std::string jsonObj);

  /*! \brief Get
   */
  std::string getId();

  /*! \brief Set
   */
  void setId(std::string id);
  /*! \brief Get
   */
  std::string getIdentifier();

  /*! \brief Set
   */
  void setIdentifier(std::string identifier);
  /*! \brief Get
   */
  bool isIsActive();

  /*! \brief Set
   */
  void setIsActive(bool isActive);
  /*! \brief Get
   */
  bool isIsConnected();

  /*! \brief Set
   */
  void setIsConnected(bool isConnected);

private:
  std::string id{};
  std::string identifier{};
  bool isActive{};
  bool isConnected{};
};
} // namespace Tiny

#endif /* TINY_CPP_CLIENT_DeviceDto_H_ */
