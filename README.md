![Windows Server Management Tools - Banner](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/banner.png?raw=true)

# The Repository

***This repository presents to you a collection of server management tools for various purposes:***

- *Clean up temporary NVIDIA GPU drivers*
- *Remote install for MSI installer*
- *Passthrough physical device into the virtual machine*
- *Activation services for the server*
- *Manage updates for servers and clients*

For better understanding, you can see the [wiki page](https://github.com/TheFlightSims/windowsserver-mgmttools/wiki).

# Use and install

***To run and use applications in this project, make sure your computer is installed with the features:***

* dotNET 2.1, dotNET 3.5, dotNET 4.8. These are built-in features and can be enabled. You can see the document [here](https://learn.microsoft.com/en-us/dotnet/framework/install/dotnet-35-windows)

* Modern applications require .NET 7.0. You can download it [here](https://dotnet.microsoft.com/en-us/download)

* Visual C++ 2015 and all versions later. You can download it [here](https://learn.microsoft.com/en-US/cpp/windows/latest-supported-vc-redist?view=msvc-170)

# The Files and Folders

***The repo consists of three parts***

* **[.makevs](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/.makevs):** Make Visual Studio offline layout. It is recommended to fully editing in this project.
* **winser-mgmttools.sln:** solution files, contains all required paths for all projects.
* **The rest of the files and folders:** Project files and folders.

# Contributing & Feedback

## Contributing

To fully edit this repository, run to download all layouts for Visual Studio 2022 that are located in **[!makevs!](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/!makevs!)**.
Also, you may need [Advanced Installer](https://www.advancedinstaller.com/) to create an installer. These are all located in the folder [!bin!](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/!bin!) in each project.

Otherwise, you can do other ways:

* [Review Wiki Page](https://github.com/TheFlightSims/windowsserver-mgmttools/wiki)
* Submit bugs and feature requests in [GitHub](https://github.com/TheFlightSims/windowsserver-mgmttools/issues) or our [Discord server](https://discord.gg/VdbJAHKhuW)
* [Review source codes](https://github.com/TheFlightSims/windowsserver-mgmttools)
* [Review and commit pull requests](https://github.com/TheFlightSims/windowsserver-mgmttools/pulls)

*Note that the Visual Studio workload can be really heavy: requires around 20-40Gb for all required libraries*

## Feedback

To feedback this repository, go to [**feedback**](https://github.com/TheFlightSims/windowsserver-mgmttools/issues) or **[join our Discord server](https://discord.gg/VdbJAHKhuW)**

# License & Original Contributors

## License

This repo uses [**GNU GPL 3.0**](https://www.gnu.org/licenses/gpl-3.0.en.html) and [**MIT License**](https://opensource.org/licenses/MIT) for all contributors to have free, no limit to 

- Use (for both private and commercial uses)
- Modify (including edit the source files and compile/decompile) 
- Distribution without permission directly from TheFlightSims, except emergency revokes permission. In that case, this repo will be moved to private and cannot be accessed by the public.

## Original Contributors

|Usage|Name|Contributors|Original path|
|--|--|--|--|
|VM Management|[Hyper-V Passthrough Device](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/hyperv-passthrough)|@[chanket](https://github.com/chanket), TheFlightSims staff|https://github.com/chanket/DDA|
|Computer Management|[NVIDIA Driver Cleaner](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/nvidia-driver-cleaner)|@[ataberkylmz](https://github.com/ataberkylmz), TheFlightSims staff|https://github.com/ataberkylmz/NvidiaDriverCleaner|
|Debugging|[PDB Downloader](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/PDB-Downloader)|@[rajkumar-rangaraj](https://github.com/rajkumar-rangaraj), TheFlightSims staff|https://github.com/rajkumar-rangaraj/PDB-Downloader
|Computer Management|[Port Scanner](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/port-scanner)|@[IceMoonHSV](https://github.com/IceMoonHSV), TheFlightSims staff|https://github.com/IceMoonHSV/PortScanner|
|Computer Management|[Remote MSI Manager](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/remote-msi-manager)|@[DCourtel](https://github.com/DCourtel), TheFlightSims staff|https://github.com/DCourtel/Remote_MSI_Manager|
|Volume Activation|[vlmcsd beta/License Manager](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/vlmcsd-beta)|@[Wind4](https://github.com/Wind4/vlmcsd), @[kkkgo](https://github.com/kkkgo), @[HarukaMa](https://github.com/HarukaMa), @[Hotbird64](https://forums.mydigitallife.net/members/hotbird64.333466/), @[Nang](https://jike.info/user/nang), TheFlightSims staff, @[Linus Torvalds](https://github.com/torvalds), Erik Andersen, Waldemar Brodkorb, Denys Vlasenko, H. Peter Anvin|https://github.com/kkkgo/vlmcsd|
|Group Policy|[GPO Checker](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/gpo-checker)|@[georgeatgrayson](https://github.com/georgeatgrayson), TheFlightSims staff|https://github.com/georgeatgrayson/Windows-GPO-Security-Checker|
|Computer Management|[Remote MSI Manager](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/wsus-package-publisher)|@[DCourtel](https://github.com/DCourtel), TheFlightSims staff|https://github.com/DCourtel/Wsus_Package_Publisher|
