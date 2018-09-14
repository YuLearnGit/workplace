#include "mainwindow.h"
#include "ui_mainwindow.h"

#include "tools.h"
#include <QProgressDialog>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    dialog = new QProgressDialog(this);
    dialog->setMinimumDuration(0);
    dialog->reset();
    dialog->setMinimum(0);
    dialog->setMaximum(100);
    dialog->setLabelText(tr("请等待配置完成"));
    dialog->setWindowTitle(tr("正在配置"));
    dialog->setCancelButtonText(nullptr);
    dialog->setWindowModality(Qt::WindowModal);


    hintDialog = new QMessageBox(this);
//    hintDialog->adjustSize();

    okBtn = new QPushButton(hintDialog);
    okBtn->setText("确定");

//    connect(okBtn,SIGNAL(clicked(bool)),hintDialog,SLOT(close()));
    hintDialog->addButton(okBtn,QMessageBox::AcceptRole);

    ui->setupUi(this);
//    listener = new UdpListenerThread(this);
//    listener->setBindInfo(getLocal_IP(),33003);
//    listener->setTimeoutMs(10000);
//    QObject::connect(listener,SIGNAL(timeout()),this,SLOT(timeout()));
//    QObject::connect(listener,SIGNAL(receivedUdpData(QByteArray)),this,SLOT(gotMsg(QByteArray)));
//    QObject::connect(ui->startListenBtn,SIGNAL(clicked(bool)),listener,SLOT(start()));
//    QObject::connect(listener,SIGNAL(started(bool)),dialog,SLOT(setVisible(bool)));

//    QObject::connect(ui->stopListenBtn,SIGNAL(clicked(bool)),listener,SLOT(stop()));

}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_sendMsgBtn_clicked()
{
    QString msg = ui->msgEdit->text();
    if (!msg.isEmpty())
    {
//        UdpSend::sendConfigInfo(getLocal_IP(),60000,msg.toStdString());
//        listener->start();
    }

}

void MainWindow::gotMsg(QByteArray msg)
{

    dialog->close();
    qDebug() << "MainWindow::gotMsg()" << msg;
    ui->showMsgEdit->append(msg);

    hintDialog->setWindowTitle(tr("配置结果"));
    hintDialog->setText(QObject::tr(msg.toStdString().c_str()));
    hintDialog->show();
    hintDialog->setMinimumSize(150,80);
    hintDialog->adjustSize();
//    listener->stop();
}

void MainWindow::timeout()
{
    dialog->close();

    hintDialog->setWindowTitle(tr("配置结果"));
    hintDialog->setText(tr("配置失败"));
    hintDialog->show();
    hintDialog->setMinimumSize(150,80);
    hintDialog->adjustSize();


}

void MainWindow::test()
{
    std::cout << "MainWindow::test()" << std::endl;
}

void MainWindow::showTraffic(QString data)
{
    ui->showMsgEdit->append(data);
}

void MainWindow::showConfigResult(bool result)
{
    ui->configResult->setChecked(result);
}

