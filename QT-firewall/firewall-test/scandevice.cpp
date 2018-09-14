#include "scandevice.h"

ScanDevice::ScanDevice(QObject *parent):QCommInfo(parent)
{
    dstPort =0;
    this->setAutoClose(false);
    connect(this,SIGNAL(stoped()),this,SLOT(showResult()));
    connect(this,SIGNAL(timeout()),this,SLOT(showResult()));
}

std::list<std::string> ScanDevice::getFwMacList() const
{
    return fwMacList;
}

void ScanDevice::handResultData(const bool result, const char *commandTypeByte, const char *commandBytes, const int commandLength, const QString &srcIp, const int srcPort)
{
    if (result)
    {
        if (*commandTypeByte == *CommInfo::scanDeviceCmdTypeByte)
        {
            resultList.push_back(QString::fromStdString(std::string(commandBytes,commandLength)));
            //                QString msg = QString::fromStdString(std::string(commandBytes,commandLength));
            //                QStringList back_info_array = msg.split("&");
            //                std::string fwIP = back_info_array[0].toStdString();
            //                std::string fwMAC = back_info_array[2].toStdString();
            //                std::string devIP = back_info_array[4].toStdString();
            //                std::string devMAC = back_info_array[1].toStdString();
        }
    }
}

void ScanDevice::analyseResultList()
{
    int size = resultList.size();
    for (int i  = 0; i < size;++i)
    {
        QString msg = resultList.at(i);
        std::cout << "check info:"<< msg.toStdString() << std::endl;
        QStringList back_info_array = msg.split("&");
        std::string fwIP = back_info_array[0].toStdString();
        std::string fwMAC = back_info_array[2].toStdString();
        std::string devIP = back_info_array[4].toStdString();
        std::string devMAC = back_info_array[1].toStdString();
        //如果防火墙设备下没有接被保护设备
        if(devMAC == "")
        {
            std::cout << "防火墙未连接任何被保护设备！" << std::endl;
            if(!contains(fwMacList,fwMAC))
            {
                FWDeviceForm fwdevform(fwIP,fwMAC);
                fwdevList.push_back(fwdevform);
                fwMacList.push_back(fwMAC);
            }
        }
        if(devMAC != "")
        {
            if(contains(fwMacList,fwMAC))
            {
                for (auto &fwdev:fwdevList)
                {

                    //                    foreach (FWDeviceForm &fwdev,fwdevList) {
                    if(fwdev.getdevMAC()==fwMAC)
                    {
                        ProtectDeviceForm protectdev(devIP,devMAC);
                        fwdev.add_protecDev(protectdev);
                    }
                }
            }
            else
            {
                FWDeviceForm fwdev(fwIP,fwMAC);
                ProtectDeviceForm protectdev(devIP,devMAC);
                fwdev.add_protecDev(protectdev);
                fwdevList.push_back(fwdev);
                fwMacList.push_back(fwMAC);
            }
        }
    }
}

void ScanDevice::showResult()
{
//    this->analyseResultList();
//    std::cout << "fwdevList" << std::endl;
//    for (auto dev : fwdevList)
//    {
//        std::cout << "ip=" << dev.getdevIP() <<  "  mac=" <<dev.getdevMAC() << std::endl;
//        for (auto pro : dev.get_protecDevSet())
//        {
//            std::cout << "  protectIp=" << pro.getdevIP() << " mac=" << pro.getdevMAC() << std::endl;
//        }
//    }
//    std::cout << "fwmacList" << std::endl;
//    for (auto mac : fwMacList)
//    {
//        std::cout << "mac=" << mac <<  std::endl;
//    }
}



int ScanDevice::sendScanDeviceCmd(const std::string &startIp, const std::string &endIp, const int dstPort)
{
    QStringList startIPList = QString::fromStdString(startIp).split(QString("."));
    QStringList endIPList = QString::fromStdString(endIp).split(QString("."));

    QString unchangePart = startIPList[0]+QString(".")+startIPList[1]+QString(".")
            +startIPList[2]+QString(".");
    //qDebug()<<"unchangePart:"<<unchangePart;
    int start = startIPList[3].toInt();
    int end = endIPList[3].toInt();
    int ip_num = end -start;
    if (ip_num < 0)
    {
        std::cout << "ScanDevice::ip range is zero." << std::endl;
        return ip_num;
    }
    for(int count=0;count + start <=end;++count)
    {
        scanIP.push_back((unchangePart+QString::number(count+start,10)).toStdString());
    }
    this->dstPort = dstPort;
    if (scanIP.size() > 0)
    {
        for(auto dstIp : scanIP)
        {
            this->appendCmd(dstIp,dstPort,CommInfo::scanDeviceCmdTypeByte,ScanDevice::getScanDeviceCmd());
        }
    }
    else
    {
        std::cout << "ScanDevice::ip range is zero." << std::endl;
    }
    this->sendCmd();
    return ip_num;
}


std::list<FWDeviceForm> ScanDevice::getFwdevList() const
{
    return fwdevList;
}

