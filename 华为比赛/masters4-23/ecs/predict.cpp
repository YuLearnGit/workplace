#include "predict.h"
#include "get_data.h"
#include "forecast.h"
#include <string.h>
#include <vector>
//#include "place.h"
#include <stdio.h>
#include "PlaceFlavor.h"


using namespace std;


//你要完成的功能总入口
void predict_server(char * info[MAX_INFO_NUM], char * data[MAX_DATA_NUM], int data_num, char * filename)
{
    Server server;                      //定义服务器类
    Flavor flavor;                      //定义虚拟机类
    TrainData transdata;                //定义训练集类
    server.getnum(info);                //提取服务器数据
    flavor.get_numtype(info);           //提取虚拟机数据
    transdata.get_data(data,data_num);  //提取训练集数据

    predict(info,data,data_num);

    //以下代码给虚拟机型号分类后放置
    int classflavor_num[3] = {0,0,0};
    vector<int> classflavor_type[3];
    for(int i=0; i<flavor.numtype; ++i)
    {
        switch(flavor.numFlavor[i][0])
        {
        //选择General
        case 2:
        case 5:
        case 8:
        case 11:
        case 14:
        case 17:
            classflavor_num[0]++;
            classflavor_type[0].push_back(i);
            break;
        //选择Large-Memory
        case 3:
        case 6:
        case 9:
        case 12:
        case 15:
        case 18:
            classflavor_num[1]++;
            classflavor_type[1].push_back(i);
            break;
        //选择High-Performance
        case 1:
        case 4:
        case 7:
        case 10:
        case 13:
        case 16:
            classflavor_num[2]++;
            classflavor_type[2].push_back(i);
            break;
        }
    }
//    for(int i=0;i<classflavor_num[2];++i){
//        cout<<"num= "<<classflavor_num[2]<<"id ="<<classflavor_type[2][i]<<endl;
//    }

    //    文件输出：
    string str_out;
    char buf[10];
    sprintf(buf,"%d",predict_total);           //输出虚拟机总数量
    str_out = str_out + buf;

    str_out = str_out + "\n";
    for(int i=0; i<flavor.numtype; i++)
    {
        str_out = str_out + "flavor";
        sprintf(buf,"%d",flavor.numFlavor[i][0]);   //输出虚拟机型号
        str_out = str_out + buf + " ";
        sprintf(buf,"%d",predict_data[flavor.numFlavor[i][0]-1]);              //输出对应型号的数量
        str_out = str_out + buf + "\n";
    }

    for(int m = 0; m<3; m++)
    {
             string allServerPlacedTotalStr = "";

        PredictFlavorFormat flavorFormat = PredictFlavorFormat(classflavor_num[m]);
        if(classflavor_num[m]!=0)
        {
            for(int i=0; i<classflavor_num[m]; ++i)
            {
                if(!classflavor_type[m].empty())
                {
                    flavorFormat.flavorInfoIndex[i].id = flavor.numFlavor[classflavor_type[m][i]][0];
                    flavorFormat.flavorInfoIndex[i].cpu = flavor.numFlavor[classflavor_type[m][i]][1];
                    flavorFormat.flavorInfoIndex[i].mem = flavor.numFlavor[classflavor_type[m][i]][2];
                }
            }

            flavorFormat.adjust();
            //flavorFormat.display();
            PredictResult predictRst = PredictResult();

            for(int i=0; i<classflavor_num[m]; ++i)
            {
                predictRst.nums[i] = predict_data[flavorFormat.flavorInfo[i].id-1];
                //printf("i=%d,id=%d,num=%d,predict_data=%d\n",i,flavorFormat.flavorInfo[i].id,predictRst.nums[i],predict_data[i]);
            }

            PhysicsServerFormat serverFormat = PhysicsServerFormat();
            int chooseServerType = m;
            //主机信息输入
            serverFormat.cpu = server.data[chooseServerType][0];
            serverFormat.mem = server.data[chooseServerType][1];
            serverFormat.disk = server.data[chooseServerType][2];
            serverFormat.display();

            PlaceManager placeManager = PlaceManager(&serverFormat,&flavorFormat,&predictRst,classflavor_num[m]);
            //进行分配
            placeManager.placeAuto();
//        str_out = str_out + "\n";
            char serverTotal[20];
            string serverPlacedTotalStr = "";
            string type = server.Type[chooseServerType];
            if(placeManager.getServerSum()!=0)
            {
                sprintf(serverTotal,"%d",placeManager.getServerSum());
                str_out +=string("\n")+type +" "+serverTotal;
            }
            int serverId = 1;
            int i=0;
            for(list<PlaceMethod *>::iterator it = placeManager.methods.begin(); it != placeManager.methods.end(); ++ it)
            {
                string serverPlacedStr = "";
                PlaceMethod *method= (*it);

                for (int n = 0; n < placeManager.flavorNum; ++n)
                {
                    if (method->placedNums[n] == 0)
                    {
                        continue;
                    }
                    char flavorNumStr[20];
                    sprintf(flavorNumStr," flavor%d %d",placeManager.flavorInfos->flavorInfo[n].id,method->placedNums[n]);
                    serverPlacedStr += string(flavorNumStr);
                }
                for (int j=0; j < placeManager.methodsNum[i]; ++j)
                {
                    char serverIdStr[5];
                    sprintf(serverIdStr,"%d",serverId);
                    ++ serverId;
                    serverPlacedTotalStr += string("\n") +type+"-"+serverIdStr + serverPlacedStr ;
                }
                ++i;
            }
              allServerPlacedTotalStr += serverPlacedTotalStr += string("\n");
        }
        str_out += allServerPlacedTotalStr;
    }




    cout << "output:" << endl;
    cout << str_out << endl;

    char str1[20000];
    strcpy(str1, str_out.c_str());

    //需要输出的内容
    char * result_file = (char *)str1;


    // 直接调用输出文件的方法输出到指定文件中(ps请注意格式的正确性，如果有解，第一行只有一个数据；第二行为空；第三行开始才是具体的数据，数据之间用一个空格分隔开)
    write_result(result_file, filename);
}



