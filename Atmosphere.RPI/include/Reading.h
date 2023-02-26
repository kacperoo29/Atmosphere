#ifndef _READING_H
#define _READING_H

#include <string>

struct Reading {
  double value;
  std::string unit;
  std::string type;
  std::string deviceAddress;
  std::string timestamp;
};

#endif