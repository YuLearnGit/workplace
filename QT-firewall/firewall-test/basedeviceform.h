#ifndef BASEDEVICEFORM_H
#define BASEDEVICEFORM_H
/*
 * 设备基类
 * 包含设备IP地址、MAC地址、端口号的get和set方法
 * Author:于仁飞
 * Date:2018-4-25
*/
#include <string>

class BaseDeviceForm
{
public:
    BaseDeviceForm(const std::string &dev_ip,const int dev_port):devIP(dev_ip),devPort(dev_port){}
    BaseDeviceForm(const std::string &dev_ip,const std::string &dev_mac):devIP(dev_ip),devMAC(dev_mac){}
    BaseDeviceForm(const std::string &dev_ip,const std::string &dev_mac,const int &dev_port):
        devIP(dev_ip),devMAC(dev_mac),devPort(dev_port){}
    //get 方法
    std::string getdevIP() const;
    std::string getdevMAC() const;
    int getdevPORT() const;

    //set 方法
    void setdevIP(const std::string &newIP);
    void setdevMAC(const std::string &newMAC);
    void setdevPORT(const int &newPORT);
protected:
    std::string devIP;
    std::string devMAC;
    int devPort;
};

#endif // BASEDEVICEFORM_H
