#include "get_data.h"

using namespace std;
int server_numtype = 0;

int sum(int y, int m, int d){
    int md[13] = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
    //假设从0年开始，也就是你不知道的第一天， 比如求x-y就等于求(x-0)-（y-0）
    int cnt = y * 365;
    //下面是求瑞年二月份多一天的算法
    cnt += (y - 1) / 4 + 1;
    cnt -= (y - 1) / 100 + 1;
    cnt += (y - 1) / 400 + 1;
    //这是求这y年开始到m月d日多少天
    for (int i = 1; i < m; ++i) cnt += md[i];
    if ((m > 2) && ((y % 4 == 0 && y % 100 != 0 )|| y % 400 == 0)) ++cnt;
    cnt += d;
    return cnt;
}

int Count(int *time1, int *time2){
    return sum(time2[0],time2[1],time2[2]) - sum(time1[0],time1[1],time1[2]);
}

void Server::getnum(char ** const info)
{
    istringstream istr (info[0]);
        istr >> numtype;
    server_numtype = numtype;

    istringstream istr01 (info[1]);
        istr01 >> Type[0];
    istringstream istr02 (info[2]);
        istr02 >> Type[1];
    istringstream istr03 (info[3]);
        istr03 >> Type[2];
    istringstream istr11 (info[1]+7);
    for(int i=0; i<3; i++){
        istr11 >> data[0][i];
    }
    data[0][1] = data[0][1]*1024;
    istringstream istr12 (info[2]+12);
    for(int i=0; i<3; i++){
        istr12 >> data[1][i];
    }
    data[1][1] = data[1][1]*1024;
    istringstream istr13 (info[3]+16);
    for(int i=0; i<3; i++){
        istr13 >> data[2][i];
    }
    data[2][1] = data[2][1]*1024;

}

void Flavor::get_numtype(char ** const info)
{
    istringstream istr (info[server_numtype + 2]);
    istr >> numtype;
    for(int i=0; i<numtype; i++){
        istringstream istr1 (info[6+i]+6);
        for(int j=0;j<3;j++){
            istr1 >> numFlavor[i][j];
        }
    }

    istringstream istr1 (info[server_numtype + numtype + 4]);
    for(int i=0;i<3;i++){
        istr1 >> time1[i];
    }
    time1[1] = -time1[1];
    time1[2] = -time1[2];
    istringstream istr2 (info[server_numtype + numtype + 5]);
    for(int i=0;i<3;i++){
        istr2 >> time2[i];
    }
    time2[1] = -time2[1];
    time2[2] = -time2[2];
    tspan = Count(time1,time2) + 1;
}

void TrainData::get_data(char **data_file, int data_num)
{
    for (int i = 0; i < data_num ; i++){
        istringstream istr1 (data_file[i]+20);
        for(int j=0; j<4; j++){
            istr1 >> data[i][j];
        }
        data[i][2] = -data[i][2];
        data[i][3] = -data[i][3];
    }
}



