#ifndef FORECAST_H_INCLUDED
#define FORECAST_H_INCLUDED

#include <string.h>
#include <vector>
#include <stdlib.h>
#include <time.h>
#include <math.h>
#include <cstdio>

#include "handle_data.h"


using namespace std;

extern int predict_flavor[15];//预测的虚拟机类型和数量，数组下标表示型号
extern int predict_flavor1[15];
extern int predict_flavor_totalnum;//预测出的虚拟机总数

void forecast(char ** const info,char ** data,int data_num);
#endif // FORECAST_H_INCLUDED
