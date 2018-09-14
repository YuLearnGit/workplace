#ifndef PLACE_FLAVOR_H
#define PLACE_FLAVOR_H

#include "memory.h"
#include "HandleData.h"
#include <list>
#include <iostream>
using namespace std;


#define PRINT_DEBUG false

class PredictResult
{
// 存放各型号的预测结果

public:
//    int sum = 0;
    int nums[15]; //往前存
//    int numsIndex[15];

    PredictResult()
    {
        memset(this->nums,0,sizeof(int) * 15);
    }

    PredictResult(PredictResult *other)
    {
        memcpy(this->nums,other->nums,sizeof(int) * 15);
//        memcpy(this->numsIndex,other->numsIndex,sizeof(int) * 15);
//        this->sum = other->sum;
    }

    void display()
    {
        printf("\n\nClass PredictResult:\n");
        printf("预测结果中，每种型号的数量：\n");
        for (int i=0; i < 15; ++i)
        {
            printf("i=%d,num=%d\n",i,nums[i]);
        }
    }
};

#if PRINT_DEBUG == true
int callDepp =0; // 用于检查递归深度，可不用
#endif // PRINT_DEBUG


/**
    放置方法类：当前对象即代表当前方法各型号机子的放置数目
    一个方法对应于一台物理机的放置方法

**/
class PlaceMethod
{
private:

#if PRINT_DEBUG == true
    int deep = 0;
#endif // PRINT_DEBUG

    int flavorNum =0; // 有多少种机子要预测

    int consumeCpuNum = 0; //当前放置方案的总CPU消耗量
    int consumeMemNum = 0; //当前放置方案的总MEM消耗量

    PhysicsServerFormat *serverInfo;
    PredictFlavorFormat *flavorInfos;
    PredictResult *predictResult; // 存储的是还应放置的各型号机子数目


    /**
        放置，对第i个位置的虚拟机进行放置，放置数量为t
        代码修改了，可以去除i参数
    **/
    void place(int i,int t=1)
    {
        this->placedNums[i] += t;
        this->consumeCpuNum += this->flavorInfos->flavorInfo[i].cpu * t;
        this->consumeMemNum += this->flavorInfos->flavorInfo[i].mem * t;
        currPlaceNum = i;
    }

    /**
        撤回放置，上一个操作的反操作
    **/
    void withdrawPlace(int t = 1)
    {
        this->placedNums[currPlaceNum] -= t;
        this->consumeCpuNum -= this->flavorInfos->flavorInfo[currPlaceNum].cpu * t;
        this->consumeMemNum -= this->flavorInfos->flavorInfo[currPlaceNum].mem * t;
    }

    /**
        初始化本对象
    **/
    void init(int *placedFlavorNums,int consumeCpuNum,int consumeMemNum,int flavorNum,PhysicsServerFormat *serverInfo,PredictFlavorFormat *flavorInfos,PredictResult *predictResult)
    {
#if PRINT_DEBUG == true
        this->deep = callDepp ++;
#endif // PRINT_DEBUG
        this->placedNums = new int[flavorNum];
        memcpy(this->placedNums,placedFlavorNums, sizeof(int) * (flavorNum));
        this->consumeCpuNum = consumeCpuNum;
        this->consumeMemNum = consumeMemNum;
        this->flavorNum = flavorNum;
        this->serverInfo = serverInfo;
        this->flavorInfos = flavorInfos;
        this->predictResult = predictResult;
        this->currPlaceNum = flavorNum -1;
#if PRINT_DEBUG == true
        printf("Place callDeep=%d\n",this->deep);
#endif // PRINT_DEBUG

    }

    /**
        更新值
    **/
    void update(PlaceMethod *other)
    {
        memcpy(this->placedNums,other->placedNums, sizeof(int) * this->flavorNum);
        this->consumeCpuNum = other->consumeCpuNum;
        this->consumeMemNum = other->consumeMemNum;
        this->placedOver = other->placedOver;
    }

