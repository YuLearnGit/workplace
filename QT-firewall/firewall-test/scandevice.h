#ifndef SCANDEVICE_H
#define SCANDEVICE_H

#include "udplistenerthread.h"
#include "qcomminfo.h"
#include "tools.h"
#include "fwdeviceform.h"
#include "protectdeviceform.h"

#include <string>
#include <QString>
#include <list>
#include <vector>

class ScanDevice:public QCommInfo
{
    Q_OBJECT
public:
    ScanDevice(QObject *parent);
    int sendScanDeviceCmd(const std::string &startIp,const std::string &endIp,const int dstPort);
    std::list<FWDeviceForm> getFwdevList() const;
    std::list<std::string> getFwMacList() const;
    static std::string getScanDeviceCmd()
    {
        return getLocal_mac();
    }
private:
    /**
     * @brief handResultData 192.16.10.111&28:d2:44:09:b8:dd&00:0b:ab:c6:5f:c5&firedeviceConfirm&192.16.10.214
     * @param result
     * @param commandTypeByte
     * @param commandBytes
     * @param commandLength
     */
    void handResultData(const bool result,const char *commandTypeByte,const char *commandBytes,const int commandLength,const QString &srcIp,const int srcPort);
    void analyseResultList();

    std::vector<std::string> scanIP;
    int dstPort;
    std::vector<QString> resultList;
    std::list<FWDeviceForm> fwdevList;
    std::list<std::string> fwMacList;
public slots:
    void showResult();
};


#endif // SCANDEVICE_H
