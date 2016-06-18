#define MyAppName "CloneDeploy"
#define MyAppVersion "1.1.0"
#define MyAppPublisher "CloneDeploy"
#define MyAppURL "http://clonedeploy.org"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppID={{5B2CCC5E-D36F-4FEC-A785-5D6A70347714}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf32}\clonedeploy
DisableDirPage=true
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=true
LicenseFile=package\pf32\License.txt
OutputBaseFilename=setup
Compression=lzma
SolidCompression=true
Uninstallable=true
VersionInfoVersion=1.1.0
VersionInfoCompany=CloneDeploy
VersionInfoDescription=CloneDeploy Server Setup
VersionInfoCopyright=2016
AlwaysRestart=false
RestartIfNeededByRun=false
AppContact=http://clonedeploy.org
UninstallDisplayName=CloneDeploy Server
AppVerName=1.1.0
AppComments=CloneDeploy
MinVersion=0,6.1
SetupLogging=yes
DisableWelcomePage=no
UninstallDisplayIcon={uninstallexe}

[Languages]
Name: english; MessagesFile: compiler:Default.isl

[Files]
Source: package\pf32\*; DestDir: {app}; Flags: ignoreversion recursesubdirs createallsubdirs; AfterInstall: UpdateStrings
Source: package\appdata\*; DestDir: {userappdata}; Flags: ignoreversion recursesubdirs createallsubdirs
Source: CloneDeploy.url; DestDir: {userdesktop};                
                                                                                                 
[Run]
;Create Shares
Filename: cmd; Parameters: "/c net user cd_share_ro {code:GetROPass} /add"; StatusMsg: "Creating Share Users";
Filename: cmd; Parameters: "/c net user cd_share_rw {code:GetRWPass} /add"; StatusMsg: "Creating Share Users";
Filename: cmd; Parameters: "/c WMIC USERACCOUNT WHERE ""Name='cd_share_ro'"" SET PasswordExpires=FALSE"; StatusMsg: "Creating Share Users";
Filename: cmd; Parameters: "/c WMIC USERACCOUNT WHERE ""Name='cd_share_rw'"" SET PasswordExpires=FALSE"; StatusMsg: "Creating Share Users";
Filename: cmd; Parameters: "/c net share ""cd_share={app}\cd_dp"" /grant:cd_share_ro,READ /grant:cd_share_rw,FULL"; StatusMsg: "Creating SMB Shares";
Filename: cmd; Parameters: "/c icacls ""{app}\cd_dp"" /T /C /grant cd_share_ro:(OI)(CI)RX /grant cd_share_rw:(OI)(CI)F"; StatusMsg: "Creating SMB Shares";

;Install MariaDB
Filename: msiexec; Parameters: "/i ""{userappdata}\clonedeploy\mariadb-10.1.11-win32.msi"" SERVICENAME=mysql-cd PASSWORD={code:GetDBPass} UTF8=1 /qb"; StatusMsg: "Installing MariaDB";
Filename: "{pf32}\MariaDB 10.1\bin\mysql.exe"; Parameters: "--user=root --password={code:GetDBPass} --execute=""create database clonedeploy"" -v"; StatusMsg: "Creating Database";
Filename: "{pf32}\MariaDB 10.1\bin\mysql.exe"; Parameters: "--user=root --password={code:GetDBPass} --database=clonedeploy --execute=""source {code:MySQLAppData}/clonedeploy/cd.sql"" -v"; StatusMsg: "Creating Database";

;Install Tftpd32
Filename: {app}\tftpd32\tftpd32_svc; Parameters: "-install"; StatusMsg: "Setting Up Tftp Server";
Filename: cmd; Parameters: "/c sc config tftpd32_svc start= auto"; StatusMsg: "Setting Up Tftp Server";
Filename: cmd; Parameters: "/c net start tftpd32_svc"; StatusMsg: "Setting Up Tftp Server";

;Install IIS Win7
Filename: cmd; Parameters: "/c dism /online /enable-feature /featurename:IIS-WebServerRole /featurename:IIS-WebServer /featurename:IIS-ISAPIFilter /featurename:IIS-ISAPIExtensions /featurename:IIS-NetFxExtensibility /featurename:IIS-ASPNET /norestart"; StatusMsg: "Installing IIS"; Flags: 32bit; Check: not IsWin64 and IsWin7;  
Filename: cmd; Parameters: "/c dism /online /enable-feature /featurename:IIS-WebServerRole /featurename:IIS-WebServer /featurename:IIS-ISAPIFilter /featurename:IIS-ISAPIExtensions /featurename:IIS-NetFxExtensibility /featurename:IIS-ASPNET /norestart"; StatusMsg: "Installing IIS"; Flags: 64bit; Check: IsWin64 and IsWin7;
 
