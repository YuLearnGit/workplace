#include <string>

#include "GeneralDev.h"

using namespace std;

string GeneralDev::getIP() {
  return IP_addr;
}

string GeneralDev::getMAC() {
  return MAC_addr;
}

void GeneralDev::setIP(const std::string &ip_addr) {
  IP_addr = ip_addr;
}

void GeneralDev::setMAC(const std::string &mac_addr) {
  MAC_addr = mac_addr;
}
