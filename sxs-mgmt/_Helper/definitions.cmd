set WD=%~dp0
call :EnumRootPath "Path_Root" "Start.cmd"

set Path_Helper=%WD:~0,-1%
set Path_Export=%Path_Root%\Output
set PathRel_Packages=Servicing\Packages
set Tool_SxSExpand=%Path_Helper%\SxSExpand.exe
set Tool_SxSExtract="%SystemRoot%\System32\cscript.exe" //NoLogo "%Path_Helper%\SxSExtract.vbs"
set Tool_TIWorker=%Path_Helper%\nsudoc.exe
REM set Tool_CabDir=%Path_Helper%\cabdir.exe
set Tool_CabArc=%Path_Helper%\cabarc.exe
set Script_Export=%Path_Helper%\export.cmd
set Parameter_SxSExtract=/VICIOUSHACKS
set ImportList=%Path_Root%\ImportList.txt
set Line=------------------------------------------------------------------------------

REM set MUI=en-US
call :EnumPrefLanguage MUI

exit /b

::------------------------------------------------------------------------------
::Subroutine
::------------------------------------------------------------------------------
:EnumPrefLanguage
for /f "tokens=6" %%m in ('dism.exe /English /Online /Get-Intl ^| find.exe /i "Default system UI language"') do (set "%~1=%%m")
REM echo Language: !%~1!
goto :EOF


:EnumRootPath 
for /f %%i in ('dir /b /s "%WD%..\%~2"') do (set "%~1=%%~dpi")
set %~1=!%~1:~0,-1!
goto :EOF