#ifndef TRAFFICFLOW_H
#define TRAFFICFLOW_H

#include <iostream>
#include <string>

#include <QByteArray>
#include <QObject>
#include <QJsonObject>
#include <QJsonDocument>
#include <QRegExp>

#include "tools.h"
#include "udplistenerthread.h"

/**
 * @brief The TrafficFlow class
 * 流量统计类,发送信号TrafficResult
 * 结果为QString,以空格区分，区分后长度为11，分别为Time,  eth0:in,eth0:out;  eth1:in,eth1:out;  eth2:in,eth2:out
 * eth3:in,eth3:out,  Total:in,Total:out
 * Author：于仁飞
 */

class TrafficFlow:public QObject
{
    Q_OBJECT
public:
    TrafficFlow(QObject *parent);
    UdpListenerThread *listenTraffic;
signals:
    /**
     * @brief TrafficResult 流量统计结果信号
     * @param handled_data 处理后的流量统计结果
     * 调用handleTraffic()函数后，会出发信号，handled_data为处理结果
     */
    void TrafficResult(QString handled_data);
private slots:
    void handleTraffic(QByteArray data);
};

#endif // TRAFFICFLOW_H
