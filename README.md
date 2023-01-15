

![Windows Server Management Tools - Banner](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/blob/master/docs_image.png?raw=true)
# The Repository

***This repository presents to you a collection of server management tools for various purposes:***
- *Mass administrative on Active Directory environment*
- *Manage multiple databases (e.g. MySQL, Microsoft SQL Server, e.t.c.)*
- *Backup all Hyper-V virtual machines*
- *Clean-up temporary NVIDIA GPU drivers*
- *Remote install for MSI installer*
- *Passthrough physical device into virtual machine*
- *Activation services for server*

...and so much more

For better understanding, you can see the [wiki page](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/wiki).


# The Files and Folders

***The repo consists of three parts***
 - **[!bin!](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/!bin!):** store compiled source codes (if the solution source needs to be complied)
 - **[!docs & ref!](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/!docs%20&%20ref!):** store documents and Visual Studio configurations. In this project, we use **Visual Studio 2022 (17.4 LTSC)** to edit the sources codes, and **Visual Studio 2019 build tools** for additional compliers. 
 - **.vs:**: includes Visual Studio source files.
 - **winser-mgmttools.sln:** solution files, contains all required paths for all projects.
 - **The rest of files and folders:** Project files and folders.

# Contributing & Feedback

## Contributing
To fully edit this repository, download the lastest **[Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)** and **[Visual Studio 2019 build tools](https://visualstudio.microsoft.com/vs/older-downloads/)**. After that, import both **.vsconfig** into **Visual Studio installer**.

Otherwise, you can do other ways:
 - [Review Wiki Page](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/wiki)
 - [Submit bugs and feature requests](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/issues)
 - [Review source codes](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools)
 - [Review and commit pull requests](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/pulls)

*Note that the Visual Studio workload can be really heavy: requires around 20-40Gb for all required libraries*

## Feedback
To feedback this repository, go to [**feedback**](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/issues) or **[join our Discord server](https://discord.gg/VdbJAHKhuW)**

# License & Original Contributors
## License 
This repo uses [**GNU GPL 3.0**](https://www.gnu.org/licenses/gpl-3.0.en.html) and [**MIT License**](https://opensource.org/licenses/MIT) for all contributors to have free, no limit to 
- Use (for both private and commercial uses)
- Modify (included edit the source files and complie/decomplied) 
- Distribution without permission directly from TheFlightSims, except emergency revokes permission. In that case, this repo will be moved to private and cannot be accessed by the public.

## Original Contributors
|Usage|Still under support|Name|Contributors|Folks from|
|--|--|--|--|--|
|IIS Management|Yes|[Check IIS](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/check-iis)|@[TheFlyingCorpse](https://github.com/TheFlyingCorpse), TheFlightSims staff|https://github.com/TheFlyingCorpse/check_iis
|Database Management|Yes|[Database Manager](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/database-manager)|@[dependabot[bot]](https://github.com/apps/dependabot), @[victor-wiki](https://github.com/victor-wiki), TheFlightSims staff|https://github.com/victor-wiki/DatabaseManager|
|VM Management|Yes|[Hyper-V Backup](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/hyperv-backup)|@[pablopioli](https://github.com/pablopioli), @[schtritoff](https://github.com/schtritoff), TheFlightSims staff|https://github.com/ColiseoSoftware/hypervbackup|
|VM Management|Yes|[Hyper-V Enhanced Session for Linux](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/hyper-v-enhanced-session-linux)|@[secana](https://github.com/secana), TheFlightSims staff|https://github.com/secana/EnhancedSessionMode|
|VM Management|Yes|[Hyper-V Passthrough Device](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/hyperv-passthrough)|@[chanket](https://github.com/chanket), TheFlightSims staff|https://github.com/chanket/DDA|
|Computer Management|Yes|[NVIDIA Driver Cleaner](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/nvidia-driver-cleaner)|@[ataberkylmz](https://github.com/ataberkylmz), TheFlightSims staff|https://github.com/ataberkylmz/NvidiaDriverCleaner|
|Computer Management|Yes|[Port Scanner](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/port-scanner)|@[IceMoonHSV](https://github.com/IceMoonHSV), TheFlightSims staff|https://github.com/IceMoonHSV/PortScanner|
|All|Yes|[Powershel Admin Scripts](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/poweshell-adminscript)|@[wikijm](https://github.com/wikijm), @[ashish](https://github.com/ashishknitcs), TheFlightSims staff|https://github.com/wikijm/PowerShell-AdminScripts|
|Computer Management|Yes|[Remote MSI Manager](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/remote-msi-manager)|@[DCourtel](https://github.com/DCourtel), TheFlightSims staff|https://github.com/DCourtel/Remote_MSI_Manager|
|Computer Management|Yes|[SxS Management](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/sxs-mgmt)| Aunty Mel|https://pastebin.com/raw/9iuAqJSn|
|Security|Yes|[User Rights](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/user-rights)|@[jcasale](https://github.com/jcasale), TheFlightSims staff|https://github.com/jcasale/UserRights|
|Volume Activation|Yes|[vlmcsd beta](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/vlmcsd-beta)|@[Wind4](https://github.com/Wind4/vlmcsd), @[kkkgo](https://github.com/kkkgo), @[HarukaMa](https://github.com/HarukaMa), Hotbird64, Nang, TheFlightSims staff|https://github.com/kkkgo/vlmcsd|
|Computer Management|Yes|[Windows Management on Web](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/winman)|@[fakoua](https://github.com/fakoua), @[fossabot](https://github.com/fossabot), @[ImgBotApp](https://github.com/ImgBotApp), TheFlightSims staff|https://github.com/fakoua/WinMan|
|Windows Update Services|Yes|[Windows Update Remote Service](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/winupdate-remote-svc)|@[EliaSaSe](https://github.com/EliaSaSe), @[dependabot[bot]](https://github.com/apps/dependabot), @[florianknecht](https://github.com/florianknecht), @[tinogruse](https://github.com/tinogruse), TheFlightSims staff|https://github.com/EliaSaSe/windows-update-remote-service|
|Windows Update Services|Yes|[WSUS Admin Assisstant](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/wsusadminassistant)|@[rjch-au](https://github.com/rjch-au), TheFlightSims staff|https://github.com/rjch-au/WSUSAdminAssistant|
|Windows Update Services|Yes|[WSUS Online Descriptions](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/wsusonlinedescriptions)|@[Physikbuddha](https://github.com/Physikbuddha), TheFlightSims staff|https://github.com/Physikbuddha/wsus-online-descriptions|
|Windows Update Services|Yes|[WSUS Package Publisher](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/wsuspackagepublisher)|@[mihailim](https://github.com/mihailim), @[DCourtel](https://github.com/DCourtel), TheFlightSims staff|https://github.com/DCourtel/Wsus_Package_Publisher|
|Windows Update Services|Yes|[WSUS Workgroup Utilities](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/wsusworkgrouputilities)|@[blndev](https://github.com/blndev/), TheFlightSims staff|https://github.com/blndev/wsusworkgroup|
|Computer Management|Yes|[XMouse Control](https://github.com/TheFlightSimulationsOfficial/windowsserver-mgmttools/tree/master/xmouse-controls-develop)|@[joelpurra](https://github.com/joelpurra), TheFlightSims staff|https://github.com/joelpurra/xmouse-controls|
