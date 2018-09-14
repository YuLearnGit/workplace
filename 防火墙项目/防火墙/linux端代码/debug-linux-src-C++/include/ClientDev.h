#ifndef CLIENTDEV_H
#define CLIENTDEV_H

#include <memory>

#include "GeneralDev.h"
//#include "Singleton.h"

class ClientDev : public GeneralDev {
 public:
  //ClientDev();
  static std::shared_ptr<ClientDev> getInstance();

 private:
  ClientDev(){};
  ClientDev(const ClientDev& rhs) {}
  ClientDev& operator = (const ClientDev& rhs) {}
  static std::shared_ptr<ClientDev> self_ptr;
};

#endif // CLIENTDEV_H
