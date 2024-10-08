# Contributing & Feedback

## Contributing to the source

Windows Server Management Tools repo uses Git submodules. It can reduce the load of non-essential projects you don't want to load. That means you can clone each repo of a part of a submodule, instead of cloning the whole project repo.

To contribute, you may need [Visual Studio 2022](https://visualstudio.microsoft.com/vs/). Required Visual Studio 2022 SDKs:

1. .NET 4.8 SDK and targeting pack
2. C++ Windows XP Support for VS 2017 (v141) Tools
3. Windows 10 SDK (10.0.18362.0)
4. Windows Universal C Runtime

### GitHub Desktop

In GitHub Desktop, consider using `Clone Repository\URL`, then enter this URL of git repo

```url
https://github.com/TheFlightSims/windowsserver-mgmttools.git
```

All submodules will be updated automatically. You don't need to use Git CLI.

### Git CLI

To clone this project using the Git command line, you need to clone all its base and submodules, following these commands:

```bash
git clone https://github.com/TheFlightSims/windowsserver-mgmttools.git
cd windowsserver-mgmttools # Make sure the repo is cloned successfully
git submodule update --init --recursive
```

## Other ways to contribute & feedback

Otherwise, you can do other ways:

* [Review Wiki Page](https://github.com/TheFlightSims/windowsserver-mgmttools/wiki)
* Submit bugs and feature requests in [GitHub](https://github.com/TheFlightSims/windowsserver-mgmttools/issues)
* [Review source codes](https://github.com/TheFlightSims/windowsserver-mgmttools)
* [Review and commit pull requests](https://github.com/TheFlightSims/windowsserver-mgmttools/pulls)
