REM
REM Autor: KNARZ
REM Purpose: Export Windows Packages (the easy way)
REM          The script is intended to work with default system UI language.
REM Credits and best whishes to Melinda / Aunty Mel.
REM Also some credit to Alex Ionescu
REM
REM V.1
REM

@echo off & cls & mode 80, 45 & color 1b
setlocal EnableDelayedExpansion

pushd %~dp0
call definitions.cmd

::Flags
set Loop=1
set Flag_AskImage=0
set Flag_Log=0

::Examples
::You can overwrite values.
REM set Flag_AskImage=0
set Path_Image=%SystemRoot%
REM set Path_Image=D:\Mount\Enterprise
REM set ImportList=C:\Kit\Win10\SomeList.txt

::More flags but not recommended to change
set Flag_NoAutoMUI=0
set Flag_CAB=1

::Workflow
call :AutoExportCheck %ImportList%
call :AskImagePath Path_Image
call :PackagesEnum "BaseMUM"

:Loop
call :SelectItem		"BaseMUM"
call :PackageExport		"%BaseMUM%"
if /i [%Flag_NoAutoMUI%] == [1] (
if NOT defined FirstRun	(call :PackagesEnum	MuiMUM "%MUI%")
	call :SelectItem	"MuiMUM"
	call :PackageExport	"!MuiMUM!"
) else (
	call :ShortBasicName	BaseMUM
	call :PackageAutoSearch	"!BaseMUM!" "~%MUI%~"
)
set FirstRun=1
call :VarClear BaseMUM
call :VarClear MuiMUM

if defined Loop goto :Loop
call :TextIntro "All Done..."

popd
endlocal
exit /b



::------------------------------------------------------------------------------
::Routines
::------------------------------------------------------------------------------
:AskImagePath
if NOT defined Path_Image if /i [%Flag_AskImage%] == [1] (
	call :TextIntro	"Please type the Windows Imagepath you want to use e.g.:"
	call :TextInfo	"'D:\Mount\Enterprise'"
	call :TextInfo	"C: &echo."
	call :TextInfo	"If you want to export matching languagepacks from mounted Image"
	call :TextOutro	"like: 'D:\Mount\Enterprise en-US'
	call :SelectImageSource	"%~1"
)
if NOT [%~2] == []	(set "MUI=%~2")
if NOT exist "%Path_Image%" (call :ThrowMessage "NotFound" "%Path_Image%")
goto :EOF

:AutoExportCheck
if exist "%~1" (
	call :TextIntro	"Auto Export Mode..."
	call :TextOutro	"echo. & echo Only displayed Packages are found and exported."
	for /f %%i in (%ImportList%) do (
		call :PackageAutoSearch "%%i" "~~"
		call :PackageAutoSearch "%%i" "~%MUI%~"
	)
	call :ThrowMessage "Done"
)
goto :EOF

:PackageAutoSearch
for /f "tokens=*" %%l in ('dir /b /o-d "%Path_Image%\%PathRel_Packages%\%~1~*%~2*.mum" 2^>nul') do (
	REM Unfortunatly with no Items 'for' stops immediately.
	if ErrorLevel 1 (goto :EOF)
	call :PackageExport "%%~nl"
)
goto :EOF

:PackageExport
for /f "tokens=1,3,4,5 delims=~" %%i in ("%~1") do (
	if [%%l] == [] (
	set Folder_PathExport=%%j\%%k
	set File_ExportName=%%i-%%j-%%k
	set "PackageName=%%i"
	) else (
	set Folder_PathExport=%%j\%%l\%%k
	set File_ExportName=%%i-%%j-%%l-%%k
	set "PackageName=%%i (%%k)"
	)
	echo In prog.: !PackageName!
)
if NOT exist "%Path_Export%\%Folder_PathExport%" (md "%Path_Export%\%Folder_PathExport%")
if /i [%Flag_CAB%] == [1] (set "Path_ExportFile=%Path_Export%\%Folder_PathExport%\%File_ExportName%.cab") else (set "Path_ExportFile=%Path_Export%\%Folder_PathExport%\%File_ExportName%")
if /i [%Flag_Log%] == [1] (set "LogFile=%Path_Export%\%Folder_PathExport%\%File_ExportName%.log") else (set "LogFile=nul")
%Tool_SxSExtract% /IMAGE:"%Path_Image%" %Parameter_SxSExtract% "%Path_Image%\%PathRel_Packages%\%~1.mum" "%Path_ExportFile%" > %LogFile%
timeout.exe /t 2 > nul
goto :EOF

:PackagesEnum
set Tip=
set Search=
if /i [%~1] == [BaseMUM] (
	set "Tip=Base-Package/s"
	set "Search=dir /b /o-d "%Path_Image%\%PathRel_Packages%\*Package~*~~*.mum^""
	set "File_PackageList=%Path_Root%\_Packagelist_Base.txt
	call :TextIntro	"Generating Base-Packagelist..."
)
if /i [%~1] == [MuiMUM] (
	set "Tip=MUI-Package/s"
	set "Search=dir /b /o-d "%Path_Image%\%PathRel_Packages%\*Package*~%MUI%~*.mum^""
	set "File_PackageList=%Path_Root%\Packagelist_%MUI%.txt
	call :TextIntro	" Generating MUI (%MUI%) Packagelist..."
)
call :Selection	"%~1" "!Search!"
call :TextInfo	"Please look in:"
call :TextInfo	"'%File_PackageList%'"
call :TextOutro	"and type the number of the !Tip:~,-2! you want to export."
goto :EOF

:Selection
type nul > %File_PackageList%
set /a CNT=0
for /f "tokens=*" %%f in ('%~2 ^| findstr.exe /i /v "_for_KB" ^| sort') do (
	set /a CNT+=1
	set "%~1!CNT!=%%~nf"
		for /f "tokens=1 delims=~" %%i in ("%%~f") do (
		REM echo 	!CNT!^) %%~i
		echo !CNT!;	%%~i>> %File_PackageList%
	)
)
echo.
goto :EOF

:SelectImageSource
set /p Imagepath=^>
if /i [%Imagepath%] == []		(call :ThrowMessage "Invalid")
set "%~1=%Imagepath%\Windows"
goto :EOF

:SelectItem
set /p SelNum=^>
if /i [%SelNum%] == []		(call :ThrowMessage "Invalid")
if /i [%SelNum%] == [X]		(call :ThrowMessage "Abort")
if %SelNum% LEQ 0			(call :ThrowMessage "Range")
if %SelNum% GTR %CNT%		(call :ThrowMessage "Range")
set "%~1=!%~1%SelNum%!"
set "SKIP="
REM timeout.exe /t 2 >nul
goto :EOF

:ShortBasicName
for /f "tokens=1 delims=~" %%i in ("!%~1!") do (set "%~1=%%i")
goto :EOF

:VarClear
set "%~1="
goto :EOF

:TextIntro
echo.
echo       %~1
echo %Line%
goto :EOF

:TextOutro
echo       %~1
echo %Line%
echo.
goto :EOF

:TextInfo
echo       %~1
goto :EOF

:ThrowMessage
echo.
if /i [%~1] == [Done] (
	echo All actions completed.
)
if /i [%~1] == [NotFound] (
	echo %~2 not found.
)
if /i [%~1] == [Invalid] (
	echo Invalid input.
)
if /i [%~1] == [Abort] (
	echo Process abort by user.
)
if /i [%~1] == [Range] (
	echo Selected number is out of range.
)
echo.
echo Press any key to terminate window...
pause >nul
exit