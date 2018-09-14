#include "configprtrules.h"


ConfigPRTRules::ConfigPRTRules(const std::string &devIP, const std::string &fw_mac, QObject *parent=nullptr)
                               :FwOrProIP(devIP),fwMAC(fw_mac),QCommInfo(parent)
{
    fileOP = new FileOperation();
    this->setBindInfo(getLocal_IP(),30333);
}

ConfigPRTRules::~ConfigPRTRules()
{
    if(fileOP != nullptr)
        delete fileOP;
    fileOP = nullptr;
}
static std::string fileStr("");
static bool fileFlag = true;
void ConfigPRTRules::ChangeDefaultRoute(const std::string &ethName, const std::string &gateway, bool addOrdel)
{
    std::string rule("");std::string pro("PRT DefaultRoute");
    if(addOrdel)
    {
        rule = "PRT1route add default ";
        fileFlag = true;
    }
    else
    {
        rule = "PRT0route del default ";
        fileFlag = false;
    }
    if(ethName != "")
        rule += " dev "+ethName;
    if(gateway != "")
        rule += " gw "+gateway;
    fileStr = pro + " "+fwMAC+" "+ethName+" "+gateway;
    this->sendCmd(FwOrProIP,22222,CommInfo::iptablesCmdTypeByte,rule);
}

void ConfigPRTRules::ChangeHostRoute(const std::string &host, const std::string &ethName, const std::string &gateway, bool addOrdel)
{
    std::string rule("");std::string pro("PRT HostRoute");
    if(addOrdel)
    {
        fileFlag = true;
        rule = "PRT1route add -host "+host;
    }
    else
    {
        fileFlag = false;
        rule = "PRT0route del -host "+host;
    }
    if(ethName != "")
        rule += " dev "+ethName;
    if(gateway != "")
        rule += " gw "+gateway;
    fileStr = pro+" "+fwMAC + " "+host+" "+ethName+" "+gateway;
    this->sendCmd(FwOrProIP,22222,CommInfo::iptablesCmdTypeByte,rule);
}

void ConfigPRTRules::ChangeNetRoute(const std::string &net, const std::string &netmask, const std::string &ethName, const std::string &gateway, bool addOrdel)
{
    std::string rule("");std::string pro("PRT NetRoute");
    if(addOrdel)
    {
        rule = "PRT1route add -net "+net+" netmask "+netmask;
        fileFlag = true;
    }
    else
    {
        rule = "PRT0route del -net "+net+" netmask "+netmask;
        fileFlag = false;
    }
    if(ethName != "any")
        rule += " dev "+ethName;
    if(gateway != "any")
        rule += " gw "+gateway;
    fileStr = pro+" "+fwMAC+" "+net+" "+netmask+" "+ethName+" "+gateway;
    this->sendCmd(FwOrProIP,22222,CommInfo::iptablesCmdTypeByte,rule);
}


void ConfigPRTRules::handResultData(const bool result, const char *commandTypeByte, const char *commandBytes, const int commandLength, const QString &srcIp, const int srcPort)
{
    if (result)
    {
        if (*commandTypeByte == *CommInfo::iptablesCmdTypeByte)
        {
            std::string data(commandBytes,commandLength);
            //std::cout << data << std::endl;
            if(data == "success")
            {
                if(fileFlag)
                    fileOP->addRule(fileStr);
                else
                    fileOP->deleteRule(fileStr);
                emit configResult(true);
            }

            if(data == "fail")
                emit configResult(false);
        }
    }
}
