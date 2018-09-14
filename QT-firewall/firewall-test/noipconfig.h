#ifndef NOIPCONFIG_H
#define NOIPCONFIG_H

#include <string>
#include <iostream>
#include <memory>

#include "qcomminfo.h"
#include "tools.h"

/**
 * @brief The NoIPConfig class 防火墙透明模式配置类
 * Author：于仁飞
 * Date:2018-7-11
 */

class NoIPConfig : public QCommInfo
{
    Q_OBJECT
public:
    /**
     * @brief NoIPConfig 构造函数
     * @param devIP  设备IP地址，用于与客户端通信，可以是防火墙也可以是防火墙下的被保护设备
     * @param fw_mac 防火墙MAC地址
     * @param parent QObject父类
     */
    NoIPConfig(const std::string& dev_IP,const std::string& fw_mac,QObject *parent = nullptr);

    /**
     * @brief TransparentMode 将防火墙配置成透明模式
     */
    void TransparentMode();

    /**
     * @brief ResetFwIP 为透明模式下的防火墙重设IP地址
     * @param ip_str 要重设的IP地址
     */
    void ResetFwIP(const std::string& ip_str);
signals:
    /**
     * @brief configResult 若配置后接收到返回消息，则触发信号
     * @param result true表示配置成功，false表示配置失败
     */
    void configResult(bool result);

private:
    void handResultData(const bool result, const char *commandTypeByte, const char *commandBytes,
                        const int commandLength, const QString &srcIp,const int srcPort);
    std::string devIP;
    std::string fwMAC;
};

#endif // NOIPCONFIG_H
