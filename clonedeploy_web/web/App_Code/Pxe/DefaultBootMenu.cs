using System;
using System.IO;
using System.Net;
using Global;

namespace Pxe
{
    /// <summary>
    ///     Summary description for DefaultBootMenu
    /// </summary>
    public class DefaultBootMenu
    {
        public string AddPwd { get; set; }
        public string BootImage { get; set; }
        public string DebugPwd { get; set; }
        public string DiagPwd { get; set; }
        public string GrubPassword { get; set; }
        public string GrubUserName { get; set; }
        public string Kernel { get; set; }
        public string OndPwd { get; set; }
        public string Type { get; set; }

        public void CreateGlobalDefaultBootMenu()
        {
            var mode = Settings.PxeMode;

            if (Type == "noprox")
            {
                if (mode.Contains("ipxe"))
                    CreateIpxeMenu();
                else if (mode.Contains("grub"))
                    CreateGrubMenu();
                else
                    CreateSyslinuxMenu();
            }
            else
            {
                CreateIpxeMenu();
                CreateSyslinuxMenu();
                CreateGrubMenu();
            }
        }

        private void CreateGrubMenu()
        {
            var webPath = Settings.WebPath;
            var globalHostArgs = Settings.GlobalHostArgs;
            var wdsKey = Settings.WebTaskRequiresLogin == "No" ? Settings.ServerKey : "";
            var lines = "insmod password_pbkdf2\r\n";
            lines += "insmod regexp\r\n";
            lines += "set default=0\r\n";
            lines += "set timeout=10\r\n";
            lines += "set pager=1\r\n";
            if (!string.IsNullOrEmpty(GrubUserName) && !string.IsNullOrEmpty(GrubPassword))
            {
                lines += "set superusers=\"" + GrubUserName + "\"\r\n";
                string sha = null;
                try
                {
                    sha =
                        new WebClient().DownloadString(
                            "http://docs.cruciblewds.org/grub-pass-gen/encrypt.php?password=" + GrubPassword);
                    sha = sha.Replace("\n \n\n\n", "");
                }
                catch
                {
                    Logger.Log("Could not generate sha for grub password.  Could not contact http://cruciblewds.org");
                }
                lines += "password_pbkdf2 " + GrubUserName + " " + sha + "\r\n";
                lines += "export superusers\r\n";
                lines += "\r\n";
            }
            lines += @"regexp -s 1:b1 '(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3})' $net_default_mac" +
                     Environment.NewLine;
            lines += @"regexp -s 2:b2 '(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3})' $net_default_mac" +
                     Environment.NewLine;
            lines += @"regexp -s 3:b3 '(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3})' $net_default_mac" +
                     Environment.NewLine;
            lines += @"regexp -s 4:b4 '(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3})' $net_default_mac" +
                     Environment.NewLine;
            lines += @"regexp -s 5:b5 '(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3})' $net_default_mac" +
                     Environment.NewLine;
            lines += @"regexp -s 6:b6 '(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3}):(.{1,3})' $net_default_mac" +
                     Environment.NewLine;
            lines += @"mac=01-$b1-$b2-$b3-$b4-$b5-$b6" + Environment.NewLine;
            lines += "\r\n";


            if (Type == "noprox")
            {
                lines += "if [ -s /pxelinux.cfg/$mac.cfg ]; then\r\n";
                lines += "configfile /pxelinux.cfg/$mac.cfg\r\n";
                lines += "fi\r\n";
            }
            else
            {
                lines += "if [ -s /proxy/efi64/pxelinux.cfg/$mac.cfg ]; then\r\n";
                lines += "configfile /proxy/efi64/pxelinux.cfg/$mac.cfg\r\n";
                lines += "fi\r\n";
            }
            lines += "\r\n";
            lines += "menuentry \"Boot To Local Machine\" --unrestricted {\r\n";
            lines += "exit\r\n";
            lines += "}\r\n";

            lines += "\r\n";

            lines += "menuentry \"Client Console\" --user {\r\n";
            lines += "echo Please Wait While The Boot Image Is Transferred.  This May Take A Few Minutes.\r\n";
            lines += "linux /kernels/" + Kernel + " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp " + " web=" + webPath +
                     " WDS_KEY=" + wdsKey + " task=debug consoleblank=0 " + globalHostArgs + "\r\n";
            lines += "initrd /images/" + BootImage + "\r\n";
            lines += "}\r\n";

            lines += "\r\n";

            lines += "menuentry \"On Demand Imaging\" --user {\r\n";
            lines += "echo Please Wait While The Boot Image Is Transferred.  This May Take A Few Minutes.\r\n";
            lines += "linux /kernels/" + Kernel + " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp " + " web=" + webPath +
                     " WDS_KEY=" + wdsKey + " task=ond consoleblank=0 " + globalHostArgs + "\r\n";
            lines += "initrd /images/" + BootImage + "\r\n";
            lines += "}\r\n";

            lines += "\r\n";

            lines += "menuentry \"Add Host\" --user {\r\n";
            lines += "echo Please Wait While The Boot Image Is Transferred.  This May Take A Few Minutes.\r\n";
            lines += "linux /kernels/" + Kernel + " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp " + " web=" + webPath +
                     " WDS_KEY=" + wdsKey + " task=register consoleblank=0 " + globalHostArgs + "\r\n";
            lines += "initrd /images/" + BootImage + "\r\n";
            lines += "}\r\n";

            lines += "\r\n";

            lines += "menuentry \"Diagnostics\" --user {\r\n";
            lines += "echo Please Wait While The Boot Image Is Transferred.  This May Take A Few Minutes.\r\n";
            lines += "linux /kernels/" + Kernel + " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp " + " web=" + webPath +
                     " WDS_KEY=" + wdsKey + " task=diag consoleblank=0 " + globalHostArgs + "\r\n";
            lines += "initrd /images/" + BootImage + "\r\n";
            lines += "}\r\n";


            var path = Settings.TftpPath + "grub" + Path.DirectorySeparatorChar + "grub.cfg";


            try
            {
                using (var file = new StreamWriter(path))
                {
                    file.WriteLine(lines);
                    Utility.Message = "Successfully Created Default Boot Menu";
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                Utility.Message = "Could Not Create Boot Menu.  Check The Exception Log For More Info.";
                return;
            }
            new FileOps().SetUnixPermissions(path);
        }

        private void CreateIpxeMenu()
        {
            string path;
            var lines = "#!ipxe\r\n";
            lines += "chain 01-${net0/mac:hexhyp}.ipxe || goto Menu\r\n";
            lines += "\r\n";
            lines += ":Menu\r\n";
            lines += "menu Boot Menu\r\n";
            lines += "item boot Boot To Local Machine\r\n";
            lines += "item console Client Console\r\n";
            lines += "item addhost Add Host\r\n";
            lines += "item ond On Demand\r\n";
            lines += "item diag Diagnostics\r\n";
            lines += "choose --default boot --timeout 5000 target && goto ${target}\r\n";
            lines += "\r\n";

            lines += ":boot\r\n";
            lines += "exit\r\n";
            lines += "\r\n";

            lines += ":console\r\n";
            lines += "set task debug\r\n";
            lines += "goto login\r\n";
            lines += "\r\n";

            lines += ":addhost\r\n";
            lines += "set task register\r\n";
            lines += "goto login\r\n";
            lines += "\r\n";

            lines += ":ond\r\n";
            lines += "set task ond\r\n";
            lines += "goto login\r\n";

            lines += "\r\n";
            lines += ":diag\r\n";
            lines += "set task diag\r\n";
            lines += "goto login\r\n";
            lines += "\r\n";

            lines += ":login\r\n";
            lines += "login\r\n";
            lines += "params\r\n";
            lines += "param uname ${username:uristring}\r\n";
            lines += "param pwd ${password:uristring}\r\n";
            lines += "param kernel " + Kernel + "\r\n";
            lines += "param bootImage " + BootImage + "\r\n";
            lines += "param task " + "${task}" + "\r\n";
            lines += "echo Authenticating\r\n";
            lines += "chain --timeout 15000 http://" + Settings.ServerIpWithPort +
                     "/cruciblewds/service/client.asmx/IpxeLogin##params || goto Menu\r\n";

            if (Type == "noprox")
            {
                path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + "default.ipxe";
            }
            else
            {
                path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + Type +
                       Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + "default.ipxe";
            }

            try
            {
                using (var file = new StreamWriter(path))
                {
                    file.WriteLine(lines);
                    Utility.Message = "Successfully Created Default Boot Menu";
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                Utility.Message = "Could Not Create Boot Menu.  Check The Exception Log For More Info.";
                return;
            }
            new FileOps().SetUnixPermissions(path);
        }

        private void CreateSyslinuxMenu()
        {
            var webPath = Settings.WebPath;
            var globalHostArgs = Settings.GlobalHostArgs;
            var wdsKey = Settings.WebTaskRequiresLogin == "No" ? Settings.ServerKey : "";
            string path;

            var lines = "DEFAULT vesamenu.c32\r\n";
            lines += "MENU TITLE Boot Menu\r\n";
            lines += "MENU BACKGROUND bg.png\r\n";
            lines += "menu tabmsgrow 22\r\n";
            lines += "menu cmdlinerow 22\r\n";
            lines += "menu endrow 24\r\n";
            lines += "menu color title                1;34;49    #eea0a0ff #cc333355 std\r\n";
            lines += "menu color sel                  7;37;40    #ff000000 #bb9999aa all\r\n";
            lines += "menu color border               30;44      #ffffffff #00000000 std\r\n";
            lines += "menu color pwdheader            31;47      #eeff1010 #20ffffff std\r\n";
            lines += "menu color hotkey               35;40      #90ffff00 #00000000 std\r\n";
            lines += "menu color hotsel               35;40      #90000000 #bb9999aa all\r\n";
            lines += "menu color timeout_msg          35;40      #90ffffff #00000000 none\r\n";
            lines += "menu color timeout              31;47      #eeff1010 #00000000 none\r\n";
            lines += "NOESCAPE 0\r\n";
            lines += "ALLOWOPTIONS 0\r\n";
            lines += "\r\n";
            lines += "LABEL local\r\n";
            lines += "localboot 0\r\n";
            lines += "MENU DEFAULT\r\n";
            lines += "MENU LABEL Boot To Local Machine\r\n";
            lines += "\r\n";

            lines += "LABEL Client Console\r\n";
            if (!string.IsNullOrEmpty(DebugPwd) && DebugPwd != "Error: Empty password")
                lines += "MENU PASSWD " + DebugPwd + "\r\n";

            lines += "kernel kernels" + Path.DirectorySeparatorChar + Kernel + "\r\n";
            lines += "append initrd=images" + Path.DirectorySeparatorChar + BootImage +
                     " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp " + " web=" + webPath + " WDS_KEY=" + wdsKey +
                     " task=debug consoleblank=0 " + globalHostArgs + "\r\n";

            lines += "MENU LABEL Client Console\r\n";
            lines += "\r\n";

            lines += "LABEL Add Host\r\n";
            if (!string.IsNullOrEmpty(AddPwd) && AddPwd != "Error: Empty password")
                lines += "MENU PASSWD " + AddPwd + "\r\n";

            lines += "kernel kernels" + Path.DirectorySeparatorChar + Kernel + "\r\n";
            lines += "append initrd=images" + Path.DirectorySeparatorChar + BootImage +
                     " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp " + " web=" + webPath + " WDS_KEY=" + wdsKey +
                     " task=register consoleblank=0 " + globalHostArgs + "\r\n";

            lines += "MENU LABEL Add Host\r\n";
            lines += "\r\n";

            lines += "LABEL On Demand\r\n";
            if (!string.IsNullOrEmpty(OndPwd) && OndPwd != "Error: Empty password")
                lines += "MENU PASSWD " + OndPwd + "\r\n";

            lines += "kernel kernels" + Path.DirectorySeparatorChar + Kernel + "\r\n";
            lines += "append initrd=images" + Path.DirectorySeparatorChar + BootImage +
                     " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp " + " web=" + webPath + " WDS_KEY=" + wdsKey +
                     " task=ond consoleblank=0 " + globalHostArgs + "\r\n";

            lines += "MENU LABEL On Demand\r\n";
            lines += "\r\n";

            lines += "LABEL Diagnostics\r\n";
            if (!string.IsNullOrEmpty(DiagPwd) && DiagPwd != "Error: Empty password")
                lines += "MENU PASSWD " + DiagPwd + "\r\n";

            lines += "kernel kernels" + Path.DirectorySeparatorChar + Kernel + "\r\n";
            lines += "append initrd=images" + Path.DirectorySeparatorChar + BootImage +
                     " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp " + " web=" + webPath + " WDS_KEY=" + wdsKey +
                     " task=diag consoleblank=0 " + globalHostArgs + "\r\n";


            lines += "MENU LABEL Diagnostics\r\n";
            lines += "\r\n";

            lines += "PROMPT 0\r\n";
            lines += "TIMEOUT 50";


            if (Type == "noprox")
            {
                path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + "default";
            }
            else
            {
                path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + Type +
                       Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + "default";
            }
            try
            {
                using (var file = new StreamWriter(path))
                {
                    file.WriteLine(lines);
                    Utility.Message = "Successfully Created Default Boot Menu";
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                Utility.Message = "Could Not Create Boot Menu.  Check The Exception Log For More Info.";
                return;
            }
            new FileOps().SetUnixPermissions(path);
        }

        public static string GetMenuText(string proxyMode)
        {
            var path = new PxeFileOps().GetDefaultMenuPath(proxyMode);
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Utility.Message = "Could Not Read Default Boot Menu.  Check The Exception Log For More Info";
                Logger.Log(ex.Message);
                return "";
            }
        }
    }
}