#ifndef CONFIGPRTRULES_H
#define CONFIGPRTRULES_H

#include <string>
#include <iostream>

#include "fileoperation.h"
#include "qcomminfo.h"
#include "tools.h"

/**
 * @brief The ConfigPRTRules class
 * 发射信号ConfigResult（bool result）
 * Author：于仁飞
 * Date：2018-7-16
 */
class ConfigPRTRules : public QCommInfo
{
    Q_OBJECT
public:
    /**
     * @brief ConfigPRTRules
     * @param devIP  设备IP地址，用于与客户端通信，可以是防火墙也可以是防火墙下的被保护设备
     * @param fw_mac 防火墙MAC地址
     * @param parent QObject父类
     */
    ConfigPRTRules(const std::string& devIP,const std::string& fw_mac,QObject *parent);
    ~ConfigPRTRules();
    /**
     * @brief ChangeDefaultRoute 配置默认路由
     * @param ethName 路由指定的网络接口，如eth0,eth2等，不配置此项则传入空字符串
     * @param gateway 路由数据包通过的网管，不配置此项则传入空字符串
     * @param addOrdel true表示添加，false表示删除
     */
    void ChangeDefaultRoute(const std::string& ethName,const std::string& gateway,bool addOrdel);

    /**
     * @brief ChangeHostRoute 配置到主机的路由
     * @param ethName 路由指定的网络接口，如eth0,eth2等，不配置此项则传入空字符串
     * @param gateway 路由数据包通过的网管，不配置此项则传入空字符串
     * @param addOrdel true表示添加，false表示删除
     */
    void ChangeHostRoute(const std::string& host,const std::string& ethName,const std::string& gateway,bool addOrdel);

    /**
     * @brief ChangeNetRoute 配置到网路的路由
     * @param net 路由的目的网络IP，如192.16.10.0，不能为空字符串
     * @param netmask 目的网络IP地址网络掩码，如255.255.255.0，不能为空字符串
     * @param ethName 路由指定的网络接口，如eth0,eth2等，不配置此项则传入空字符串
     * @param gateway 路由数据包通过的网管，不配置此项则传入空字符串
     * @param addOrdel true表示添加，false表示删除
     */
    void ChangeNetRoute(const std::string& net,const std::string& netmask,const std::string& ethName,const std::string& gateway,bool addOrdel);
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

#endif // CONFIGPRTRULES_H
