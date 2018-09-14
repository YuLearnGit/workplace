#ifndef SENDLOG_H_INCLUDED
#define SENDLOG_H_INCLUDED

#include <memory>
#include <string>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

#include "SecurityDev.h"
#include "ClientDev.h"

void sendlog(std::shared_ptr<SecurityDev> s_ptr, std::shared_ptr<ClientDev> c_ptr);

#endif // SENDLOG_H_INCLUDED
