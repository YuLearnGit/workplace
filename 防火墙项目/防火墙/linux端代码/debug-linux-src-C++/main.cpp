#include <iostream>
#include <memory>
#include <thread>

#include "SecurityDev.h"
#include "ClientDev.h"
#include "ProtectedDev.h"
#include "ProcessData.h"
#include "ProcessRules.h"
#include "sendlog.h"

using namespace std;

int main(int argc, char const *argv[]) {
daemon(0,0);    
shared_ptr<SecurityDev> securdev_ptr = SecurityDev::getInstance();
    shared_ptr<ClientDev> clidev_ptr = ClientDev::getInstance();
    shared_ptr<ProcessRules> prorule_ptr = ProcessRules::getInstance();


    prorule_ptr->connectMysql();
    prorule_ptr->loadRules();
    prorule_ptr->closeMysql();
    prorule_ptr->executeAllRule();

    //log sendlog
   // thread th_sendlog(sendlog, securdev_ptr, clidev_ptr);
   // th_sendlog.detach();

    ProcessData processdata(securdev_ptr, clidev_ptr, prorule_ptr);
    processdata.processData();

    return 0;
}
