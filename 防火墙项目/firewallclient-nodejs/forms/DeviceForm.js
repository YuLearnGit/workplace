module.exports=DeviceForm;

/**
 * 普通设备对象
 * function：用于获取或者设置设备IP和设备端口
 * Author：于仁飞
 * @param {*设备IP地址} dev_IP 
 * @param {*设备端口信息或者MAC地址} dev_info 
 */

function DeviceForm(dev_IP,dev_info){
   this.dev_IP=dev_IP;//设备IP
   this.dev_info=dev_info;//端口或者MAC地址


this.getDev_IP=function(){
    console.log(dev_IP);
    return dev_IP;
};

this.setDev_IP=function(dev_IP){
    this.dev_IP=dev_IP;
    console.log(dev_IP);
};

this.getDev_info=function(){
    console.log(dev_info);
    return dev_info;
};

this.setDev_info=function(dev_info){
    this.dev_info=dev_info;
    console.log(dev_info);
};
}

