#ifndef _TEMP_READER_H
#define _TEMP_READER_H

#include "one_wire.h"
#include <string>
#include <deque>

struct TempResult {
  std::string deviceAddress;
  float value;

  std::string toJson();
};

class TempReader {
public:
  TempReader(uint port);
  std::deque<TempResult> pollResults();

private:
  One_wire _ow;
};

#endif