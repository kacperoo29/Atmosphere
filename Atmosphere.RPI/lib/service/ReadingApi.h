#ifndef TINY_CPP_CLIENT_ReadingApi_H_
#define TINY_CPP_CLIENT_ReadingApi_H_

#include "Arduino.h"
#include "Helpers.h"
#include "Response.h"
#include "Service.h"
#include <list>

#include "CreateReading.h"

namespace Tiny {

/**
 *  Class
 * Generated with openapi::tiny-cpp-client
 */

class ReadingApi : public Service {
public:
  ReadingApi() = default;

  virtual ~ReadingApi() = default;

  /**
   * .
   *
   *
   * \param createReading
   */
  Response<String> apiReadingCreateReadingPost(

      CreateReading createReading

  );
  /**
   * .
   *
   *
   * \param deviceId
   */
  Response<String> apiReadingGetAllReadingsGet(

      std::string deviceId

  );
};

} // namespace Tiny

#endif /* TINY_CPP_CLIENT_ReadingApi_H_ */