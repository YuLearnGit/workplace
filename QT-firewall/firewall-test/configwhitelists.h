#ifndef CONFIGWHITELISTS_H
#define CONFIGWHITELISTS_H

#include <string>
#include "fileoperation.h"
#include "qcomminfo.h"
#include "tools.h"

/*
 * 白名单规则配置类
 * Author:于仁飞
 * Date:2018-6-21
*/
class configwhitelists:public QCommInfo
{
    Q_OBJECT
public:
    /**
     * @brief configwhitelists 构造函数
     * @param devIP  设备IP地址，用于与客户端通信，可以是防火墙也可以是防火墙下的被保护设备
     * @param fw_mac 防火墙MAC地址
     * @param parent QObject父类
     */
    configwhitelists(const std::string& devIP,const std::string& fw_mac,QObject *parent=nullptr);
    ~configwhitelists();
public:
    /**
     * @brief ChangeWhiteLists
     * @param dst_ip 规则中的目的IP地址 不能为any
     * @param src_ip 规则中的源IP地址  不能为any
     * @param dst_port 规则中的目的端口地址
     * @param src_port 规则中的源端口地址
     * @param add_delete 添加规则或者删除规则(值为true时添加规则)
     * @return
     */
    void ChangeWhiteLists(const std::string& dst_ip, const std::string& src_ip,
                          const std::string& dst_port,const std::string& src_port,bool add_delete);
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

#endif // CONFIGWHITELISTS_H
