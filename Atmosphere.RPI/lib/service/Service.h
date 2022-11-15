#ifndef TINY_CPP_CLIENT_ABSTRACTSERVICE_H_
#define TINY_CPP_CLIENT_ABSTRACTSERVICE_H_

#include "HTTPClient.h"
#include "Response.h"
#include <list>
namespace Tiny {

/**
 *  Class
 * Generated with openapi::tiny-cpp-client
 */
class Service {
public:
  HTTPClient http;
  std::string basepath =
      "http://192.168.0.50:5000"; // TODO: change to your url
  std::string url = "";
  std::string payload = "";
  std::list<std::tuple<std::string, std::string>> headers;
  std::list<std::tuple<std::string, std::string>> queryParams;
  std::list<std::tuple<std::string, std::string>> formParams;

  int sendRequest(std::string url, const char *type, uint8_t *payload,
                  size_t size);
  String getResponseBody();

  void addQueryParam(std::string key, std::string value);
  void addFormParam(std::string key, std::string value);
  void addHeader(std::string key, std::string value);

private:
  void begin(std::string url);
  void prepareRequest();
  std::string
  encodeKeyValueTuple(std::list<std::tuple<std::string, std::string>> params);

}; // end class
} // namespace Tiny

#endif /* TINY_CPP_CLIENT_ABSTRACTSERVICE_H_ */
