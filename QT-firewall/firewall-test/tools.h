#ifndef TOOLS_H
#define TOOLS_H

/*
 * 功能性函数
 * Author:于仁飞
 * Date:2018-4-25
*/
/*std headers*/
#include <string>
#include <iostream>
#include <list>
#include <stdexcept>
/*QT headers*/
#include <QNetworkInterface>
#include <QList>
#include <QThread>
#include <QUdpSocket>
#include <QHostAddress>


std::string getLocal_mac();//获取本机MAC地址
QString getLocal_IP();//获取本机IP地址

bool contains(const std::list<std::string>& lhs,const std::string& rhs);


/**
 * 监听返回数据，扫描设备返回消息存入back_result列表中*
 *                                            已废弃，请使用QCommInfo通信或UdpListenerThread监听
 */
class listenThread : public QThread
{
public:
    listenThread(const int &listenPort):port(listenPort){}
    std::list<QString> back_result;
protected:
    void run();
private:
    const int port;
};
#endif // TOOLS_H
