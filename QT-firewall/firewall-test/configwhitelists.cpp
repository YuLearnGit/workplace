#include "configwhitelists.h"

using namespace std;


configwhitelists::configwhitelists(const string &devIP, const string &fw_mac, QObject *parent)
    :FwOrProIP(devIP),fwMAC(fw_mac),QCommInfo(parent)
{
    fileOP = new FileOperation();
    this->setBindInfo(getLocal_IP(),30333);
}

configwhitelists::~configwhitelists()
{
    if(fileOP != nullptr)
        delete fileOP;
    fileOP = nullptr;
}

static std::string file_rule("");
static bool fileFlag = true;
void configwhitelists::ChangeWhiteLists(const std::string &dst_ip,
                                        const std::string &src_ip,
                                        const std::string &dst_port,
                                        const std::string &src_port,
                                        bool add_delete)
{
    string pro = "WHL white";
    string rule = "iptables -A FORWARD -p tcp -s "+src_ip+" -d "+dst_ip
            +" --sport "+src_port+" --dport "+dst_port+" -j ACCEPT";
    string reverse_rule = "iptables -A FORWARD -p tcp -s "+dst_ip+" -d "+src_ip
            +" --sport "+dst_port+" --dport "+src_port+" -j ACCEPT";
    string log_rule = "iptables -A FORWARD -p tcp -s "+src_ip+" -d "+dst_ip
            +" --sport "+src_port+" --dport "+dst_port+" -j LOG --log-prefix "+"\"ACCEPT&WHL \"";
    file_rule = pro+" "+fwMAC+" "+dst_ip+" "+src_ip+" "+dst_port+" "+src_port;
    //cout<<file_rule<<endl;
    string flag = "WHL0";
    if(add_delete)
    {
        flag.replace(flag.begin(),flag.end(),"WHL1");
        fileFlag = true;
    }
    else
    {
        fileFlag = false;
    }
    rule = flag + rule + " && " +reverse_rule + " && " + log_rule;
    this->sendCmd(FwOrProIP,22222,CommInfo::iptablesCmdTypeByte,rule);
}

void configwhitelists::handResultData(const bool result, const char *commandTypeByte, const char *commandBytes, const int commandLength, const QString &srcIp, const int srcPort)
{
    if (result)
    {
        if (*commandTypeByte == *CommInfo::iptablesCmdTypeByte)
        {
            std::string data(commandBytes,commandLength);
            std::cout << data << std::endl;
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
