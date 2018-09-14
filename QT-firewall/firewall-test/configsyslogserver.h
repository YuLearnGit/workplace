#ifndef CONFIGSYSLOGSERVER_H
#define CONFIGSYSLOGSERVER_H

#include "qcomminfo.h"
#include <string>
#include <iostream>
#include <ctime>
#include <cstdlib>
#include <QByteArray>
#include <list>

class ConfigSyslogServer:public QCommInfo
{
    Q_OBJECT
public:
    ConfigSyslogServer(QObject *parent):QCommInfo(parent)
    {

    }
signals:
    /**
     * @brief writeSyslogServerResult 写入结果信号
     * @param result
     * @param data
     */
    void writeSyslogServerResult(bool result, std::string data,QString srcIp, int srcPort);

    /**
     * @brief readSyslogServerResult 读取结果信号
     * @param result 是否读取成功（目前只为true）
     * @param data 读取的结果数据
     *              如果有配置，返回结果是：*.* @192.16.10.254:514
     *                  可调用SyslogServerInfo::parseReadCmdResult(data)进行解析
     *              如果没有配置，返回结果是：空字符串
     */
    void readSyslogServerResult(bool result, std::string data,QString srcIp, int srcPort);
public:
    /**
     * @brief handResultData 继承自QCommInfo，每次收到数据后，并协议检查通过后，会调用本函数
     * @param result    是否成功（目前只为true）
     * @param commandTypeByte 协议中第14字节的命令类型，检查此位可判断是否是当前类型命令的返回包
     * @param commandBytes 协议中第15字节到结束字节之前（不包含结束字节）
     * @param commandLength commandBytes长度
     * @param srcIp 此数据包的源IP
     * @param srcPort 此数据包的源Port
     */
    void handResultData(const bool result,const char *commandTypeByte,const char *commandBytes,const int commandLength,const QString &srcIp,const int srcPort)
    {
        std::cout << "ConfigSyslogServer::handResultData()" << std::endl;
        if (result)
        {
            if (*commandTypeByte == *CommInfo::syslogServerCmdTypeByte)
            {
                const char *operationTypeByte = commandBytes;
                if (*operationTypeByte == *SyslogServerInfo::writeServerOperationTypeByte)
                {
                    std::string data(commandBytes+1,commandLength-1);
                    std::cout << "write server result=" << data << std::endl;
                    emit writeSyslogServerResult(true,data,srcIp,srcPort);
                }
                else if (*operationTypeByte == *SyslogServerInfo::readServerOperationTypeByte)
                {
                    std::string data(commandBytes+1,commandLength-1);
                    std::cout << "read server result=" << data << "\ndataLength:" << data.length() << std::endl;
//                    SyslogServerInfo *info = SyslogServerInfo::parseReadCmdResult(data);
//                    if (info != nullptr)
//                    {
//                        info->display();
//                        DELETE(info);
//                    }
                    emit readSyslogServerResult(true,data,srcIp,srcPort);
                }
                else
                {
                    std::cout << "no type" << std::endl;
                    for (int i=0;i < commandLength;i++)
                    {
                        putchar((unsigned char)commandBytes[i]);
                    }
                    std::cout << std::endl;
                }
            }
        }
    }
public: // 与syslog服务器配置有关的操作

    // 发送 写入syslog服务器的配置 的命令
    void sendWriteServerCmd(const std::string dstIp,const int dstPort,const SyslogServerInfo &serverInfo)
    {
        QCommInfo::sendCmd(dstIp,dstPort,CommInfo::syslogServerCmdTypeByte,SyslogServerInfo::composeWriteServerCmd(serverInfo));
    }

    // 发送 读取syslog服务器配置的结果 的命令
    void sendReadServerCmd(const std::string dstIp,const int dstPort)
    {
        QCommInfo::sendCmd(dstIp,dstPort,CommInfo::syslogServerCmdTypeByte,SyslogServerInfo::composeReadServerCmd());
    }

    /**
     * @brief sendWriteServerCmd 同时给多个防火墙ip配置syslog服务器
     *          在handResultData接收函数中或信号writeSyslogServerResult中，可根据srcIp区分不同的IP返回的消息
     * @param dstIpList 要发送的防火墙Ip列表
     * @param dstPort   要通信的Port，需是同一通信端口
     * @param serverInfo    配置的同一syslog服务器信息
     */
    void sendWriteServerCmd(const std::list<std::string> dstIpList, const int dstPort, const SyslogServerInfo &serverInfo)
    {
        std::string cmdStr = SyslogServerInfo::composeWriteServerCmd(serverInfo);
        for (auto dstIp: dstIpList)
        {
            this->appendCmd(dstIp,dstPort,CommInfo::syslogServerCmdTypeByte,cmdStr);
        }
        this->sendCmd();
    }

    /**
     * @brief sendWriteServerCmd 同时读取多个防火墙ip配置syslog服务器
     *              在handResultData接收函数中或信号readSyslogServerResult中，可根据srcIp区分不同的IP返回的消息
     * @param dstIpList 要读取的防火墙Ip列表
     * @param dstPort   要通信的Port，需是同一通信端口
     */
    void sendReadServerCmd(const std::list<std::string> dstIpList, const int dstPort)
    {
        std::string cmdStr = SyslogServerInfo::composeReadServerCmd();
        for (auto dstIp: dstIpList)
        {
            this->appendCmd(dstIp,dstPort,CommInfo::syslogServerCmdTypeByte,cmdStr);
        }
        this->sendCmd();
    }
};


#endif // SYSLOGSERVERCONFIG_H
