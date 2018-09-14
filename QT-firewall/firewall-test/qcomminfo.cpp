#include "qcomminfo.h"
#include <iostream>
#include <iomanip>
#include <ios>

std::ostream & operator <<(std::ostream &out, SendInfo &info)
{

    out << "DstIp:" << info.dstIp << std::endl;
    out << "DstPort:" << info.dstPort << std::endl;
    out << "CmdTye:" << std::setiosflags(std::ios_base::hex) << info.cmdTypeByte << std::endl;
    out << "CmdStr:" << info.cmdStr << std::endl;
    return out;
}

QCommInfo::QCommInfo(QObject *parent):UdpListenerThread(parent)
{
    connect(this,SIGNAL(started(bool)),this,SLOT(whenListenerStarted(bool)));
    connect(this,SIGNAL(receivedUdpData(QByteArray,QString,int)),this,SLOT(receivedBytes(QByteArray,QString,int)));
    this->setAutoClose(true);
    this->setActivateListener(true);
    this->onSendFlag = false;
}

void QCommInfo::appendCmd(const std::string &dstIp, const int dstPort, const char *cmdTypeByte, const std::string &cmdStr)
{
    if (this->isOnSend())
    {
        std::cout << "you cannot append new cmd when is sending." << std::endl;
        return;
    }
    SendInfo info(dstIp,dstPort,cmdTypeByte,cmdStr);
    sendQueue.push(info);
}

void QCommInfo::sendCmd(const std::string &dstIp, const int dstPort, const char *cmdTypeByte, const std::string &cmdStr)
{

    this->appendCmd(dstIp,dstPort,cmdTypeByte,cmdStr);
    if (this->isActivateListener())
    {
        if (this->isOnSend())
        {
            std::cout << "cannot sendCmd() when is sending." << std::endl;
            return;
        }
        else if (!this->isStarted() || !this->isOnStart())
        {
            this->start();
        }
        else
        {
            randomNum = QCommInfo::sendCmdQueue();
        }
    }
    else
    {
        randomNum = QCommInfo::sendCmdQueue();
    }
}

void QCommInfo::sendCmd()
{
    if (this->isActivateListener())
    {
        if (!this->isStarted() && !this->isOnStart())
        {
            this->start();
        }
        else
        {
            randomNum = this->sendCmdQueue();
            return;
        }
    }
    else
    {
        randomNum = this->sendCmdQueue();
        std::cout << "random result=" <<  randomNum  << std::endl;
    }
}

int QCommInfo::sendCmdQueue()
{
    if (this->isOnSend())
    {
        std::cout << "please wait for last send." << std::endl;
        return -1;
    }
    this->onSendFlag = true;
    int randomNum = -1;
    if (!sendQueue.empty())
    {
        int size = sendQueue.size();
        SendInfo info;
        MyQUdpSocket *socket = new MyQUdpSocket(this);
        char *timeBytes = CommInfo::getTimeBytes();
        unsigned int timeNum = CommInfo::parseBytesToUint(timeBytes);
        randomNum = CommInfo::getRandomNum(timeNum);
        for (int i=0;i < size;++i)
        {
            if (sendQueue.empty())
            {
                break;
            }
            info = sendQueue.front();
            QCommSend::udpSendCmd(socket,info.dstIp,info.dstPort,timeBytes,info.cmdTypeByte,info.cmdStr);
            sendQueue.pop();
        }
        DELETE_ARRAY(timeBytes);
        socket->close();
    }
    else
    {
        std::cout << "the number of sendInfo is 0." << std::endl;
    }
    this->onSendFlag = false;
    return randomNum;
}

bool QCommInfo::isOnSend() const
{
    return this->onSendFlag;
}

void QCommInfo::setAutoClose(bool closeWhenReceived)
{
    if (this->isStarted())
    {
        std::cout << "cannot change auto close statues when listener started." << std::endl;
        return;
    }
    this->autoCloseFlag = closeWhenReceived;
}

bool QCommInfo::isAutoClose() const
{
    return this->autoCloseFlag;
}

void QCommInfo::setActivateListener(bool activate)
{
    if (this->isStarted())
    {
        std::cout << "cannot change activate listener statues when listener started." << std::endl;
        return;
    }
    this->activateListenerFlag = activate;
}

bool QCommInfo::isActivateListener() const
{
    return this->activateListenerFlag;
}

