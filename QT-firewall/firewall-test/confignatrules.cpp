#include "confignatrules.h"
#include "iostream"

ConfigNATRules::ConfigNATRules(const std::string &devIP, const std::string &fw_mac, QObject *parent=nullptr)
                                :FwIP(devIP),fwMAC(fw_mac),QCommInfo(parent)
{
    fileOP = new FileOperation();
    this->setBindInfo(getLocal_IP(),30333);
}

ConfigNATRules::~ConfigNATRules()
{
    if(fileOP != nullptr)
        delete fileOP;
    fileOP = nullptr;
}

static std::string fileStr("");
static bool fileFlag = true;

void ConfigNATRules::ChangeSNAT(const std::string &ethName, const std::string &ethIP, const std::string &srcIP, bool addOrdel)
{
    std::string bridge_rule = "brctl delif br0 "+ethName;
    std::string ethIP_rule = "ifconfig "+ethName+" "+ethIP+" up";
    std::string rule = "iptables -t nat -A POSTROUTING -s "+srcIP+" -o br0 -j SNAT --to-source "+FwIP;

    std::string pro("NAT SNAT");
    std::string flag("NAT1");
    if(addOrdel)
    {
        fileFlag = true;
        std::string configInfo = flag+bridge_rule+" && "+ethIP_rule+" && "+rule;
        this->sendCmd(FwIP,22222,CommInfo::iptablesCmdTypeByte,configInfo);
    }
    else
    {
        flag.assign("NAT0");
        fileFlag = false;
        std::string configInfo = flag+bridge_rule+" && "+ethIP_rule+" && "+rule;
        this->sendCmd(FwIP,22222,CommInfo::iptablesCmdTypeByte,configInfo);
        std::string cmd = "brctl addif br0 "+ethName + " && " + "ifconfig "+ethName+" 0.0.0.0 up";
        this->sendCmd(FwIP,22222,CommInfo::commonShellCmdTypeByte,cmd);
    }

    fileStr = pro+" "+fwMAC+" "+ethName+" "+ethIP+" "+srcIP;
}

void ConfigNATRules::ChangeDNAT(const std::string &originDstIP, const std::string &originDstPort, const std::string &mapIP, const std::string &mapPort, bool addOrdel)
{
    std::string pro("NAT DNAT");
    std::string odp = " --dport "+originDstPort;
    std::string mdp = " --dport "+mapPort;
    if(originDstPort == "any")
        odp.assign("");
    if(mapPort == "any")
        mdp.assign("");
    std::string flag("NAT1");
    if(addOrdel)
    {
        fileFlag = true;
    }
    else
    {
        fileFlag = false;
        flag.assign("NAT0");
    }
    std::string config = flag+"iptables -t nat -A PREROUTING -d "+originDstIP+odp+" -j DNAT --to-destination "+mapIP+mdp;
    fileStr = pro+" "+fwMAC+" "+originDstIP+" "+originDstPort+" "+mapIP+" "+mapPort;
    std::cout<<config<<std::endl;

    this->sendCmd(FwIP,22222,CommInfo::iptablesCmdTypeByte,config);
}

//void ConfigNATRules::ClearAllNATRules()
//{
//    std::string cmd = "iptables -t nat -F";
//    this->sendCmd(FwIP,22222,CommInfo::commonShellCmdTypeByte,cmd);
//}

void ConfigNATRules::handResultData(const bool result, const char *commandTypeByte, const char *commandBytes, const int commandLength, const QString &srcIp, const int srcPort)
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
