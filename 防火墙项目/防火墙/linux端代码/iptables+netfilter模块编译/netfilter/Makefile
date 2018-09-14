
obj-m += xtmodbusTcp.o
xtmodbusTcp-objs = xt_modbusTcp.o modbus_decode.o general_api.o
KDIR := /lib/modules/$(shell uname -r)/build
PWD := $(shell pwd)

all:
	$(MAKE) -C $(KDIR) M=$(PWD) modules

clean:
	rm -rf *.ko *.o *.mod.c .*.cmd *.markers *.order *.symvers .tmp_versions
