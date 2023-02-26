#include "AuthApi.h"

using namespace Tiny;

Response<String> AuthApi::apiAuthActivateUserIdPut(

    std::string id

) {
  std::string url = basepath + "/api/Auth/ActivateUser/{id}"; // id

  // Headers  |

  // Query    |

  // Form     |

  std::string s_id("{");
  s_id.append("id");
  s_id.append("}");

  int pos = url.find(s_id);

  url.erase(pos, s_id.length());
  url.insert(pos, stringify(id));

  std::string payload = "";
  // Send Request
  // METHOD | PUT
  // Body     |
  int httpCode = sendRequest(
      url, "PUT", reinterpret_cast<uint8_t *>(&payload[0]), payload.length());

  // Handle Request
  String output = getResponseBody();
  std::string output_string = output.c_str();

  Response<String> response(output, httpCode);
  return response;
}

Response<AuthResponseDto> AuthApi::apiAuthAuthenticatePost(

    Authenticate authenticate

) {
  std::string url = basepath + "/api/Auth/Authenticate"; //

  // Headers  |

  // Query    |

  // Form     |
  addHeader("Content-Type", "application/json");

  std::string payload = "";
  // Send Request
  // METHOD | POST
  // Body     | authenticate

  payload = authenticate.toJson().dump();

  int httpCode = sendRequest(
      url, "POST", reinterpret_cast<uint8_t *>(&payload[0]), payload.length());

  // Handle Request
  String output = getResponseBody();
  std::string output_string = output.c_str();
  if (httpCode != 200) {
    Response<AuthResponseDto> response(AuthResponseDto(), httpCode);
    return response;
  }

  AuthResponseDto obj(output_string);

  Response<AuthResponseDto> response(obj, httpCode);
  return response;
}

Response<BaseUserDto> AuthApi::apiAuthGetCurrentUserGet() {
  std::string url = basepath + "/api/Auth/GetCurrentUser"; //

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

  BaseUserDto obj(output_string);

  Response<BaseUserDto> response(obj, httpCode);
  return response;
}

Response<DeviceDto> AuthApi::apiAuthRegisterDevicePost(

    RegisterDevice registerDevice

) {
  std::string url = basepath + "/api/Auth/RegisterDevice"; //

  // Headers  |

  // Query    |

  // Form     |
  addHeader("Content-Type", "application/json");

  std::string payload = "";
  // Send Request
  // METHOD | POST
  // Body     | registerDevice

  payload = registerDevice.toJson().dump();

  int httpCode = sendRequest(
      url, "POST", reinterpret_cast<uint8_t *>(&payload[0]), payload.length());

  // Handle Request
  String output = getResponseBody();
  std::string output_string = output.c_str();

  DeviceDto obj(output_string);

  Response<DeviceDto> response(obj, httpCode);
  return response;
}
