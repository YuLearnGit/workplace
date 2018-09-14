#include "configcncrules.h"

ConfigCNCRules::ConfigCNCRules(const std::string &dev_IP, const std::string &fw_mac, QObject *parent=nullptr)
    :FwOrProIP(dev_IP),fwMAC(fw_mac),QCommInfo(parent)
{
    fileOP = new FileOperation();
    this->setBindInfo(getLocal_IP(),30333);
}

ConfigCNCRules::~ConfigCNCRules()
{
    if(fileOP != nullptr)
        delete fileOP;
    fileOP = nullptr;
}

static bool fileFlag = true;
static std::string file_rule("");

void ConfigCNCRules::ChangeConnectionControl(const std::string &srcIP, const std::string &dstIP, const std::string &sport, const std::string &dport, int number, bool addOrdel)
{
    std::string pro = "CNC ConnectionControl";
    std::string src = " -s "+srcIP;
    std::string dst = " -d "+dstIP;
    if(srcIP == "any")
        src.assign("");
    if(dstIP == "any")
        dst.assign("");
    std::string flag = "";std::string rule("");
    if(addOrdel)
    {
        flag.assign("CNC1");
        fileFlag = true;
    }
    else
    {
        flag.assign("CNC0");
        fileFlag = false;
    }

    rule = flag+"iptables -A FORWARD -p tcp --syn"+src+" --sport "+sport+dst+" --dport "
            +dport+" -m connlimit --connlimit-above "+std::to_string(number);
    file_rule = pro+" "+fwMAC + " "+srcIP+" "+dstIP + " "+sport+" "+dport+" "+std::to_string(number);
    this->sendCmd(FwOrProIP,22222,CommInfo::iptablesCmdTypeByte,rule);
}

void ConfigCNCRules::handResultData(const bool result, const char *commandTypeByte, const char *commandBytes, const int commandLength, const QString &srcIp, const int srcPort)
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
                    fileOP->addRule(file_rule);
                else
                    fileOP->deleteRule(file_rule);
                emit configResult(true);
            }
            if(data == "fail")
                emit configResult(false);
        }
    }
}
