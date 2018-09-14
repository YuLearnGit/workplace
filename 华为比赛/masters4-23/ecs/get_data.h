#ifndef __GET_DATA_H__
#define __GET_DATA_H__

#include "lib_io.h"
#include <stdio.h>
#include <stdlib.h>
#include <iostream>
#include <fstream>
#include <sstream>

using namespace std;

int sum(int y, int m, int d);           //计算日期总天数
int Count(int *time1, int *time2);      //计算两个日期间隔天数

class Server
{
public:
    int numtype;                //主机总类数量
    string Type[3];      //主机型号
    int data[3][3];      //主机数据，横坐标表示主机类型，纵坐标依次为CPU、内存、硬盘

    void getnum(char ** const info);
};

class Flavor
{
public:
    int numtype;                //虚拟机类别种类数量
    int numFlavor[18][3];       //横坐标表示虚拟机序号，对应型号虚拟机型号\CPU核心数\内存
    int tspan;                  //预测时间跨度
    int time1[3];
    int time2[3];
    void get_numtype(char ** const info);
};

class TrainData
{
public:
    int data[20000][4];     //traindata文件读取，依次：虚拟机序号，年，月，日
    void get_data(char **data_file, int data_num);  //参数为训练数据、数据行数
};

#endif
