#ifndef TINY_CPP_CLIENT_AuthApi_H_
#define TINY_CPP_CLIENT_AuthApi_H_

#include "Arduino.h"
#include "Helpers.h"
#include "Response.h"
#include "Service.h"
#include <list>

#include "AuthResponseDto.h"
#include "Authenticate.h"
#include "BaseUserDto.h"
#include "DeviceDto.h"
#include "RegisterDevice.h"

namespace Tiny {

/**
 *  Class
 * Generated with openapi::tiny-cpp-client
 */

class AuthApi : public Service {
public:
  AuthApi() = default;

  virtual ~AuthApi() = default;

  /**
   * .
   *
   *
   * \param id  *Required*
   */
  Response<String> apiAuthActivateUserIdPut(

      std::string id

  );
  /**
   * .
   *
   *
   * \param authenticate  *Required*
   */
  Response<AuthResponseDto> apiAuthAuthenticatePost(

      Authenticate authenticate

  );
  /**
   * .
   *
   *
   */
  Response<BaseUserDto> apiAuthGetCurrentUserGet();
  /**
   * .
   *
   *
   * \param registerDevice  *Required*
   */
  Response<DeviceDto> apiAuthRegisterDevicePost(

      RegisterDevice registerDevice

  );
};

} // namespace Tiny

#endif /* TINY_CPP_CLIENT_AuthApi_H_ */