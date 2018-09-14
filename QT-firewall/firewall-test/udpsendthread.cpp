
#include "udpsendthread.h"

UdpSendThread::UdpSendThread(QObject *parent):QThread(parent)
{
    timer = new QTimer(nullptr);
    timer->setSingleShot(false);
    this->setIntervalMs(5000);
    this->setSendTimes(1);
    this->startedFlag = false;
    this->sendMessage = nullptr;
    this->sendLength = 0;
    this->dstIp = QString();
    this->dstPort = 0;
    this->sendOnstartFlag = true;
    connect(this,SIGNAL(startTimer(int)),timer,SLOT(start(int)),Qt::QueuedConnection);
    connect(this,SIGNAL(stopTimer()),timer,SLOT(stop()),Qt::QueuedConnection);
    connect(timer,SIGNAL(timeout()),this,SLOT(whenTimeout()),Qt::QueuedConnection);

    timer->moveToThread(this);

    QThread::start();
    std::cout << "UdpSendThread()" << std::endl;
}

UdpSendThread::~UdpSendThread()
{
    this->stop();
    this->timer->deleteLater();
    this->quit();
    this->wait();
    DELETE_ARRAY(this->sendMessage);
    std::cout << "~UdpSendThread()" << std::endl;
}

void UdpSendThread::setIntervalMs(int interval_ms)
{
    if (interval_ms <= 0)
    {
        std::cout << "you should set a positive value to interval_ms." << std::endl;
        return;
    }
    this->interval_ms = interval_ms;
}

int UdpSendThread::getIntervalMs() const
{
    return this->interval_ms;
}

int UdpSendThread::getSendTimes() const
{
    return sendTimes;
}

void UdpSendThread::setSendTimes(int value)
{
    if (this->isStarted())
    {
        std::cout << "cannot change sendTimes when started" << std::endl;
        return;
    }
    if (value == 0)
    {
        std::cout << "set 0 to sendTime, it will doesn't work, so set other number,please." << std::endl;
        return;
    }
    this->sendTimes = value;
    this->remainSendTimes = value;
}

int UdpSendThread::getRemainSendTimes() const
{
    return this->remainSendTimes;
}

bool UdpSendThread::isStarted() const
{
    return this->startedFlag;
}

void UdpSendThread::setSendMessage(const char *msg, int msgLen)
{
    if (this->isStarted())
    {
        std::cout << "cannot set sendMessage when started." << std::endl;
        return;
    }
    DELETE_ARRAY(this->sendMessage);
    this->sendLength = msgLen;
    this->sendMessage = new char[this->sendLength + 1];
    this->sendMessage[this->sendLength] = '\0';
    CommInfo::copyBytes(this->sendMessage,msg,this->sendLength);

}

void UdpSendThread::setDstInfo(QString dstIp, int dstPort)
{
    if (this->isStarted())
    {
        std::cout << "cannot set dstInfo when started." << std::endl;
        return;
    }
    this->dstIp = dstIp;
    this->dstPort = dstPort;
}

bool UdpSendThread::checkDstInfo() const
{
    if (this->dstIp.isEmpty() || this->dstPort <= 0)
    {
        std::cout << "you should set dst info before you start." << std::endl;
        return false;
    }
    return true;
}

bool UdpSendThread::checkSendMsg()
{
    if (this->sendMessage == nullptr || this->sendLength == 0)
    {
        std::cout << "you should set sendMessage before you start." << std::endl;
        return false;
    }
    return true;
}

void UdpSendThread::setSendOnStartFlag(bool immediately)
{
    if (this->isStarted())
    {
        std::cout << "cannot set sendOnStart when started." << std::endl;
        return;
    }
    this->sendOnstartFlag = immediately;
}

bool UdpSendThread::getSendOnStartFlag() const
{
    return this->sendOnstartFlag;
}

void UdpSendThread::whenTimeout()
{
    if (this->isStarted())
    {
        QCommSend::send(this->dstIp.toStdString(),this->dstPort,this->sendMessage,this->sendLength);
        emit sended();
        std::cout << "UdpSendThread()::whenTimeout()=" << this->getRemainSendTimes() << std::endl;

        if (this->getRemainSendTimes() > 0)
        {
            --this->remainSendTimes;
        }
        if (this->getRemainSendTimes() == 0)
        {
            this->stop();
        }
        // 当剩余次数为负数时，无限次发送
        emit timeout();
    }
}

void UdpSendThread::start()
{
    if (!checkDstInfo() || !checkSendMsg())
    {
        emit started(false);
    }
    else
    {
        if (!this->isStarted())
        {
            if (this->getSendTimes() == 1 && this->getSendOnStartFlag()) // 如果，设置了启动时发送，并且只发送一次，则不启动定时器，而是直接发出去。
            {
                QCommSend::send(this->dstIp.toStdString(),this->dstPort,this->sendMessage,this->sendLength);
                emit sended();
                return;
            }

            this->startedFlag = true;
            emit started(true);
            if (this->getSendOnStartFlag())
            {
                if (this->getSendTimes() > 0)
                {
                    --this->remainSendTimes;
                }
                QCommSend::send(this->dstIp.toStdString(),this->dstPort,this->sendMessage,this->sendLength);
                emit sended();
            }
            emit startTimer(this->interval_ms);
            std::cout << "UdpSendThread()::start()" << std::endl;
        }
        else
        {
            emit started(false);
            std::cout << "UdpSendThread()::start()=hasStarted" << std::endl;
        }
    }
}

void UdpSendThread::start(const QString &dstIp, const int dstPort, const char *msg, const int msgLength)
{
    this->setDstInfo(dstIp,dstPort);
    this->setSendMessage(msg,msgLength);
    this->start();
}

void UdpSendThread::start(const int interval_ms, const int sendTimes)
{
    this->setIntervalMs(interval_ms);
    this->setSendTimes(sendTimes);
    this->start();
}

void UdpSendThread::start(const int interval_ms)
{
    this->setIntervalMs(interval_ms);
    this->start();
}

void UdpSendThread::stop()
{
    if (this->isStarted())
    {
        emit stopTimer();
        this->remainSendTimes = this->sendTimes;
        this->startedFlag = false;
        emit stoped();
    }
    std::cout << "UdpSendThread()::stop()" << std::endl;
}


void UdpSendThread::run()
{
    std::cout << "UdpSendThread()::run()" << "threadId=" << thread()->currentThreadId()  << std::endl;
    exec();
}


