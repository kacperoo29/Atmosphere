#ifndef TINY_CPP_CLIENT_ReadingApi_H_
#define TINY_CPP_CLIENT_ReadingApi_H_

#include "Arduino.h"
#include "Helpers.h"
#include "Response.h"
#include "Service.h"
#include <list>

#include "CreateReading.h"
#include "ReadingDto.h"

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
   * \param createReading  *Required*
   */
  Response<ReadingDto> apiReadingCreateReadingPost(

      CreateReading createReading

  );
  /**
   * .
   *
   *
   */
  Response<std::list<ReadingDto>> apiReadingGetAllReadingsGet();
  /**
   * .
   *
   *
   * \param startDate
   * \param endDate
   */
  Response<std::list<ReadingDto>> apiReadingGetReadingsByDateGet(

      std::string startDate,

      std::string endDate

  );
  /**
   * .
   *
   *
   * \param deviceId
   */
  Response<std::list<ReadingDto>> apiReadingGetReadingsByDeviceGet(

      std::string deviceId

  );
};

} // namespace Tiny

#endif /* TINY_CPP_CLIENT_ReadingApi_H_ */