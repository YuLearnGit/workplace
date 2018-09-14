/********************************************************************************
** Form generated from reading UI file 'mainwindow.ui'
**
** Created by: Qt User Interface Compiler version 5.10.1
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_MAINWINDOW_H
#define UI_MAINWINDOW_H

#include <QtCore/QVariant>
#include <QtWidgets/QAction>
#include <QtWidgets/QApplication>
#include <QtWidgets/QButtonGroup>
#include <QtWidgets/QCheckBox>
#include <QtWidgets/QHeaderView>
#include <QtWidgets/QLineEdit>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QMenuBar>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QStatusBar>
#include <QtWidgets/QTextEdit>
#include <QtWidgets/QToolBar>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_MainWindow
{
public:
    QWidget *centralWidget;
    QPushButton *startListenBtn;
    QTextEdit *showMsgEdit;
    QPushButton *stopListenBtn;
    QPushButton *sendMsgBtn;
    QLineEdit *msgEdit;
    QCheckBox *configResult;
    QMenuBar *menuBar;
    QToolBar *mainToolBar;
    QStatusBar *statusBar;

    void setupUi(QMainWindow *MainWindow)
    {
        if (MainWindow->objectName().isEmpty())
            MainWindow->setObjectName(QStringLiteral("MainWindow"));
        MainWindow->resize(985, 465);
        centralWidget = new QWidget(MainWindow);
        centralWidget->setObjectName(QStringLiteral("centralWidget"));
        startListenBtn = new QPushButton(centralWidget);
        startListenBtn->setObjectName(QStringLiteral("startListenBtn"));
        startListenBtn->setGeometry(QRect(10, 90, 75, 23));
        showMsgEdit = new QTextEdit(centralWidget);
        showMsgEdit->setObjectName(QStringLiteral("showMsgEdit"));
        showMsgEdit->setGeometry(QRect(10, 120, 881, 201));
        stopListenBtn = new QPushButton(centralWidget);
        stopListenBtn->setObjectName(QStringLiteral("stopListenBtn"));
        stopListenBtn->setGeometry(QRect(90, 90, 75, 23));
        sendMsgBtn = new QPushButton(centralWidget);
        sendMsgBtn->setObjectName(QStringLiteral("sendMsgBtn"));
        sendMsgBtn->setGeometry(QRect(140, 20, 75, 23));
        msgEdit = new QLineEdit(centralWidget);
        msgEdit->setObjectName(QStringLiteral("msgEdit"));
        msgEdit->setGeometry(QRect(20, 20, 113, 20));
        configResult = new QCheckBox(centralWidget);
        configResult->setObjectName(QStringLiteral("configResult"));
        configResult->setGeometry(QRect(370, 20, 351, 41));
        QFont font;
        font.setPointSize(12);
        configResult->setFont(font);
        MainWindow->setCentralWidget(centralWidget);
        menuBar = new QMenuBar(MainWindow);
        menuBar->setObjectName(QStringLiteral("menuBar"));
        menuBar->setGeometry(QRect(0, 0, 985, 22));
        MainWindow->setMenuBar(menuBar);
        mainToolBar = new QToolBar(MainWindow);
        mainToolBar->setObjectName(QStringLiteral("mainToolBar"));
        MainWindow->addToolBar(Qt::TopToolBarArea, mainToolBar);
        statusBar = new QStatusBar(MainWindow);
        statusBar->setObjectName(QStringLiteral("statusBar"));
        MainWindow->setStatusBar(statusBar);

        retranslateUi(MainWindow);

        QMetaObject::connectSlotsByName(MainWindow);
    } // setupUi

    void retranslateUi(QMainWindow *MainWindow)
    {
        MainWindow->setWindowTitle(QApplication::translate("MainWindow", "MainWindow", nullptr));
        startListenBtn->setText(QApplication::translate("MainWindow", "\345\274\200\345\247\213\347\233\221\345\220\254", nullptr));
        stopListenBtn->setText(QApplication::translate("MainWindow", "\345\201\234\346\255\242\347\233\221\345\220\254", nullptr));
        sendMsgBtn->setText(QApplication::translate("MainWindow", "\345\217\221\351\200\201\346\266\210\346\201\257", nullptr));
        configResult->setText(QApplication::translate("MainWindow", "\345\213\276\351\200\211\350\241\250\346\230\216\350\247\204\345\210\231\351\205\215\347\275\256\350\277\224\345\233\236\344\277\241\345\217\267true", nullptr));
    } // retranslateUi

};

namespace Ui {
    class MainWindow: public Ui_MainWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MAINWINDOW_H
