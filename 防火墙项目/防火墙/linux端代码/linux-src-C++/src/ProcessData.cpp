#include <string>
#include <iostream>
#include <memory>
#include <sys/stat.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <stdlib.h>
#include <linux/ip.h>
#include <netinet/in.h>
#include <linux/tcp.h>
#include <linux/udp.h>
#include <linux/icmp.h>
#include <sys/socket.h>
#include <sys/ioctl.h>
#include <sys/types.h>
#include <netinet/if_ether.h>
#include <linux/sockios.h>
#include <string.h>
#include <asm/types.h>
#include <linux/socket.h>
#include <errno.h>
#include <linux/netfilter_ipv4/ipt_ULOG.h>
#include <linux/netdevice.h>
extern "C" {
#include <libnetfilter_log/libnetfilter_log.h>
#include <libnetfilter_log/linux_nfnetlink_log.h>
#include <libnfnetlink/libnfnetlink.h>
}

#include "ProtectedDev.h"
#include "sendinfo.h"
#include "ProcessData.h"

using namespace std;

shared_ptr<SecurityDev> ProcessData::securdev_ptr = nullptr;
shared_ptr<ClientDev> ProcessData::clidev_ptr = nullptr;
shared_ptr<ProcessRules> ProcessData::prorule_ptr = nullptr;
map<string, shared_ptr<ProtectedDev>> ProcessData::protec_devs = map<string, shared_ptr<ProtectedDev>>();

ProcessData::ProcessData(shared_ptr<SecurityDev> s_ptr,
                         shared_ptr<ClientDev> c_ptr,
                         shared_ptr<ProcessRules> p_ptr)
{
    securdev_ptr = s_ptr;
    clidev_ptr = c_ptr;
    prorule_ptr = p_ptr;
}

void ProcessData::addProtecdev(const string &dev_ip, shared_ptr<ProtectedDev> protec_ptr)
{
    protec_devs[dev_ip] = protec_ptr;
}

void ProcessData::processData()
{
    static int lb_nflog_fd;
    static struct nflog_handle *handle;
    static struct nflog_g_handle *group_handle;

    handle = nflog_open();  /*打开nflog*/
    nflog_bind_pf(handle, AF_INET);  /*绑定地址族*/
    group_handle = nflog_bind_group(handle, NETLINK_GROUP);	/*绑定netlink组*/
    nflog_set_mode(group_handle, NFULNL_COPY_PACKET, LOG_CP_RANGE);		/*设置拷贝的数据包范围*/
    nflog_set_qthresh(group_handle, 1);		/*设置数据包缓存数量*/
    nflog_callback_register(group_handle, ProcessData::packet_callback, NULL);		/*注册回掉函数handle_packet,收到数据包后调用handle_packet处理*/
    lb_nflog_fd = nflog_fd(handle);

    char buf[MAX_MSG_SIZE];  /*接收缓冲区*/
    while(1)
    {
        int res = recv(lb_nflog_fd, buf, sizeof(buf),0);	/*接收一组数据包，存储在buf中，返回数据长度*/
        //cout << "res is:" << res << endl;
        nflog_handle_packet(handle, buf, res);		/*由回调函数处理数据包*/
    }
}

int ProcessData::packet_callback(struct nflog_g_handle *gh, struct nfgenmsg *nfmsg,
                                 struct nflog_data *nfa, void *data)
{

    #ifdef DEBUG
        cout << "********start processing data********" << endl;
    #endif

    char *payload = nullptr;
    int test_status = nflog_get_payload(nfa, &payload);
    struct iphdr *iph = (struct iphdr *)payload;	/*iph指向IP数据包包头位置*/

    /*获得源IP地址*/
    struct in_addr src_addr;
    src_addr.s_addr = iph->saddr;
    char *src_ip = inet_ntoa(src_addr);
    string srcIP(src_ip);
    clidev_ptr->setIP(srcIP);

    #ifdef DEBUG
        cout << "srcIP is : " << srcIP << endl;
    #endif

    /*获得目的IP地址*/
    struct in_addr dst_addr;
    dst_addr.s_addr = iph->daddr;
    char *dst_ip = inet_ntoa(dst_addr);
    string dstIP(dst_ip);
    shared_ptr<ProtectedDev> protecdev_ptr;

    if(protec_devs.find(dstIP) == protec_devs.end())
    {
        protecdev_ptr = make_shared<ProtectedDev>(dstIP);
        addProtecdev(dstIP, protecdev_ptr);
        protecdev_ptr->setMAC();
    }
    else
        protecdev_ptr = protec_devs[dstIP];

    #ifdef DEBUG
        cout << "dstIP is : " << dstIP << endl;
        cout << "dstMAC is : " << protecdev_ptr->getMAC() << endl;
    #endif

    /*get the packet content*/
    struct udphdr *udph;
    udph = (struct udphdr *)((void *)iph+4*(iph->ihl));  /*udph指向udp数据包头*/
    char *selfh1 = (char *)((void *)udph + 8);  /*selfh指向自定义数据包包头，包头为3个字节*/
    char *selfh2 = (char *)((void *)udph + 9);
    char *selfh3 = (char *)((void *)udph + 10);
    char *selfh4 = (char *)((void *)udph + 11);
    char *selfh5 = (char *)((void *)udph + 12);
    char *selfh6 = (char *)((void *)udph + 13);
    char *selfh7 = (char *)((void *)udph + 14); /*self4指向自定义数据包的数据内容*/

