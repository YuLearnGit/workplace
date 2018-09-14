#ifndef CONFIGSTDRULES_H
#define CONFIGSTDRULES_H


#include <string>
#include <memory>
#include <iostream>

#include "fileoperation.h"
#include "qcomminfo.h"
#include "tools.h"

class ConfigSTDRules: public QCommInfo
{
    Q_OBJECT
public:
    ConfigSTDRules(const std::string& devIP,const std::string& fw_mac,QObject *parent=nullptr);
    ~ConfigSTDRules();
    void ChangeStateDetect(const std::string& protocol,const std::string& srcIP,const std::string& dstIP,
                           const std::string& sport,const std::string& dport,bool addOrdel);
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

#endif // CONFIGSTDRULES_H
