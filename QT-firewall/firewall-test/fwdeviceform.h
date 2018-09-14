#ifndef FWDEVICEFORM_H
#define FWDEVICEFORM_H

/*
 * 防火墙设备类(继承设备基类)
 * 函数成员：被保护设备列表的get和add方法
 * 数据成员：被保护设备列表
 * Author:于仁飞
 * Date:2018-5-02
*/

#include <string>
#include <list>
#include <algorithm>
#include "basedeviceform.h"
#include "protectdeviceform.h"

class FWDeviceForm:public BaseDeviceForm
{
public:
    //FWDeviceForm(const std::string &dev_ip, const std::string &dev_mac):BaseDeviceForm(dev_ip,dev_mac){}
    FWDeviceForm(const std::string &ip,const std::string &mac):BaseDeviceForm(ip,mac){}
    std::list<ProtectDeviceForm> get_protecDevSet() const;//获取被保护设备列表
    bool add_protecDev(const ProtectDeviceForm &dev);//添加被保护设备

    //重载关系运算符!=
//    FWDeviceForm operator !=(const FWDeviceForm& g);
private:
    std::list<ProtectDeviceForm> protecDevList;
};

#endif // FWDEVICEFORM_H
