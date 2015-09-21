using System;
using System.IO;
using Global;
using Logic;
using Models;

namespace Pxe
{
    public class TaskBootMenu
    {
        public string Arguments { get; set; }
        public string BootImage { get; set; }
        public string Direction { get; set; }
        public bool IsMulticast { get; set; }
        public string Kernel { get; set; }
        public string PxeHostMac { get; set; }

        public bool CreatePxeBoot()
        {
            var proxyDhcp = Settings.ProxyDhcp;
            var tftpPath = Settings.TftpPath;
            var webPath = Settings.WebPath;
            var globalHostArgs = Settings.GlobalHostArgs;
            string path = null;
            var lines = new string[] {};
            var wdsKey = Settings.WebTaskRequiresLogin == "No" ? Settings.ServerKey : "";

            var mode = Settings.PxeMode;

            if (proxyDhcp == "Yes")
            {
                var biosPathipxe = tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                                   Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                   PxeHostMac +
                                   ".ipxe";
                string[] biosLinesipxe =
                {
                    @"#!ipxe",
                    @"kernel " + webPath + "IpxeBoot?filename=" + Kernel +
                    "&type=kernel" + " initrd=" + BootImage +
                    " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + Direction + " consoleblank=0" +
                    " web=" +
                    webPath + " WDS_KEY=" + wdsKey + " " +
                    globalHostArgs +
                    " " + Arguments,
                    @"imgfetch --name " + BootImage + " " + webPath +
                    "IpxeBoot?filename=" + BootImage + "&type=bootimage",
                    @"boot"
                };

                var biosPath = tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                               Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac;
                string[] biosLines =
                {
                    @"DEFAULT cruciblewds",
                    @"LABEL cruciblewds", @"KERNEL kernels" + Path.DirectorySeparatorChar + Kernel,
                    @"APPEND initrd=images" + Path.DirectorySeparatorChar + BootImage +
                    " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + Direction + " consoleblank=0" +
                    " web=" +
                    webPath + " WDS_KEY=" + wdsKey + " " +
                    globalHostArgs +
                    " " + Arguments
                };


                var efi32Pathipxe = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                                    Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                    PxeHostMac +
                                    ".ipxe";
                string[] efi32Linesipxe =
                {
                    @"#!ipxe",
                    @"kernel " + webPath + "IpxeBoot?filename=" + Kernel +
                    "&type=kernel" + " initrd=" + BootImage +
                    " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + Direction + " consoleblank=0" +
                    " web=" +
                    webPath + " WDS_KEY=" + wdsKey + " " +
                    globalHostArgs +
                    " " + Arguments,
                    @"imgfetch --name " + BootImage + " " + webPath +
                    "IpxeBoot?filename=" + BootImage + "&type=bootimage",
                    @"boot"
                };

                var efi32Path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac;
                string[] efi32Lines =
                {
                    @"DEFAULT cruciblewds",
                    @"LABEL cruciblewds", @"KERNEL kernels" + Path.DirectorySeparatorChar + Kernel,
                    @"APPEND initrd=images" + Path.DirectorySeparatorChar + BootImage +
                    " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + Direction + " consoleblank=0" +
                    " web=" +
                    webPath + " WDS_KEY=" + wdsKey + " " +
                    globalHostArgs +
                    " " + Arguments
                };


                var efi64Pathipxe = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                                    Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                    PxeHostMac +
                                    ".ipxe";
                string[] efi64Linesipxe =
                {
                    @"#!ipxe",
                    @"kernel " + webPath + "IpxeBoot?filename=" + Kernel +
                    "&type=kernel" + " initrd=" + BootImage +
                    " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + Direction + " consoleblank=0" +
                    " web=" +
                    webPath + " WDS_KEY=" + wdsKey + " " +
                    globalHostArgs +
                    " " + Arguments,
                    @"imgfetch --name " + BootImage + " " + webPath +
                    "IpxeBoot?filename=" + BootImage + "&type=bootimage",
                    @"boot"
                };

                var efi64Path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac;
                string[] efi64Lines =
                {
                    @"DEFAULT cruciblewds",
                    @"LABEL cruciblewds", @"KERNEL kernels" + Path.DirectorySeparatorChar + Kernel,
                    @"APPEND initrd=images" + Path.DirectorySeparatorChar + BootImage +
                    " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + Direction + " consoleblank=0" +
                    " web=" +
                    webPath + " WDS_KEY=" + wdsKey + " " +
                    globalHostArgs +
                    " " + Arguments
                };

                var efi64PathGrub = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                                    Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                    PxeHostMac +
                                    ".cfg";
                string[] efi64LinesGrub =
                {
                    @"set default=0",
                    @"set timeout=0",
                    @"menuentry ""CrucibleWDS"" --unrestricted {",
                    @"echo Please Wait While The Boot Image Is Transferred.  This May Take A Few Minutes.",
                    @"linux /kernels/" + Kernel + " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" +
                    Direction + " consoleblank=0" + " web=" + webPath + " WDS_KEY=" +
                    wdsKey + " " +
                    globalHostArgs + " " + Arguments,
                    @"initrd /images/" + BootImage,
                    @"}"
                };

                var fileOps = new FileOps();
                var host = new ComputerLogic().GetComputerFromMac(Utility.PxeMacToMac(PxeHostMac));
                if (File.Exists(biosPath))
                {  
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        fileOps.MoveFile(biosPath, biosPath + ".custom");
                    }
                    else
                    {
                        Utility.Message = "The PXE File Already Exists";
                        return false;
                    }
                }

                if (File.Exists(biosPathipxe))
                {
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        fileOps.MoveFile(
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac +
                            ".ipxe",
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac +
                            ".ipxe.custom");
                    }
                    else
                    {
                        Utility.Message = "The PXE File Already Exists";
                        return false;
                    }
                }

