#include "forecast.h"

int predict_data[18] = {0};     //预测各型号虚拟机数量，下表表示虚拟机型号
int predict_data1[18] = {0};    //预测各型号虚拟机数量，下表表示顺序存储序号
int predict_total = 0;          //预测虚拟机总数
void predict(char ** const info, char **data, int data_num)
{           //输入文件           //训练书记        //训练数据行数
    Server server;                      //定义服务器类
    Flavor flavor;                      //定义虚拟机类
    TrainData transdata;                //定义训练集类
    server.getnum(info);                //提取服务器数据
    flavor.get_numtype(info);           //提取虚拟机数据
    transdata.get_data(data,data_num);  //提取训练集数据

    int transpan0;
    transpan0 = Count(transdata.data[data_num-1]+1,flavor.time1);
    int transpan;
    transpan = Count(transdata.data[0]+1,transdata.data[data_num-1]+1) + 1;     //测试数据时间跨度
    cout << "测试数据时间跨度:" << transpan0 << "\t" << transpan << endl;
    cout << "数据长度：" << data_num << endl;

    int t=0;
    int start_time[360] = {0};      //每新的一天开始的行数
    int d=1;                        //下标，第几天
    for(int i=0; i<data_num-1; i++){
        t = Count(transdata.data[i]+1,transdata.data[i+1]+1);     //测试数据时间跨度
        if(t > 0){
            for(int j=0; j<t; j++){
                start_time[d] = i+1;
                d++;
            }
        }
    }
    start_time[d] = data_num;      //最后一天，无效数据
    cout << "调试数据：" << d << "\t" << transdata.data[start_time[4]][0] << "\t" << transdata.data[start_time[5]][0] << endl;
    cout << "每天开始行数:" << start_time[0] << "\t" << start_time[1] << "\t" << start_time[d-1] << "\t" << start_time[d]<< endl;


    int train_num[360][18];         //训练数据缓存，参数依次：第几天，虚拟机型号
    for(int j=0; j<transpan; j++){
        for(int i=start_time[j]; i < start_time[j+1]; i++){
            if(transdata.data[i][0]<=18){
                train_num[j][transdata.data[i][0]-1]++;
            }
        }
    }

    int train_sum0[18];
    int train_aver[18];
    for(int i=0; i<18; i++){
        for(int j=0; j<transpan; j++){
            train_sum0[i] += train_num[j][i];
        }
        train_aver[i] = train_sum0[i]/transpan;
    }

    for(int i=0; i<18; i++){
        cout << "flavor" << i+1 << ":\t";
        for(int j=0; j<transpan; j++){
            cout << train_num[j][i] << "  ";
        }
        cout << "ave:" << train_aver[i] << endl;
    }

    for(int i=0; i<18; i++){
        for(int j=1; j<transpan; j++){
            if(train_num[j][i] - train_aver[i] > 15){
                train_num[j][i] = train_aver[i] + 16;
            }else if(train_num[j][i] == 0){
                train_num[j][i] = (train_num[j-1][i] + train_num[j+1][i])/2;
            }
        }
    }
    for(int i=0; i<18; i++){
        cout << "flavor1" << i+1 << ":\t";
        for(int j=0; j<transpan; j++){
            cout << train_num[j][i] << "  ";
        }
        cout << "ave:" << train_aver[i] << endl;
    }

    int transpan1[3] = {0};
    if((transpan - flavor.tspan - flavor.tspan - flavor.tspan) >= 0 ){
        transpan1[0] = transpan - flavor.tspan - flavor.tspan - flavor.tspan;
        transpan1[1] = transpan - flavor.tspan - flavor.tspan;
        transpan1[2] = transpan - flavor.tspan;
    }else{
        transpan1[0] = 0;
        transpan1[1] = transpan/3 + 1;
        transpan1[2] = transpan - transpan1[1] - transpan1[0];
    }
    cout << "transpan:" << transpan1[0] << " " << transpan1[1] << " " << transpan1[2] << " "  << transpan << endl;

    int train_sum1[3][18] = {0};
    for(int j=0; j<15; j++){
        for(int i=transpan1[0]+1; i<=transpan1[1]; i++){
            train_sum1[0][j] += train_num[i][j];
        }
        for(int i=transpan1[1]+1; i<=transpan1[2]; i++){
            train_sum1[1][j] += train_num[i][j];
        }
        for(int i=transpan1[2]+1; i<=transpan; i++){
            train_sum1[2][j] += train_num[i][j];
        }
    }

    double a = 0.050;
    double b = 0.332;
    double c = 0.618;
    double x = 1.617;
    double y, w;
    y = 1 + double((transpan0-1)/flavor.tspan);
    w = pow(x, y);
    cout <<"w:"<< w <<"\t" << y << "\t" << transpan0 << "\t" << flavor.tspan << endl;
    for(int i=0; i < flavor.numtype; i++){
        predict_data[flavor.numFlavor[i][0]-1] = w * (a * train_sum1[0][flavor.numFlavor[i][0]-1]
                                                    + b * train_sum1[1][flavor.numFlavor[i][0]-1]
                                                    + c * train_sum1[2][flavor.numFlavor[i][0]-1])
                                                    ;

        predict_data1[i] = predict_data[flavor.numFlavor[i][0]-1];
        predict_total += predict_data[flavor.numFlavor[i][0]-1];
    }
}
