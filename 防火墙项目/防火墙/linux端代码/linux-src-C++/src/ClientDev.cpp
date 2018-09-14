#include <memory>

#include "ClientDev.h"

using namespace std;

shared_ptr<ClientDev> ClientDev::self_ptr = nullptr;

shared_ptr<ClientDev> ClientDev::getInstance() {
  if(self_ptr == nullptr)
    self_ptr = shared_ptr<ClientDev>(new ClientDev);
  return self_ptr;
}

