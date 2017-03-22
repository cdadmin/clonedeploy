using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CloneDeploy_ApiCalls;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;

namespace CloneDeploy_Services.Workflows
{
    public class ClobberBootMenu
    {
        private readonly bool _promptComputerName;
        private readonly ImageProfileEntity _imageProfile;

        public ClobberBootMenu(int imageProfileId, bool promptComputerName)
        {
            _promptComputerName = promptComputerName;
            _imageProfile = new ImageProfileServices().ReadProfile(imageProfileId);
        }

        public bool CreatePxeBootFiles()
        {
            var webPath = Settings.WebPath;
            var globalComputerArgs = Settings.GlobalComputerArgs;
            string namePromptArg = "";
            if (_promptComputerName)
                namePromptArg = " name_prompt=true";

            var userToken = Settings.ClobberRequiresLogin == "No" ? Settings.UniversalToken : "";
            const string newLineChar = "\n";


            var ipxe = new StringBuilder();
            ipxe.Append("#!ipxe" + newLineChar);
            ipxe.Append("kernel " + webPath + "IpxeBoot?filename=" + _imageProfile.Kernel +
                        "&type=kernel" + " initrd=" + _imageProfile.BootImage +
                        " root=/dev/ram0 rw ramdisk_size=156000 task=clobber " + "imageProfileId=" + _imageProfile.Id + namePromptArg +
                        " consoleblank=0" + " web=" + webPath + " USER_TOKEN=" + userToken + " " + globalComputerArgs +
                        " " + _imageProfile.KernelArguments + newLineChar);
            ipxe.Append("imgfetch --name " + _imageProfile.BootImage + " " + webPath +
                        "IpxeBoot?filename=" + _imageProfile.BootImage + "&type=bootimage" + newLineChar);
            ipxe.Append("boot" + newLineChar);


            var sysLinux = new StringBuilder();
            sysLinux.Append("DEFAULT clonedeploy" + newLineChar);
            sysLinux.Append("LABEL clonedeploy" + newLineChar);
            sysLinux.Append("KERNEL kernels" + Path.DirectorySeparatorChar + _imageProfile.Kernel + newLineChar);
            sysLinux.Append("APPEND initrd=images" + Path.DirectorySeparatorChar + _imageProfile.BootImage +
                            " root=/dev/ram0 rw ramdisk_size=156000 task=clobber " + "imageProfileId=" + _imageProfile.Id + namePromptArg +
                            " consoleblank=0" + " web=" + webPath + " USER_TOKEN=" + userToken + " " + globalComputerArgs +
                            " " + _imageProfile.KernelArguments + newLineChar);


            var grub = new StringBuilder();
            grub.Append("set default=0" + newLineChar);
            grub.Append("set timeout=0" + newLineChar);
            grub.Append("menuentry CloneDeploy --unrestricted {" + newLineChar);
            grub.Append("echo Please Wait While The Boot Image Is Transferred.  This May Take A Few Minutes." +
                        newLineChar);
            grub.Append("linux /kernels/" + _imageProfile.Kernel +
                        " root=/dev/ram0 rw ramdisk_size=156000 task=clobber " + "imageProfileId=" + _imageProfile.Id + namePromptArg + " consoleblank=0" + " web=" + webPath + " USER_TOKEN=" +
                        userToken + " " +
                        globalComputerArgs + " " + _imageProfile.KernelArguments + newLineChar);
            grub.Append("initrd /images/" + _imageProfile.BootImage + newLineChar);
            grub.Append("}" + newLineChar);

            var list = new List<Tuple<string, string, string>>
            {
                Tuple.Create("bios", "", sysLinux.ToString()),
                Tuple.Create("bios", ".ipxe", ipxe.ToString()),
                Tuple.Create("efi32", "", sysLinux.ToString()),
                Tuple.Create("efi32", ".ipxe", ipxe.ToString()),
                Tuple.Create("efi64", "", sysLinux.ToString()),
                Tuple.Create("efi64", ".ipxe", ipxe.ToString()),
                Tuple.Create("efi64", ".cfg", grub.ToString())
            };

            //In proxy mode all boot files are created regardless of the pxe mode, this way computers can be customized
            //to use a specific boot file without affecting all others, using the proxydhcp reservations file.
            if (Settings.ProxyDhcp == "Yes")
            {
                string path = null;

                if (Settings.OperationMode == "Single" || (Settings.OperationMode == "Cluster Primary" && Settings.TftpServerRole))
                {
                    foreach (var bootMenu in list)
                    {
                        switch (bootMenu.Item2)
                        {
                            case ".cfg":
                                path = Settings.TftpPath + "grub" + Path.DirectorySeparatorChar + "grub.cfg";
                                break;
                            case ".ipxe":
                                path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + bootMenu.Item1 +
                                       Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                       "default.ipxe";
                                break;
                            case "":
                                path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + bootMenu.Item1 +
                                       Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                       "default";
                                break;
                        }


                        if (!new FileOps().WritePath(path, bootMenu.Item3))
                            return false;
                    }
                }
                else
                {
                    var tftpServers =
                        new SecondaryServerServices().SearchSecondaryServers().Where(x => x.TftpRole == 1);
                    foreach (var tftpServer in tftpServers)
                    {
                        var tftpPath =
                            new APICall(new SecondaryServerServices().GetApiToken(tftpServer.Name))
                                .SettingApi.GetSetting("Tftp Path").Value;

                        foreach (var bootMenu in list)
                        {
                            switch (bootMenu.Item2)
                            {
                                case ".cfg":
                                    path = tftpPath + "grub" + Path.DirectorySeparatorChar + "grub.cfg";
                                    break;
                                case ".ipxe":
                                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + bootMenu.Item1 +
                                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                           "default.ipxe";
                                    break;
                                case "":
                                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + bootMenu.Item1 +
                                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                           "default";
                                    break;
                            }

                            var tftpFile = new TftpFileDTO();
                            tftpFile.Contents = bootMenu.Item3;
                            tftpFile.Path = path;

                            new APICall(new SecondaryServerServices().GetApiToken(tftpServer.Name))
                                .ServiceAccountApi.WriteTftpFile(tftpFile);
                        
                        }
                    }
                }
            }
            //When not using proxy dhcp, only one boot file is created
            else
            {
                var mode = Settings.PxeMode;
                string path = null;
                string fileContents = null;

                if (Settings.OperationMode == "Single" ||
                    (Settings.OperationMode == "Cluster Primary" && Settings.TftpServerRole))
                {

                    if (mode == "pxelinux" || mode == "syslinux_32_efi" || mode == "syslinux_64_efi")
                    {
                        path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + "default";
                        fileContents = sysLinux.ToString();
                    }

                    else if (mode.Contains("ipxe"))
                    {
                        path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + "default.ipxe";
                        fileContents = ipxe.ToString();
                    }
                    else if (mode.Contains("grub"))
                    {
                        path = Settings.TftpPath + "grub" + Path.DirectorySeparatorChar + "grub.cfg";
                        fileContents = grub.ToString();
                    }

                    if (!new FileOps().WritePath(path, fileContents))
                        return false;
                }
                else
                {
                     var tftpServers =
                        new SecondaryServerServices().SearchSecondaryServers().Where(x => x.TftpRole == 1);
                    foreach (var tftpServer in tftpServers)
                    {
                        var tftpPath =
                            new APICall(new SecondaryServerServices().GetApiToken(tftpServer.Name))
                                .SettingApi.GetSetting("Tftp Path").Value;

                        if (mode == "pxelinux" || mode == "syslinux_32_efi" || mode == "syslinux_64_efi")
                        {
                            path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + "default";
                            fileContents = sysLinux.ToString();
                        }

                        else if (mode.Contains("ipxe"))
                        {
                            path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + "default.ipxe";
                            fileContents = ipxe.ToString();
                        }
                        else if (mode.Contains("grub"))
                        {
                            path = tftpPath + "grub" + Path.DirectorySeparatorChar + "grub.cfg";
                            fileContents = grub.ToString();
                        }

                            var tftpFile = new TftpFileDTO();
                            tftpFile.Contents = fileContents;
                            tftpFile.Path = path;

                            new APICall(new SecondaryServerServices().GetApiToken(tftpServer.Name))
                                .ServiceAccountApi.WriteTftpFile(tftpFile);
                                           
                    }            
                }
            }
            return true;
        }
    }
}