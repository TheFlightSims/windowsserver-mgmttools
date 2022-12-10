# vlmcsd - Portable open-source KMS Emulator in C  
**vlmcsd-1113-2020-03-28   [Releases][1]**   

|Supported operating systems / run-time environments|
:-|
Supported operating systems：Linux, GNU/Linux, uclibc/Linux, musl/Linux, Android (bionic/Linux), FreeBSD, FreeBSD with glibc (e.g. debian/kFreeBSD), OpenBSD, NetBSD, DragonflyBSD, Solaris, Open Indiana, Dyson, Minix, Darwin, Mac OS, iOS, Windows, Cygwin, WSL, Wine, The Hurd. |
Supported CPUs : x86，arm, mips, PowerPC, Sparc, s390|
Number of explicitly supported products to activate : 202|

NAME|DESCRIPTION|USAGE
:-|-|-
vlmcs | a client for testing and/or charging KMS servers|vlmcs [ options ] [ target ] [options ]
vlmcsd | a fully Microsoft compatible KMS server|vlmcsd [ options ] 
vlmcsdmulti | vlmcsdmulti is a multi-call binary that contains vlmcs and vlmcsd in  a  single binary. |vlmcsdmulti  vlmcs [options ] [ hostname\|ip-address[:port] ] [ options ] or vlmcsd [ option ]|

### To view the documentation cd to the [directory][2] containing the distribution  
**files and type**  

 - man man/vlmcsd.8  
	to see documentation for vlmcsd  

 - man man/vlmcs.1  
	to see documentation for vlmcs  

 - man man/vlmcsd.7  
	to see general documentation for kms  

If you don't have man, you may also use the .txt, .html and .pdf files in [the man directory][2]. 


----------


**vlmcsd is**   

- a replacement for Microsoft's KMS server It contains vlmcs a KMS test client, mainly for debugging purposes, that also can "charge" a genuine KMS server designed to run on an always-on or often-on device,e.g. router, NAS Box, ... intended to help people who lost activation of their legally-owned licenses, e.g. due to a change of hardware (motherboard, CPU, ...)  

**vlmcsd is not**

 - a one-click activation or crack tool intended to activate illegal copies of software (Windows, Office, Project, Visio)  

----------

### Valid Apps 
# To see vaild applications can be activated using 
