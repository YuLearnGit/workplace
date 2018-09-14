#include "predict.h"
#include "handle_data.h"
#include "forecast.h"
//#include "place.h"
#include <stdio.h>
#include "MyDateUtil.h"
#include "PlaceFlavor.h"
#include "Output.h"


//你要完成的功能总入口
void predict_server(char * info[MAX_INFO_NUM], char * data[MAX_DATA_NUM], int data_num, char * filename)
{
    traindata traininfo;
    inputdata serverAndflavor;
    traininfo.hanle_traindata(data, data_num); //训练数据
    serverAndflavor.handle_inputdata(info);//输入文件数据
    forecast(info, data, data_num);

    //下面部分我的分配方案
    PhysicsServerFormat serverFormat = PhysicsServerFormat();

    //物理服务器信息的接口
    serverFormat.cpu = serverAndflavor.serverCPU;
    serverFormat.mem = serverAndflavor.serverMEM;
    serverFormat.disk = serverAndflavor.serverDisk;

//    serverFormat.display();

    PredictFlavorFormat flavorFormat = PredictFlavorFormat();

    //虚拟机种类的接口
    flavorFormat.num = serverAndflavor.numtype;
    for (int i=0; i <15; ++i)
    {

        flavorFormat.flavorInfoIndex[i].id = serverAndflavor.flavorinfo[i][0];
        flavorFormat.flavorInfoIndex[i].cpu = serverAndflavor.flavorinfo[i][1];
        flavorFormat.flavorInfoIndex[i].mem = serverAndflavor.flavorinfo[i][2];
    }

    flavorFormat.adjust(&serverFormat);//此函数必须调用
//    flavorFormat.display();

    PredictResult predictRst = PredictResult();

    //预测结果的接口
    for (int i=0; i <15; ++i)
    {
        predictRst.nums[i] = predict_flavor1[flavorFormat.flavorInfo[i].id-1];
    }
//    predictRst.display();


    //上述部分将预测结果通过接口，存储到了我定义的对象中，使得我可以按我定义的对象进行操作
    //下面是调用分配算法进行分配
    PlaceManager placeManager = PlaceManager(&serverFormat,&flavorFormat,&predictRst,flavorFormat.num);
    //进行分配
    placeManager.placeAuto(serverAndflavor.target == "CPU");
//    placeManager.display();

    //得到字符串输出
    OutputUtils output = OutputUtils(&placeManager);
    string str = output.getStr();
    //打印我的分配方案
    printf("\nzj:\n\n");
    printf("%s\n",str.c_str());

    char buff[65535];
    strcpy(buff,str.c_str());


//     需要输出的内容
    char * result_file = buff;

    // 直接调用输出文件的方法输出到指定文件中(ps请注意格式的正确性，如果有解，第一行只有一个数据；第二行为空；第三行开始才是具体的数据，数据之间用一个空格分隔开)
    write_result(result_file, filename);
}
