#ifndef LOGINTIME_H
#define LOGINTIME_H

#include "udplistenerthread.h"

class IptablesLog
{
    /*
     * "Jul  7 16:37:54 hehe-develop kernel: [417215.277506] testIN=br0 OUT= PHYSIN=eth2 MAC=00:0b:ab:c6:5f:c5:94:de:80:2f:f2:dd:08:00
     *  SRC=192.16.10.254 DST=192.16.10.8 LEN=84 TOS=0x00 PREC=0x00 TTL=64 ID=38800 PROTO=ICMP TYPE=0 CODE=0 ID=5956 SEQ=186 $00:0b:ab:c6:5f:c5"
     *
     */
public:
    std::string logTime;
    std::string hostName;
    std::string processName;
    std::string logPrefix;
    std::string inInterface;// 数据包进入的接口，若为空表示本机产生，接口还有eth0、br0等
    std::string outInterface;// 数据包离开的接口，若为空表示本机接收
    std::string physin;
    std::string srcMac;
    std::string dstMac;
    std::string srcIp;
    std::string dstIp;
    int len;
    std::string tos;
    std::string prec;
    int ttl;
    int id1;
    std::string proto;
    std::string type;
    std::string code;
    int id2;
    int seq;
    std::string firewallMac;

};

class LogInTime:public UdpListenerThread
{
    Q_OBJECT
public:
    LogInTime(QObject *parent):UdpListenerThread(parent)
    {

    }
    ~LogInTime()
    {

    }
public:
    static std::string analyseLog(QByteArray data)
    {

    }
};



#endif // LOGINTIME_H
