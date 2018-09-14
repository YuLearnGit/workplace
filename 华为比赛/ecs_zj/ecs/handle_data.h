#ifndef HANDLE_TRAINDATA_H
#define HANDLE_TRAINDATA_H

#include <iostream>
#include <string>
#include <map>
#include <vector>

#include "lib_io.h"

using namespace std;

//input文件相关信息获取
class inputdata {
public:
    //物理服务器信息
    int serverCPU;
    int serverMEM;
    int serverDisk;
    //虚拟机
    int flavorinfo[15][3];
    int numtype;
    //要优化的对象
    string target;

    //预测的时间跨度
    int timesec;
    void handle_inputdata(char ** const info);
};

class traindata {
public:
    int flavor_data[5000][4];//列依次为：型号，年，月，日
    void hanle_traindata(char ** const data, int data_num);

};


string int2str(const int &i);


int daylength(int y,int m,int d);

//计算时间跨度
int countlength(int *firsttime,int *lasttime);
#endif // HANDLE_TRAINDATA_H
