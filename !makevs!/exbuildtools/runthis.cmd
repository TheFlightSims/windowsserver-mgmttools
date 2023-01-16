@echo off
echo ====================================================
echo +                                                  +
echo +                                                  +
echo +         ----------------------------------       +
echo +         + Visual Studio 2019 build tools +       +
echo +         ----------------------------------       +
echo +                                                  +
echo +  This script is trying to create offline layout  +
echo + for this repo.                                   +
echo +  Note that this make take few mins, so do not    +
echo + worry (around 50Gb)                              +
echo +                                                  +
echo ====================================================
cd /d %~dp0
goto start

:start
echo Debugging started...
@echo on
vs_BuildTools.exe --layout "%~dp0\buildtools" --add "Microsoft.VisualStudio.Component.Roslyn.Compiler --add Microsoft.Component.MSBuild --add Microsoft.VisualStudio.Component.CoreBuildTools --add Microsoft.VisualStudio.Workload.MSBuildTools --add Microsoft.Net.Component.4.8.SDK --add Microsoft.Net.Component.4.6.1.TargetingPack --add Microsoft.VisualStudio.Component.NuGet.BuildTools --add Microsoft.Net.Component.4.7.2.TargetingPack --add Microsoft.Net.Component.4.TargetingPack --add Microsoft.Net.Component.4.5.TargetingPack --add Microsoft.Net.Component.4.5.1.TargetingPack --add Microsoft.Net.Component.4.5.2.TargetingPack --add Microsoft.Net.Component.4.6.TargetingPack --add Microsoft.VisualStudio.Component.AspNet45 --add Microsoft.NetCore.Component.Runtime.3.1 --add Microsoft.NetCore.Component.Runtime.5.0 --add Microsoft.NetCore.Component.SDK --add Microsoft.Net.Component.3.5.DeveloperTools --add Microsoft.Net.Component.4.6.2.TargetingPack --add Microsoft.Net.Component.4.7.TargetingPack --add Microsoft.Net.Component.4.7.1.TargetingPack --add Microsoft.Net.Component.4.8.TargetingPack --add Microsoft.NetCore.BuildTools.ComponentGroup --add Microsoft.VisualStudio.Workload.NetCoreBuildTools --add Microsoft.Component.NetFX.Native --add Microsoft.Net.Component.4.7.2.SDK --add Microsoft.Net.Core.Component.SDK.3.0" --includeRecommended --includeOptional