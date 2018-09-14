#ifndef HANDLE_DATA_H
#define HANDLE_DATA_H

#include <stdio.h>
#include <memory.h>
#include <stdlib.h>
#include <string>

#include "MyDateUtil.h"
using namespace std;

class PhysicsServerFormat
{
public:
    // 物理服务器信息
    int cpu;
    int mem;
    int disk;

    void display()
    {
        printf("\n\nClass PhysicsServerFormat:\n");
        printf("物理服务器信息：\n");
        printf("  CPU=%d\n",cpu);
        printf("  MEM=%d\n",mem);
        printf("  DISK=%d\n",disk);
    }
};

class FlavorFormat
{
public:
    int id=0;
    int cpu=0;
    int mem=0;
    int disk=0;
    void display()
    {
        printf("\n\nFlavor info:\n");
        printf("  id=%d\n",id);
        printf("  cpu=%d\n",cpu);
        printf("  mem=%d\n",mem);
        printf("  disk=%d\n",disk);
    }
    static int compare(const void *a, const void *b)
    {
        FlavorFormat * flavor1 =(FlavorFormat*)a;
        FlavorFormat * flavor2 =(FlavorFormat*)b;
        return flavor1->id - flavor2->id;  //按 id 从小到大排序
    }
};

class PredictFlavorFormat
{
public:
    int num;
    FlavorFormat flavorInfoIndex[15];//按照下标对应关系存储，用于读入数据
    FlavorFormat flavorInfo[15];//往前存，与下标没有直接关系，用于后续计算
    int times[15]; //暂未用到
    int mapToId[15]; //往前存，存储的是flavorId
    int mapToIndex[15];//在对应flavorId-1的位置上，存储的是往前存时（mapToId）的下标值。两个联合可做索引
    void adjust(PhysicsServerFormat *serverFormat)
    {
        // 读入的数据放在 flavorInfoIndex 中，使用本函数，将待预测机型排序（因后续分配要从大到小分），
        // 放到flavorInfo中，以及生成另外几个数组的值。
        qsort(flavorInfoIndex,15,sizeof(FlavorFormat),FlavorFormat::compare);
        int index = 0;
        for (int i=0; i<15; ++i)
        {
            if (this->flavorInfoIndex[i].id == 0)
            {
                continue;
            }
            this->flavorInfo[index].id = this->flavorInfoIndex[i].id;
            this->flavorInfo[index].cpu = this->flavorInfoIndex[i].cpu;
            this->flavorInfo[index].mem = this->flavorInfoIndex[i].mem;
            this->mapToId[index] = this->flavorInfoIndex[i].id;
            this->mapToIndex[this->flavorInfoIndex[i].id -1] = index;
            int ts1 = serverFormat->cpu / this->flavorInfo[index].cpu;
            int ts2 = serverFormat->mem / this->flavorInfo[index].mem;
            times[index] =  ts1 <= ts2 ? ts1:ts2;

            ++index;

        }
    }
    void display()
    {
        printf("\n\nClass PredictFlavorFormat:\n");
        for (int i=0; i < this->num; ++i)
        {
            printf("i=%d,id=%d,CPU=%d,MEM=%d\n",i,this->flavorInfo[i].id,this->flavorInfo[i].cpu,this->flavorInfo[i].mem);
        }
    }
};


// input文件的信息
class InputData
{
private:
    void readPhysicsServerInfo(char * const info)
    {
        sscanf(info,"%d %d %d",&(this->physicsServerInfo.cpu),&(this->physicsServerInfo.mem),&(this->physicsServerInfo.disk));
        this->physicsServerInfo.mem *= 1024;
    }
    void readFlavorInfo(char * const info)
    {
        int flavorId,cpu,mem;
        sscanf(info,"flavor%d %d %d",&flavorId,&cpu,&mem);
        this->predictFlavorInfo.flavorInfoIndex[flavorId-1].id = flavorId;
        this->predictFlavorInfo.flavorInfoIndex[flavorId-1].cpu = cpu;
        this->predictFlavorInfo.flavorInfoIndex[flavorId-1].mem = mem;
    }
    void readPredictFlavorInfo(char ** const info)
    {
        sscanf(info[2],"%d",&(this->predictFlavorInfo.num));
        for (int i=0; i < this->predictFlavorInfo.num; ++i)
        {
            this->readFlavorInfo(info[3+i]);
        }
        this->predictFlavorInfo.adjust(&(this->physicsServerInfo));
    }
    void readOptimalizeInfo(char * const info)
    {
        char obj[4];
        sscanf(info,"%c%c%c",&obj[0],&obj[1],&obj[2]);
        obj[3] = '\0';
        this->optimalizeObject = string(obj);
    }
    void readPredictDateInfo(char ** const info)
    {
        int position = this->predictFlavorInfo.num + 6;
        int year,month,day;
        sscanf(info[position],"%d-%d-%d",&year,&month,&day);
        MyDate predictBeginDay = MyDate(year,month,day);
        sscanf(info[position + 1],"%d-%d-%d",&year,&month,&day);
        MyDate predictEndDay = MyDate(year,month,day);
        this->predictDaysLength = MyDate::interval(&predictBeginDay,&predictEndDay);
    }
    void readInputData(char ** const info)
    {
        this->readPhysicsServerInfo(info[0]);
        this->readPredictFlavorInfo(info);
        this->readOptimalizeInfo(info[this->predictFlavorInfo.num + 4]);
        this->readPredictDateInfo(info);
    }
public:
    PhysicsServerFormat physicsServerInfo = PhysicsServerFormat();
    //虚拟机
    PredictFlavorFormat predictFlavorInfo = PredictFlavorFormat();
    //优化目标
    string optimalizeObject;
    //预测时间跨度
    int predictDaysLength;
    InputData(char ** const info)
    {
        readInputData(info);
    }

    void display()
    {
        printf("\n\nClass InputData\n");
        printf("物理服务器信息:\n");
        printf("CPU=%d,MEM=%d,DISK=%d\n",this->physicsServerInfo.cpu,this->physicsServerInfo.mem,this->physicsServerInfo.disk);
        printf("待预测虚拟机信息：\n");
        printf("数量：%d\n",this->predictFlavorInfo.num);
        for (int i=0; i < this->predictFlavorInfo.num; ++i)
        {
            printf("i=%d,id=%d,CPU=%d,MEM=%d\n",i,this->predictFlavorInfo.flavorInfo[i].id,this->predictFlavorInfo.flavorInfo[i].cpu,this->predictFlavorInfo.flavorInfo[i].mem);
        }
        printf("优化目标：%s\n",this->optimalizeObject.c_str());
        printf("时间跨度：%d\n",this->predictDaysLength);
        printf("indicate:\n");
        for (int i=0; i < this->predictFlavorInfo.num ; ++i)
        {
            printf("%d  ",this->predictFlavorInfo.mapToId[i]);
        }
        printf("\n");
    }
};

// 历史数据
class HistoryData
{
private:
    int flavorNum = 15;
    int maxLength = 10000;
    void handLine(int flavorData[]);
    void readLine(char *line,int flavorData[]);
public:
//56498c56-af45	flavor15	2015-01-02 22:22:06

    int trainData[15][10000]; //按下标对应关系存放数据
    int startDays = 0;
    int historyDaysLength = 0;
    void display(int len=10);
    HistoryData(char ** const data, int data_num)
    {
        this->readTrainData(data,data_num);
    }
    void readTrainData(char ** const data, int data_num);
    int getData(int flavorId,int row)
    {
        return this->trainData[flavorId][row];
    }
};

#endif // HANDLE_DATA_H
