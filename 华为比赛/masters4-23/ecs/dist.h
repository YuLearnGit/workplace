#ifndef __DIST_H__
#define __DIST_H__

#include <string.h>
#include <vector>
#include "get_data.h"
#include "forecast.h"
#include "math.h"

#define DEBUG true

//虚拟机结构，包含虚拟机的CPU和内存大小
struct flavor_id_mem
{
public:
    int cpu = 0;
    int mem = 0;
};
/**
    预测结果来自forecast.cpp
    数据预处理
**/
class predictResult
{
public:

    predictResult(Flavor &flavor)
    {
        this->flavorinfo = flavor;
    }
    //打印预测结果
    void display()
    {
        cout<<"predict result display:"<<endl;
        for(int i =1; i<=flavorinfo.numtype; i++)
        {
            cout<<"id="<<flavorinfo.numFlavor[i-1][0]<<",num="<<predict_data1[i-1]<<endl;;
        }
    }

    void data_preprocessing();//数据预处理

private:
    Flavor flavorinfo;
    vector<flavor_id_mem> flavor_to_dist;//存放要预测的虚拟机类型的cpu和mem
};

/**
    放置类
**/
class dist
{
private:
    int flavorNum = 0;//要预测的虚拟机种类数
    int consumeCpuNum = 0;//当前放置方案的总CPU消耗量
    int consumeMemNum = 0;//当前放置方案的总MEM消耗量
    Server *serverInfo;

public:
    dist();
    void distribution(char ** const info, char **data, int data_num);
    int dist_result[3][500][18];//第一列代表服务器类型，第二列代表服务器的序号，第三列代表虚拟机类型
    int server_NUM[3];//三种服务器使用的个数
};

#endif
