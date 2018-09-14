#ifndef PROTECTDEVICEFORM_H
#define PROTECTDEVICEFORM_H

/*
 * 被保护设备
 * 包含设备IP地址、MAC地址、端口号的get和set方法
 * 成员函数：getdev_type,获取被保护设备的设备厂商
 * Author:于仁飞
 * Date:2018-4-25
*/
#include <string>
#include <iostream>
#include <QJsonDocument>
#include <QJsonObject>
#include <QJsonArray>
#include <QTextStream>
#include <QFile>
#include <QDir>
#include <QString>
#include <QHostAddress>
#include "basedeviceform.h"

class ProtectDeviceForm:public BaseDeviceForm
{
public:
    ProtectDeviceForm(const std::string &ip,const std::string &mac):BaseDeviceForm(ip,mac){}
    std::string getdev_type(const std::string& devmac) const;
};

#endif // PROTECTDEVICEFORM_H