;Install IIS Win8 or Win10
Filename: cmd; Parameters: "/c powershell -command ""enable-windowsoptionalfeature -online -featurename iis-webserverrole -norestart;enable-windowsoptionalfeature -online -featurename iis-webserver -norestart;enable-windowsoptionalfeature -online -featurename iis-isapifilter -norestart;enable-windowsoptionalfeature -online -featurename iis-isapiextensions -norestart; enable-windowsoptionalfeature -online -featurename netfx4extended-aspnet45 -norestart; enable-windowsoptionalfeature -online -featurename iis-netfxextensibility45 -norestart;enable-windowsoptionalfeature -online -featurename iis-aspnet45 -all -norestart;"""; StatusMsg: "Installing IIS"; Flags: 32bit; Check: not IsWin64 and (IsWin8 or IsWin10);
Filename: cmd; Parameters: "/c powershell -command ""enable-windowsoptionalfeature -online -featurename iis-webserverrole -norestart;enable-windowsoptionalfeature -online -featurename iis-webserver -norestart;enable-windowsoptionalfeature -online -featurename iis-isapifilter -norestart;enable-windowsoptionalfeature -online -featurename iis-isapiextensions -norestart; enable-windowsoptionalfeature -online -featurename netfx4extended-aspnet45 -norestart; enable-windowsoptionalfeature -online -featurename iis-netfxextensibility45 -norestart;enable-windowsoptionalfeature -online -featurename iis-aspnet45 -all -norestart;"""; StatusMsg: "Installing IIS"; Flags: 64bit; Check: IsWin64 and (IsWin8 or IsWin10);

;Install IIS Server 2008
Filename: servermanagercmd; Parameters: "-install web-server"; StatusMsg: "Installing IIS"; Flags: 32bit; Check: not IsWin64 and IsServer2008;
Filename: servermanagercmd; Parameters: "-install web-server"; StatusMsg: "Installing IIS"; Flags: 64bit; Check: IsWin64 and IsServer2008;
Filename: servermanagercmd; Parameters: "-install web-asp-net"; StatusMsg: "Installing IIS"; Flags: 32bit; Check: not IsWin64 and IsServer2008;
Filename: servermanagercmd; Parameters: "-install web-asp-net"; StatusMsg: "Installing IIS"; Flags: 64bit; Check: IsWin64 and IsServer2008;

;Install IIS Server 2012
Filename: cmd; Parameters: "/c powershell -command ""import-module servermanager; add-windowsfeature web-server"""; StatusMsg: "Installing IIS"; Flags: 64bit; Check: IsServer2012;
Filename: cmd; Parameters: "/c powershell -command ""import-module servermanager; add-windowsfeature web-asp-net"""; StatusMsg: "Installing IIS"; Flags: 64bit; Check: IsServer2012;
Filename: cmd; Parameters: "/c powershell -command ""import-module servermanager; add-windowsfeature web-asp-net45"""; StatusMsg: "Installing IIS"; Flags: 64bit; Check: IsServer2012;
Filename: cmd; Parameters: "/c powershell -command ""import-module servermanager; add-windowsfeature web-mgmt-console"""; StatusMsg: "Installing IIS"; Flags: 64bit; Check: IsServer2012;
 
;Create Web Application
Filename: cmd; Parameters: "/c {sys}\inetsrv\appcmd add app /site.name:""Default Web Site"" /path:/clonedeploy /physicalpath:""{app}\web"" "; 

;Register Web Application
Filename: cmd; Parameters: "/c {win}\microsoft.net\framework\v4.0.30319\aspnet_regiis.exe -i -enable"; Check: IsWin7 or IsServer2008;

;Set IIS Permissions
Filename: cmd; Parameters: "/c icacls ""{app}\web"" /T /C /grant IIS_IUSRS:(OI)(CI)M"; StatusMsg: "Installing IIS";
Filename: cmd; Parameters: "/c icacls ""{app}\cd_dp"" /T /C /grant IIS_IUSRS:(OI)(CI)M"; StatusMsg: "Installing IIS";
Filename: cmd; Parameters: "/c icacls ""{app}\tftpboot"" /T /C /grant IIS_IUSRS:(OI)(CI)M"; StatusMsg: "Installing IIS";

