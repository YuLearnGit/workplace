#include "noipconfig.h"


NoIPConfig::NoIPConfig(const std::string &dev_IP, const std::string &fw_mac, QObject *parent)
                       :devIP(dev_IP),fwMAC(fw_mac),QCommInfo(parent)
{
    this->setBindInfo(getLocal_IP(),30335);
}

void NoIPConfig::TransparentMode()
{
    std::string cmd = "ifconfig br0 down && ifconfig br0 0.0.0.0 up";
    this->sendCmd(devIP,22222,CommInfo::commonShellCmdTypeByte,cmd);
}

void NoIPConfig::ResetFwIP(const std::string &ip_str)
{
    std::string cmd = "ifconfig br0 down && ifconfig br0 " +ip_str+" up";
    this->sendCmd(devIP,22222,CommInfo::commonShellCmdTypeByte,cmd);
}

void NoIPConfig::handResultData(const bool result, const char *commandTypeByte, const char *commandBytes, const int commandLength, const QString &srcIp, const int srcPort)
{
    if (result)
    {
        if (*commandTypeByte == *CommInfo::commonShellCmdTypeByte)
        {
            std::string data(commandBytes,commandLength);
            std::cout << data << std::endl;
            if(data == "success")
                emit configResult(true);
            if(data == "fail")
                emit configResult(false);
        }
    }
}
