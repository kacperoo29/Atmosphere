#include "ReadingApi.h"

using namespace Tiny;

Response<String> ReadingApi::apiReadingCreateReadingPost(

    CreateReading createReading

) {
  std::string url = basepath + "/api/Reading/CreateReading"; //

  // Headers  |

  // Query    |

  // Form     |
  addHeader("Content-Type", "application/json");

  std::string payload = "";
  // Send Request
  // METHOD | POST
  // Body     | createReading

  payload = createReading.toJson().dump();

  int httpCode = sendRequest(
      url, "POST", reinterpret_cast<uint8_t *>(&payload[0]), payload.length());

  // Handle Request
  String output = getResponseBody();
  std::string output_string = output.c_str();

  Response<String> response(output, httpCode);
  return response;
}

Response<String> ReadingApi::apiReadingGetAllReadingsGet(

    std::string deviceId

) {
  std::string url = basepath + "/api/Reading/GetAllReadings"; //

  // Headers  |

  // Query    | deviceId
  addQueryParam("deviceId", deviceId);

  // Form     |

  std::string payload = "";
  // Send Request
  // METHOD | GET
  // Body     |
  int httpCode = sendRequest(
      url, "GET", reinterpret_cast<uint8_t *>(&payload[0]), payload.length());

  // Handle Request
  String output = getResponseBody();
  std::string output_string = output.c_str();

  Response<String> response(output, httpCode);
  return response;
}
