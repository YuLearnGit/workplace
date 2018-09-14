#include "dist.h"

void predictResult::data_preprocessing()
{
    for(int i =1; i<=flavorinfo.numtype; ++i)
    {
        flavor_id_mem a;
        a.cpu = flavorinfo.numFlavor[i-1][1];
        a.mem = flavorinfo.numFlavor[i-1][2];
        flavor_to_dist.push_back(a);
    }
    #if DEBUG
    cout<<"CPU and MEM:"<<endl;
    for(auto item : flavor_to_dist)
        cout<<item.cpu<<" "<<item.mem<<",";
    cout<<endl;
    #endif // DEBUG
}


dist::dist()
{
    for(int i=0; i<3; ++i)
        server_NUM[i] = 0;
    for(int j=0; j<3; j++)
        for(int m =0; m<500; ++m)
            for(int n=0; n<18; ++n)
                dist_result[j][m][n] = 0;
}

void dist::distribution(char ** const info, char **data, int data_num)
{
    Server server;                      //定义服务器类
    Flavor flavor;                      //定义虚拟机类
    server.getnum(info);                //提取服务器数据
    flavor.get_numtype(info);           //提取虚拟机数据

    //显示预测结果
    predictResult play_result(flavor);
    play_result.display();
    play_result.data_preprocessing();

}

