#ifndef OUTPUT_H
#define OUTPUT_H
#include "PlaceFlavor.h"

class OutputUtils
{
public:
    PlaceManager *placeManager;

    OutputUtils(PlaceManager *placeManager)
    {
        this->placeManager = placeManager;
    }

    string getStr()
    {
        string str = "";
        char flavorTotal[20];
        sprintf(flavorTotal,"%d",this->placeManager->getFlavorSum());
        str += string(flavorTotal);
        str += "\n";
//        printf("1-%s",str.c_str());
        string predictFlavorsNumsStr = "";
        for (int i=0; i<this->placeManager->flavorNum; ++i)
        {
            char flavorNumStr[20];
            sprintf(flavorNumStr,"flavor%d %d",this->placeManager->flavorInfos->flavorInfo[i].id,this->placeManager->predictResult->nums[i]);
            predictFlavorsNumsStr += string(flavorNumStr);
            predictFlavorsNumsStr += "\n";
        }

        str += predictFlavorsNumsStr;

        char serverTotal[20];
        sprintf(serverTotal,"%d",this->placeManager->getServerSum());
        str += "\n";
        str += serverTotal;
//        str += "\n";

        string serverPlacedTotalStr = "";
        int serverId = 1;
        int i=0;
        for(list<PlaceMethod *>::iterator it = this->placeManager->methods.begin(); it != this->placeManager->methods.end(); ++ it)
        {
            string serverPlacedStr = "";
            PlaceMethod *method= (*it);

            for (int n = 0; n < this->placeManager->flavorNum; ++n)
            {
                if (method->placedNums[n] == 0)
                {
                    continue;
                }
                char flavorNumStr[20];
                sprintf(flavorNumStr," flavor%d %d",this->placeManager->flavorInfos->flavorInfo[n].id,method->placedNums[n]);
                serverPlacedStr += string(flavorNumStr);
            }
            for (int j=0; j < this->placeManager->methodsNum[i]; ++j)
            {
                char serverIdStr[5];
                sprintf(serverIdStr,"%d",serverId);
                ++ serverId;
                serverPlacedTotalStr += string("\n") + serverIdStr + serverPlacedStr ;
            }
            ++i;
        }
        str += serverPlacedTotalStr;
        return str;
    }
};

#endif // OUTPUT_H
