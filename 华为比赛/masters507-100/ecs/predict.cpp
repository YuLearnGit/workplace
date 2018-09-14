#include "predict.h"
#include "get_data.h"
#include "forecast.h"
#include <string.h>
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


    PredictFlavorFormat flavorFormat = PredictFlavorFormat(flavor.numtype);
//    flavorFormat.num = flavor.numtype;

    for(int i=0;i<flavorFormat.num;++i)
    {
        flavorFormat.flavorInfoIndex[i].id = flavor.numFlavor[i][0];
        flavorFormat.flavorInfoIndex[i].cpu = flavor.numFlavor[i][1];
        flavorFormat.flavorInfoIndex[i].mem = flavor.numFlavor[i][2];
    }


    flavorFormat.adjust();
//    flavorFormat.display();
    PredictResult predictRst = PredictResult();

    for(int i=0;i<18;++i)
    {
        predictRst.nums[i] = predict_data[flavorFormat.flavorInfoIndex[i].id-1];
//        printf("i=%d,id=%d,num=%d,predict_data=%d\n",i,flavorFormat.flavorInfo[i].id,predictRst.nums[i],predict_data[i]);
    }

    int sumCpu = 0;
    int sumMem = 0;
    for(int i=0;i<18;++i)
    {
        sumCpu += predictRst.nums[i] * flavorFormat.flavorInfoIndex[i].cpu;
        sumMem += predictRst.nums[i] * flavorFormat.flavorInfoIndex[i].mem / 1024;
    }
    printf("sumCpu=%d,sumMem=%d\n",sumCpu,sumMem);
     //上述部分将预测结果通过接口，存储到了我定义的对象中，使得我可以按我定义的对象进行操作
    //下面是调用分配算法进行分配

//    int chooseServerType =0;
//    if (sumMem / (double)sumCpu >= 3.0)
//    {
//        chooseServerType = 1;
//    }
//    else if (sumMem / (double)sumCpu <= 1.8)
//    {
//        chooseServerType = 2;
//    }
//    else
//    {
//        chooseServerType = 0;
//    }

    PhysicsServerManage serverManage = PhysicsServerManage();
    serverManage.serverInfos[0].cpu = server.data[0][0];
    serverManage.serverInfos[0].mem = server.data[0][1];
    serverManage.serverInfos[0].disk = server.data[0][2];
//    serverManage.serverInfos[0].name += server.Type[0];
memcpy(serverManage.serverInfos[0].name,server.Type[0].c_str(),strlen(server.Type[0].c_str()));
//    printf("type=%s\n",server.Type[0].c_str());
    serverManage.serverInfos[1].cpu = server.data[1][0];
    serverManage.serverInfos[1].mem = server.data[1][1];
    serverManage.serverInfos[1].disk = server.data[1][2];
    memcpy(serverManage.serverInfos[1].name,server.Type[1].c_str(),strlen(server.Type[1].c_str()));
//    printf("type=%s\nname=%s",server.Type[1].c_str(),serverManage.serverInfos[1].name.c_str());
    serverManage.serverInfos[2].cpu = server.data[2][0];
    serverManage.serverInfos[2].mem = server.data[2][1];
    serverManage.serverInfos[2].disk = server.data[2][2];
    memcpy(serverManage.serverInfos[2].name,server.Type[2].c_str(),strlen(server.Type[2].c_str()));
//    serverManage.serverInfos[2].name = server.Type[2];
//    printf("type=%s\n",server.Type[2].c_str());
//    serverManage.display();
    serverManage.adjust();
    serverManage.display();
//    PhysicsServerFormat serverFormat = PhysicsServerFormat();
//
//    //采用主机
//    serverFormat.cpu = server.data[chooseServerType][0];
//    serverFormat.mem = server.data[chooseServerType][1];
//    serverFormat.disk = server.data[chooseServerType][2];
//    serverFormat.display();

    PlaceManager placeManager = PlaceManager(&serverManage,&flavorFormat,&predictRst,flavorFormat.num);
    //进行分配
    placeManager.placeAuto();

//    int chooseServerType =0;
//    文件输出：
    string str_out;
    char buf[10];
    sprintf(buf,"%d",predict_total);           //输出虚拟机总数量
    str_out = str_out + buf;

    str_out = str_out + "\n";
    for(int i=0; i<flavor.numtype; i++){
        str_out = str_out + "flavor";
        sprintf(buf,"%d",flavor.numFlavor[i][0]);   //输出虚拟机型号
        str_out = str_out + buf + " ";
        sprintf(buf,"%d",predict_data[flavor.numFlavor[i][0]-1]);              //输出对应型号的数量
        str_out = str_out + buf + "\n";
    }
//    str_out = str_out;
    char serverNumTotal[20];
    string allServerPlacedTotalStr = "";
    for (int type=0;type < 3; ++ type)
    {
        string serverPlacedTotalStr = "";
        string typeName = serverManage.serverInfos[type].name;
        int serverNum = placeManager.getServerSum(type);
        if (serverNum == 0)
        {
            continue;
        }
        sprintf(serverNumTotal,"%d",serverNum);
        serverPlacedTotalStr += string("\n") + typeName + " "+ serverNumTotal;
        int serverId = 1;
        int i=0;
        for(list<PlaceMethod *>::iterator it = placeManager.methods[type].begin(); it != placeManager.methods[type].end(); ++ it)
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
            for (int j=0; j < placeManager.methodsNum[type][i]; ++j)
            {
                char serverIdStr[5];
                sprintf(serverIdStr,"%d",serverId);
                ++ serverId;
                serverPlacedTotalStr += string("\n") +typeName+"-"+string(serverIdStr) + serverPlacedStr ;
            }
            ++i;
        }
        allServerPlacedTotalStr += serverPlacedTotalStr + string("\n");
    }

    str_out += allServerPlacedTotalStr;


    cout << "output:" << endl;
    cout << str_out << endl;

    char str1[20000];
    strcpy(str1, str_out.c_str());

	 //需要输出的内容
	char * result_file = (char *)str1;


	// 直接调用输出文件的方法输出到指定文件中(ps请注意格式的正确性，如果有解，第一行只有一个数据；第二行为空；第三行开始才是具体的数据，数据之间用一个空格分隔开)
	write_result(result_file, filename);
}



