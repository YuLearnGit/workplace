#ifndef CONFIGNATRULES_H
#define CONFIGNATRULES_H


#include <string>
#include <iostream>

#include "fileoperation.h"
#include "qcomminfo.h"
#include "tools.h"

/**
 * @brief The ConfigNATRules class
 * 发射信号ConfigResult（bool result）
 * Author：于仁飞
 * Date：2018-7-13
 */
class ConfigNATRules:public QCommInfo
{
    Q_OBJECT
public:
    /**
     * @brief ConfigNATRules 构造函数
     * @param devIP  设备IP地址，用于与客户端通信，可以是防火墙也可以是防火墙下的被保护设备
     * @param fw_mac 防火墙MAC地址
     * @param parent QObject父类
     */
    ConfigNATRules(const std::string& devIP,const std::string& fw_mac,QObject *parent);
    ~ConfigNATRules();
    /**
     * @brief ChangeSNAT 修改源地址映射规则
     * @param ethName 网卡名字（eth0....)
     * @param ethIP 网卡IP地址
     * @param srcIP 设备IP地址（与网卡IP地址网段相同，与防火墙IP地址无关）
     * @param addOrdel true表示添加,false表示删除
     */
    void ChangeSNAT(const std::string& ethName,const std::string& ethIP,const std::string& srcIP,bool addOrdel);
    /**
     * @brief ChangeDNAT 修改目的地址映射规则
     * @param originDstIP 原目的IP地址
     * @param originDstPort 原目的端口（any表示任意端口）
     * @param mapIP 映射到的IP地址
     * @param mapPort 映射到的目的端口(any表示任意端口)
     * @param addOrdel true表示添加,false表示删除
     */
    void ChangeDNAT(const std::string& originDstIP,const std::string& originDstPort,const std::string& mapIP,const std::string& mapPort,bool addOrdel);
    //void ClearAllNATRules();
signals:
    /**
     * @brief configResult 若配置后接收到返回消息，则触发信号
     * @param result true表示配置成功，false表示配置失败
     */
    void configResult(bool result);

private:
    void handResultData(const bool result, const char *commandTypeByte, const char *commandBytes,
                        const int commandLength, const QString &srcIp,const int srcPort);
    std::string FwIP;
    std::string fwMAC;
    FileOperation *fileOP;
};

#endif // CONFIGNATRULES_H
