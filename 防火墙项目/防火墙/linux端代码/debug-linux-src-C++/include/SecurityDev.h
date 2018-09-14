#ifndef SECURITYDEV_H
#define SECURITYDEV_H

#include <map>
#include <memory>
#include <string>

#include "ProtectedDev.h"
#include "GeneralDev.h"
//#include "Singleton.h"

#define DEBUG

class SecurityDev : public GeneralDev {
 public:
  static std::shared_ptr<SecurityDev> getInstance();

 private:
  SecurityDev();
  SecurityDev(const SecurityDev& rhs) {};
  SecurityDev& operator = (const SecurityDev& rhs) {};

  std::string getLocalIP();
  std::string getLocalMAC();

  static std::shared_ptr<SecurityDev> self_ptr;
};

#endif // SECURITYDEV_H
