#ifndef PROTECTEDDEV_H
#define PROTECTEDDEV_H

#include <string>

#include "GeneralDev.h"

#define DEBUG

class ProtectedDev : public GeneralDev {
 public:
  ProtectedDev();
  ProtectedDev(const std::string &ip_addr) : GeneralDev(ip_addr) {}
  void setMAC();

 private:
  std::string caculateMAC();
};

#endif // PROTECTEDDEV_H
