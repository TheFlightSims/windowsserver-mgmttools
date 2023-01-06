Option Explicit

Const Banner1 = _
"Aunty Mel's Cheap And Nasty SxS Package Extractor, 2013/11/09"
Const Banner2 = _
"Copyright (C) 2012-2013 Melinda Bellemore. All rights reserved."

Const Line79 =_
"-------------------------------------------------------------------------------"
Const Space79 =_
"                                                                               "

Const IdentityPath = "//assembly/assemblyIdentity"
Const DependencyPath = "//assembly/dependency/dependentAssembly/assemblyIdentity"
Const PackagePath = "//assembly/package/update/package/assemblyIdentity"
Const ComponentPath = "//assembly/package/update/component/assemblyIdentity"
Const DriverPath = "//assembly/package/update/driver/assemblyIdentity"

Const ForReading = 1
Const ForWriting = 2
Const ForAppending = 8

Const FileFlagNone = 0
Const FileFlagError = 1
Const FileFlagCompressed = 2

Const MAX_PATH = 260

Dim Shell
Dim FSO

Dim TempFolder
Dim DebugMode
Dim SystemRoot
Dim IncludeRes
Dim ViciousHacks

Dim SxSExpandAvailable
Dim CABArcAvailable

Dim MakingCAB
Dim InputPath
Dim OutputPath

Dim SwitchLoop
Dim ParamIndex

Dim CopyList

' Simple logging functions.
Sub LogBare(AText)
	WScript.Echo AText
End Sub

Sub LogInfo(AText)
	Call LogBare("[Information] " & AText)
End Sub

Sub LogError(AText)
	Call LogBare("[   Error   ] " & AText)
End Sub

Sub LogFatal(AText)
	Call LogBare("[Fatal error] " & AText)
End Sub

Sub LogDebug(AText)
	If DebugMode Then
		Call LogBare("[   Debug   ] " & AText)
	End If
End Sub


' Adds a file or folder to the copy list, with error-checking to work around
' the fact that VBScript doesn't like having the same "key" added multiple
' times.
Sub CopyListAdd(ASource, ATarget)
	If CopyList Is NOTHING Then
		Exit Sub
	End If

	If CopyList.Exists(ASource) Then
		Call LogDebug("CopyList key already exists: " & ASource)
	Else
		CopyList.Add ASource, ATarget
	End If
End Sub


' Checks whether the file is compressed with Microsoft's new SxS compression
' scheme.
Function IsCompressed(AFileName)
	Dim File
	Dim Signature

	Set File = FSO.OpenTextFile(AFileName, ForReading)
	Signature = File.Read(4)
	File.Close
	Set File = NOTHING

	' I've seen these signatures. I wonder if there's more?
	If Signature = "DCM" & Chr(1) Then
		IsCompressed = TRUE
	ElseIf Signature = "DCN" & Chr(1) Then
		IsCompressed = TRUE
	ElseIf Signature = "PA30" Then
		IsCompressed = TRUE
	Else
		IsCompressed = FALSE
	End If
End Function

