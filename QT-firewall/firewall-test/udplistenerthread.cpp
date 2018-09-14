#include "udplistenerthread.h"

#include <iostream>
using namespace std;

QString UdpListenerThread::getBindIp() const
{
    return bindIp;
}

void UdpListenerThread::setBindIp(const QString &value)
{
    bindIp = value;
}

int UdpListenerThread::getBindPort() const
{
    return bindPort;
}

void UdpListenerThread::setBindPort(int value)
{
    bindPort = value;
}

UdpListenerThread::UdpListenerThread( QObject *parent):MyQThread(parent)
{
    this->onStartFlag = false;
    this->startedFlag = false;
    this->activateTimeoutFlag = false;
    this->bindFlag = false;
//    this->onBindFlag = false;
    this->bindIp = QString();
    this->bindPort =0;
    listener = new MyQUdpSocket(nullptr);
    listener->moveToThread(this);

    timer = new MyQTimer(nullptr);
    timer->setSingleShot(true);
    timer->moveToThread(this);

    connect(this,SIGNAL(startListener(QString,quint16)),listener,SLOT(bind(QString,quint16)),Qt::QueuedConnection);
    connect(this,SIGNAL(startTimer(int)),timer,SLOT(start(int)),Qt::QueuedConnection);

    connect(this,SIGNAL(closeListener()),listener,SLOT(close()),Qt::QueuedConnection);
    connect(this,SIGNAL(stopTimer()),timer,SLOT(stop()),Qt::QueuedConnection);

    connect(timer,SIGNAL(timeout()),this,SLOT(whenTimeout()),Qt::QueuedConnection);

    connect(listener,SIGNAL(readyRead()),this,SLOT(received()),Qt::DirectConnection);
    connect(listener,SIGNAL(bindResult(bool)),this,SLOT(bindResult(bool)),Qt::QueuedConnection);

    this->setTimeoutMs(5000);

    MyQThread::start();
//    std::cout << "UdpListenerThread()" << std::endl;
}

UdpListenerThread::~UdpListenerThread()
{
    this->listener->deleteLater();
    this->timer->deleteLater();
    this->stop();
    this->quit();
    this->wait();
//    std::cout << "~UdpListenerThread()" << std::endl;
}

bool UdpListenerThread::isStarted() const
{
    return this->startedFlag;
}

bool UdpListenerThread::isOnStart() const
{
    return this->onStartFlag;
}

void UdpListenerThread::setTimeoutMs(int timeout_ms)
{
    if (this->isStarted())
    {
        std::cout << "you can't set timeout when listener have started." << std::endl;
        return;
    }
    if (timeout_ms <= 0)
    {
        this->activateTimeoutFlag = false;
    }
    else
    {
        this->activateTimeoutFlag = true;
    }
    this->timeout_ms = timeout_ms;
}

int UdpListenerThread::getTimeoutMs() const
{
    return this->timeout_ms;
}

void UdpListenerThread::setBindInfo(const QString &bindIp, const int bindPort)
{
    if (this->isStarted())
    {
        std::cout << "you can't set timeout when listener have started." << std::endl;
        return;
    }
    this->bindIp = bindIp;
    this->bindPort = bindPort;
}

bool UdpListenerThread::checkBindInfo()
{
    if (this->bindIp.isEmpty() || this->bindPort <= 0)
    {
        std::cout << "you should set bind info before you start listener." << std::endl;
        return false;
    }
    return true;
}

void UdpListenerThread::bindResult(bool result)
{
    if (!this->isStarted())
    {
        if (result)
        {
//            if (this->stopingFlag)
//            {
//                return;
//            }
            // 如果listener启动成功
            if (this->activateTimeoutFlag)
            {
                // 如果使用超时，则发送开启计时器信号。
                emit startTimer(this->timeout_ms);
            }
        }
        this->startedFlag = result;
        // 发送启动结果信号，参数为是否启动成功
        this->bindFlag = result;
        emit started(result);
//        std::cout << "UdpListenerThread()::bindResult()=" << result << "threadId=" << thread()->currentThreadId() << std::endl;
    }
    else
    {
        std::cout << "UdpListenerThread()::bindResult()=hasListened" << std::endl;
    }
//    this->onBindFlag = false;
    this->onStartFlag = false;
}

void UdpListenerThread::whenTimeout()
{
    if (this->isStarted())
    {
        emit closeListener();
        this->startedFlag = false;
        this->onStartFlag = false;
        std::cout << "UdpListenerThread()::whenTimeout()" << std::endl;
        emit timeout();
//        emit stoped();
    }
}

void UdpListenerThread::received()
{

    // std::cout << "UdpListenerThread::dataReceived():" << std::endl;
    while(listener->hasPendingDatagrams())
    {
        QNetworkDatagram datagram = listener->receiveDatagram();
//        qDebug() << datagram.data();
//        qDebug() << datagram.destinationAddress();
//        qDebug() << datagram.destinationPort();
//        qDebug() << datagram.senderAddress().toString();
//        qDebug() << datagram.senderPort();
        if (this->isStarted())
        {
            emit receivedUdpData(datagram.data(),datagram.senderAddress().toString(),datagram.senderPort());
        }

    }

}

void UdpListenerThread::start()
{
    if (this->checkBindInfo())
    {
        //        this->stopingFlag = false;
        //    this->startingFlag = true;
        if (!this->isStarted())
        {
            this->onStartFlag = true;
            //            this->onBindFlag = true;
            emit startListener(this->bindIp,this->bindPort);
            std::cout << "UdpListenerThread()::start()" << std::endl;
        }
        else
        {
            this->onStartFlag = false;
            emit started(false);
            std::cout << "UdpListenerThread()::start()=hasListened" << std::endl;
        }
    }
    else
    {
        emit started(false);
    }
}

void UdpListenerThread::start(const QString &bindIp, const int bindPort, int timeout_ms)
{
    this->setBindInfo(bindIp,bindPort);
    this->start(timeout_ms);
}

void UdpListenerThread::start(int timeout_ms)
{
    this->setTimeoutMs(timeout_ms);
    this->start();

}

void UdpListenerThread::stop()
{
//    this->stopingFlag = true;
    if (this->isStarted())
    {
        emit closeListener();
        emit stopTimer();
        this->startedFlag = false;
        this->onStartFlag = false;
        emit stoped();
    }
    std::cout << "UdpListenerThread()::stop()" << std::endl;
}

void UdpListenerThread::deleteLater()
{
//    std::cout << "UdpListenerThread()::deleteLater()"  << std::endl;
    this->~UdpListenerThread();
}

void UdpListenerThread::run()
{
//    std::cout << "UdpListenerThread()::run()" << "threadId=" << thread()->currentThreadId()  << std::endl;
    exec();
}
