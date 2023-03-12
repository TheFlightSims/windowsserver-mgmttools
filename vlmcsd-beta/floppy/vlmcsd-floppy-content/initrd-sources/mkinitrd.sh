#!/bin/sh
find . | cpio -o -H newc > ../initrd~
cd ..
lzma initrd~
mv initrd~.lzma initrd

#Save at ../initrd. Tested in Ubuntu 20.04 LTS on WSL. See more at aka.ms/wsl