    /**
        检查第 i个位置，是否还可以放置（至少一个），可以：返回true
    **/
    bool check(int i)
    {
        if (this->consumeCpuNum + this->flavorInfos->flavorInfo[i].cpu > this->serverInfo->cpu ||
                this->consumeMemNum + this->flavorInfos->flavorInfo[i].mem > this->serverInfo->mem)
        {
            return false;
        }
        if (this->placedNums[i] >= this->predictResult->nums[i])
        {
            return false;
        }
        return true;
    }

    /**
        在本地测试阶段检查数据错误使用，可去除
    **/
    void checkTestError()
    {
        for (int i=0; i< flavorNum; ++i)
        {
            if (this->flavorInfos->flavorInfo[i].cpu > this->serverInfo->cpu ||
                    this->flavorInfos->flavorInfo[i].mem > this->serverInfo->mem ||
                    this->flavorInfos->flavorInfo[i].disk > this->serverInfo->disk )
            {
                printf("数据错误，虚拟机规格不能比物理机高！错误的规格：\n");
                this->flavorInfos->flavorInfo[i].display();
                this->serverInfo->display();
                exit(-1);
            }
        }
    }

    /**
        检查是否分配完毕，即还应放置的数目全为0
    **/
    bool isWorkOut()
    {
        for (int i=0; i<flavorNum; ++i)
        {
            if (predictResult->nums[i] != 0)
            {
                return false;
            }
        }
        return true;
    }


public:

    int *placedNums; // 数组，存储的是，当前放置方案，各型号的放置数目
    bool placedOver = false; // 是否放置完毕标记
    int currPlaceNum = -1; // 记号，记录的当前方法是放置的哪位位置的机子

    // 初始构造函数，不进行任何放置
    PlaceMethod(PhysicsServerFormat *serverInfo,PredictFlavorFormat *flavorInfos,PredictResult *predictResult,int flavorNum)
    {
        int *initPlacedNums = new int[flavorNum];
        for (int i=0; i < flavorNum; ++i)
        {
            initPlacedNums[i] = 0;
        }

        this->init(initPlacedNums, 0, 0,flavorNum,serverInfo,flavorInfos,predictResult);
        delete initPlacedNums;
        this->checkTestError();
        if (isWorkOut())
        {
            printf("\nError:can not happend this\n");
        }
    }

    // 构造函数，placeNum 表示此对象要对第几个位置的机子进行放置
    PlaceMethod(PlaceMethod *copyPlaceCpu,int placeNum)
    {
        this->init(copyPlaceCpu->placedNums,copyPlaceCpu->consumeCpuNum,copyPlaceCpu->consumeMemNum,copyPlaceCpu->flavorNum,
                   copyPlaceCpu->serverInfo,copyPlaceCpu->flavorInfos,copyPlaceCpu->predictResult);
//        this->place(placeNum);
        this->currPlaceNum = placeNum;
    }


    // 检查当前方法是否占用最大CPU或MEM，是：返回true
    bool occupiedAll(bool is_optimize_CPU)
    {
        if (is_optimize_CPU)
        {
            if (this->consumeCpuNum == this->serverInfo->cpu)
            {
                this->placedOver = true;
                return true;
            }
        }
        else
        {
            if (this->consumeMemNum == this->serverInfo->mem)
            {
                this->placedOver = true;
                return true;
            }
        }
        for (int i=0; i < flavorNum; ++i)
        {
            if (this->placedNums[i] != this->predictResult->nums[i])
            {
                return false;
            }
        }
        this->placedOver = true;
        return true;
    }

    // 析构函数
    ~PlaceMethod()
    {
#if PRINT_DEBUG == true
        printf("~~~~Place callDeep=%d\n",this->deep);
#endif // PRINT_DEBUG

        if (placedNums != nullptr)
        {
            delete []placedNums;
        }

        if (maxMethod != nullptr)
        {
            delete maxMethod;
            maxMethod = nullptr;
            maxMethod2 = nullptr;
        }
    }






