
obj-m += xtDNP3.o
xtDNP3-objs = xt_DNP3.o DNP3_decode.o general_api.o
KDIR := /lib/modules/$(shell uname -r)/build
PWD := $(shell pwd)

all:
	$(MAKE) -C $(KDIR) M=$(shell pwd) modules

clean:
	rm -rf *.ko *.o *.mod.c .*.cmd *.markers *.order *.symvers .tmp_versions
