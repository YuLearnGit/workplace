#ifndef __FORECAST_H__
#define __FORECAST_H__

#include "get_data.h"
#include <string.h>
#include "math.h"

using namespace std;

extern int predict_data[18];     //预测各型号虚拟机数量，下表表示虚拟机型号
extern int predict_data1[18];    //预测各型号虚拟机数量，下表表示顺序存储序号
extern int predict_total;        //预测虚拟机总数
void predict(char ** const info, char **data, int data_num);

#endif