    PlaceMethod *maxMethod = nullptr;
    PlaceMethod *maxMethod2 = nullptr;

    // 核心函数：
    // 寻找在本对象的放置方案下，id更小的机子的放置方案
    PlaceMethod *getMaxMethodInCurrCase(bool is_optimize_CPU)
    {
        if (this->occupiedAll(is_optimize_CPU))
        {
            return this;
        }
        PlaceMethod *maxMethodInCase = nullptr;
        for (int i= this->currPlaceNum - 1; i >= 0 ; --i)
        {
            if ( ! this->check(i))
            {
                continue;
            }

            PlaceMethod *tryMethod = new PlaceMethod(this,i);
            tryMethod->placeAuto(is_optimize_CPU);
            if (tryMethod->placedOver)
            {
                this->update(tryMethod);
                delete tryMethod;
                return this;
            }
            maxMethodInCase = PlaceMethod::MAX_SUE(maxMethodInCase,tryMethod,is_optimize_CPU);
        }
        return maxMethodInCase;
    }

    /**
        核心函数：递归函数
        返回在上一个（对递归而言，称上一个）配置方案的情况下，本方案的最大放置方案
        本方案的最大放置方案，由本方案生成的下一个放置方案返回
    **/
    PlaceMethod *placeAuto(bool is_optimize_CPU)
    {
        if (currPlaceNum == flavorNum - 1)
        {
            int leaveNums = this->predictResult->nums[currPlaceNum] - this->placedNums[currPlaceNum];
            int couldPlacedNum1 = (this->serverInfo->cpu - this->consumeCpuNum) / this->flavorInfos->flavorInfo[currPlaceNum].cpu;
            int couldPlacedNum2 = (this->serverInfo->mem - this->consumeMemNum) / this->flavorInfos->flavorInfo[currPlaceNum].mem;
            int couldPlacedNum = couldPlacedNum1 <= couldPlacedNum2 ? couldPlacedNum1 : couldPlacedNum2;
            int maxTimes = leaveNums <= couldPlacedNum ? leaveNums : couldPlacedNum;
            if (maxTimes == 0)
            {
                maxMethod2 = getMaxMethodInCurrCase(is_optimize_CPU);
                if (maxMethod2 == nullptr)
                {
                    printf("\nMay be work out!\n");
                    return this;
                }
                else if (maxMethod2->placedOver)
                {
                    this->update(maxMethod2);
                    return this;
                }
                maxMethod = PlaceMethod::MAX_SUE(maxMethod,maxMethod2,is_optimize_CPU);

            }
            else
            {
                for (int t = maxTimes; t >= 1; --t)
                {
                    this->place(currPlaceNum,t);
                    maxMethod2 = getMaxMethodInCurrCase(is_optimize_CPU);
                    if (maxMethod2 == nullptr)
                    {
                        this->withdrawPlace(t);
                        continue;
                    }
                    else if (maxMethod2->placedOver)
                    {
                        this->update(maxMethod2);
                        return this;
                    }
                    maxMethod = PlaceMethod::MAX_SUE(maxMethod,maxMethod2,is_optimize_CPU);

                    this->withdrawPlace(t);
                }
            }
            if (maxMethod != nullptr)
            {
                this->update(maxMethod);
            }
            else
            {
                this->place(currPlaceNum,maxTimes);
            }
            return this;
        }

        int leaveNums = this->predictResult->nums[currPlaceNum] - this->placedNums[currPlaceNum];
        int couldPlacedNum1 = (this->serverInfo->cpu - this->consumeCpuNum) / this->flavorInfos->flavorInfo[currPlaceNum].cpu;
        int couldPlacedNum2 = (this->serverInfo->mem - this->consumeMemNum) / this->flavorInfos->flavorInfo[currPlaceNum].mem;
        int couldPlacedNum = couldPlacedNum1 <= couldPlacedNum2 ? couldPlacedNum1 : couldPlacedNum2;
        int maxTimes = leaveNums <= couldPlacedNum ? leaveNums : couldPlacedNum;

        if (maxTimes == 0)
        {
            printf("Error:maxTimes==0\n");
        }
        for (int t = maxTimes; t>=1; --t)
        {
            this->place(currPlaceNum,t);

            maxMethod2 = getMaxMethodInCurrCase(is_optimize_CPU);
            if (maxMethod2 == nullptr)
            {
                this->withdrawPlace(t);
                continue;
            }
            else if (maxMethod2->placedOver)
            {
                this->update(maxMethod2);
                return this;
            }
            maxMethod = PlaceMethod::MAX_SUE(maxMethod,maxMethod2,is_optimize_CPU);
            this->withdrawPlace(t);
        }
        if (maxMethod != nullptr)
        {
            this->update(maxMethod);
        }
        else
        {
            this->place(currPlaceNum,maxTimes);
        }
        return this;
    }

