
# VLMCSD & License Manager

***VLMCSD*** is a free, open-source software to provide an activation service (KMS) for any computer using Office and Windows products

***License Manager*** is a free, open-source software to manage licenses of local or remote computers

**For detailed information (written by the original developers), see [here](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/vlmcsd/man)**

## Original Contributors

| Contributor | Description |
|--|--|
| [Linus Torvalds](https://github.com/torvalds) | Original developer of Linux kernel, used in VLMCSD on Floppy |
| Hotbird64 | Original developer of VLMCSD, License Manager, VLMCSD on Floppy, VLMCSD on WSL |
| Erik Andersen | Original developer of VLMCSD | 
| Waldemar Brodkorb | Original developer of VLMCSD |
| Denys Vlasenko | Original developer of VLMCSD |
| H. Peter Anvin | Original developer of VLMCSD |
| [Wind4](https://github.com/Wind4/vlmcsd) | Contributor of VLMCSD |
| [kkkgo](https://github.com/kkkgo) | Contributor of VLMCSD, VLMCSD on Floppy |
| [HarukaMa](https://github.com/HarukaMa) | Contributor of VLMCSD, VLMCSD on Floppy |
| Nang | Contributor of VLMCSD database
| [takineko](https://github.com/takinekotfs) | Contributor of VLMCSD, License Manager, VLMCSD on Floppy, VLMCSD on WSL, VLMCSD Database |

## The Files and Folders

| Folder Name | Description |
|--|--|
| [database-config](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/database-config) | Database for `vlmcsd`, License Manager, and  MSSQL 2022. It also contains `getkey.py` and Microsoft SQL Server 2022 key management notebook |
| [license-manager](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/license-manager) | License Manager source folder |
| [vlmcsd-docker](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/vlmcsd-docker) | Binary file for Docker container |
| [vlmcsd-floppy](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/vlmcsd-floppy) | `vlmcsd` floppy disk |
| [vlmcsd-wsl](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/vlmcsd-wsl) | `vlmcsd` WSL distro |
| [vlmcsd](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/vlmcsd) | `vlmcsd` source code |

## Use VLMCSD & License Manager

### Use the VLMCSD command line

| VLMCSD App | Description | Command line |
| -- | -- | -- |
| vlmcs | vlmcs is a program that can be used to test a KMS server that provides activation for Microsoft products. The KMS server may also be an emulator. It supports KMS 4,5,6 protocols | [See the document](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/vlmcsd/man/vlmcs.1.unix.txt)
| vlmcsd | vlmcsd is a fully Microsoft-compatible KMS server that provides product activation services to clients. It is a drop-in replacement for a Microsoft KMS server (Windows computer with KMS key entered). It supports KMS 4,5,6 protocols | [See the document](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/vlmcsd/man/vlmcsd.8.unix.txt) |
| vlmcsdmulti | vlmcsdmulti is a multi-call binary that contains vlmcs and vlmcsd in a  single binary. Since both programs share much code and data, the combined binary is significantly smaller than the sum of both files. | [See the document](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/vlmcsd/man/vlmcsdmulti.1.dos.txt)

### Use VLMCSD on Docker

On a system having too many applications running, using `docker` is the better way to deploy, maintain, and retire `vlmcsd`

So far, we have published `vlmcsd` on [Docker Hub](https://hub.docker.com/r/theflightsims/vlmcsd) for both Linux and Windows containers.  By default, the container will expose port 1688.

* On Windows, we only have Windows Server 2022 LTSC `(amd64)`

```cmd
docker run -p 1688:1688 theflightsims/vlmcsd:ltsc2022-amd64
```

* On Linux, you can pull this `amd64`

```shell
docker run -p 1688:1688 theflightsims/vlmcsd:linux-amd64
```

### Use the VLMCSD on Floppy

Typically, VLMCSD can be installed and running as an activation service, but in some cases, VLMCSD on Floppy provides a light-weight activation service for the lab to test; or provide an internal activation for the host (e.g., you activate Windows Server 2022 Datacenter, but you cannot activate Windows locally by installing it as a local service, due the error `0x8007000D` prevents Windows from activating locally)

> Note:
>
> - When creating a VM on Hyper-V that uses VLMCSD floppy, make sure that you choose Generation 1 because of the incompatible with the Hyper-V Generation 2.
> - When creating a VM on VMWare or Virtual Box, you must configure your boot OS as Linux (x86) to sync the VM BIOS with the OS

**1. Diskless System**

The  VLMCSD on Floppy is a diskless system that works entirely on RAM. The file system is a RAM disk created from the `initrd` file on the floppy image. That means anything you have made from inside the virtual machine will be lost when you reboot the machine. 

**2. System startup**
The kernel boots up while the init script (`/sbin/init`) waits 5 seconds. While this, you can: 

```text
Press m to enter IPv4 & timezone configuration
Press t to start configure timezone configuration only
Press s to enter pre—vlmcsd—service shell.
```

You can skip waiting for 5 seconds by pressing any other key. You also will see the IP addresses and a table containing user names and passwords.

**3. Logging into the system**

5 local login shells are provided on /dev/tty2 to /dev/tty6. To switch between the logins, you can press ALT and F2 to F6, or return to the service logs by pressing ALT-F1. This allows users can use multiple terminals at once.

The floppy image only provides basic Unix commands—type busybox or /bin to get a list. The only editor available is vi.  If you don't like vi,  you may transfer config files via FTP to edit them with the editor of your choice and transfer them back.

**4. The menu system**
You may need some just-in-time commands by pressing ALT-F8. 

```text
1. Restart vlmcsd service                             
2. Stop vlmcsd service                                
3. Restart network service (includes telnet and ftp)  
4. Stop network service (includes telnet and ftp)     
5. Change the timezone                                
6. Show all kernel boot parameters                    
7. Show boot log                                      
8. Show TCP/IP configuration                          
9. Show running processes                             
                                                        
k. Change keyboard layout                             
                                                        
s. Shutdown the computer                              
r. Reboot the computer                                
```

### Use the VLMCSD on WSL

VLMCSD on WSL is recommended for multi-system on Windows. To see how WSL works, see [here](https://aka.ms/wsl)

> You can use VLMCSD on WSL as you do on Floppy. However, the changes you have made on the WSL distro are permanent.

To use VLMCSD on WSL, import it into the WSL database. To do this, change your active directory on the terminal to the folder [(project path)/vlmcsd-beta/wsl](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/vlmcsd-wsl)

```cmd
wsl --import <Your Distro Name> <Your installation directory> vlmcsd.tar
```

### Use the License Manager

> This guide only applies to License Manager, version 5.5.0, built and released by TheFlightSims. If you use your own copy of License Manager, or repack by others, the guide may differ.

**1. The User Interface**

![image](https://user-images.githubusercontent.com/99700363/225807709-d048e0cd-db62-497d-85e5-d3f8c8ada8e3.png)

(1) License Manager tools (stores in tabs and quick access tools)

(2) Connection credential and selected edition

(3) Verbose license information

(4) Verbose machine information

(5) Progress bar


**2.  Install a Generic Key for the local computer**

![image](https://user-images.githubusercontent.com/99700363/225810410-8003e36e-cb60-4c90-8e87-bce2586c53d3.png)

Click on "Install a Generic Key", and select the key that you want to install. You can copy-n-paste the key, or click "Check" and then click "Install" on the Product Finder window.

> For Microsoft Visual Studio, Microsoft SQL Server, and Microsoft SCCM, you need to copy-n-paste in the product key box. License Manager cannot install these keys on your machine

**3. Activate your Windows or Office using KMS Server**

> To activate using KMS Server, your destination computer needs a Generic Volume License Key (GVLK) installed. See [this document](https://learn.microsoft.com/en-us/windows-server/get-started/kms-client-activation-keys) to know how. Normally, the product keys stores in the License Manager that you can install are public GVLKs, so you will not need to find it on the Internet!

![image](https://user-images.githubusercontent.com/99700363/225835744-c7ae792e-adaa-4bb7-86b8-3e63a1b22183.png)

You will need to determine which server responds to your request by entering the field Override KMS Host data with the IP Address or DNS Name, or you just need to determine the KMS Domain. Don't forget to save the settings!

> Pro tip: if you use the Server edition, License Manager can verify whether the server can respond with your product activation in "Start a KMS Client"
>![image](https://user-images.githubusercontent.com/99700363/225837594-879e4761-0129-4dd4-af4f-75b0d81e4e4b.png)

**4. Install or verify a product key**

![image](https://user-images.githubusercontent.com/99700363/225839967-4452368e-b911-486e-9b85-8fc890be052a.png)

You can just simply click on Product Finder and paste your product key: you will see your product key details, including its EPID and Complete PID.

You can choose to Install it or Check the availability online.

> Note that you can only verify Microsoft Office, Microsoft Windows, and Microsoft Visual Studio. However, Microsoft Visual Studio cannot be installed - you must enter the product key manually.

**5. Export to Database**

You can export to these specific files in License Manager:
|Export format| File extension | Description |
|--|--|--|
| vlmcsd | .kmd | This format used by vlmcsd, as the external (add-on) database |
| py-kms classic | .txt | This format used by py-kms (KMS for Python 2) |
| Generic C/C++/C# | .txt | This format helps developers to modify the vlmcsd source code |
| License Manager database, py-kms | .xml | This format used by License Manager

> Unless you export the database for License Manager only, you MUST delete the Visual Studio keys, SQL Server keys, and SCCM keys because these are NOT KMS keys and cannot be activated as KMS protocols.

## Build VLMCSD, License Manager & Edit Database

### Build VLMCSD on Visual Studio (recommended) 

1. Open **winser-mgmttools.sln**, then click on **vlmcsd.sln** and open with Visual Studio

![image](https://user-images.githubusercontent.com/99700363/225601211-38d78847-3421-40d5-8bbb-1e4ad0d793c3.png)

> If the VS 2022 selection isn't in Open With, you can add VS2022 within the Add Program
![image](https://user-images.githubusercontent.com/99700363/225601991-c4a7b0a6-2fd2-4ce0-87cc-4ae7eeb091ca.png)

2. After opening this, you can build VLMCSD

![image](https://user-images.githubusercontent.com/99700363/225604558-90a9c712-6d66-4c2f-aac8-671288741637.png)

The Final build locates in the [!bin!](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/!bin!) folder. 

> If you are trying to build VLMCSD for Linux distribution, see [here](https://learn.microsoft.com/en-us/cpp/linux/connect-to-your-remote-linux-computer?view=msvc-170).

### Build VLMCSD on Floppy

You must know to edit the Floppy.

> Note that the floppy does NOT contain mkinitrd.guide file. It helps our developers in rebuilding the initrd

| File in floppy | Description |
| -- | -- |
|[bzImage](https://en.wikipedia.org/wiki/Vmlinux)| Linux kernel, version 3.12
|[initrd](https://docs.kernel.org/admin-guide/initrd.html) | initial RAM disk, compressed as CPIO + LZMA
| ldlinux.sys | Linux bootloader for floppy |
| [syslinux.cfg](https://wiki.syslinux.org/wiki/index.php?title=Config) | Linux System Configuration |

> Note: We sort by zero which means you have to follow these instructions in order, do it in order of sections.

1. Configure and rebuild initrd

 > To follow more straightforward, we use the default path [(project path)/vlmcsd-beta/vlmcsd-floppy/vlmcsd-floppy-content](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/vlmcsd-floppy/vlmcsd-floppy-content). You must configure your terminal to this path to this path to follow this guide easier

- Configure initrd
  
  The list of files below helps you to determine each purpose of the files in initrd in floppy disk
  > In here, we use the Linux root path, which is extracted in the folder [(project path)/vlmcsd-beta/vlmcsd-floppy/vlmcsd-floppy-content/initrd](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/vlmcsd-floppy/vlmcsd-floppy-content/initrd)

- Rebuild the initrd by using these commands in WSL (recommend Debian or Ubuntu) or run them in Linux

```shell
#You must configure your terminal to (project path)/vlmcsd-beta/floppy/vlmcsd-floppy-content
rm initrd 
cd initrd-sources 
find . | cpio -o -H newc > ../initrd~
cd ..
lzma initrd~
mv initrd~.lzma initrd
```

> You still can change the behavior of the compression model (e.g., compress with a smaller packet size), but you must know what you do.
> To understand how to use WSL, you can see [here](https://aka.ms/wsl). Although WSL1 and WSL2 can do the same, we recommend WSL2 because more stable and secure for your device

1. Edit the virtual floppy disk

You may need to use [OSFMount](https://www.osforensics.com/tools/mount-disk-images.html) to mount the formatted-FAT16-[floppy disk](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/floppy/vlmcsd-floppy.vfd).
 After mounting the floppy disk, copy-n-paste the files you have already edited.

### Build License Manager

1.  Open  **winser-mgmttools.sln**, then click on  **LicenseManager.sln**  and open with Visual Studio
![image](https://user-images.githubusercontent.com/99700363/225634845-54651f8d-62da-4de1-a146-292722d64643.png)

> If the VS 2022 selection isn't in Open With, you can add VS2022 within the Add Program![image](https://user-images.githubusercontent.com/99700363/225601991-c4a7b0a6-2fd2-4ce0-87cc-4ae7eeb091ca.png)

2. After opening this, you can build License Manager

![image](https://user-images.githubusercontent.com/99700363/225635348-5dd4ad70-754b-4d56-97bf-387cf0ec9af4.png)

### Build & Manage Database

- [License Manager + VLMCSD database](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/database-config/license-manager)

License Manager is a XML database, stores License Manager product keys and Windows versions. You can follow [this form](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/database-config/license-manager/KmsDataBase.xml) to know how to edit this.

VLMCSD Service configuration (or known as [vlmcsd.ini](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/database-config/license-manager/vlmcsd.ini)) is as vlmcsd service configuration.

VLMCSD Database (or known as [vlmcsd.kmd](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/database-config/license-manager/vlmcsd.kmd)) is the binary, external vlmcsd database. You can configure it in [vlmcsd.ini](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/database-config/license-manager/vlmcsd.ini), or you can use CLI.

> In the vlmcsd.kmd, make sure that Microsoft Visual Studio, Microsoft SQL Server and Microsoft SCCM keys are not in vlmcsd.kmd because these are NOT KMS keys and cannot be activated as KMS protocols, and causes vlmcsd service crash to desktop (CTD).

- [Product Key database on Microsoft SQL Server 2022](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/database-config/sql)

To use the database, [restore](https://learn.microsoft.com/en-us/sql/relational-databases/backup-restore/quickstart-backup-restore-database?view=sql-server-ver16) the database from the file [PDKDB.bak](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/database-config/sql/PDKDB.bak)

> The database is only readable on [Microsoft SQL Server 2022](https://www.microsoft.com/en-us/sql-server/sql-server-2022). Consider to upgrade your own SQL Server to use it.

To manage the Product Key database, use the [Azure Data Studio](https://azure.microsoft.com/en-us/products/data-studio) to open [Product Key Notebook.ipynb](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/database-config/sql/Product%20Key%20Notebook.ipynb), and use it to connect and manage the Product Key database.

- [Product Key tables](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/db/tables) are the .csv portable file that can open on Microsoft Excel, or WPS Excel.

## Activation Error Codes & Limitations

### Limitation

Check the list of [product keys](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/db/tables/Product%20Keys.csv) that VLMCSD can activate

### Error codes

|Error code |Error message |Activation&nbsp;type|
|-----------|--------------|----------------|
|0x8007000D | The KMS host you are using cannot handle your product. It only supports legacy versions. | KMS client |
|0x8004FE21|This computer is not running genuine Windows.  |MAK<br />KMS client |
|0x80070005 |Access denied. The requested action requires elevated privileges. |MAK<br />KMS client<br />KMS host |
|0x8007007b| DNS name does not exist. |KMS client |
|0x80070490|The product key you entered didn't work. Check the product key and try again, or enter a different one. |MAK |
|0x800706BA |The RPC server is unavailable. |KMS client |
|0x8007232A|DNS server failure.  |KMS host  |
|0x8007232B|DNS name does not exist. |KMS client |
|0x8007251D|No records found for DNS query. |KMS client |
|0x80092328|DNS name does not exist.  |KMS client |
|0xC004B100 |The activation server determined that the computer could not be activated. |MAK |
|0xC004C001|The activation server determined the specified product key is invalid |MAK|
|0xC004C003 |The activation server determined the specified product key is blocked |MAK |
|0xC004C008 |The activation server determined that the specified product key could not be used. |KMS |
|0xC004C020|The activation server reported that the Multiple Activation Key has exceeded its limit. |MAK |
|0xC004C021|The activation server reported that the Multiple Activation Key extension limit has been exceeded. |MAK |
|0xC004F009 |The Software Protection Service reported that the grace period expired. |MAK |
|0xC004F00F|The Software Licensing Server reported that the hardware ID binding is beyond level of tolerance. |MAK<br />KMS client<br />KMS host |
|0xC004F014|The Software Protection Service reported that the product key is not available |MAK<br />KMS client |
|0xC004F02C|The Software Protection Service reported that the format for the offline activation data is incorrect. |MAK<br />KMS client |
|0xC004F035|The Software Protection Service reported that the computer could not be activated with a Volume license product key. |KMS client<br />KMS host |
|0xC004F038 |The Software Protection Service reported that the computer could not be activated. The count reported by your Key Management Service (KMS) is insufficient. Please contact your system administrator. |KMS client |
|0xC004F039|The Software Protection Service reported that the computer could not be activated. The Key Management Service (KMS) is not enabled. |KMS client |
|0xC004F041|The Software Protection Service determined that the Key Management Server (KMS) is not activated. KMS needs to be activated.  |KMS client |
|0xC004F042 |The Software Protection Service determined that the specified Key Management Service (KMS) cannot be used. |KMS client |
|0xC004F050|The Software Protection Service reported that the product key is invalid. |MAK<br />KMS<br />KMS client |
|0xC004F051|The Software Protection Service reported that the product key is blocked. |MAK<br />KMS |
|0xC004F064|The Software Protection Service reported that the non-genuine grace period expired. |MAK |
|0xC004F065|The Software Protection Service reported that the application is running within the valid non-genuine period. |MAK<br />KMS client |
|0xC004F06C|The Software Protection Service reported that the computer could not be activated. The Key Management Service (KMS) determined that the request timestamp is invalid. |KMS client |
|0xC004F074|The Software Protection Service reported that the computer could not be activated. No Key Management Service (KMS) could be contacted. Please see the Application Event Log for additional information. |KMS client |

## List of  activation keys
Click to see the list of [product keys](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/db/tables/Product%20Keys.csv) and the [list of keys available](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/db/tables/Total%20Keys.csv). Note that each product can only see 5 public keys.

To see the complete list of product keys, you can check out the Wiki to learn how to import and list from SQL Database.
