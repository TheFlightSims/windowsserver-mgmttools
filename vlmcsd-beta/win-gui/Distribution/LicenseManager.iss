#define ApplicationVersion GetFileVersion('..\LicenseManager\bin\Release\LicenseManager.exe')

[Setup]
AppName=License Manager
AppVersion={#ApplicationVersion}
AppVerName=License Manager {#ApplicationVersion}
AppCopyright=Copyright (c) 2012 - 2018, Hotbird HGM
DefaultDirName={pf}\Hotbird64\License Manager
UninstallDisplayIcon={app}\LicenseManager.exe
Compression=lzma2
SolidCompression=yes
DefaultGroupName=License Manager
AppPublisher=Hotbird HGM
SetupIconFile=..\LicenseManager\Icon1.ico
OutputBaseFilename=LicenseManager-Installer
OutputDir=.
AppUpdatesURL=https://forums.mydigitallife.net/threads/miscellaneous-kms-related-developments.52594/page-9999
AppSupportURL=https://forums.mydigitallife.net/threads/miscellaneous-kms-related-developments.52594/page-9999
AppPublisherURL=https://forums.mydigitallife.net
MinVersion=0,6.0.6002
WizardImageFile=Wizard.bmp
WizardImageStretch=yes
ArchitecturesInstallIn64BitMode=x64

[Files]
Source: "..\LicenseManager\bin\Release\LicenseManager.exe"; DestDir: "{app}"; Components: "lm";
Source: "vlmcsd\libkms32.dll"; DestDir: "{app}"; Components: "libkms";
Source: "vlmcsd\libkms64.dll"; DestDir: "{app}"; Check: IsWin64(); Components: "libkms";
Source: ".NET\dotNetFx45_Full_setup.exe"; DestDir: "{tmp}"; Check: not IsDotNetDetected('v4.5', 0); Components: "lm";
Source: "OpenVPN\tap-windows-9.24.2-I601-Win7.exe"; DestDir: "{tmp}"; Components: "vpn"
Source: "OpenVPN\tap-windows-9.24.2-I601-Win10.exe"; DestDir: "{tmp}"; Components: "vpn"
Source: "vlmcsd\vlmcs-Windows-x64.exe"; DestDir: "{app}"; DestName:"vlmcs.exe"; Components: "vlmcsd"; Check: IsWin64();
Source: "vlmcsd\vlmcsd-Windows-x64.exe"; DestDir: "{app}"; DestName:"vlmcsd.exe"; Components: "vlmcsd"; Check: IsWin64();
Source: "vlmcsd\vlmcsd.kmd"; DestDir: "{app}"; Components: "vlmcsd";
Source: "..\LicenseManager\KmsDataBase.xsd"; DestDir: "{app}"; Components: "lm";
Source: "..\LicenseManager\KmsDataBase.xml"; DestDir: "{app}"; Components: "lm";
Source: "vlmcsd\vlmcs-Windows-x86.exe"; DestDir: "{app}"; DestName:"vlmcs.exe"; Components: "vlmcsd"; Check: not IsWin64();
Source: "vlmcsd\vlmcsd-Windows-x86.exe"; DestDir: "{app}"; DestName:"vlmcsd.exe"; Components: "vlmcsd"; Check: not IsWin64();
Source: "vlmcsd\vlmcsd.ini"; DestDir:"{commonappdata}\Hotbird64\vlmcsd"; Components: "vlmcsd"; Flags: "onlyifdoesntexist ignoreversion uninsneveruninstall";
Source: "vlmcsd\vlmcsd.7.pdf"; DestDir: "{app}\Docs"; Components: "vlmcsd";
Source: "vlmcsd\vlmcsd.8.pdf"; DestDir: "{app}\Docs"; Components: "vlmcsd";
Source: "vlmcsd\vlmcs.1.pdf"; DestDir: "{app}\Docs"; Components: "vlmcsd";
Source: "vlmcsd\vlmcsd.ini.5.pdf"; DestDir: "{app}\Docs"; Components: "vlmcsd";

[Run]
Filename: "{tmp}\dotNetFx45_Full_setup.exe"; Description:"Install .NET Framework 4.5"; WorkingDir: "{tmp}"; Parameters: "/quiet";  StatusMsg: "Installing .NET Framework 4.5 ..."; Check: not IsDotNetDetected('v4.5', 0); BeforeInstall: SetMarqueeProgress(True); Flags: runhidden;
Filename: "{tmp}\tap-windows-9.24.2-I601-Win7.exe"; Description:"Install TAP adapter"; WorkingDir: "{tmp}"; Parameters: "/S";  StatusMsg: "Installing OpenVPN TAP Adapter ..."; BeforeInstall: SetMarqueeProgress(True); Tasks: "vpnsilent"; Components:"vpn"; OnlyBelowVersion: 10.0;
Filename: "{tmp}\tap-windows-9.24.2-I601-Win7.exe"; Description:"Install TAP adapter"; WorkingDir: "{tmp}"; StatusMsg: "Installing OpenVPN TAP Adapter ..."; BeforeInstall: SetMarqueeProgress(True); Tasks: "not vpnsilent"; Components:"vpn"; OnlyBelowVersion: 10.0;
Filename: "{tmp}\tap-windows-9.24.2-I601-Win10.exe"; Description:"Install TAP adapter"; WorkingDir: "{tmp}"; Parameters: "/S";  StatusMsg: "Installing OpenVPN TAP Adapter ..."; BeforeInstall: SetMarqueeProgress(True); Tasks: "vpnsilent"; Components:"vpn"; MinVersion: 10.0;
Filename: "{tmp}\tap-windows-9.24.2-I601-Win10.exe"; Description:"Install TAP adapter"; WorkingDir: "{tmp}"; StatusMsg: "Installing OpenVPN TAP Adapter ..."; BeforeInstall: SetMarqueeProgress(True); Tasks: "not vpnsilent"; Components:"vpn"; MinVersion: 10.0; 
Filename: "{app}\vlmcsd.exe"; Parameters: "-s -U /n -i ""{commonappdata}\Hotbird64\vlmcsd\vlmcsd.ini"""; StatusMsg: "Setting up vlmcsd as a service"; Tasks:"vlmcsdservice and not vlmcsdservice\vpn"; Components: "vlmcsd"; Flags: "runhidden";
Filename: "{app}\vlmcsd.exe"; Parameters: "-s -O. -U /n -i ""{commonappdata}\Hotbird64\vlmcsd\vlmcsd.ini"""; StatusMsg: "Setting up vlmcsd as a service"; Tasks:"vlmcsdservice and vlmcsdservice\vpn"; Components: "vlmcsd"; Flags: "runhidden";
Filename: "{app}\vlmcsd.exe"; Parameters: "-S"; StatusMsg: "Deleting vlmcsd Service"; Tasks:"not vlmcsdservice"; Components: "vlmcsd"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters:"advfirewall firewall delete rule name=LicenseManager"; StatusMsg:"Removing obsolete Windows Firewall rules ..."; Components: "libkms"; Flags: "runhidden"; Tasks: "Firewall";
Filename: "{sys}\netsh.exe"; Parameters:"advfirewall firewall delete rule name=vlmcsd"; StatusMsg:"Removing obsolete Windows Firewall rules ..."; Components: "vlmcsd"; Flags: "runhidden"; Tasks: "Firewall";
Filename: "{sys}\netsh.exe"; Parameters:"advfirewall firewall add rule name=LicenseManager action=allow protocol=TCP edge=yes dir=in program=""{app}\LicenseManager.exe"""; StatusMsg:"Adding Windows Firewall rule for License Manager ..."; Components: "libkms"; Flags: "runhidden"; Tasks: "Firewall and not Firewall\private";
Filename: "{sys}\netsh.exe"; Parameters:"advfirewall firewall add rule name=vlmcsd action=allow protocol=TCP edge=yes dir=in program=""{app}\vlmcsd.exe"""; StatusMsg:"Adding Windows Firewall rule for vlmcsd ..."; Components: "vlmcsd"; Flags: "runhidden"; Tasks: "Firewall and not Firewall\private";
Filename: "{sys}\netsh.exe"; Parameters:"advfirewall firewall add rule name=LicenseManager remoteip=192.168.0.0/16,10.0.0.0/8,169.254.0.0/16,172.16.0.0/12,fc00::/7,fec0::/10,fe80::/64,127.0.0.0/8 action=allow protocol=TCP edge=no dir=in program=""{app}\LicenseManager.exe"""; StatusMsg:"Adding Windows Firewall rule for License Manager ..."; Components: "libkms"; Flags: "runhidden"; Tasks: "Firewall and Firewall\private";
Filename: "{sys}\netsh.exe"; Parameters:"advfirewall firewall add rule name=vlmcsd remoteip=192.168.0.0/16,10.0.0.0/8,169.254.0.0/16,172.16.0.0/12,fc00::/7,fec0::/10,fe80::/64,127.0.0.0/8 action=allow protocol=TCP edge=no dir=in program=""{app}\vlmcsd.exe"""; StatusMsg:"Adding Windows Firewall rule for vlmcsd ..."; Components: "vlmcsd"; Flags: "runhidden"; Tasks: "Firewall and Firewall\private";
Filename: "{app}\LicenseManager.exe"; Description: "Launch License Manager now"; Flags: nowait postinstall skipifsilent runascurrentuser unchecked
Filename: "{sys}\net.exe"; Parameters:"start vlmcsd"; Description:"Start vlmcsd Service now"; Components: "vlmcsd"; Tasks:"vlmcsdservice"; Flags: "runhidden nowait postinstall skipifsilent runascurrentuser unchecked";
Filename: "{commonappdata}\Hotbird64\vlmcsd\vlmcsd.ini"; Description:"Edit vlmcsd.ini now"; Components: "vlmcsd"; Tasks:"vlmcsdservice"; Flags:"nowait postinstall skipifsilent shellexec unchecked"; Verb:"edit";
;Filename: "{sys}\cscript.exe"; Parameters:"//NOLOGO {sys}\slmgr.vbs /skms 10.10.10.10"; StatusMsg:"Setting KMS server to 10.10.10.10"; Components:"vpn"; Tasks:"vlmcsdservice\vpn"; Flags:"runhidden"
[Registry]
Root: HKLM; Subkey: "SYSTEM\CurrentControlSet\Control\Session Manager\Environment"; ValueType: expandsz; ValueName: "Path"; ValueData: "{olddata};{app};"; Check: NeedsAddPath(ExpandConstant('{app}')); Components:"vlmcsd";

[UninstallRun]
Filename: "{app}\vlmcsd.exe"; Parameters: "-S"; StatusMsg: "Deleting vlmcsd Service"; Tasks:"vlmcsdservice"; Components: "vlmcsd"; Flags: runhidden
Filename: "{sys}\netsh.exe"; Parameters:"advfirewall firewall delete rule name=LicenseManager"; StatusMsg:"Removing Windows Firewall exceptions ..."; Components: "libkms"; Flags: "runhidden"; Tasks: "Firewall";
Filename: "{sys}\netsh.exe"; Parameters:"advfirewall firewall delete rule name=vlmcsd"; StatusMsg:"Removing Windows Firewall exceptions ..."; Components: "vlmcsd"; Flags: "runhidden"; Tasks: "Firewall";

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "General Options"; Flags: unchecked
Name: "vpnsilent"; Description: "Use OpenVPN TAP driver silent install"; GroupDescription: "General Options";  Components: "vpn";
Name: "Firewall"; Description:"Add exception to Windows Firewall"; GroupDescription: "General Options"; Components:"libkms or vlmcsd"; Flags:"checkablealone";
Name: "Firewall\private"; Description: "Block public (Internet) IP addresses"; GroupDescription:"General Options"; Components:"libkms or vlmcsd";
Name: "vlmcsdservice"; Description: "Install vlmcsd as a Windows Service"; GroupDescription:"vlmcsd Options"; Components: "vlmcsd"; Flags: "checkablealone";
Name: "vlmcsdservice\vpn"; Description: "Use OpenVPN or Teamviewer Adapter for fake IP 10.10.10.10"; GroupDescription:"vlmcsd Options"; Components: "vpn"; Flags: "dontinheritcheck";

[Components]
Name: "lm"; Description: "License Manager"; Types: "default custom minimum"; Flags: "fixed";
Name: "libkms"; Description: "KMS Library for License Manager";  Types: "default custom";
Name: "vlmcsd"; Description: "vlmcsd"; Types: "default";
Name: "vpn"; Description: "OpenVPN TAP Adapter"; Types: "default"; ExtraDiskSpaceRequired: 275636

[Types]
Name: "default"; Description: "Default installation"
Name: "minimum"; Description: "Minimum installation"
Name: "custom"; Description: "Custom installation"; Flags: iscustom

[Icons]
Name: "{commonprograms}\Hotbird64\License Manager\License Manager"; Filename: "{app}\LicenseManager.exe";
Name: "{commonprograms}\Hotbird64\License Manager\Uninstall License Manager"; Filename: "{uninstallexe}";
Name: "{userdesktop}\License Manager"; Filename: "{app}\LicenseManager.exe"; Tasks: "desktopicon";

[Messages]
SetupWindowTitle=License Manager {#ApplicationVersion} Setup
WindowsVersionNotSupported=License Manager requires Windows Vista SP2, Windows Server 2008 SP2, Windows 7 SP1, Windows Server 2008 R2 SP1 or a later version of Windows.

[Code]
procedure AddVpnUsage();
begin
  if IsTaskSelected('vlmcsdservice\vpn') then
    SaveStringToFile(ExpandConstant('{commonappdata}')+'\Hotbird64\vlmcsd\vlmcsd.ini', #13#10 + 'VPN=.' + #13#10, True);
end;

function NeedsAddPath(Param: string): boolean;
var
  OrigPath: string;
begin
  if not RegQueryStringValue(HKEY_LOCAL_MACHINE,
    'SYSTEM\CurrentControlSet\Control\Session Manager\Environment',
    'Path', OrigPath)
  then begin
    Result := True;
    exit;
  end;
  { look for the path with leading and trailing semicolon }
  { Pos() returns 0 if not found }
  Result := Pos(';' + Param + ';', ';' + OrigPath + ';') = 0;
end;

const
  EnvironmentKey = 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment';

procedure RemovePath(Path: string);
var
  Paths: string;
  P: Integer;
begin
  if not RegQueryStringValue(HKEY_LOCAL_MACHINE, EnvironmentKey, 'Path', Paths) then
  begin
    Log('PATH not found');
  end
    else
  begin
    Log(Format('PATH is [%s]', [Paths]));

    P := Pos(';' + Uppercase(Path) + ';', ';' + Uppercase(Paths) + ';');
    if P = 0 then
    begin
      Log(Format('Path [%s] not found in PATH', [Path]));
    end
      else
    begin
      Delete(Paths, P - 2, Length(Path) + 2);
      Log(Format('Path [%s] removed from PATH => [%s]', [Path, Paths]));

      if RegWriteStringValue(HKEY_LOCAL_MACHINE, EnvironmentKey, 'Path', Paths) then
      begin
        Log('PATH written');
      end
        else
      begin
        Log('Error writing PATH');
      end;
    end;
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then
  begin
    RemovePath(ExpandConstant('{app}'));
  end;
end;

function IsDotNetDetected(version: String; service: Integer): Boolean;
var
    key, versionKey: string;
    install, release, serviceCount, versionRelease: cardinal;
    success: boolean;
begin
    versionKey := version;
    versionRelease := 0;

    // .NET 1.1 and 2.0 embed release number in version key
    if version = 'v1.1' then begin
        versionKey := 'v1.1.4322';
    end else if version = 'v2.0' then begin
        versionKey := 'v2.0.50727';
    end

    // .NET 4.5 and newer install as update to .NET 4.0 Full
    else if Pos('v4.', version) = 1 then begin
        versionKey := 'v4\Full';
        case version of
          'v4.5':   versionRelease := 378389;
          'v4.5.1': versionRelease := 378675; // 378758 on Windows 8 and older
          'v4.5.2': versionRelease := 379893;
          'v4.6':   versionRelease := 393295; // 393297 on Windows 8.1 and older
          'v4.6.1': versionRelease := 394254; // 394271 before Win10 November Update
          'v4.6.2': versionRelease := 394802; // 394806 before Win10 Anniversary Update
          'v4.7':   versionRelease := 460798; // 460805 before Win10 Creators Update
        end;
    end;

    // installation key group for all .NET versions
    key := 'SOFTWARE\Microsoft\NET Framework Setup\NDP\' + versionKey;

    // .NET 3.0 uses value InstallSuccess in subkey Setup
    if Pos('v3.0', version) = 1 then begin
        success := RegQueryDWordValue(HKLM, key + '\Setup', 'InstallSuccess', install);
    end else begin
        success := RegQueryDWordValue(HKLM, key, 'Install', install);
    end;

    // .NET 4.0 and newer use value Servicing instead of SP
    if Pos('v4', version) = 1 then begin
        success := success and RegQueryDWordValue(HKLM, key, 'Servicing', serviceCount);
    end else begin
        success := success and RegQueryDWordValue(HKLM, key, 'SP', serviceCount);
    end;

    // .NET 4.5 and newer use additional value Release
    if versionRelease > 0 then begin
        success := success and RegQueryDWordValue(HKLM, key, 'Release', release);
        success := success and (release >= versionRelease);
    end;

    result := success and (install = 1) and (serviceCount >= service);
end;

procedure SetMarqueeProgress(Marquee: Boolean);
begin
  if Marquee then
  begin
    WizardForm.ProgressGauge.Style := npbstMarquee;
  end
    else
  begin
    WizardForm.ProgressGauge.Style := npbstNormal;
  end;
end;