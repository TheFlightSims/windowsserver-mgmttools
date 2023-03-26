::[Bat To Exe Converter]
::
::YAwzoRdxOk+EWAnk
::fBw5plQjdG8=
::YAwzuBVtJxjWCl3EqQJgSA==
::ZR4luwNxJguZRRnk
::Yhs/ulQjdF+5
::cxAkpRVqdFKZSjk=
::cBs/ulQjdF+5
::ZR41oxFsdFKZSDk=
::eBoioBt6dFKZSDk=
::cRo6pxp7LAbNWATEpSI=
::egkzugNsPRvcWATEpCI=
::dAsiuh18IRvcCxnZtBJQ
::cRYluBh/LU+EWAnk
::YxY4rhs+aU+IeA==
::cxY6rQJ7JhzQF1fEqQJhZksaHErSXA==
::ZQ05rAF9IBncCkqN+0xwdVsFAlTMbCXqZg==
::ZQ05rAF9IAHYFVzEqQISJxRHWETEfFiuB6cI/fi72/iIt08RUfE3d4DP36bu
::eg0/rx1wNQPfEVWB+kM9LVsJDCaNM3iuRvNc2/z6/f6Ssg0pRu0qeoPeybyBL/QW+AvhbZNN
::fBEirQZwNQPfEVWB+kM9LVsJDCaNM3iuRvNc2/z6/f6Ssg0pRu0qeoPeybyBL/QW+CU=
::cRolqwZ3JBvQF1fEqQJQ
::dhA7uBVwLU+EWG2M8WQ8IRxcWDeLMXja
::YQ03rBFzNR3SWATE80skIA5WAgeNMSSODrA65OH85/60q0AOG/U2dovUzaedJfIF71epeJEgwm5Vl8cfbA==
::dhAmsQZ3MwfNWATE02wFaDxkYETRcjva
::ZQ0/vhVqMQ3MEVWAtB9wSA==
::Zg8zqx1/OA3MEVWAtB9wSA==
::dhA7pRFwIByZRRnk
::Zh4grVQjdCuDJBqX+1ciKx4ZTwuGOVetD7sY5//o/O+VtEgPGe84dZvP1buCM9xS6EzqNKoP2XVJjItKTChMfwC/ex90nXxNo2aJMtSUvAD1T1q1xEQ/CHE0wSPjmCopdMZ42vcR3DW88Ejsl6ofxX3tEKwWEAM=
::YB416Ek+ZW8=
::
::
::978f952a14a936cc963da21a135fa983
@echo off
cd /d %~dp0
cls
title Boost and Startup Troubleshoot for Windows 10 and up
color 0E

:begin
echo The current time is: %time%
echo ----------------------------------------------------------------------
echo Make sure that this program is currently running as Administrator
echo and your Operating System is at least Windows 10/Server 2016 and up.
echo ----------------------------------------------------------------------
echo If you try to run in lower level of OS, this app won't run properly.
echo ----------------------------------------------------------------------
color 0F
goto start

:start
cd /d %~dp0 
echo Would you like to...
echo [1.] Fix error when Booting to Windows.
echo [2.] Just optimize my disk.
echo [3.] Just clean up the trash on my PC.
echo [4.] No, I don't have any problem with my PC.
echo *Note: Press other keys that aren't listed, the program will exit, or
echo automatic choose vaild option.*
choice /n /c 1234
if ERRORLEVEL 4 goto :exit 4
if ERRORLEVEL 3 goto :sol3 3
if ERRORLEVEL 2 goto :sol2 2
if ERRORLEVEL 1 goto :sol1 1


