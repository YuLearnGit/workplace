#include "trafficflow.h"

TrafficFlow::TrafficFlow(QObject *parent=nullptr):QObject(parent)
{
    listenTraffic = new UdpListenerThread(this);
    listenTraffic->setBindInfo(getLocal_IP(),44444);
    listenTraffic->setTimeoutMs(0);
    connect(listenTraffic,SIGNAL(receivedUdpData(QByteArray,QString,int)),this,SLOT(handleTraffic(QByteArray)));
    listenTraffic->start();
}

void TrafficFlow::handleTraffic(QByteArray data)
{
    //std::cout<<data.toStdString()<<std::endl;
    QString QdataStr(data);
    QdataStr = QdataStr.simplified();
    //qDebug()<<QdataStr;
    QStringList data_list = QdataStr.split(QString(" "));

    emit TrafficResult(QdataStr);
}
