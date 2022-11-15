#ifndef TINY_CPP_CLIENT_AuthApi_H_
#define TINY_CPP_CLIENT_AuthApi_H_

#include "Arduino.h"
#include "Helpers.h"
#include "Response.h"
#include "Service.h"
#include <list>

#include "Authenticate.h"
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
   * \param authenticate
   */
  Response<String> apiAuthAuthenticatePost(

      Authenticate authenticate

  );
  /**
   * .
   *
   *
   * \param registerDevice
   */
  Response<String> apiAuthRegisterDevicePost(

      RegisterDevice registerDevice

  );
};

} // namespace Tiny

#endif /* TINY_CPP_CLIENT_AuthApi_H_ */