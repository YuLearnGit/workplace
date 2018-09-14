#include "configapcrules.h"



std::map<std::string,std::string> ConfigAPCRules::protocol_port({{ "ftp", "21"},
                                                                 { "http", "80"},
                                                                 { "telnet", "23"},
                                                                 { "smtp", "25"},
                                                                 { "pop3","110"},
                                                                 { "ssh" ,"22"}});
ConfigAPCRules::ConfigAPCRules(const std::string &dev_IP, const std::string &fw_mac, QObject *parent=nullptr)
    :FwOrProIP(dev_IP),fwMAC(fw_mac),QCommInfo(parent)
{
    fileOP = new FileOperation();
    this->setBindInfo(getLocal_IP(),30333);
}

ConfigAPCRules::~ConfigAPCRules()
{
    if(fileOP != nullptr)
        delete fileOP;
    fileOP = nullptr;
}

static std::string file_rule("");
static std::string old_file_rule("");
void ConfigAPCRules::ChangeApplicationProRules(const std::string &protocol, bool status)
{
    std::string pro = "APC ApplicationProtocol";
    std::string port = protocol_port[protocol];
    std::string rule("");

    if(status)
    {
        rule = "iptables -A INPUT -p tcp --dport " + port + " -j ACCEPT && " +
                "iptables -A OUTPUT -p tcp --dport " + port + " -j ACCEPT && " +
                "iptables -A FORWARD -p tcp --sport " + port + " -j ACCEPT && " +
                "iptables -A FORWARD -p tcp --dport " + port + " -j ACCEPT";
        file_rule = pro+" "+fwMAC+" " + protocol + " ACCEPT";
        old_file_rule = pro+" "+fwMAC+" " + protocol + " DROP";
    }
    else
    {
        rule = "iptables -A INPUT -p tcp --dport " + port + " -j DROP && " +
                "iptables -A OUTPUT -p tcp --dport " + port + " -j DROP && " +
                "iptables -A FORWARD -p tcp --sport " + port + " -j DROP && " +
                "iptables -A FORWARD -p tcp --dport " + port + " -j DROP";
        file_rule = pro+" "+fwMAC+" " + protocol + " DROP";
        old_file_rule = pro+" "+fwMAC+" " + protocol + " ACCEPT";
    }
    rule = "APC2" + rule + "#" + protocol;
    //std::cout<<rule<<std::endl;
    this->sendCmd(FwOrProIP,22222,CommInfo::iptablesCmdTypeByte,rule);
}

void ConfigAPCRules::handResultData(const bool result, const char *commandTypeByte, const char *commandBytes, const int commandLength, const QString &srcIp, const int srcPort)
{
    if (result)
    {
        if (*commandTypeByte == *CommInfo::iptablesCmdTypeByte)
        {
            std::string data(commandBytes,commandLength);
            //std::cout << data << std::endl;
            if(data == "success")
            {
                fileOP->updateRule(old_file_rule,file_rule);
                emit configResult(true);
            }
            if(data == "fail")
                emit configResult(false);
        }
    }
}
