
obj-m += xtProfinet.o
xtProfinet-objs = xt_Profinet.o Profinet_decode.o general_api.o
KDIR := /lib/modules/$(shell uname -r)/build
PWD := $(shell pwd)

all:
	$(MAKE) -C $(KDIR) M=$(shell pwd) modules

clean:
	rm -rf *.ko *.o *.mod.c .*.cmd *.markers *.order *.symvers .tmp_versions
