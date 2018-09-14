#include "mainwindow.h"
#include <QApplication>
#include <iostream>
#include "tools.h"
#include "basedeviceform.h"
#include "fwdeviceform.h"
#include "protectdeviceform.h"
#include "configdpirules.h"

#include <string>
#include <QJsonDocument>
#include <QFile>
#include <QDir>
#include <QThread>
#include <fileoperation.h>
#include <QTimer>

#include "qcomminfo.h"
#include "udpsendthread.h"
#include "configsyslogserver.h"
#include "scandevice.h"
#include "trafficflow.h"
#include "configwhitelists.h"
#include "noipconfig.h"
#include "configapcrules.h"
#include "configcncrules.h"
#include "configstdrules.h"
#include "confignatrules.h"
#include "configprtrules.h"

using namespace std;

int main(int argc, char *argv[])
{

    QApplication a(argc, argv);
    MainWindow w;
    w.show();

    //    std::cout << "main()" << "threadId=" << w.thread()->currentThreadId() << std::endl;



    // 测试扫描设备 And_ZJ
//    ScanDevice scan(&w);
//    scan.setBindInfo(getLocal_IP(),30331);
//    scan.setTimeoutMs(6000);
//    scan.setAutoClose(false);
//    scan.sendScanDeviceCmd("192.16.10.210","192.16.10.220",22222);


//    TrafficFlow *tra = new TrafficFlow(&w);
//    QObject::connect(tra,SIGNAL(TrafficResult(QString)),&w,SLOT(showTraffic(QString)));
    // 配置syslog服务器 And_ZJ
    //    ConfigSyslogServer syslog(&w);
    //    syslog.setAutoClose(true);
    //    syslog.setBindInfo(getLocal_IP(),30333);
    //    syslog.setTimeoutMs(10000);
    //    //    SyslogServerInfo serverInfo(SyslogServerInfo::UDP,"192.16.10.254","514"); // 构造一个新的syslog服务器信息，并写入防火墙
    //    //    syslog.sendWriteServerCmd(std::string("192.16.10.8"),22222,serverInfo); // 写入syslog服务器配置
    //    syslog.sendReadServerCmd(std::string("192.16.10.8"),22222); // 读取syslog服务器配置


    // 测试定时发送功能
    //    std::string msg = "hello world";
    //    UdpSendThread *sendThread = new UdpSendThread(&w);
    //    sendThread->setDstInfo("192.16.10.8",40000);
    //    sendThread->setIntervalMs(5000);
    //    sendThread->setSendTimes(2);
    //    sendThread->setSendOnStartFlag(true);
    //    sendThread->setSendMessage(msg.c_str(),msg.length());
    //    sendThread->start();


    /*测试深度包规则，此设计不好，需重写*/

//        ConfigDPIRules testdpi("192.16.10.8","hehhehe",&w);
//        testdpi.ChangeModbusTcpRules("any","192.16.10.2","22","33","6","100","600",true,false);
        //testdpi.ChangeOPCRules("any","any",true,true);
//        testdpi.ChangeDNP3Rules("ant","any",true,true);
//    QObject::connect(&testdpi,SIGNAL(configResult(bool)),&w,SLOT(showConfigResult(bool)));

    /*测试白名单配置*/
        configwhitelists testwhl("192.16.10.214","hehhehe",&w);
        testwhl.ChangeWhiteLists("192.16.10.22","192.16.10.33","32323","64564",false);
        QObject::connect(&testwhl,SIGNAL(configResult(bool)),&w,SLOT(showConfigResult(bool)));

        NoIPConfig testIP("192.16.10.214","hehhehe",&w);
//        testIP.TransparentMode();
////        testIP.ResetFwIP("192.16.10.8");
//        QObject::connect(&testCNC,SIGNAL(configResult(bool)),&w,SLOT(showConfigResult(bool)));
    /*测试应用层规则*/
        ConfigAPCRules testAPC("192.16.10.214","hehhehe",&w);
//        testAPC.ChangeApplicationProRules("http",true);
//        testAPC.ChangeApplicationProRules("telnet",true);
//        testAPC.ChangeApplicationProRules("pop3",true);
//        testAPC.ChangeApplicationProRules("ftp",true);
//        testAPC.ChangeApplicationProRules("smtp",true);
//        testAPC.ChangeApplicationProRules("ssh",true);
//        QObject::connect(&testAPC,SIGNAL(configResult(bool)),&w,SLOT(showConfigResult(bool)));

/*测试连接数控制规则*/
        ConfigCNCRules testCNC("192.16.10.214","hehhehe",&w);
//        testCNC.ChangeConnectionControl("any","any","23232","32323",22,true);
//        QObject::connect(&testCNC,SIGNAL(configResult(bool)),&w,SLOT(showConfigResult(bool)));
    //    ProtectDeviceForm device("192.16.10.3","B4:52:7E");
    //    device.getdev_type(device.getdevMAC());
/*测试状态检测*/
        ConfigSTDRules testSTD("192.16.10.214","hehhehe",&w);
//        testSTD.ChangeStateDetect("tcp","any","any","22222","33333",false);
//        QObject::connect(&testSTD,SIGNAL(configResult(bool)),&w,SLOT(showConfigResult(bool)));
/*NAT规则*/
        ConfigNATRules testNAT("192.16.10.214","hehhehe",&w);
//        testNAT.ChangeDNAT("192.16.10.230","any","192.16.10.1","any",false);
//        testNAT.ChangeSNAT("eth0","192.16.10.33","192.16.10.34",false);
//        QObject::connect(&testNAT,SIGNAL(configResult(bool)),&w,SLOT(showConfigResult(bool)));
        /*PRT 规则*/
        ConfigPRTRules testPRT("192.16.10.8","hehehe",&w);
//        testPRT.ChangeDefaultRoute("eth3","any",true);
//        testPRT.ChangeHostRoute("192.163.1.1","eth3","any",false);
//        testPRT.ChangeNetRoute("192.168.1.1","255.255.255.0","eth2","any",true);
        QObject::connect(&testPRT,SIGNAL(configResult(bool)),&w,SLOT(showConfigResult(bool)));

    //    QJsonDocument document;
    //    document.setObject(json);
    //    QByteArray byte_array = document.toJson(QJsonDocument::Compact);
    //    QString json_str(byte_array);
    //    QDir dir;
    //    if(!dir.exists("config"))//判断需要创建的文件夹是否存在
    //    {
    //        dir.mkdir("config"); //创建文件夹
    //    }
    //    QFile file("config/hehe.txt");
    //    if(file.open(QIODevice::WriteOnly))
    //    {
    //        qDebug()<<"open file success";
    //        QTextStream in(&file);
    //        in<<json_str;
    //        file.flush();
    //        file.close();
    //    }
    return a.exec();
    //    return 0;
}


