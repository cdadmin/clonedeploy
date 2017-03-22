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
    public class TaskBootMenu
    {
        private readonly ComputerEntity _computer;
        private readonly string _direction;
        private readonly ImageProfileEntity _imageProfile;

        public TaskBootMenu(ComputerEntity computer, ImageProfileEntity imageProfile, string direction)
        {
            _computer = computer;
            _direction = direction;
            _imageProfile = imageProfile;
        }

        public bool CreatePxeBootFiles()
        {
            var pxeComputerMac = Utility.MacToPxeMac(_computer.Mac);
            var webPath = Settings.WebPath;
            var globalComputerArgs = Settings.GlobalComputerArgs;
            var userToken = Settings.WebTaskRequiresLogin == "No" ? Settings.UniversalToken : "";
            const string newLineChar = "\n";


            var ipxe = new StringBuilder();
            ipxe.Append("#!ipxe" + newLineChar);
            ipxe.Append("kernel " + webPath + "IpxeBoot?filename=" + _imageProfile.Kernel +
                        "&type=kernel" + " initrd=" + _imageProfile.BootImage +
                        " root=/dev/ram0 rw ramdisk_size=156000 task=" + _direction +
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
                            " root=/dev/ram0 rw ramdisk_size=156000 task=" + _direction +
                            " consoleblank=0" + " web=" + webPath + " USER_TOKEN=" + userToken + " " +
                            globalComputerArgs +
                            " " + _imageProfile.KernelArguments + newLineChar);


            var grub = new StringBuilder();
            grub.Append("set default=0" + newLineChar);
            grub.Append("set timeout=0" + newLineChar);
            grub.Append("menuentry CloneDeploy --unrestricted {" + newLineChar);
            grub.Append("echo Please Wait While The Boot Image Is Transferred.  This May Take A Few Minutes." +
                        newLineChar);
            grub.Append("linux /kernels/" + _imageProfile.Kernel +
                        " root=/dev/ram0 rw ramdisk_size=156000 task=" +
                        _direction + " consoleblank=0" + " web=" + webPath + " USER_TOKEN=" +
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
                if (Settings.OperationMode == "Single")
                {
                    foreach (var bootMenu in list)
                    {
                        var path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + bootMenu.Item1 +
                                   Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                   pxeComputerMac +
                                   bootMenu.Item2;

                        if (!new FileOps().WritePath(path, bootMenu.Item3))
                            return false;
                    }
                }
                else
                {
                    var clusterGroup = new ComputerServices().GetClusterGroup(_computer.Id);
                    var tftpServers =
                        new ClusterGroupServices().GetClusterServers(clusterGroup.Id).Where(x => x.TftpRole == 1);
                    foreach (var tftpServer in tftpServers)
                    {
                        foreach (var bootMenu in list)
                        {
                            if (tftpServer.SecondaryServerId == -1)
                            {
                                var path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + bootMenu.Item1 +
                                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                           pxeComputerMac +
                                           bootMenu.Item2;

                                if (!new FileOps().WritePath(path, bootMenu.Item3))
                                    return false;
                            }
                            else
                            {
                                var secondaryServer =
                                    new SecondaryServerServices().GetSecondaryServer(tftpServer.SecondaryServerId);
                                var tftpPath =
                                    new APICall(new SecondaryServerServices().GetApiToken(secondaryServer.Name))
                                        .SettingApi.GetSetting("Tftp Path").Value;

                                var path = tftpPath + "proxy" + Path.DirectorySeparatorChar + bootMenu.Item1 +
                                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                           pxeComputerMac +
                                           bootMenu.Item2;

                                if (
                                    !new APICall(new SecondaryServerServices().GetApiToken(secondaryServer.Name))
                                        .ServiceAccountApi.WriteTftpFile(new TftpFileDTO()
                                        {
                                            Path = path,
                                            Contents = bootMenu.Item3
                                        }))
                                    return false;
                            }

                        }

                    }
                }
            }
            //When not using proxy dhcp, only one boot file is created
            else
            {
                var mode = Settings.PxeMode;
                var path = "";
                if (Settings.OperationMode == "Single")
                {
                    path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeComputerMac;

                    string fileContents = null;
                    if (mode == "pxelinux" || mode == "syslinux_32_efi" || mode == "syslinux_64_efi")
                    {
                        fileContents = sysLinux.ToString();
                    }

                    else if (mode.Contains("ipxe"))
                    {
                        path += ".ipxe";
                        fileContents = ipxe.ToString();
                    }
                    else if (mode.Contains("grub"))
                    {
                        path += ".cfg";
                        fileContents = grub.ToString();
                    }

                    if (!new FileOps().WritePath(path, fileContents))
                        return false;
                }
                else
                {
                    var clusterGroup = new ComputerServices().GetClusterGroup(_computer.Id);
                    var tftpServers =
                        new ClusterGroupServices().GetClusterServers(clusterGroup.Id).Where(x => x.TftpRole == 1);
                    var secondaryServer = new SecondaryServerEntity();
                    foreach (var tftpServer in tftpServers)
                    {
                        if (tftpServer.SecondaryServerId == -1)
                        {
                            path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeComputerMac;
                        }
                        else
                        {
                            secondaryServer =
                                new SecondaryServerServices().GetSecondaryServer(tftpServer.SecondaryServerId);
                            var tftpPath =
                                new APICall(new SecondaryServerServices().GetApiToken(secondaryServer.Name)).SettingApi
                                    .GetSetting("Tftp Path").Value;

                            path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeComputerMac;
                        }

                        string fileContents = null;
                        if (mode == "pxelinux" || mode == "syslinux_32_efi" || mode == "syslinux_64_efi")
                        {
                            fileContents = sysLinux.ToString();
                        }

                        else if (mode.Contains("ipxe"))
                        {
                            path += ".ipxe";
                            fileContents = ipxe.ToString();
                        }
                        else if (mode.Contains("grub"))
                        {
                            path += ".cfg";
                            fileContents = grub.ToString();
                        }

                        if (tftpServer.SecondaryServerId == -1)
                        {
                            if (!new FileOps().WritePath(path, fileContents))
                                return false;
                        }
                        else
                        {
                            if (
                                !new APICall(new SecondaryServerServices().GetApiToken(secondaryServer.Name))
                                    .ServiceAccountApi.WriteTftpFile(new TftpFileDTO()
                                    {
                                        Path = path,
                                        Contents = fileContents
                                    }))
                                return false;
                        }

                    }
                }
            }

            return true;
        }
    }
}