void QCommInfo::whenListenerStarted(bool result)
{
    if (!this->isActivateListener())
    {
        return;
    }
    if (result)
    {
        randomNum = this->sendCmdQueue();
        //            std::cout << "random result=" <<  randomNum  << std::endl;
    }
    else
    {
        std::cout << "listen error" << std::endl;
    }
}

void QCommInfo::receivedBytes(QByteArray dataBytes,QString srcIp,int srcPort)
{
#ifdef DEBUG_PRINT1
    std::cout << "QCommInfo::receiveBytes():" << std::endl;
    for (int i=0;i<dataBytes.size();i++)
    {
        printf("%x ",(unsigned char)dataBytes.at(i));
    }
    std::cout << std::endl;
#endif
    char *randomBytes;
    char *commandTypeByte;
    char *commandBytes;
    int commandLength;
    bool result = CommInfo::discomposeUdpData(dataBytes.data(),dataBytes.size(),&randomBytes,&commandTypeByte,&commandBytes,&commandLength);
    if (result)
    {
        if (randomNum == CommInfo::parseBytesToUint(randomBytes))
        {
            std::cout << "random check pass" << std::endl;
            // 调用虚函数处理返回结果
            this->handResultData(true,commandTypeByte,commandBytes,commandLength,srcIp,srcPort);
            //            if (*commandTypeByte == *CommInfo::iptablesCmdTypeByte)
            //            {
            //                std::cout << "CommInfo::iptablesCmdTypeByte" << std::endl;
            //                std::cout << string(commandBytes,commandLength) << endl;
            //            }

            //            else if (*commandTypeByte == *CommInfo::scanDeviceCmdTypeByte)
            //            {
            //                std::cout << "CommInfo::scanDeviceCmdTypeByte" << std::endl;
            //            }
            //            else
            //            {

            //            }
            if (this->isAutoClose())
            {
                this->stop(); // 收到认可的第一条返回数据后自动关闭
            }
        }
        else
        {
            std::cout << "randomNum=" << randomNum << std::endl;
            std::cout << "receivedRandomNum=" << CommInfo::parseBytesToUint(randomBytes) << std::endl;
        }
    }

}

void QCommInfo::handResultData(const bool result, const char *commandTypeByte, const char *commandBytes, const int commandLength, const QString &srcIp, const int srcPort)
{
    std::cout << ">> This received data printed by virtual function QCommInfo::handResultData(), if you want to hand it, please inherit class QCommInfo and implement this function:" << std::endl;
    if (result)
    {
        printf("- srcIp=%s\n",srcIp.toStdString().c_str());
        printf("- srcpPort=%d\n",srcPort);
        printf("- commandTypeByte=%#02x\n",(unsigned char)*commandTypeByte);
        printf("- commandBytes=");
        for (int i=0;i < commandLength;i++)
        {
            putchar(commandBytes[i]);
        }
        putchar('\n');
    }
    else
    {
        printf("- result false");
    }
}



int QCommSend::send(const std::string &dstIp, const int dstPort, const char *data, const int dataLength)
{
    MyQUdpSocket *client = new MyQUdpSocket();
    int num = client->writeDatagram(data,dataLength,QHostAddress(QString(dstIp.c_str())),dstPort);
    client->close();
    client->~MyQUdpSocket();
    return num;
}

int QCommSend::sendCmd(const std::string &dstIp, const int dstPort, const char *timeBytes, const char *cmdTypeByte, const std::string &cmdStr)
{
    MyQUdpSocket *client = new MyQUdpSocket();
    int num = QCommSend::udpSendCmd(client,dstIp,dstPort,timeBytes,cmdTypeByte,cmdStr);
    client->close();
    client->~MyQUdpSocket();
    return num;
}

int QCommSend::udpSend(MyQUdpSocket *udpSocket, const std::string &dstIp, const int dstPort, const char *data, const int dataLength)
{
    int num = udpSocket->writeDatagram(data,dataLength,QHostAddress(QString(dstIp.c_str())),dstPort);
    return num;
}

int QCommSend::udpSendCmd(MyQUdpSocket *udpsocket, const std::string &dstIp, const int dstPort, const char *timeBytes, const char *cmdTypeByte, const std::string &cmdStr)
{
    const char *commandBytes =cmdStr.c_str();
    int udpDataLength = 0;
    char *udpDataBytes = CommInfo::composeUdpData(timeBytes,cmdTypeByte,commandBytes,&udpDataLength);
    int num = QCommSend::udpSend(udpsocket,dstIp,dstPort,udpDataBytes,udpDataLength);
    DELETE_ARRAY(udpDataBytes);
    return num;
}
