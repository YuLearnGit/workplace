#include <memory>
#include <string>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

#include "sendlog.h"
#include "sendinfo.h"
#include "SecurityDev.h"
#include "ClientDev.h"

using namespace std;

void sendlog(std::shared_ptr<SecurityDev> s_ptr, std::shared_ptr<ClientDev> c_ptr) {
  char buff[350];
  const char *srcip = "1.1.1.1";

  while(true) {
    FILE *fp;
    fp = fopen("/var/log/kern.log", "r+");
    if(fp == NULL) {
      sleep(3);
      fp = fopen("/var/log/kern.log", "r+");
    }

    while(fgets(buff, 350, fp) != NULL) {
      if(buff[strlen(buff) - 1] == '\n') {
        char *p = strstr(buff, "MAC");
        char *q = strstr(buff, "PHYSIN");
        string secur_mac = s_ptr->getMAC();

        char *Firewall_mac = const_cast<char*>(secur_mac.c_str());
        strncat(buff, "$", strlen("$"));
        strncat(buff,Firewall_mac,strlen(Firewall_mac));
        string dstIP = c_ptr->getIP();
        const char *dstip = dstIP.c_str();
        string dstMAC = c_ptr->getMAC();
        const char *dstmac = dstMAC.c_str();
        if(p != NULL && q != NULL && strlen(dstmac)!=0 && strlen(dstip)!=0) {
          sendinfo(dstIP, string(srcip), dstMAC,
                   s_ptr->getMAC(), 8000, 8000,string(buff));
          *p = 0;
          *q = 0;
        }
      }
    }
    fclose(fp);
    system(":>/var/log/kern.log");
	system(":>/var/log/messages");
	system(":>/var/log/syslog");
    sleep(1);
  }
}
