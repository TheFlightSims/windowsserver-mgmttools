![Windows Server Management Tools Banner](https://github.com/TheFlightSims/windowsserver-mgmttools/blob/master/banner.png?raw=true)

![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/TheFlightSims/windowsserver-mgmttools/total)
![Docker Tracker](https://img.shields.io/docker/pulls/theflightsims/vlmcsd)
![GitHub language count](https://img.shields.io/github/languages/count/TheFlightSims/windowsserver-mgmttools)
![GitHub repo size](https://img.shields.io/github/repo-size/TheFlightSims/windowsserver-mgmttools)
![GitHub Repo stars](https://img.shields.io/github/stars/TheFlightSims/windowsserver-mgmttools)

# The Repository

***This repository presents to you a collection of server management tools for various purposes:***

* *Checking Group Policy vulnerabilities*
* *Passthrough physical device into the virtual machine*
* *Activation services for the server*
* *Scanning open ports on the target*

For better understanding, you can see the [wiki page](https://github.com/TheFlightSims/windowsserver-mgmttools/wiki).

## Use and install

***To run and use applications in this project, make sure your computer is installed with the features:***

* dotNET 2.1, dotNET 3.5, dotNET 4.8. These are built-in features and can be enabled. You can see the document [here](https://learn.microsoft.com/en-us/dotnet/framework/install/dotnet-35-windows)
* Visual C++ 2015 and all versions later. You can download it [here](https://learn.microsoft.com/en-US/cpp/windows/latest-supported-vc-redist?view=msvc-170)

## The Files and Folders

***The repo consists of three parts***

* **[.makevs](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/.makevs):** Make Visual Studio offline layout. It is recommended to fully edit this project.
* **winser-mgmttools.sln:** solution files, contains all required paths for all projects.
* **The rest of the files and folders:** Project files and folders.

## Contributing & Feedback

### Contributing

To fully edit this repository, run to download all layouts for Visual Studio 2022 that are located in **[!makevs!](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/!makevs!)**.
Also, you may need [Advanced Installer](https://www.advancedinstaller.com/) to create an installer. These are all located in the folder [!bin!](https://github.com/TheFlightSims/windowsserver-mgmttools/tree/master/!bin!) in each project.

To clone this project, you need to clone all its base and submodules, following these commands:

```bash
git clone https://github.com/TheFlightSims/windowsserver-mgmttools.git
git submodule update --init --recursive
```

Otherwise, you can do other ways:

* [Review Wiki Page](https://github.com/TheFlightSims/windowsserver-mgmttools/wiki)
* Submit bugs and feature requests in [GitHub](https://github.com/TheFlightSims/windowsserver-mgmttools/issues)
* [Review source codes](https://github.com/TheFlightSims/windowsserver-mgmttools)
* [Review and commit pull requests](https://github.com/TheFlightSims/windowsserver-mgmttools/pulls)

> *Note that the Visual Studio workload can be really heavy: requires around 20-40Gb for all required libraries*

### Feedback

To feedback on this repository, go to [**feedback**](https://github.com/TheFlightSims/windowsserver-mgmttools/issues)

## License & Original Contributors

### License

This repo uses [**MIT License**](https://opensource.org/licenses/MIT) for all contributors to have free, no limit to

* Use (for both private and commercial uses)
* Modify (including editing the source files)
* Distribution without permission directly from TheFlightSims, except emergency revokes permission. In that case, this repo will be moved to private and cannot be accessed by the public.
