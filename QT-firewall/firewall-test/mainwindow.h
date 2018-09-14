#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QProgressDialog>
#include <QPushButton>
#include <QMessageBox>

#include "udplistenerthread.h"
namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    UdpListenerThread *listener;
    QProgressDialog *dialog;
    QPushButton *okBtn;
    QMessageBox *hintDialog;
private slots:

    void on_sendMsgBtn_clicked();

    void gotMsg(QByteArray msg);

    void timeout();

    void test();

    void showTraffic(QString data);

    void showConfigResult(bool result);
private:
    Ui::MainWindow *ui;
};

#endif // MAINWINDOW_H