    /**
        核心函数：比较函数
        比较两个放置方案谁更优
        目前按照谁放置的CPU或MEM个数多，进行比较
    **/
    static PlaceMethod *MAX_SUE(PlaceMethod *method1,PlaceMethod *method2,bool is_optimize_CPU)
    {

        if (method1 == nullptr)
        {
            return method2;
        }
        if (is_optimize_CPU)
        {

            if (method1->consumeCpuNum >= method2->consumeCpuNum)
            {
                delete method2;
                return method1;
            }
            else
            {
                delete method1;
                return method2;
            }
        }
        else
        {
//            if (method1->consumeMemNum == method2->consumeMemNum)
//            {
//                if (method1->consumeCpuNum > method2->consumeCpuNum)
//                {
//                    delete method2;
//                    return method1;
//                }
//                else
//                {
//                    delete method1;
//                    return method2;
//                }
//            }
            if (method1->consumeMemNum >= method2->consumeMemNum)
            {
                delete method2;
                return method1;
            }
            else
            {
                delete method1;
                return method2;
            }
        }

    }

    void display()
    {
        printf("\n\nClass PlaceMethod:\n");
        printf("Consume CPU=%d\n",consumeCpuNum);
        printf("Consume MEM=%d\n",consumeMemNum);
//        printf("Flavor nums=%d\n",flavorNum);
        for (int i=0; i < flavorNum; ++i)
        {
            printf("i=%d,id=%d,num=%d,CPU=%d,MEM=%d\n",i,this->flavorInfos->mapToId[i],placedNums[i],
                   this->flavorInfos->flavorInfo[i].cpu,this->flavorInfos->flavorInfo[i].mem);
        }
        printf("\n");
    }
};

/**
    放置方法管理类
    对预测结果进行物理机放置
    每个物理机，对应一个放置方案，即PlaceMethod
**/
class PlaceManager
{
public:

    int flavorNum =0; //要预测的机子种类
    PhysicsServerFormat *serverInfo;
    PredictFlavorFormat *flavorInfos;
    PredictResult *predictResult; // 总的预测结果
    bool is_optimize_CPU = true; // 是否优化CPU
    int serversSum =0;

