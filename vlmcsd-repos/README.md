# VLMCSD & License Manager

![Docker Tracker](https://img.shields.io/docker/pulls/theflightsims/vlmcsd)

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
| [shiroineko](https://github.com/shiroinekotfs) | Contributor of VLMCSD, License Manager, VLMCSD on Floppy, VLMCSD on WSL, VLMCSD Database |

## The Files and Folders

| Folder Name | Description |
|--|--|
| [database-config](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/database-config) | Database for `vlmcsd`, License Manager, and  MSSQL 2022. It also contains `getkey.py` and Microsoft SQL Server 2022 key management notebook |
| [license-manager](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/license-manager) | License Manager source folder |
| [vlmcsd-docker](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/vlmcsd-docker) | Binary file for Docker container |
| [vlmcsd-floppy](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/vlmcsd-floppy) | `vlmcsd` floppy disk |
| [vlmcsd-wsl](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/vlmcsd-wsl) | `vlmcsd` WSL distro |
| [vlmcsd](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta/vlmcsd) | `vlmcsd` source code |

## Build and Run

Check out this wiki to build VLMCSD, License Manager, vlmcsd on floppy, and vlmcsd on WSL.

## List of  activation keys
Click to see the list of [product keys](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/database-config/tables/Product%20Keys.csv) and the [list of keys available](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/vlmcsd-beta/database-config/tables/Total%20Keys.csv). Note that each product can only see 5 public keys. 

To see the complete list of product keys, you can check out the Wiki to learn how to import and list from SQL Database.
