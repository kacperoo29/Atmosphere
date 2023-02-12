#include "ReadingApi.h"

using namespace Tiny;

Response<ReadingDto> ReadingApi::apiReadingCreateReadingPost(

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

  ReadingDto obj(output_string);

  Response<ReadingDto> response(obj, httpCode);
  return response;
}

Response<std::list<ReadingDto>> ReadingApi::apiReadingGetAllReadingsGet() {
  std::string url = basepath + "/api/Reading/GetAllReadings"; //

  // Headers  |

  // Query    |

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

  std::list<ReadingDto> obj = std::list<ReadingDto>();
  bourne::json jsonPayload(output_string);

  for (auto &var : jsonPayload.array_range()) {
    ReadingDto tmp(var.dump());
    obj.push_back(tmp);
  }

  Response<std::list<ReadingDto>> response(obj, httpCode);
  return response;
}

Response<std::list<ReadingDto>> ReadingApi::apiReadingGetReadingsByDateGet(

    std::string startDate,

    std::string endDate

) {
  std::string url = basepath + "/api/Reading/GetReadingsByDate"; //

  // Headers  |

  // Query    | startDate endDate
  addQueryParam("startDate", startDate);
  addQueryParam("endDate", endDate);

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

  std::list<ReadingDto> obj = std::list<ReadingDto>();
  bourne::json jsonPayload(output_string);

  for (auto &var : jsonPayload.array_range()) {
    ReadingDto tmp(var.dump());
    obj.push_back(tmp);
  }

  Response<std::list<ReadingDto>> response(obj, httpCode);
  return response;
}

Response<std::list<ReadingDto>> ReadingApi::apiReadingGetReadingsByDeviceGet(

    std::string deviceId

) {
  std::string url = basepath + "/api/Reading/GetReadingsByDevice"; //

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

  std::list<ReadingDto> obj = std::list<ReadingDto>();
  bourne::json jsonPayload(output_string);

  for (auto &var : jsonPayload.array_range()) {
    ReadingDto tmp(var.dump());
    obj.push_back(tmp);
  }

  Response<std::list<ReadingDto>> response(obj, httpCode);
  return response;
}