;Setup Firewall
Filename: cmd; Parameters: "/c netsh advfirewall firewall add rule name=dhcps dir=in action=allow protocol=UDP localport=67 profile=any "; StatusMsg: "Creating Firewall Exceptions";
Filename: cmd; Parameters: "/c netsh advfirewall firewall add rule name=dhcpd dir=in action=allow protocol=UDP localport=68 profile=any "; StatusMsg: "Creating Firewall Exceptions";
Filename: cmd; Parameters: "/c netsh advfirewall firewall add rule name=tftp dir=in action=allow protocol=UDP localport=69 profile=any "; StatusMsg: "Creating Firewall Exceptions";
Filename: cmd; Parameters: "/c netsh advfirewall firewall add rule name=http dir=in action=allow protocol=TCP localport=80 profile=any "; StatusMsg: "Creating Firewall Exceptions";
Filename: cmd; Parameters: "/c netsh advfirewall firewall add rule name=https dir=in action=allow protocol=TCP localport=443 profile=any "; StatusMsg: "Creating Firewall Exceptions";
Filename: cmd; Parameters: "/c netsh advfirewall firewall add rule name=proxyd dir=in action=allow protocol=UDP localport=4011 profile=any "; StatusMsg: "Creating Firewall Exceptions";
Filename: cmd; Parameters: "/c netsh advfirewall firewall add rule name=udp-sender dir=in action=allow program=""{app}\web\private\apps\udp-sender.exe"" enable=yes profile=any "; StatusMsg: "Creating Firewall Exceptions";
Filename: cmd; Parameters: "/c netsh advfirewall firewall add rule name=udp-sender dir=out action=allow program=""{app}\web\private\apps\udp-sender.exe"" enable=yes profile=any "; StatusMsg: "Creating Firewall Exceptions";
Filename: cmd; Parameters: "/c netsh advfirewall firewall set rule group=""File and Printer Sharing"" new enable=Yes"; StatusMsg: "Creating Firewall Exceptions";

;Create Symlinks For ProxyDHCP
Filename: cmd; Parameters: "/c mklink /J ""{app}\tftpboot\proxy\bios\kernels"" ""{app}\tftpboot\kernels"""; StatusMsg: "Creating Symlinks";
Filename: cmd; Parameters: "/c mklink /J ""{app}\tftpboot\proxy\bios\images"" ""{app}\tftpboot\images"""; StatusMsg: "Creating Symlinks";
Filename: cmd; Parameters: "/c mklink /J ""{app}\tftpboot\proxy\efi32\kernels"" ""{app}\tftpboot\kernels"""; StatusMsg: "Creating Symlinks";
Filename: cmd; Parameters: "/c mklink /J ""{app}\tftpboot\proxy\efi32\images"" ""{app}\tftpboot\images"""; StatusMsg: "Creating Symlinks";
Filename: cmd; Parameters: "/c mklink /J ""{app}\tftpboot\proxy\efi64\kernels"" ""{app}\tftpboot\kernels"""; StatusMsg: "Creating Symlinks";
Filename: cmd; Parameters: "/c mklink /J ""{app}\tftpboot\proxy\efi64\images"" ""{app}\tftpboot\images"""; StatusMsg: "Creating Symlinks";
[Registry]
Root: "HKLM32"; Subkey: "SOFTWARE\CloneDeploy"; ValueType: string; ValueName: "AppVersion"; ValueData: "1100"; Flags: createvalueifdoesntexist; Check: not IsWin64
Root: "HKLM64"; Subkey: "SOFTWARE\CloneDeploy"; ValueType: string; ValueName: "AppVersion"; ValueData: "1100"; Flags: createvalueifdoesntexist; Check: IsWin64

[Code]
var

AuthPage : TInputQueryWizardPage;
InstallType : String;

procedure InitializeWizard;
begin
AuthPage := CreateInputQueryPage(wpWelcome,
    'Setup', 'Information Needed',
    'A Read Only And Read/Write SMB Share Will Be Created.  Create A Password For Each.  Also Create A Database Password.  Do Not Use The Following Characters < > " Ampersand  '+Chr(39)+'');
  AuthPage.Add('Share Read Only Password: (Complexity Requirements Apply To Server 2008, 2012)', True);
  AuthPage.Add('Share Read Write Password: (Complexity Requirements Apply To Server 2008, 2012)', True);
  AuthPage.Add('Database Password:', True);
end;

function IsWin7(): Boolean;
var
  Version: TWindowsVersion;
begin
  GetWindowsVersionEx(Version);
  if (Version.Major = 6)  and
     (Version.Minor = 1) and
     (Version.ProductType = VER_NT_WORKSTATION)
  then
    Result := True
  else
    Result := False
end;

function IsWin8(): Boolean;
var
  Version: TWindowsVersion;
begin
  GetWindowsVersionEx(Version);
  if (Version.Major = 6)  and
     ((Version.Minor = 2) or (Version.Minor = 3)) and
     (Version.ProductType = VER_NT_WORKSTATION)
  then
    Result := True
  else
    Result := False
end;

function IsWin10(): Boolean;
var
  Version: TWindowsVersion;
