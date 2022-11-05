#ifndef _TEMP_READER_H
#define _TEMP_READER_H

#include "one_wire.h"
#include <string>
#include <vector>

struct TempResult {
  std::string deviceAddress;
  float value;
};

class TempReader {
public:
  TempReader(uint port);
  std::vector<TempResult> pollResults();

private:
  One_wire _ow;
};

#endif