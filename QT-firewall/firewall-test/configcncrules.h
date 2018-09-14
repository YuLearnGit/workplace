#ifndef CONFIGCNCRULES_H
#define CONFIGCNCRULES_H

#include <string>
#include <iostream>

#include "qcomminfo.h"
#include "fileoperation.h"
#include "tools.h"

/**
 * @brief The ConfigCNCRules class  连接数控制类
 * 发射信号ConfigResult（bool result）
 * Author：于仁飞
 * Date：2018-7-12
 */
class ConfigCNCRules:public QCommInfo
{
    Q_OBJECT
public:
    /**
     * @brief ConfigCNCRules 构造函数
     * @param devIP  设备IP地址，用于与客户端通信，可以是防火墙也可以是防火墙下的被保护设备
     * @param fw_mac 防火墙MAC地址
     * @param parent QObject父类
     */
    ConfigCNCRules(const std::string& dev_IP,const std::string& fw_mac,QObject *parent);
    ~ConfigCNCRules();
    /**
     * @brief ChangeConnectionControl
     * @param srcIP  规则中源IP地址
     * @param dstIP  规则中目的IP地址
     * @param sport  规则中源端口
     * @param dport  规则中目的端口
     * @param number 最大连接数
     * @param addOrdel  true表示添加，false表示删除
     */
    void ChangeConnectionControl(const std::string& srcIP,const std::string& dstIP,const std::string& sport,const std::string& dport,int number,bool addOrdel);
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
};



#endif // CONFIGCNCRULES_H