                if (File.Exists(efi32Path))
                {
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        fileOps.MoveFile(efi32Path, efi32Path + ".custom");
                    }
                    else
                    {
                        Utility.Message = "The PXE File Already Exists";
                        return false;
                    }
                }

                if (File.Exists(efi32Pathipxe))
                {
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        fileOps.MoveFile(
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac +
                            ".ipxe",
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac +
                            ".ipxe.custom");
                    }
                    else
                    {
                        Utility.Message = "The PXE File Already Exists";
                        return false;
                    }
                }

                if (File.Exists(efi64Path))
                {
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        fileOps.MoveFile(efi64Path, efi64Path + ".custom");
                    }
                    else
                    {
                        Utility.Message = "The PXE File Already Exists";
                        return false;
                    }
                }

                if (File.Exists(efi64Pathipxe))
                {
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        fileOps.MoveFile(
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac +
                            ".ipxe",
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac +
                            ".ipxe.custom");
                    }
                    else
                    {
                        Utility.Message = "The PXE File Already Exists";
                        return false;
                    }
                }

                if (File.Exists(efi64PathGrub))
                {
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        fileOps.MoveFile(
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac +
                            ".cfg",
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac +
                            ".cfg.custom");
                    }
                    else
                    {
                        Utility.Message = "The PXE File Already Exists";
                        return false;
                    }
                }

                try
                {
                    File.WriteAllLines(biosPath, biosLines);
                    fileOps.SetUnixPermissions(biosPath);

                    File.WriteAllLines(efi32Path, efi32Lines);
                    fileOps.SetUnixPermissions(efi32Path);

                    File.WriteAllLines(efi64Path, efi64Lines);
                    fileOps.SetUnixPermissions(efi64Path);

                    File.WriteAllLines(biosPathipxe, biosLinesipxe);
                    fileOps.SetUnixPermissions(biosPathipxe);

                    File.WriteAllLines(efi32Pathipxe, efi32Linesipxe);
                    fileOps.SetUnixPermissions(efi32Pathipxe);

                    File.WriteAllLines(efi64Pathipxe, efi64Linesipxe);
                    fileOps.SetUnixPermissions(efi64Pathipxe);

                    File.WriteAllLines(efi64PathGrub, efi64LinesGrub);
                    fileOps.SetUnixPermissions(efi64Path);
                }

                catch (Exception ex)
                {
                    Logger.Log(ex.ToString());
                    Utility.Message = "Could Not Create PXE File";
                    return false;
                }
            }
            else
            {
                if (mode == "pxelinux" || mode == "syslinux_32_efi" || mode == "syslinux_64_efi")
                {
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac;
                    string[] tmplines =
                    {
                        @"DEFAULT cruciblewds",
                        @"LABEL cruciblewds", @"KERNEL " + "kernels" + Path.DirectorySeparatorChar + Kernel,
                        @"APPEND initrd=" + "images" + Path.DirectorySeparatorChar + BootImage +
                        " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + Direction +
                        " consoleblank=0" +
                        " web=" + webPath + " WDS_KEY=" + wdsKey + " " +
                        globalHostArgs + " " + Arguments
                    };
                    lines = tmplines;
                }

                else if (mode.Contains("ipxe"))
                {
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac +
                           ".ipxe";

                    string[] tmplines =
                    {
                        @"#!ipxe",
                        @"kernel " + webPath + "IpxeBoot?filename=" + Kernel +
                        "&type=kernel" + " initrd=" + BootImage +
                        " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + Direction +
                        " consoleblank=0" +
                        " web=" + webPath + " WDS_KEY=" + wdsKey + " " +
                        globalHostArgs + " " + Arguments,
                        @"imgfetch --name " + BootImage + " " + webPath +
                        "IpxeBoot?filename=" + BootImage + "&type=bootimage",
                        @"boot"
                    };
                    lines = tmplines;
                }
                else if (mode.Contains("grub"))
                {
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + PxeHostMac +
                           ".cfg";

                    string[] tmplines =
                    {
                        @"set default=0",
                        @"set timeout=0",
                        @"menuentry ""CrucibleWDS"" --unrestricted {",
                        @"echo Please Wait While The Boot Image Is Transferred.  This May Take A Few Minutes.",
                        @"linux /kernels/" + Kernel +
                        " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + Direction +
                        " consoleblank=0" +
                        " web=" + webPath + " WDS_KEY=" + wdsKey + " " +
                        globalHostArgs + " " + Arguments,
                        @"initrd /images/" + BootImage,
                        @"}"
                    };
                    lines = tmplines;
                }

                var fileOps = new FileOps();
                if (File.Exists(path))
                {
                    var host = new Computer {Mac = Utility.PxeMacToMac(PxeHostMac)};
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        if (mode.Contains("ipxe"))
                            fileOps.MoveFile(
                                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                PxeHostMac + ".ipxe",
                                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                PxeHostMac + ".ipxe.custom");
                        else if (mode.Contains("grub"))
                            fileOps.MoveFile(
                                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                PxeHostMac + ".cfg",
                                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                PxeHostMac + ".cfg.custom");
                        else
                            fileOps.MoveFile(
                                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                PxeHostMac,
                                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                PxeHostMac + ".custom");
                    }
                    else
                    {
                        Utility.Message = "The PXE File Already Exists";
                        return false;
                    }
                }

                try
                {
                    if (path != null)
                    {
                        File.WriteAllLines(path, lines);
                        fileOps.SetUnixPermissions(path);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.ToString());
                    Utility.Message = "Could Not Create PXE File";
                    return false;
                }
            }

            return true;
        }
    }
}