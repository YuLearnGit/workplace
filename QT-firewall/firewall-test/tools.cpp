#include "tools.h"
using namespace std;

//获取本机MAC地址
std::string getLocal_mac()
{
    QList<QNetworkInterface> nets = QNetworkInterface::allInterfaces();// 获取所有网络接口列表
    int nCnt = nets.count();
    QString strMacAddr = "";
    for(int i = 0; i < nCnt; i ++)
    {
        // 如果此网络接口被激活并且正在运行并且不是回环地址，则就是我们需要找的Mac地址
        if(nets[i].flags().testFlag(QNetworkInterface::IsUp) && nets[i].flags().testFlag(QNetworkInterface::IsRunning) && !nets[i].flags().testFlag(QNetworkInterface::IsLoopBack))
        {
            strMacAddr = nets[i].hardwareAddress();
            break;
        }
    }
    //qDebug()<<"MAC= "<<strMacAddr;
    std::string MACAddr = strMacAddr.toStdString();
    //cout<<"MAC= "<<MACAddr<<endl;
    return MACAddr;
}

//获取本机IP地址
QString getLocal_IP()
{
    QString strIpAddress = "";// 获取第一个有效的IPv4地址
    QList<QHostAddress> ipAddressesList = QNetworkInterface::allAddresses();
//    qDebug() << "ipAddressesList" << ipAddressesList;

    int nListSize = ipAddressesList.size();
    // 排除掉 本地链路地址 和 localhost
    quint32 nMinRange = QHostAddress("169.254.1.0").toIPv4Address();
    quint32 nMaxRange = QHostAddress("169.254.254.255").toIPv4Address();
    quint32 localHost = QHostAddress(QHostAddress::LocalHost).toIPv4Address();
    for (int i = 0; i < nListSize; ++i)
    {
        QHostAddress hostAddress = ipAddressesList.at(i);
        if (hostAddress.protocol() == QAbstractSocket::IPv4Protocol)
        {
            quint32 address = hostAddress.toIPv4Address();
//            qDebug() << address;
            if (address != localHost && (address < nMinRange || address > nMaxRange))
            {
                strIpAddress = hostAddress.toString();
                break;
            }
        }
    }
    // 如果没有找到，则以0.0.0.0为IP
    if (strIpAddress.isEmpty())
        strIpAddress = QHostAddress(QHostAddress::AnyIPv4).toString();

//    qDebug()<<"Local IP addr:"<<strIpAddress;
    return strIpAddress;
//    return QString("192.16.10.215");
}


bool contains(const std::list<std::string>& lhs,const std::string& rhs)
{
    for(auto item: lhs)
    {
        if(rhs == item)
            return true;
    }
    return false;
}

/*
 * 监听端口线程类
*/

void listenThread::run()
{
    //qDebug()<<"start listen";
    //接收返回信息
    //QUdpSocket *listener = new QUdpSocket();
    std::shared_ptr <QUdpSocket> listener = std::make_shared<QUdpSocket>();
    listener->bind(QHostAddress(getLocal_IP()),port);
//    qDebug() << listener->isOpen() << listener->isValid();
    try
    {
        while(true)
        {
            if(listener->hasPendingDatagrams())
            {
                int size = listener->pendingDatagramSize();
                char datagram[size + 1];
                listener->readDatagram(datagram,size);
                datagram[size] = '\0';
                QString msg = QString(datagram);
                if(msg != "" && msg.contains(QString("firedeviceConfirm")))
                {
                    qDebug()<<"catch back info:"<<msg;
                    back_result.push_back(msg);
                }
            }

        }
    }
    catch (std::exception)
    {
        listener->close();
    }
}
