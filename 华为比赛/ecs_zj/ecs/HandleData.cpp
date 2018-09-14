#include "HandleData.h"

void HistoryData::readLine(char * line,int flavorData[])
{
    char * f = strstr(line,"flavor");
    f = f + 6;
    sscanf(f,"%d\t%d-%d-%d",&flavorData[0],&flavorData[1],&flavorData[2],&flavorData[3]);
}

void HistoryData::handLine(int flavorData[])
{
    if (flavorData[0] > 0 && flavorData[0] < 16)
    {
        int days = MyDate::getDays(flavorData[1],flavorData[2],flavorData[3]);
        this->historyDaysLength = days - this->startDays;
        this->trainData[flavorData[0] - 1][this->historyDaysLength] = this->trainData[flavorData[0] - 1][this->historyDaysLength] + 1 ;
    }
}

void HistoryData::readTrainData(char ** const data, int data_num)
{
    //56498c56-af45	flavor15	2015-01-02 22:22:06
    this->historyDaysLength = 0;
    memset(this->trainData,0,sizeof(int) * this->flavorNum *this->maxLength);
    int flavorData[4];
    this->readLine(data[0],flavorData);
    this->startDays = MyDate::getDays(flavorData[1],flavorData[2],flavorData[3]);

    this->handLine(flavorData);
    for(int i=1; i < data_num; ++i)
    {
        this->readLine(data[i],flavorData);
        this->handLine(flavorData);
    }
    this->historyDaysLength += 1;
}


void HistoryData::display(int len)
{
    for (int i=0; i<len; ++i)
    {
        for (int j=0; j<15; ++j)
        {
            printf("%d  ",this->trainData[j][i]);
        }
        printf("\n");
    }
}
