#ifndef CONFIGAPCRULES_H
#define CONFIGAPCRULES_H

#include <string>
#include <map>
#include <iostream>

#include "fileoperation.h"
#include "tools.h"
#include "qcomminfo.h"

/**
 * @brief The ConfigAPCRules class 应用层协议控制，防火墙默认允许通过应用层协议
 * 发射信号ConfigResult（bool result）
 * Author：于仁飞
 * Date：2018-7-12
 */
class ConfigAPCRules : public QCommInfo
{
    Q_OBJECT
public:
    /**
     * @brief ConfigAPCRules 构造函数
     * @param devIP  设备IP地址，用于与客户端通信，可以是防火墙也可以是防火墙下的被保护设备
     * @param fw_mac 防火墙MAC地址
     * @param parent QObject父类
     */
    ConfigAPCRules(const std::string& dev_IP,const std::string& fw_mac,QObject *parent);
    ~ConfigAPCRules();
    /**
     * @brief ChangeApplicationProRules
     * @param protocol  应用层协议（http,telnet,pop3,ftp,smtp,ssh）
     * @param status  true表示允许，false表示禁止
     */
    void ChangeApplicationProRules(const std::string& protocol,bool status);
signals:
    /**
     * @brief configResult 若配置后接收到返回消息，则触发信号
     * @param result true表示配置成功，false表示配置失败
     */
    void configResult(bool result);

private:
    void handResultData(const bool result, const char *commandTypeByte, const char *commandBytes,
                        const int commandLength, const QString &srcIp,const int srcPort);
    std::string FwOrProIP;
    std::string fwMAC;
    FileOperation *fileOP;
    static std::map<std::string,std::string> protocol_port;
};

#endif // CONFIGAPCRULES_H
