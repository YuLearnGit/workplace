#ifndef CONFIGDPIRULES_H
#define CONFIGDPIRULES_H

/*
 * DPI规则配置类
 * 构造函数：dev为被保护设备或者防火墙设备类，fw_mac为规则配置的防火墙MAC地址
 * 成员函数：参数若传入字符串any则代表此项值为任意（仅对IP地址有效）,各参数禁止为空
 * 发射信号ConfigResult（bool result）
 * Author：于仁飞
 * Date：2018-5-8
*/
#include <string>
#include <memory>
#include <iostream>

#include "fileoperation.h"
#include "qcomminfo.h"
#include "tools.h"
class ConfigDPIRules:public QCommInfo
{
    Q_OBJECT
public:
    ConfigDPIRules(const std::string& devIP,const std::string& fw_mac,QObject *parent);
    ~ConfigDPIRules();
    // <summary>
    // 修改modbusTcp规则
    // </summary>
    // <param name="dst_IP">modbusTcp规则中的目的地址,即从机IP</param>
    // <param name="src_startIP">modbusTcp规则中源地址范围的起始地址</param>
    // <param name="src_endIP">modbusTcp规则中源地址范围的结束地址</param>
    // <param name="min_addr">modbusTcp规则中的最小线圈地址</param>
    // <param name="max_addr">modbusTcp规则中的最大线圈地址</param>
    // <param name="func">modbusTcp规则中允许通过的功能码，该字符串数组元素只能表示为十进制形式，如“01”“33”</param>
    // <param name="log_record">是否记录日志</param>
    // <param name="add_delete">添加规则或者删除规则(值为true时添加规则)</param>
    void ChangeModbusTcpRules(const std::string& dst_IP, const std::string& src_IP,
                              const std::string& min_addr,
                              const std::string& max_addr,
                              const std::string& func,
                              const std::string& min_data,
                              const std::string& max_data,bool log_record,bool add_delete);

    // <summary>
    // 修改OPC规则
    // </summary>
    // <param name="dst_IP">OPC规则中的目的地址</param>
    // <param name="src_IP">OPC规则中的源地址</param>
    // <param name="log_record">是否记录日志</param>
    // <param name="add_delete">添加规则或者删除规则(值为true时添加规则)</param>
    void ChangeOPCRules(const std::string& dst_IP, const std::string& src_IP, bool log_record,bool add_delete);

    // <summary>
    // 修改DNP3规则
    // </summary>
    // <param name="dst_IP">DNP3规则中的目的地址</param>
    // <param name="src_IP">DNP3规则中的源地址</param>
    // <param name="log_record">是否记录日志</param>
    // <param name="add_delete">添加规则或者删除规则(值为true时添加规则)</param>
    void ChangeDNP3Rules(const std::string& dst_IP,const std::string& src_IP, bool log_record, bool add_delete);

    // <summary>
    // 清除所有DPI规则
    // </summary>
    // <param name="dev_IP">防火墙设备IP地址</param>
//    bool ClearAllDPIRules(std::string& dev_IP);
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

#endif // CONFIGDPIRULES_H