    /*防火墙规则配置*/
    if(iph->protocol == IPPROTO_UDP && ntohs(udph->dest) == 22222
            && *selfh1 == 0x0f && *selfh2 == 0x0e && *selfh3 ==0x0d
            && *selfh4 == 0x0c && *selfh5 == 0x0b && *selfh6 ==0x0a)
    {
        string content(selfh7);
        string rules;
        bool status = false;

        size_t pos = content.find("\x18");
        if(pos != string::npos)
        {
            rules = content.substr(0, pos);
            cout << rules << "\\x18" << endl;
        }
        else
		{
			pos = content.find("*");
			if(pos != string::npos)
			{
				rules = content.substr(0, pos);
				cout << rules << "*" << endl;
			}
			else
			{
				rules = content;
			}
		}

		// cout << "RULE:" << rules << endl;
        string rule_flag = rules.substr(0, 3);
        string oper_flag = rules.substr(3, 1);
        string rule = rules.substr(4, string::npos);
	//cout << rule_flag << " " << oper_flag << " " << rule << endl;
        //add iptables rules
        if(oper_flag == "1")
        {
            /*
            #ifdef DEBUG
            cout << "new rules are : " << rule << endl;
            #endif
            */
            system("iptables -D FORWARD -j DROP");
            if(prorule_ptr->connectMysql() &&
                    prorule_ptr->executeRule(rule)&&
                    prorule_ptr->addRule(rule, rule_flag))
                status = true;
            else
                status = false;

            prorule_ptr->closeMysql();
            system("iptables -A FORWARD -j DROP");
        }

        //delete iptables rules
        else if(oper_flag == "0")
        {
            /*
            #ifdef DEBUG
            cout << "deleted rules are : " << rule << endl;
            #endif
            */
            if(prorule_ptr->connectMysql())
            {
                if(rule_flag != "PRT")
                {
                    if(prorule_ptr->deleteRule(rule, rule_flag))
                    {
                        string reset = "iptables -F && iptables -t nat -F && iptables-restore</etc/iptables.up.rules";
                        status = prorule_ptr->executeRule(reset) || status;
                        status = prorule_ptr->executeAllRule() && status;
                    }
                }
                else
                {
                    prorule_ptr->executeRule(rule);
                    rule = rule.replace(rule.find_first_of("d"), 3, "add");
                    // cout << "route rule is : " << rule << endl;
                    status = prorule_ptr->deleteRule(rule, rule_flag) || status;
                }
            }
            else
                status = false;

            prorule_ptr->closeMysql();
        }

        //update iptables rules
        else if(oper_flag == "2")
        {
            /*
            #ifdef DEBUG
            cout << "update rules are : " << rule << endl;
            #endif
            */
            if(prorule_ptr->connectMysql() && prorule_ptr->updateRule(rule, rule_flag))
            {
                string reset = "iptables -F && iptables -t nat -F && iptables-restore</etc/iptables.up.rules";
                system("iptables -D FORWARD -j DROP");
                status = status || prorule_ptr->executeRule(reset);
                status = status && prorule_ptr->executeAllRule();
                system("iptables -A FORWARD -j DROP");
            }
            else
                status = false;

            prorule_ptr->closeMysql();
        }

        //execute other rules
        else
{
			cout << "Other Rules:" << rules << endl;
            status = prorule_ptr->executeRule(rules);
}
        string confirm_info = "fail";
        if(status)
        {
            confirm_info = "success";
            /*
            #ifdef DEBUG
            cout << "add/delete rules success" <<endl;
            #endif
            */
        }
        sendinfo(clidev_ptr->getIP(), protecdev_ptr->getIP(),
                 clidev_ptr->getMAC(), securdev_ptr->getMAC(),
                 30332, 30333, confirm_info);
    }

    /*防火墙设备确认*/
    if(iph->protocol == IPPROTO_UDP && ntohs(udph->dest) == 33333
            && *selfh1 == 0x0f && *selfh2 == 0x0e && *selfh3 ==0x0d
            && *selfh4 == 0x0c && *selfh5 == 0x0b && *selfh6 ==0x0a)
    {
        string client_mac(selfh7);
        clidev_ptr->setMAC(client_mac.substr(0, client_mac.find("*")));
        string content = securdev_ptr->getIP() + "&" + protecdev_ptr->getMAC() +
                         "&" + securdev_ptr->getMAC() + "&firedeviceConfirm&" +
                         protecdev_ptr->getIP();
        cout<<"send content: "<<content<<endl;
        sendinfo(clidev_ptr->getIP(), protecdev_ptr->getIP(),
                 clidev_ptr->getMAC(), securdev_ptr->getMAC(),
                 30330, 30331, content);
        //cout<<"IP: "<<clidev_ptr->getIP()<<"MAC: "<<clidev_ptr->getMAC()<<endl;
    }

    return 1;
}
