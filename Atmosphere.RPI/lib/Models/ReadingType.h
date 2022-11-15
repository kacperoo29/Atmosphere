
/*
 * ReadingType.h
 *
 *
 */

#ifndef TINY_CPP_CLIENT_ReadingType_H_
#define TINY_CPP_CLIENT_ReadingType_H_

#include "Helpers.h"
#include "bourne/json.hpp"
#include <string>

namespace Tiny {

enum class ReadingType {
  Unknown = -1,
  Temperature = 0,
  Humidity = 1,
  Pressure = 2,
};

std::string ReadingTypeToString(ReadingType value);
ReadingType ReadingTypeFromString(std::string value);

} // namespace Tiny

#endif /* TINY_CPP_CLIENT_ReadingType_H_ */