begin
  GetWindowsVersionEx(Version);
               Log('The value is');
  if (Version.Major = 10)  and
     (Version.Minor = 0) and
     (Version.ProductType = VER_NT_WORKSTATION)
  then
    Result := True
  else
    Result := False
end;

function IsServer2008(): Boolean;
var
  Version: TWindowsVersion;
begin
  GetWindowsVersionEx(Version);

  if (Version.Major = 6)  and
     ((Version.Minor = 0) or (Version.Minor = 1)) and
     (Version.ProductType = VER_NT_SERVER)
  then
    Result := True
  else
    Result := False
end;

function IsServer2012(): Boolean;
var
  Version: TWindowsVersion;
begin
  GetWindowsVersionEx(Version);

  if (Version.Major = 6)  and
     ((Version.Minor = 2) or (Version.Minor = 3)) and
     (Version.ProductType = VER_NT_SERVER)
  then
    Result := True
  else
    Result := False
end;

function GetROPass(Param: String): string;
begin
result := AuthPage.Values[0];
end;

function GetRWPass(Param: String): string;
begin
result := AuthPage.Values[1];
end;

function GetDBPass(Param: String): string;
begin
result := AuthPage.Values[2];
end;

procedure UpdateStrings;
var
    FileData: String;
begin
    LoadStringFromFile(ExpandConstant('{app}\web\web.config'), FileData);
    StringChange(FileData, 'xx_marker1_xx', ExpandConstant('{code:GetDBPass}'));
    SaveStringToFile(ExpandConstant('{app}\web\web.config'), FileData, False);

    LoadStringFromFile(ExpandConstant('{app}\web\web.config'), FileData);
    StringChange(FileData, 'xx_marker2_xx', ExpandConstant('{code:GetROPass}{code:GetRWPass}'));
    SaveStringToFile(ExpandConstant('{app}\web\web.config'), FileData, False);

    LoadStringFromFile(ExpandConstant('{app}\tftpd32\tftpd32.ini'), FileData);
    StringChange(FileData, 'xx_marker1_xx', ExpandConstant('{app}\tftpboot'));
    SaveStringToFile(ExpandConstant('{app}\tftpd32\tftpd32.ini'), FileData, False);    
end;

function MySQLAppData(Param: String): string;
var
mysqlapp: string;
begin
mysqlapp := expandconstant('{userappdata}');
StringChange(mysqlapp, '\', '/');
result := mysqlapp
end;

function IsDotNetDetected(version: string; service: cardinal): boolean;
// Indicates whether the specified version and service pack of the .NET Framework is installed.
//
// version -- Specify one of these strings for the required .NET Framework version:
//    'v1.1'          .NET Framework 1.1
//    'v2.0'          .NET Framework 2.0
//    'v3.0'          .NET Framework 3.0
//    'v3.5'          .NET Framework 3.5
//    'v4\Client'     .NET Framework 4.0 Client Profile
//    'v4\Full'       .NET Framework 4.0 Full Installation
//    'v4.5'          .NET Framework 4.5
//    'v4.5.1'        .NET Framework 4.5.1
//    'v4.5.2'        .NET Framework 4.5.2
//    'v4.6'          .NET Framework 4.6
//    'v4.6.1'        .NET Framework 4.6.1
//
// service -- Specify any non-negative integer for the required service pack level:
//    0               No service packs required
//    1, 2, etc.      Service pack 1, 2, etc. required
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
          'v4.5.1': versionRelease := 378675; // or 378758 on Windows 8 and older
          'v4.5.2': versionRelease := 379893;
          'v4.6':   versionRelease := 393295; // or 393297 on Windows 8.1 and older
          'v4.6.1': versionRelease := 394254; // or 394271 on Windows 8.1 and older
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

function InitializeSetup(): Boolean;

begin
    MsgBox('Warning: This Installer Is Only For New Installations.'#13#13
            'If You Are Upgrading, Exit Now And Follow The Upgrade Documentation.'#13
            'See http://clonedeploy.org for more info', mbInformation, MB_OK);

    if not IsDotNetDetected('v4.5', 0) then begin
        MsgBox('Clone Deploy requires Microsoft .NET Framework 4.5.'#13#13
            'See http://clonedeploy.org for more info.'#13
            'Run setup again after installation.', mbInformation, MB_OK);
        Result := False;
        Exit;
    end
    Result := True;
   


end;

procedure DeinitializeSetup();
var
  logfilepathname, logfilename, newfilepathname: string;

begin
  logfilepathname := expandconstant('{log}');
  logfilename := ExtractFileName(logfilepathname);
  // Set the new target path as the directory where the installer is being run from
  newfilepathname := expandconstant('{src}\') +logfilename;

  filecopy(logfilepathname, newfilepathname, false);
end; 


