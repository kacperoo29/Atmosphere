#include "temp_reader.h"

std::string TempResult::toJson() {
  std::string json = "{\"deviceAddress\":\"" + deviceAddress +
                     "\",\"value\":" + std::to_string(value) + "}";

  return json;
}

TempReader::TempReader(uint port) : _ow(One_wire(port)) {
  _ow.init();
}

std::deque<TempResult> TempReader::pollResults() {
  int count = _ow.find_and_count_devices_on_bus();
  std::deque<TempResult> results;
  rom_address_t null_address{};
  _ow.convert_temperature(null_address, true, true);
  for (int i = 0; i < count; ++i) {
    auto address = One_wire::get_address(i);

    std::string addr;
    for (int i = 0; i < 8; ++i) {
      char buf[3];
      std::sprintf(buf, "%02x", address.rom[i]);
      addr += buf;
    }

    TempResult result{.deviceAddress = addr, .value = _ow.temperature(address)};
    results.push_back(result);
  }

  return results;
}
