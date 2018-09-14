#ifndef HANDLE_DATA_H
#define HANDLE_DATA_H

#include <stdio.h>
#include <memory.h>
#include <stdlib.h>
#include <string>
#include "get_data.h"
#include "forecast.h"
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
    FlavorFormat flavorInfoIndex[18];//按照下标对应关系存储，用于读入数据
    FlavorFormat flavorInfo[18];//往前存，与下标没有直接关系，用于后续计算
    PredictFlavorFormat(int num)
    {
        this->num = num;
    }
    void adjust()
    {
        // 读入的数据放在 flavorInfoIndex 中，使用本函数，将待预测机型排序（因后续分配要从大到小分），
        // 放到flavorInfo中，以及生成另外几个数组的值。
        qsort(flavorInfoIndex,this->num,sizeof(FlavorFormat),FlavorFormat::compare);

        int index = 0;
        for (int i=0; i<this->num; ++i)
        {
            if (this->flavorInfoIndex[i].id == 0)
            {
                continue;
            }
            this->flavorInfo[index].id = this->flavorInfoIndex[i].id;
            this->flavorInfo[index].cpu = this->flavorInfoIndex[i].cpu;
            this->flavorInfo[index].mem = this->flavorInfoIndex[i].mem;
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

#endif // HANDLE_DATA_H
