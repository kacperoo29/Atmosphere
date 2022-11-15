#include "Service.h"
#include "Arduino.h"

void Tiny::Service::begin(std::string url) {
  http.begin(String(url.c_str()));
  for (const auto &header : headers) {
    http.addHeader(String(std::get<0>(header).c_str()),
                   String(std::get<1>(header).c_str()));
  }

  headers.clear();
  queryParams.clear();
  formParams.clear();
}

void Tiny::Service::addHeader(std::string key, std::string value) {
  headers.push_back(std::make_tuple(key, value));
}

void Tiny::Service::addQueryParam(std::string key, std::string value) {
  formParams.push_back(std::make_tuple(key, value));
}

void Tiny::Service::addFormParam(std::string key, std::string value) {
  formParams.push_back(std::make_tuple(key, value));
}

int Tiny::Service::sendRequest(std::string url, const char *type,
                               uint8_t *payload, size_t size) {
  this->url = url;
  prepareRequest();

  begin(this->url);

  int httpCode = http.sendRequest(type, payload, size);
  return httpCode;
}

void Tiny::Service::prepareRequest() {
  if (!queryParams.empty()) {
    this->url += "?" + encodeKeyValueTuple(this->queryParams);
  }

  if (!formParams.empty()) {
    this->payload = encodeKeyValueTuple(this->formParams);
  }
}

std::string Tiny::Service::encodeKeyValueTuple(
    std::list<std::tuple<std::string, std::string>> params) {
  std::string encoded = "";
  for (auto const &tuple : params) {
    encoded += std::get<0>(tuple) + "=" + std::get<1>(tuple) + "&";
  }

  // Remove last '&' char from url
  if (!encoded.empty())
    encoded.pop_back();

  return encoded;
}

String Tiny::Service::getResponseBody() {
  if (http.getSize() <= 0) {
    http.end();

    return String();
  }

  String respBody = http.getString();
  http.end();

  return respBody;
}