    list<PlaceMethod *> methods = list<PlaceMethod *>(); //存放不同物理机的放置方案
    int methodsNum[1000]; // 有的放置方案，可以放多台物理机，此处记录每个方案放多少个物理机
    int placedNums[15] = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}; // 当前已放置的机子个数，往前存

    // 构造函数，初始化
    PlaceManager(PhysicsServerFormat *serverInfo,PredictFlavorFormat *flavorInfos,PredictResult *predictResult,int flavorNum)
    {
        this->flavorNum = flavorNum;
        this->serverInfo = serverInfo;
        this->flavorInfos = flavorInfos;
        this->predictResult = predictResult;
        memset(methodsNum,0,sizeof(int) * 1000);
    }

    /**
        核心函数：寻找一台物理的最优放置方案，并尝试放多台物理机
    **/
    void placeAuto(bool is_optimize_CPU)
    {
        PredictResult remainResult = PredictResult(this->predictResult);
//        for (int j=0; j<3; ++j)
        while ( ! this->isWorkOut())
        {
            PlaceMethod *method = new PlaceMethod(this->serverInfo,this->flavorInfos,&remainResult,this->flavorNum);
            method->placeAuto(is_optimize_CPU);
            this->useMethodToPlace(method);
            for (int i=0; i < flavorNum; ++i)
            {
                // 有了放置方案之后，就会减少还应放置的个数，此处存放还应放置的个数
                remainResult.nums[i] = predictResult->nums[i] - this->placedNums[i];
            }
//            remainResult.display();
#if PRINT_DEBUG == true
            callDepp = 0;
#endif // PRINT_DEBUG
        }
//        this->display();
    }

    // 所有机子放完了？是：返回true
    bool isWorkOut()
    {
        for (int i=0; i<flavorNum; i++)
        {
            if ( this->placedNums[i] < this->predictResult->nums[i] && this->placedNums[i] >=0)
            {
                return false;
            }
        }
        return true;
    }

    // 用当前找到的最优放置方案，判断此方案可放多少个物理机，并进行放置
    void useMethodToPlace(PlaceMethod *method)
    {
        int times = 10000;
        int temp = times;
        for (int i=0; i < flavorNum; ++i)
        {
            temp = 0;
            if (method->placedNums[i] != 0)
            {
                temp = (int)(this->predictResult->nums[i] - this->placedNums[i])/method->placedNums[i];
            }
            if (temp < times && temp != 0)
            {
                times = temp;
            }
        }
        this->methods.push_back(method);
        // 记录放置的物理机个数
        this->methodsNum[this->methods.size()-1] = times;

#if PRINT_DEBUG == true
        printf("times=%d\n",times);
#endif // PRINT_DEBUG
        for (int i=0; i<flavorNum; ++i)
        {
            this->placedNums[i] = this->placedNums[i] + method->placedNums[i] * times;
        }

    }

    // 析构函数
    ~PlaceManager()
    {
        int length = methods.size();
        for(int i=0; i < length; ++i )
        {
            delete  (methods.front());
            methods.pop_front();
        }
    }

    // 物理机总数目
    int getServerSum() const
    {
        int sum =0;
        int length = methods.size();
        for (int i=0; i < length; ++i )
        {
            sum += methodsNum[i];
        }
        return sum;
    }
    // 虚拟机总数目
    int getFlavorSum() const
    {
        int sum =0;
        for (int i=0; i<flavorNum; ++i)
        {
            sum += this->predictResult->nums[i];
        }
        return sum;
    }

    void display() const
    {
        printf("\n\nClass PlaceManager:\n");
        printf("虚拟机放置情况:\n");
        for (int i=0; i<flavorNum; ++i)
        {
            printf("i=%d,num=%d\n",i,this->placedNums[i]);
        }
        printf("共需要物理服务器数量：%d\n",this->getServerSum());
        printf("每个方案的数目：\n");
        for (unsigned int i=0; i<this->methods.size(); ++i)
        {
            printf("i=%d,num=%d\n",i,this->methodsNum[i]);
        }
        printf("放置方案：\n");
//        for(auto it = this->methods.begin(); it != this->methods.end(); ++ it)
//        {
//            for (int j = 0; j < this->methodsNum[i] ; ++j )
//            {
//                ++num;
//                printf("\n第%d台物理服务器放置方案:\n",num);
//                (*it)->display();
//            }
//            ++i;
//        }
    }


};



#endif // PLACE_FLAVOR_H
