#include <iostream>
#include <string>
#include <map>
#include <sstream>
#include <vector>

#include <stdio.h>
#include <stdlib.h>
#include <string.h>


#include "handle_data.h"
#include "lib_io.h"

#define DEBUG 0

using namespace std;

string int2str(const int &i)
{
    string str;
    stringstream stream;
    stream << i;
    str = stream.str();
    return str;
}

int daylength(int y, int m, int d)
{
    int md[13] = {0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

    int cnt = y * 365;
    //下面是求瑞年二月份多一天的算法
    cnt += (y - 1) / 4 + 1;
    cnt -= (y - 1) / 100 + 1;
    cnt += (y - 1) / 400 + 1;
    //这是求这y年开始到m月d日多少天
    for (int i = 1; i < m; ++i)
        cnt += md[i];
    if ((m > 2) && ((y % 4 == 0 && y % 100 != 0) || y % 400 == 0))
        ++cnt;
    cnt += d;
    return cnt;
}

int countlength(int *firsttime, int *lasttime)
{
    return daylength(lasttime[0], lasttime[1], lasttime[2]) - daylength(firsttime[0], firsttime[1], firsttime[2]);
}

void inputdata::handle_inputdata(char ** const info)
{
    istringstream istr (info[0]);
    int a[3];
    for(int i = 0; i < 3; i++) {
        istr >> a[i];
    }
    serverCPU = a[0];
    serverMEM = a[1] * 1024;
    serverDisk = a[2];

    istringstream istr2 (info[2]);
    istr2 >> numtype;

    for(int i = 0; i < numtype; ++i) {
        istringstream istr3(info[3 + i] + 6);
        for(int j=0;j<3;j++){
            istr3>>flavorinfo[i][j];
        }
    }

    char str_object = *info[numtype+4];
    if(str_object == 'C'){
        target = "CPU";
    }else if(str_object == 'M'){
        target = "MEM";
    }

    int firsttime[3],lasttime[3];
    istringstream istrf(info[numtype+6]);
    istringstream istrl(info[numtype+7]);
    for(int i=0;i<3;++i){
        istrf>>firsttime[i];
        istrl>>lasttime[i];
    }
    firsttime[1]=-firsttime[1];
        firsttime[2]=-firsttime[2];
        lasttime[1]=-lasttime[1];
        lasttime[2]=-lasttime[2];

    timesec=countlength(firsttime,lasttime);
}

void traindata::hanle_traindata(char ** const data, int data_num)
{
    for(int i=0;i<data_num;++i){
        istringstream istr(data[i]+20);
        for(int j=0;j<4;++j){
            istr >> flavor_data[i][j];
        }
        flavor_data[i][2]= -flavor_data[i][2];
        flavor_data[i][3]= -flavor_data[i][3];
    }


}





