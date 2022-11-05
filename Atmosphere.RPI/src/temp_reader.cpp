#include "temp_reader.h"

TempReader::TempReader(uint port) : _ow(One_wire(port)) { _ow.init(); }

std::vector<TempResult> TempReader::pollResults() {
  int count = _ow.find_and_count_devices_on_bus();
  std::vector<TempResult> results;
  rom_address_t null_address{};
  _ow.convert_temperature(null_address, true, true);
  for (int i = 0; i < count; ++i) {
    auto address = One_wire::get_address(i);

    auto format = "%02x%02x%02x%02x%02x%02x%02x%02x";
    std::string addr(17, '\0');
    std::snprintf(&addr[0], 17, format, address.rom[0], address.rom[1],
                  address.rom[2], address.rom[3], address.rom[4],
                  address.rom[5], address.rom[6], address.rom[7]);

    TempResult result{.deviceAddress = addr, .value = _ow.temperature(address)};
    results.push_back(result);
  }

  return results;
}