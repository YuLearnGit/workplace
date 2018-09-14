
#ifndef GENERALDEV_H
#define GENERALDEV_H

#include <string>

class GeneralDev
{
public:
    GeneralDev() {};
    virtual ~GeneralDev(){}
    GeneralDev(const std::string &ip_addr)
    {
        IP_addr = ip_addr;
    }
    std::string getIP();
    std::string getMAC();
    void setIP(const std::string &ip_addr);
    void setMAC(const std::string &mac_addr);

protected:
    std::string IP_addr = "";
    std::string MAC_addr = "";
};

#endif // GENERALDEV_H
