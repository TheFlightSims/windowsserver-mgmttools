@echo off

setlocal enabledelayedexpansion

set PATH_OUTPUT="%~dp0version.props"

if exist %PATH_OUTPUT% (
    del /Q %PATH_OUTPUT%
)

for /F "tokens=*" %%a IN ('git.exe describe HEAD --tags --long') do set INFORMATIONAL_VERSION=%%a
if "%INFORMATIONAL_VERSION%" == "" (
    set INFORMATIONAL_VERSION=0.0.0.1-dev
) else (
    set INFORMATIONAL_VERSION=!INFORMATIONAL_VERSION:~1!
)

for /f "tokens=1 delims=-" %%a IN ("%INFORMATIONAL_VERSION%") do set VERSION=%%a

(
echo ^<Project^>
echo ^ ^ ^<PropertyGroup^>
echo ^ ^ ^ ^ ^<AssemblyVersion^>%VERSION%^</AssemblyVersion^>
echo ^ ^ ^ ^ ^<FileVersion^>%VERSION%^</FileVersion^>
echo ^ ^ ^ ^ ^<Version^>%VERSION%^</Version^>
echo ^ ^ ^ ^ ^<InformationalVersion^>%INFORMATIONAL_VERSION%^</InformationalVersion^>
echo ^ ^ ^</PropertyGroup^>
echo ^</Project^>
)>%PATH_OUTPUT%

echo VersionInfo: Wrote props for %INFORMATIONAL_VERSION%.