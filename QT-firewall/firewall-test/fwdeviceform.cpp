#include "fwdeviceform.h"

/*
 * 获取被保护设备集合
*/
std::list<ProtectDeviceForm> FWDeviceForm::get_protecDevSet() const
{
    return protecDevList;
}

/*
 * 向集合中添加被保护设备
*/
bool FWDeviceForm::add_protecDev(const ProtectDeviceForm &dev)
{
    for(ProtectDeviceForm item : protecDevList)
    {
        if(item.getdevIP() == dev.getdevIP() && item.getdevMAC()==dev.getdevMAC())
        {
            return false;
        }
    }
    //如果列表中不存在此设备，则将设备添加至列表
    protecDevList.push_back(dev);
    return true;
}