:sol1
cls
cd /d %~dp0
color 0E
echo ----------------------------------------------------------------------
echo Checking Local Disk Health...
echo ----------------------------------------------------------------------
color 0F
sfc /scannow
cls
color 0E
echo ----------------------------------------------------------------------
echo We just scanned your disk. Now we'll restore your important files 
echo you've lost (If we found)
echo .
echo This progress may take about 1-2 hours. 
echo Although your PC may appear inactive at times, the progress is still  
echo running. 
echo Please keep your PC always on, close all tasks (such as Edge, OBS
echo GIMP, Chrome, Blender,...), keep your PC idle. Otherwise the your 
echo system won't boot.
echo Thanks you.
echo ----------------------------------------------------------------------
color 0F
cd C:\Windows\System32
dism.exe /Online /Cleanup-Image /RestoreHealth
timeout 5
cls
color 0E
echo ----------------------------------------------------------------------
echo Cleaning up trash...
echo ----------------------------------------------------------------------
color 0F
rd "%USERPROFILE%\AppData\Local\Microsoft\Windows\INetCache\.\" /s /q
rd "%USERPROFILE%\AppData\Local\Microsoft\Windows\INetCookies\.\" /s /q
rd "%USERPROFILE%\AppData\Local\Temp\.\" /s /q
cls
cd C:\Windows\System32
cleanmgr.exe /dC: /VERYLOWDISK
cls
color 0E
echo ----------------------------------------------------------------------
echo Defragmenting your disk...This progress may take an hour...
echo ----------------------------------------------------------------------
color 0F
defrag C: /o
timeout 3
cls
color 0E
echo ----------------------------------------------------------------------
echo You've completed Booting to Windows troubleshooting
echo Now, let's check if your Windows is fixed.
echo .
echo Please move around by opening some programs you usually use.
echo ----------------------------------------------------------------------
color 0F
echo Did we fix your problem on your PC? (Yes/No)
set /p typesol1=
if %typesol1%==Yes goto restart-windows
if %typesol1%==y goto restart-windows
if %typesol1%==No goto restart-troubleshoot
if %typesol1%==n goto restart-troubleshoot

:sol2
cd /d %~dp0
cls
color 0E
echo ----------------------------------------------------------------------
echo Cleaning up trash...
echo ----------------------------------------------------------------------
color 0F
rd "%USERPROFILE%\AppData\Local\Microsoft\Windows\INetCache\.\" /s /q
rd "%USERPROFILE%\AppData\Local\Microsoft\Windows\INetCookies\.\" /s /q
rd "%USERPROFILE%\AppData\Local\Temp\.\" /s /q
cls
cd C:\Windows\System32
cleanmgr.exe /dC: /VERYLOWDISK
cls
color 0E
echo ----------------------------------------------------------------------
echo Defragmenting your disk...This progress may take an hour...
echo ----------------------------------------------------------------------
color 0F
defrag C: /o
timeout 3
cls
color 0E
echo ----------------------------------------------------------------------
echo Optimization has been completed.
echo ----------------------------------------------------------------------
color 0F
timeout 5
cls
color 0E
echo Do you want to restart the troubleshooting again? (Restart/Exit)
color 0F
set /p typesol2=
if %typesol2%==Restart goto re-start
if %typesol2%==Exit goto exit

:sol3
cd /d %~dp0
cls
color 0E
echo ----------------------------------------------------------------------
echo Cleaning up trash...
echo ----------------------------------------------------------------------
color 0F
rd "%USERPROFILE%\AppData\Local\Microsoft\Windows\INetCache\.\" /s /q
rd "%USERPROFILE%\AppData\Local\Microsoft\Windows\INetCookies\.\" /s /q
rd "%USERPROFILE%\AppData\Local\Temp\.\" /s /q
cls
cd C:\Windows\System32
cleanmgr.exe /dC: /VERYLOWDISK
echo Do you want to restart the troubleshooting again? (Restart/Exit)
set /p typesol3=
if %typesol3%==Restart goto re-start
if %typesol3%==Exit goto exit

:restart-windows
cls
echo ----------------------------------------------------------------------
echo A restart is required. Close all programs, save your works!
echo ----------------------------------------------------------------------
pause
shutdown -r
exit

:restart-troubleshoot
cls
color 0E
echo Sorry, we cannot help you. Do you want to restart the troubleshooting 
echo again? (Restart/Exit)
color 0F
set /p restart-troubleshoot1=
if %restart-troubleshoot1%==Restart goto re-start
if %restart-troubleshoot1%==Exit goto exit

:exit
cls
color 0E
echo ----------------------------------------------------------------------
echo Exiting...
echo ----------------------------------------------------------------------
color 0F
timeout 2
exit

:re-start
cls
goto begin