#include "basedeviceform.h"

//get 方法
std::string BaseDeviceForm::getdevIP() const
{
    return devIP;
}

std::string BaseDeviceForm::getdevMAC() const
{
    return devMAC;
}

int BaseDeviceForm::getdevPORT() const
{
    return devPort;
}


//set 方法
void BaseDeviceForm::setdevIP(const std::string &newIP)
{
    this->devIP = newIP;
}

void BaseDeviceForm::setdevMAC(const std::string &newMAC)
{
    this->devMAC = newMAC;
}

void BaseDeviceForm::setdevPORT(const int &newPORT)
{
   this->devPort = newPORT;
}