' Calls my tool to expand a file compressed by Microsoft's new SxS compression
' scheme.
Function SxSExpand(AFileName)
	Dim TheCommand
	Dim WinStyle
	Dim ReturnCode

	Dim TempFileName

	SxSExpand = EMPTY

	' Die if SxS Expand isn't available.
	If Not SxSExpandAvailable Then
		Exit Function
	End If

	' Make a temporary file to expand to.
	Do
		TempFileName = FSO.BuildPath(TempFolder, FSO.GetTempName)
	Loop Until _
		(Not FSO.FileExists(TempFileName))

	If DebugMode Then
		WinStyle = 1
		TheCommand = "SXSEXPAND.EXE /DEBUG """ & AFileName & """ """ & _
			TempFileName & """"
	Else
		WinStyle = 0
		TheCommand = "SXSEXPAND.EXE """ & AFileName & """ """ & _
			TempFileName & """"
	End If
	Call LogDebug("Executing: " & TheCommand)

	ReturnCode = Shell.Run(TheCommand, WinStyle, TRUE)
	If ReturnCode = 0 Then
		SxSExpand = TempFileName
	Else
		Call LogError("SxSExpand call failed - error code: " & _
			CStr(ReturnCode))
	End If
End Function


' Check if the external tools are available.
Sub CheckExternals
	Dim ReturnCode

	Call LogDebug("Checking for external tools - SXSEXPAND.EXE and CABARC.EXE.")

	' Use ridiculous return codes to be sure.
	ReturnCode = -1.17
	On Error Resume Next
	ReturnCode = Shell.Run("SXSEXPAND.EXE", 0, TRUE)
	On Error Goto 0
	If ReturnCode = -1.17 Then
		SxSExpandAvailable = FALSE
	Else
		SxSExpandAvailable = TRUE
	End If
	ReturnCode = -3.14
	On Error Resume Next
	ReturnCode = Shell.Run("CABARC.EXE", 0, TRUE)
	On Error Goto 0
	If ReturnCode = -3.14 Then
		CABArcAvailable = FALSE
	Else
		CABArcAvailable = TRUE
	End If
End Sub


' Use SUBST.EXE for path shortening. VBScript has a tight limit (260
' characters) on path lengths.
Function Subst(APath)
	Dim TheCommand
	Dim WinStyle
	Dim ReturnCode

	Dim FoundDrive
	Dim Drive

	Subst = EMPTY

	' Find an unused drive letter.
	Drive = "A"
	Do While ((FSO.DriveExists(Drive)) And (Asc(Drive) < (Asc("Z") + 1)))
		Drive = Chr(Asc(Drive) + 1)
	Loop

	' All drive letters might be in use.
	If Asc(Drive) = (Asc("Z") + 1) Then
		Call LogError("Couldn't find an unused drive letter - will perform " & _
			"file copying using full paths.")
		Subst = APath
		Exit Function
	End If

	' Call SUBST to do the deed.
	If DebugMode Then
		WinStyle = 1
	Else
		WinStyle = 0
	End If
	TheCommand = "SUBST.EXE " & Drive & ": """ & APath & """"

	Call LogDebug("Executing: " & TheCommand)
	ReturnCode = Shell.Run(TheCommand, WinStyle, TRUE)
	If ReturnCode = 0 Then
		Subst = Drive & ":\"
		Call LogInfo("SUBST called: " & APath & " associated with " & _
			Subst & ".")
	Else
		Call LogError("SUBST failed - will perform file copying using full " & _
			"paths. Error code: " & CStr(ReturnCode))
		Subst = APath
	End If
End Function

Sub UnSubst(APath)
	Dim TheCommand
	Dim WinStyle
	Dim ReturnCode

	If DebugMode Then
		WinStyle = 1
	Else
		WinStyle = 0
	End If
	TheCommand = "SUBST.EXE " & Left(APath, 2) & " /D"

	Call LogDebug("Executing: " & TheCommand)
	ReturnCode = Shell.Run(TheCommand, WinStyle, TRUE)
	If ReturnCode = 0 Then
		Call LogInfo("SUBST /D called: " & APath & " deassociated.")
	Else
		Call LogError("SUBST /D failed - error code: " & CStr(ReturnCode))
	End If
End Sub

Function CreatePackageID(APackageName, APublicKeyToken, AArch, ALang, _
	AVersion)
	CreatePackageID = APackageName
	CreatePackageID = CreatePackageID & "~"

	CreatePackageID = CreatePackageID & APublicKeyToken
	CreatePackageID = CreatePackageID & "~"

	CreatePackageID = CreatePackageID & AArch
	CreatePackageID = CreatePackageID & "~"

	If (Not (StrComp(ALang, "neutral", vbTextCompare) = 0)) And _
		(Not (StrComp(ALang, "none", vbTextCompare) = 0))Then
		CreatePackageID = CreatePackageID & ALang
	End If
	CreatePackageID = CreatePackageID & "~"

	CreatePackageID = CreatePackageID & AVersion
End Function

Function CreateAssemblyIDWildcard(APackageName, APublicKeyToken, AArch, _
	ALang, AVersion)
	Const HashWildcard = "????????????????"

	CreateAssemblyIDWildcard = AArch
	CreateAssemblyIDWildcard = CreateAssemblyIDWildcard & "_"

	If Len(APackageName) > 40 Then
		CreateAssemblyIDWildcard = CreateAssemblyIDWildcard & _
			Left(APackageName, 19) & ".." & Right(APackageName, 19)
	Else
		CreateAssemblyIDWildcard = CreateAssemblyIDWildcard & APackageName
	End If
	CreateAssemblyIDWildcard = CreateAssemblyIDWildcard & "_"

	CreateAssemblyIDWildcard = CreateAssemblyIDWildcard & APublicKeyToken
	CreateAssemblyIDWildcard = CreateAssemblyIDWildcard & "_"

	CreateAssemblyIDWildcard = CreateAssemblyIDWildcard & AVersion
	CreateAssemblyIDWildcard = CreateAssemblyIDWildcard & "_"

	If (ALang = "") Or (IsEmpty(ALang)) Or (StrComp(ALang, "neutral", _
		vbTextCompare) = 0) Then
		CreateAssemblyIDWildcard = CreateAssemblyIDWildcard & "none"
	Else
		CreateAssemblyIDWildcard = CreateAssemblyIDWildcard & ALang
	End If
	CreateAssemblyIDWildcard = CreateAssemblyIDWildcard & "_"
	CreateAssemblyIDWildcard = CreateAssemblyIDWildcard & HashWildcard
End Function


Function MatchWildcard(AFileName)
	Dim ThePath
	Dim Pattern
	Dim TheCommand
	Dim WinStyle

	Dim TempFileName
	Dim ReturnCode
	Dim DIROutput

	Dim Matches()
	Dim MatchIndex

	Dim BackslashPosition

	MatchWildcard = EMPTY
	MatchIndex = 0

	' No wildcard. Fast exit.
	If (InStr(1, AFileName, "*", vbTextCompare) = 0) And _
		(InStr(1, AFileName, "?", vbTextCompare) = 0) Then
		Call LogDebug("MatchWildcard called with non-wildcard pattern: " & _
			AFileName)
		If (FSO.FileExists(AFileName)) Or (FSO.FolderExists(AFileName)) Then
			' Turn the return value into an array.
			ReDim Matches(0)
			Matches(0) = AFileName
			MatchWildcard = Matches
		End If

		Exit Function
	End If

	' Can't have wildcard characters in anything but the last path element.
	BackslashPosition = InStrRev(AFileName, "\", -1, vbTextCompare)
	If BackslashPosition = 0 Then
		' No path? Use current folder.
		ThePath = FSO.GetFolder(".")
	Else
		If (Not (InStrRev(AFileName, "*", BackslashPosition, _
			vbTextCompare) = 0)) Or (Not (InStrRev(AFileName, "?", _
			BackslashPosition, vbTextCompare) = 0)) Then
			Call LogError("Invalid wildcard: " & AFileName)
			Exit Function
		End If
		ThePath = FSO.GetParentFolderName(AFileName)
	End If

	' Get the pattern - use "*" if there isn't one.
	Pattern = Mid(AFileName, BackslashPosition + 1)
	If Pattern = "" Then
		Pattern = "*"
	End If

	' Cop out: I started to write my own wildcard matcher. Being incredibly
	' lazy though, the moment I hit trouble, I fell back on using DIR - which
	' is also much faster than an interpreted language at checking thousands
	' of files. :)
	Call LogDebug("Matching wildcard: " & FSO.BuildPath(ThePath, Pattern))
	Do
		TempFileName = FSO.BuildPath(TempFolder, FSO.GetTempName)
	Loop Until _
		(Not FSO.FileExists(TempFileName))

	If DebugMode Then
		WinStyle = 1
	Else
		WinStyle = 0
	End If
	TheCommand = "CMD.EXE /C DIR /A /B """ & FSO.BuildPath(ThePath, _
		Pattern) & """ > """ & TempFileName & """"
	Call LogDebug("Executing: " & TheCommand)
	ReturnCode = Shell.Run(TheCommand, WinStyle, TRUE)

	' Error-check.
	If Not (FSO.FileExists(TempFileName)) Then
		Call LogError("Couldn't read folder contents: " & ThePath)
		Exit Function
	End If

	' If the DIR output isn't zero, let's get parsing!
	If Not (FSO.GetFile(TempFileName).Size = 0) Then
		Set DIROutput = FSO.OpenTextFile(TempFileName, ForReading)
		Do While (Not DIROutput.AtEndOfStream)
			ReDim Preserve Matches(MatchIndex)
			Matches(MatchIndex) = FSO.BuildPath(ThePath, DIROutput.ReadLine)
			Call LogDebug("Wildcard match: " & Matches(MatchIndex))
			MatchIndex = MatchIndex + 1
		Loop
		DIROutput.Close
		Set DIROutput = NOTHING
	End If

	' Delete temporary file.
	FSO.GetFile(TempFileName).Delete TRUE

	' Any matches are returned as an array, even if there was only a single
	' match - makes it easier for the main loop.
	If Not (MatchIndex = 0) Then
		MatchWildcard = Matches
	End If
End Function

Function FindPossibleManifestFiles(APackageName, APublicKeyToken, AArch, _
	ALang, AVersion)
	Dim WorkPath
	Dim TempArray

	FindPossibleManifestFiles = EMPTY
	Call LogInfo("Finding matching manifest for assembly reference: " & _
		APackageName & "," & APublicKeyToken & "," & AArch & "," & ALang & _
		"," & AVersion)

	' First try the package folder.
	WorkPath = FSO.BuildPath(SystemRoot, "\Servicing\Packages")
	FindPossibleManifestFiles = FSO.BuildPath(WorkPath, _
		CreatePackageID(APackageName, APublicKeyToken, AArch, ALang, _
		AVersion) & ".mum")

	Call LogDebug("Trying file name: " & FindPossibleManifestFiles)
	If FSO.FileExists(FindPossibleManifestFiles) Then
		' Turn the return value into an array.
		ReDim TempArray(0)
		TempArray(0) = FindPossibleManifestFiles
		FindPossibleManifestFiles = TempArray
		Exit Function
	End If

	' Try the SxS manifest folder (by wildcard, because of those hashes).
	WorkPath = FSO.BuildPath(SystemRoot, "\WinSxS\Manifests")
	FindPossibleManifestFiles = FSO.BuildPath(WorkPath, _
		CreateAssemblyIDWildcard(APackageName, APublicKeyToken, AArch, _
		ALang, AVersion) & ".manifest")

	Call LogDebug("Trying wildcard: " & FindPossibleManifestFiles)
	FindPossibleManifestFiles = MatchWildcard(FindPossibleManifestFiles)
End Function


Sub FindReferencedAssemblies(AXML, APath)
	Dim ElementList
	Dim CurrentElement

	Dim ReferencedAssemblyName
	Dim ReferencedPublicKeyToken
	Dim ReferencedArch
	Dim ReferencedLang
	Dim ReferencedVersion

	Call LogDebug("Checking XML path for assembly references: " & APath)

	Set ElementList = AXML.DocumentElement.SelectNodes(APath)
	For Each CurrentElement In ElementList
		ReferencedAssemblyName = CurrentElement.GetAttribute("name")
		ReferencedPublicKeyToken = CurrentElement.GetAttribute("publicKeyToken")
		ReferencedArch = CurrentElement.GetAttribute("processorArchitecture")
		ReferencedLang = CurrentElement.GetAttribute("language")
		ReferencedVersion = CurrentElement.GetAttribute("version")

		If (ReferencedLang = "*") And (Not IncludeRes) Then
			' Handle /INCLUDERES switch by skipping this reference.
			Call LogError("Appears to be a MUI reference - skipping: " & _
				ReferencedAssemblyName)
		Else
			Call RecurseManifestHierarchy(ReferencedAssemblyName, _
				ReferencedPublicKeyToken, ReferencedArch, ReferencedLang, _
				ReferencedVersion)
		End If
	Next
End Sub

Sub ExtractPossibleAssemblyFolders(AWildcard)
	Dim Loop2

	Dim SourceList
	Dim TargetFolder

	SourceList = MatchWildcard(AWildcard)
	If Not IsEmpty(SourceList) Then
		For Loop2 = 0 To UBound(SourceList)
			' Only copy folders (hence this extra check).
			If FSO.FolderExists(SourceList(Loop2)) Then
				Call LogInfo("Found associated assembly folder: " & _
					FSO.GetFileName(SourceList(Loop2)))
				TargetFolder = FSO.GetFileName(SourceList(Loop2))
				Call CopyListAdd(SourceList(Loop2), TargetFolder)
			End If
		Next
	End If
End Sub

Function RecurseManifestHierarchy(APackageName, APublicKeyToken, AArch, ALang, AVersion)
	Dim Loop1

	Dim XML

	Dim FirstFile
	Dim FileList

	Dim ElementList
	Dim CurrentElement

	Dim AssemblyName
	Dim AssemblyPublicKeyToken
	Dim AssemblyArch
	Dim AssemblyLang
	Dim AssemblyVersion

	Dim SourceFile
	Dim TargetFile

	Dim SxSPath

	Dim CurrentManifest
	Dim FileFlag
	Dim ArraySize

	SxSPath = FSO.BuildPath(SystemRoot, "\WinSxS")
	RecurseManifestHierarchy = FALSE

	Set XML = CreateObject("MSXML2.DOMDocument")
	If Err.Number <> 0 Then
		Call LogFatal("Couldn't initialise MSXML. Error code: " & _
			CStr(Err.Number))
		WScript.Quit 1
	End If

	' On first call, most of this will be empty. Just use the package name
	' argument as the file name.
	If IsEmpty(APublicKeyToken) And IsEmpty(AArch) And IsEmpty(ALang) And _
		IsEmpty(AVersion) Then
		FirstFile = TRUE
		If FSO.FileExists(APackageName) Then
			ReDim FileList(0)
			FileList(0) = APackageName
		Else
			FileList = EMPTY
		End If
	Else
		' Build manifest file name.
		FileList = FindPossibleManifestFiles(APackageName, APublicKeyToken, _
			AArch, ALang, AVersion)
		FirstFile = FALSE
	End If

	' Jump ship if not found.
	If IsEmpty(FileList) Then
		Call LogError("Couldn't find a matching manifest.")
		Exit Function
	End If

	For Loop1 = 0 To UBound(FileList)
		FileFlag = FileFlagNone
		CurrentManifest = FileList(Loop1)

		' If it's compressed, decompress to temporary folder for parsing.
		If IsCompressed(CurrentManifest) Then
			If Not SxSExpandAvailable Then
				Call LogError("SxS File Expander not available - can't " & _
					"access compressed files.")
				FileFlag = FileFlag Or FileFlagError
			Else
				CurrentManifest = SxSExpand(CurrentManifest)
				If IsEmpty(CurrentManifest) Then
					Call LogError("Couldn't decompress manifest.")
					FileFlag = FileFlag Or FileFlagError
				Else
					Call LogDebug("Decompressed manifest to temporary " & _
						"file: " & CurrentManifest)
					FileFlag = FileFlag Or FileFlagCompressed
				End If
			End If
		End If

		If (FileFlag And FileFlagError) = 0 Then
			' Load the manifest, and leave if anything goes wrong.
			Call LogInfo("Loading manifest: " & FileList(Loop1))
			XML.Async = FALSE
			XML.Load CurrentManifest
			If Err.Number <> 0 Then
				Call LogError("Couldn't load manifest. Error := " & Err.Number)
				FileFlag = FileFlag Or FileFlagError
			End If
		End If

		If (FileFlag And FileFlagError) = 0 Then
			If XML.DocumentElement Is NOTHING Then
				Call LogError("Couldn't load manifest - probably missing or corrupt.")
				FileFlag = FileFlag Or FileFlagError
			End If
		End If

		If (FileFlag And FileFlagError) = 0 Then
			' Get all the information required...
			Set CurrentElement = XML.DocumentElement.SelectSingleNode(IdentityPath)
			AssemblyName = CurrentElement.GetAttribute("name")
			AssemblyPublicKeyToken = CurrentElement.GetAttribute("publicKeyToken")
			AssemblyArch = CurrentElement.GetAttribute("processorArchitecture")
			AssemblyLang = CurrentElement.GetAttribute("language")
			AssemblyVersion =  CurrentElement.GetAttribute("version")

			' And sanity check.
			If Not FirstFile Then
				If (APackageName = AssemblyName) And _
					(APublicKeyToken = AssemblyPublicKeyToken) And _
					(AArch = AssemblyArch) And _
					(AVersion = AssemblyVersion) Then
					' Handle wildcard language references. Allows extraction
					' of Snipping Tool.
					If Not (ALang = "*") Then
						If Not (ALang = AssemblyLang) Then
							FileFlag = FileFlag Or FileFlagError
						End If
					End If
				Else
					FileFlag = FileFlag Or FileFlagError
				End If

				If (FileFlag And FileFlagError) = 0 Then
					Call LogDebug("Manifest matches parent assembly reference.")
				Else
					Call LogError("Appear to have loaded the wrong " & _
						"manifest - skipping.")
				End If
			End If
		End If

		If (FileFlag And FileFlagError) = 0 Then
			' Give some text to the user for head-scratching. :)
			Call LogBare("")
			Call LogBare(AssemblyName)
			Call LogBare(Line79)
			Call LogBare(Left("Version:          " & AssemblyVersion & _
				Space79, 39) & " Architecture:     " & AssemblyArch)
			Call LogBare(Left("Language:         " & AssemblyLang & _
				Space79, 39) & " Public key token: " & AssemblyPublicKeyToken)
			Call LogBare("")

			' Fill associative array with file (and folders) to copy.
			If StrComp(FSO.GetExtensionName(FileList(Loop1)), "mum", vbTextCompare) = 0 Then
				' If it's a .mum file, also copy associated catalogue file.
				SourceFile = Replace(FileList(Loop1), ".mum", ".cat", 1, -1, vbTextCompare)
				If FirstFile Then
					' First file is always called 'update.*'.
					TargetFile = "update.cat"
				Else
					TargetFile = FSO.GetFileName(SourceFile)
				End If
				If FSO.FileExists(SourceFile) Then
					Call CopyListAdd(SourceFile, TargetFile)
				End If
			End If

			' First file is always called 'update.*'.
			SourceFile = FileList(Loop1)
			If FirstFile Then
				TargetFile = "update.mum"
			Else
				TargetFile = FSO.GetFileName(SourceFile)
			End If

			If FSO.FileExists(SourceFile) Then
				Call CopyListAdd(SourceFile, TargetFile)
			End If
	
			' Copy any SxS assembly folders (of course). If it's a .mum file, convert
			' the file name to assembly ID format. Manifests are already in assembly
			' ID format, but I prefix match them anyway.
			Call LogInfo("Finding possible associated assembly folders.")
			ArraySize = CopyList.Count
			Call ExtractPossibleAssemblyFolders(FSO.BuildPath(SxSPath, _
				CreateAssemblyIDWildcard(AssemblyName, AssemblyPublicKeyToken, _
				AssemblyArch, AssemblyLang, AssemblyVersion)))

			If ViciousHacks Then
				' VICIOUS HACK to allow extraction of TFTP Client.
				If StrComp(Replace(AssemblyName, "-Package", "", 1, -1, _
					vbTextCompare), AssemblyName) = 0 Then
					Call LogInfo("Vicious hack: add '-Package' to " & _
						"assembly name.")
					Call ExtractPossibleAssemblyFolders(FSO.BuildPath(SxSPath, _
						CreateAssemblyIDWildcard(AssemblyName & "-Package", _
						AssemblyPublicKeyToken, AssemblyArch, AssemblyLang, _
						AssemblyVersion)))
				End If

				' REALLY VICIOUS HACK to allow extraction of Adobe Flash for
				' Windows 8.x.
				If Not (StrComp(Replace(AssemblyName, "-Package", "", 1, _
					-1, vbTextCompare), AssemblyName) = 0) Then
					Call LogInfo("Vicious hack: remove '-Package' from " & _
						"assembly name.")
					Call ExtractPossibleAssemblyFolders(FSO.BuildPath(SxSPath, _
						CreateAssemblyIDWildcard(Replace(AssemblyName, _
						"-Package", "", 1, -1, vbTextCompare), _
						AssemblyPublicKeyToken, AssemblyArch, AssemblyLang, _
						AssemblyVersion)))
				End If
			End If

			' Provide message if nothing was copied.
			If ArraySize = CopyList.Count Then
				Call LogInfo("Couldn't find any associated assembly folders.")
			End If

			' And (possibly) recurse down for more references.
			Call FindReferencedAssemblies(XML, DependencyPath)
			Call FindReferencedAssemblies(XML, PackagePath)
			Call FindReferencedAssemblies(XML, ComponentPath)
			Call FindReferencedAssemblies(XML, DriverPath)
		End If

		' If this is a compressed manifest, delete the temporary.
		If Not (FileFlag And FileFlagCompressed) = 0 Then
			Call LogDebug("Deleting temporary manifest file: " & CurrentManifest)
			FSO.GetFile(CurrentManifest).Delete TRUE
		End If
	Next

	' Success!
	Set XML = NOTHING
	RecurseManifestHierarchy = TRUE
End Function


' File copy helper.
Function CopyObject(ASource, ATarget)
	Dim TargetFolder

	Dim SourceObj

	Dim SourcePath
	Dim TargetPath

	Dim FileCompressed
	Dim UncompSource

	CopyObject = EMPTY

	' Make sure the paths aren't too long.
	If Len(ASource) > MAX_PATH Then
		CopyObject = "path too long: " & ASource
		Exit Function
	End If
	If Len(ATarget) > MAX_PATH Then
		CopyObject = "path too long: " & ATarget
		Exit Function
	End If

	If FSO.FolderExists(ASource) Then
		' Create the target folder.
		Set TargetFolder = FSO.CreateFolder(ATarget)
		Call LogDebug("Created folder: " & TargetFolder.Path)

		' Recurse to copy folder to target.
		For Each SourceObj In FSO.GetFolder(ASource).Files
			SourcePath = SourceObj.Path
			TargetPath = FSO.BuildPath(TargetFolder.Path, SourceObj.Name)
			CopyObject = CopyObject(SourcePath, TargetPath)
			If Not IsEmpty(CopyObject) Then
				Exit Function
			End If
		Next
		For Each SourceObj In FSO.GetFolder(ASource).SubFolders
			SourcePath = SourceObj.Path
			TargetPath = FSO.BuildPath(TargetFolder.Path, SourceObj.Name)
			CopyObject = CopyObject(SourcePath, TargetPath)
			If Not IsEmpty(CopyObject) Then
				Exit Function
			End If
		Next
	ElseIf FSO.FileExists(ASource) Then
		' Copy file source to target.
		FileCompressed = IsCompressed(ASource)
		If FileCompressed Then
			Call LogInfo("Decompressing and copying file: " & ASource & _
				" --> " & ATarget)

			' Decompress source file.
			UncompSource = SxSExpand(ASource)
			If IsEmpty(UncompSource) Then
				FileCompressed = FALSE
				UncompSource = ASource
				Call LogDebug("Couldn't decompress file - copying it in " & _
					"compressed form.")
			End If
		Else
			Call LogInfo("Copying file: " & ASource & " --> " & ATarget)
			UncompSource = ASource
		End If
		
		On Error Resume Next
		FSO.GetFile(UncompSource).Copy ATarget, TRUE
		If Not (Err.Number = 0) Then
			CopyObject = Err.Description
			Exit Function
		End If
		On Error GoTo 0

		' Remove possible temporary uncompressed file.
		If FileCompressed Then
			Call LogDebug("Deleting temporary decompressed file: " & _
				UncompSource)
			FSO.GetFile(UncompSource).Delete TRUE
		End If

		CopyObject = EMPTY
	Else
		' Non-fatal.
		Call LogDebug("CopyObject called on non-existent/invalid file/folder: " & _
			ASource)
		CopyObject = EMPTY
	End If
End Function

Sub CopyPackage(AOutputPath)
	Dim Loop1

	Dim CopyKeys
	Dim CopyItems

	Dim COResult
	Dim OutputPath

	Dim Folder
	Dim Source
	Dim ObjType

	Dim BatchFileName
	Dim BatchFile
	Dim TheCommand
	Dim WinStyle

	Call LogInfo("Starting package creation. Number of files/folders: " & _
		CStr(CopyList.Count))

	' Create (possibly temporary) target folder.
	If MakingCAB Then
		Set Folder = FSO.CreateFolder(FSO.GetBaseName(AOutputPath))
		Call LogDebug("Created temporary target folder: " & Folder.Path)
	Else
		Set Folder = FSO.CreateFolder(AOutputPath)
		Call LogDebug("Created target folder: " & Folder.Path)
	End If
	OutputPath = Subst(Folder.Path)

	CopyKeys = CopyList.Keys
	CopyItems = CopyList.Items
	For Loop1 = 0 To CopyList.Count - 1
		If FSO.FileExists(CopyKeys(Loop1)) Then
			Set Source = FSO.GetFile(CopyKeys(Loop1))
		ElseIf FSO.FolderExists(CopyKeys(Loop1)) Then
			Set Source = FSO.GetFolder(CopyKeys(Loop1))
		End If

		COResult = CopyObject(Source, FSO.BuildPath(OutputPath, _
			CopyItems(Loop1)))
		If Not IsEmpty(COResult) Then
			Call LogFatal("Copy failure - " & COResult)

			' Remove partial target folder.
			Call UnSubst(OutputPath)
			Call LogInfo("Deleting incomplete target folder: " & Folder.Path)
			Folder.Delete TRUE
			WScript.Quit 1
		End If
	Next

	' Make a cabinet file.
	If MakingCAB Then
		Call LogInfo("Compressing folder to cabinet file: " & AOutputPath)

		' Generate command line parameters for CABARC.EXE.
		TheCommand = "-m LZX:21 -r -p N """ & _
			FSO.GetAbsolutePathName(AOutputPath) & """ *.*"
		Call LogDebug("Command-line parameters for CABARC.EXE: " & TheCommand)

		' Write a batch file to invoke CABARC.EXE.
		Do
			BatchFileName = FSO.BuildPath(TempFolder, FSO.GetTempName + ".BAT")
		Loop Until _
			(Not FSO.FileExists(BatchFileName))
		Set BatchFile = FSO.CreateTextFile(BatchFileName, TRUE, FALSE)
		BatchFile.WriteLine("@ECHO OFF")
		BatchFile.WriteLine("CD /D " & OutputPath)
		BatchFile.WriteLine("CABARC.EXE " & TheCommand)
		BatchFile.WriteLine("IF ERRORLEVEL 9009 GOTO RETRY")
		BatchFile.WriteLine("GOTO END")
		BatchFile.WriteLine(":RETRY")
		BatchFile.WriteLine("""" & FSO.BuildPath(FSO.GetFolder(".").Path, _
			"CABARC.EXE") & """ " & TheCommand)
		BatchFile.WriteLine(":END")
		BatchFile.Close
		Set BatchFile = NOTHING
		Call LogDebug("Created CABARC.EXE invocation batch file: " & _
			BatchFileName)

		' Invoke the created batch file..
		If DebugMode Then
			WinStyle = 1
		Else
			WinStyle = 0
		End If
		TheCommand = "CMD.EXE /C """ & BatchFileName & """"
		Call LogDebug("Executing: " & TheCommand)

		' Shell.Run on its own, without checking return code.
		Shell.Run TheCommand, WinStyle, TRUE

		Call UnSubst(OutputPath)
		If Not FSO.FileExists(AOutputPath) Then
			' Error creating cabinet. Leave the target folder for the user to
			' deal with.
			Call LogError("Couldn't create target cabinet - not deleting " & _
				"temporary target folder: " & Folder.Path)
		Else
			' Remove target folder, leaving just the cabinet.
			Call LogDebug("Deleting temporary target folder: " & Folder.Path)
			Folder.Delete TRUE
		End If

		' Remove invocation batch file.
		Call LogDebug("Deleting CABARC.EXE invocation batch file: " & _
			BatchFileName)
		FSO.GetFile(BatchFileName).Delete TRUE
	Else
		Call UnSubst(OutputPath)
	End If

	Set Folder = NOTHING
End Sub


' Neat routine that not only matches switches, but will return parameters
' specified by a switch if you set ASwitchLength properly.
Function MatchSwitch(AIndex, ASwitch, ASwitchLength)
	Dim SwitchText

	MatchSwitch = EMPTY

	' If the switch doesn't exist, it obviously can't be matched.
	If AIndex < WScript.Arguments.Count Then
		SwitchText = WScript.Arguments(AIndex)

		' Must start with a switch character.
		If (Not (Left(SwitchText, 1) = "/")) And _
			(Not (Left(SwitchText, 1) = "-")) Then
			Exit Function
		End If

		' Snip off the switch character for comparison.
		SwitchText = Mid(SwitchText, 2)

		If ASwitchLength = -1 Then
			' The easy one.
			If StrComp(SwitchText, ASwitch, vbTextCompare) = 0 Then
				MatchSwitch = ASwitch
				Exit Function
			End If
		Else
			' The hard one.			
			If StrComp(Left(SwitchText, ASwitchLength), ASwitch, _
				vbTextCompare) = 0 Then
				MatchSwitch = Mid(SwitchText, ASwitchLength + 1)
				Exit Function
			End If
		End If
	End If
End Function


' Big scary banner to frighten the user with. :P
Sub DisplayBanner
	Call LogBare(Banner1)
	Call LogBare(Banner2)
	Call LogBare("")
End Sub

' Help text.
Sub DisplayHelp
	Call LogBare("CSCRIPT.EXE SXSEXTRACT.VBS [/?,/H,/HELP] [/DEBUG,/V] </ONLINE|/IMAGE:<folder>>")
	Call LogBare("  [/INCLUDERES] [/VICIOUSHACKS] <source>.mum [<target>[.cab]]")
	Call LogBare("")
	Call LogBare("Parses a Windows Side-by-Side package manifest file, chases up all the")
	Call LogBare("references, and copies all the files associated with the package.")
	Call LogBare("")
	Call LogBare("  /?,/H,/HELP      Display this help text.")
	Call LogBare("  /DEBUG,/V        Enable verbose debug output.")
	Call LogBare("  /ONLINE          Uses %SYSTEMROOT% (usually C:\Windows) as the root folder to")
	Call LogBare("                   search for associated files.")
	Call LogBare("  /IMAGE:<folder>  Uses <folder> as the root folder to search for associated")
	Call LogBare("                   files instead.")
	Call LogBare("  /INCLUDERES      Extract MUI resources as well, for packages that don't have")
	Call LogBare("                   associated language packs.")
	Call LogBare("  /VICIOUSHACKS    Activates a couple of hacks to the script, for extracting")
	Call LogBare("                   'difficult' packages that don't reference assemblies by")
	Call LogBare("                   exact name.")
	Call LogBare("  <source>.mum     Specifies a package manifest file. Wildcards are not")
	Call LogBare("                   permitted.")
	Call LogBare("  <target>[.cab]   Specifies the target folder for extracted files. If the")
	Call LogBare("                   extension '.cab' is specified, the script will call")
	Call LogBare("                   CABARC.EXE to create a cabinet of the extracted files.")
	Call LogBare("")
	Call LogBare("This script requires both SXSEXPAND.EXE and CABARC.EXE, either in the")
	Call LogBare("current folder or on the system PATH, for full functionality.")
End Sub


' Script entry point.
Call DisplayBanner

ParamIndex = 0
MakingCAB = FALSE
SystemRoot = EMPTY
DebugMode = FALSE
IncludeRes = FALSE
ViciousHacks = FALSE

' Create useful objects.
Set Shell = CreateObject("WScript.Shell")
Set FSO = CreateObject("Scripting.FileSystemObject")

' Check on externals.
Call CheckExternals

' Populate useful variables.
TempFolder = Shell.ExpandEnvironmentStrings("%TEMP%")

' Parse command line.
For SwitchLoop = 0 To WScript.Arguments.Count - 1
	If (Not IsEmpty(MatchSwitch(SwitchLoop, "?", -1))) Or _
		(Not IsEmpty(MatchSwitch(SwitchLoop, "h", -1))) Or _
		(Not IsEmpty(MatchSwitch(SwitchLoop, "help", -1))) Then
		Call DisplayHelp
		WScript.Quit 0

	ElseIf (Not IsEmpty(MatchSwitch(SwitchLoop, "debug", -1))) Or _
		(Not IsEmpty(MatchSwitch(SwitchLoop, "v", -1))) Then
		DebugMode = TRUE

	ElseIf Not IsEmpty(MatchSwitch(SwitchLoop, "online", -1)) Then
		If Not IsEmpty(SystemRoot) Then
			Call LogFatal("Conflicting switch: " & WScript.Arguments(SwitchLoop))
			WScript.Quit 1
		End If
		SystemRoot = Shell.ExpandEnvironmentStrings("%SYSTEMROOT%")

	ElseIf Not IsEmpty(MatchSwitch(SwitchLoop, "image:", 6)) Then
		If Not IsEmpty(SystemRoot) Then
			Call LogFatal("Conflicting switch: " & WScript.Arguments(SwitchLoop))
			WScript.Quit 1
		End If
		SystemRoot = MatchSwitch(SwitchLoop, "image:", 6)

	ElseIf Not IsEmpty(MatchSwitch(SwitchLoop, "includeres", -1)) Then
		IncludeRes = TRUE

	ElseIf Not IsEmpty(MatchSwitch(SwitchLoop, "vicioushacks", -1)) Then
		ViciousHacks = TRUE

	Else
		' Might be the end of switches. Check.
		If (Left(WScript.Arguments(SwitchLoop), 1) = "/") Or _
			(Left(WScript.Arguments(SwitchLoop), 1) = "-") Then
			' No, it isn't.
			Call LogFatal("Unknown switch: " & WScript.Arguments(SwitchLoop))
			WScript.Quit 1
		Else
			' Move parameter pointer back, and exit loop.
			ParamIndex = ParamIndex - 1
			SwitchLoop = WScript.Arguments.Count - 1
		End If
	End If
	ParamIndex = ParamIndex + 1
Next

' Switch parsing complete. Parse the parameters.
If ParamIndex = WScript.Arguments.Count Then
	' No parameters.
	Call LogFatal("Package file not specified.")
	WScript.Quit 1
ElseIf (ParamIndex + 1) = WScript.Arguments.Count Then
	' One parameter. Assume it's the source package file, and base an target
	' upon it.
	InputPath = WScript.Arguments(ParamIndex)
	OutputPath = FSO.GetBaseName(InputPath)
	Call LogError("Target folder/file not specified, assuming: " & OutputPath)
ElseIf (ParamIndex + 2) = WScript.Arguments.Count Then
	' Both parameters: source package file and target path.
	InputPath = WScript.Arguments(ParamIndex)
	OutputPath = WScript.Arguments(ParamIndex + 1)
Else
	' Too many parameters!
	Call LogFatal("Too many parameters.")
	WScript.Quit 1
End If

' Check for obviously stupid/too long file names.
If (Not (InStr(InputPath, "*") = 0)) Or _
	(Not (InStr(InputPath, "?") = 0)) Or _
	(Len(InputPath) > MAX_PATH) Then
	Call LogFatal("Invalid package file name: " & InputPath)
	WScript.Quit 1
End If
If (Not (InStr(OutputPath, "*") = 0)) Or _
	(Not (InStr(OutputPath, "?") = 0)) Or _
	(Len(OutputPath) > MAX_PATH) Then
	Call LogFatal("Invalid target folder: " & OutputPath)
	WScript.Quit 1
End If

' Check that the package file actually exists.
If Not FSO.FileExists(InputPath) Then
	Call LogFatal("Package file doesn't exist: " & InputPath)
	WScript.Quit 1
End If

' Flag whether the script makes a cabinet or not.
If StrComp(FSO.GetExtensionName(OutputPath), "cab", vbTextCompare) = 0 Then
	If Not CABArcAvailable Then
		Call LogError("MS Cabinet Tool not available - can't create cabinet file.")
		MakingCAB = FALSE
	Else
		MakingCAB = TRUE
	End If
Else
	MakingCAB = FALSE
End If

' Make sure the target cabinet and/or folder don't already exist.
If MakingCAB Then
	If FSO.FileExists(OutputPath) Then
		Call LogFatal("Target file already exists: " & OutputPath)
		WScript.Quit 1
	End If
	If FSO.FolderExists(FSO.GetBaseName(OutputPath)) Then
		Call LogFatal("Temporary target folder already exists: " & _
			FSO.GetBaseName(OutputPath))
		WScript.Quit 1
	End If
Else
	If FSO.FolderExists(OutputPath) Then
		Call LogFatal("Target folder already exists: " & OutputPath)
		WScript.Quit 1
	End If
End If

' Check the external tools are available.
Call CheckExternals

' And start the ball rolling.
Set CopyList = CreateObject("Scripting.Dictionary")

Call RecurseManifestHierarchy(InputPath, EMPTY, EMPTY, EMPTY, EMPTY)
Call CopyPackage(OutputPath)

Set CopyList = NOTHING

Set FSO = NOTHING
Set Shell = NOTHING

WScript.Quit 0