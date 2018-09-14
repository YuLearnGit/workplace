#include "myqt.h"

MyQThread::MyQThread(QObject *parent):QThread(parent)
{

}

MyQThread::~MyQThread()
{
//    std::cout << "~MyQThread()" << std::endl;
}



MyQTimer::MyQTimer(QObject *parent):QTimer(parent)
{

}

MyQTimer::~MyQTimer()
{
//    std::cout << "~MyQTimer()" << std::endl;
}

void MyQTimer::deleteLater()
{
//    std::cout << "MyQTimer::deleteLater()" << std::endl;
    QTimer::deleteLater();
}

void MyQTimer::start(int mesc)
{
//    std::cout << "MyQTimer::start("<< mesc <<")" << std::endl;
    QTimer::start(mesc);
}

void MyQTimer::start()
{
//    std::cout << "MyQTimer::start()" << std::endl;
    QTimer::start();
}

MyQUdpSocket::MyQUdpSocket(QObject *parent):QUdpSocket(parent)
{
//    std::cout << "MyQUdpSocket()" << std::endl;
}

MyQUdpSocket::~MyQUdpSocket()
{
//    std::cout << "~MyQUdpSocket()" << std::endl;
}

void MyQUdpSocket::close()
{
//    std::cout << "MyQUdpSocket()::close()" << std::endl;
    QUdpSocket::close();
}

void MyQUdpSocket::deleteLater()
{
//    std::cout << "MyQUdpSocket::deleteLater()" << std::endl;
    QUdpSocket::deleteLater();
}

bool MyQUdpSocket::bind(QString bindIp, quint16 bindPort)
{
//    std::cout << "MyQUdpSocket::bindAddress()=" << bindIp.toStdString() << " " << bindPort << std::endl;;
    bool result = QUdpSocket::bind(QHostAddress(bindIp),bindPort);
    emit bindResult(result);
    return result;
}

