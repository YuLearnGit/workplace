#include <string>
#include <iostream>

#include <sys/types.h>
#include <unistd.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "ProtectedDev.h"

using namespace std;

ProtectedDev::ProtectedDev() {}

void ProtectedDev::setMAC() {
  MAC_addr = caculateMAC();
}

string ProtectedDev::caculateMAC() {
  if(IP_addr == "0.0.0.0")
    return "00:00:00:00:00:00";

  string ping_cmd = "ping -c 1 " + IP_addr;
  string arp_cmd = "arp " + IP_addr;

  char buf[1024], dev_mac[128];
  memset(buf, '\0', sizeof(buf));
  memset(dev_mac, '\0', sizeof(dev_mac));

  system(ping_cmd.c_str());
  FILE *stream = popen(arp_cmd.c_str(), "r");
  size_t num = fread(buf, sizeof(char), sizeof(buf), stream);
  pclose(stream);

  if(num != 0) {
    char *ans = strchr(buf, ':');
    if(ans != NULL) {
      strncpy(dev_mac, ans-2, 17*sizeof(size_t));
      dev_mac[17] = '\0';
    }
  }
  return string(dev_mac);
}
