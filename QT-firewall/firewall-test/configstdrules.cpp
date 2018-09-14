#include "configstdrules.h"

ConfigSTDRules::ConfigSTDRules(const std::string &devIP, const std::string &fw_mac, QObject *parent)
    :FwOrProIP(devIP),fwMAC(fw_mac),QCommInfo(parent)
{
    fileOP = new FileOperation();
    this->setBindInfo(getLocal_IP(),30333);
}

ConfigSTDRules::~ConfigSTDRules()
{
    if(fileOP != nullptr)
        delete fileOP;
    fileOP = nullptr;
}

static bool fileFlag = true;
static std::string file_rule("");
void ConfigSTDRules::ChangeStateDetect(const std::string &protocol, const std::string &srcIP, const std::string &dstIP,
                                       const std::string &sport, const std::string &dport, bool addOrdel)
{
    std::string std_pro("STD StateDetection");
    std::string src = " -s "+srcIP;
    std::string dst = " -d "+dstIP;
    if(srcIP == "any")
        src.assign("");
    if(dstIP == "any")
        dst.assign("");
    std::string flag("");
    if(addOrdel)
    {
        flag.assign("STD1");
        fileFlag = true;
    }
    else
    {
        flag.assign("STD0");
        fileFlag = false;
    }
    std::string rule1 = "iptables -A FORWARD -p "+protocol+src+dst+" --sport "+sport+" --dport "+dport;
    std::string rule = flag+rule1 + " -m state --state NEW -j ACCEPT";
    std::string log_rule = rule1+" -m state --state NEW -j LOG";
    rule = rule + " && "+log_rule;
    file_rule = std_pro + " "+fwMAC+" "+protocol+" "+srcIP+" "+dstIP+" "+sport+" "+dport;
    this->sendCmd(FwOrProIP,22222,CommInfo::iptablesCmdTypeByte,rule);
}

void ConfigSTDRules::handResultData(const bool result, const char *commandTypeByte, const char *commandBytes, const int commandLength, const QString &srcIp, const int srcPort)
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
