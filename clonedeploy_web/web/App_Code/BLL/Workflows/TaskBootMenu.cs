using System;
using System.Collections.Generic;
using System.IO;
using BLL;
using Helpers;

namespace Pxe
{
    public class TaskBootMenu
    {
        private readonly Models.Computer _computer;
        private readonly string _direction;

        public TaskBootMenu(Models.Computer computer,string direction)
        {
            _computer = computer;
            _direction = direction;
        }

        public bool CreatePxeBoot()
        {
            //FIX ME
            var imageProfile = BLL.LinuxProfile.ReadProfile(_computer.ImageProfile);
            
            var pxeHostMac = Utility.MacToPxeMac(_computer.Mac);
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
                List<Tuple<string, string, string>> list = new List<Tuple<string, string, string>>
                {
                    Tuple.Create("bios", "", bootMenu.BiosMenu),
                    Tuple.Create("bios", ".ipxe", bootMenu.BiosMenu),
                    Tuple.Create("efi32", "",bootMenu.Efi32Menu),
                    Tuple.Create("efi32", ".ipxe", bootMenu.Efi32Menu),
                    Tuple.Create("efi64", "" , bootMenu.Efi64Menu),
                    Tuple.Create("efi64", ".ipxe", bootMenu.Efi64Menu),
                    Tuple.Create("efi64", ".cfg", bootMenu.Efi64Menu)
                };
                //var biosPathipxe = tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                 //                  Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                  //                 pxeHostMac +
                   //                ".ipxe";
                string[] biosLinesipxe =
                {
                    @"#!ipxe",
                    @"kernel " + webPath + "IpxeBoot?filename=" + imageProfile.Kernel +
                    "&type=kernel" + " initrd=" + imageProfile.BootImage +
                    " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + _direction + " consoleblank=0" +
                    " web=" +
                    webPath + " WDS_KEY=" + wdsKey + " " +
                    globalHostArgs +
                    " " + imageProfile.KernelArguments,
                    @"imgfetch --name " + imageProfile.BootImage + " " + webPath +
                    "IpxeBoot?filename=" + imageProfile.BootImage + "&type=bootimage",
                    @"boot"
                };

                var biosPath = tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                               Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac;
                string[] biosLines =
                {
                    @"DEFAULT cruciblewds",
                    @"LABEL cruciblewds", @"KERNEL kernels" + Path.DirectorySeparatorChar + imageProfile.Kernel,
                    @"APPEND initrd=images" + Path.DirectorySeparatorChar + imageProfile.BootImage +
                    " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + _direction + " consoleblank=0" +
                    " web=" +
                    webPath + " WDS_KEY=" + wdsKey + " " +
                    globalHostArgs +
                    " " + imageProfile.KernelArguments
                };


                var efi32Pathipxe = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                                    Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                    pxeHostMac +
                                    ".ipxe";
                string[] efi32Linesipxe =
                {
                    @"#!ipxe",
                    @"kernel " + webPath + "IpxeBoot?filename=" + imageProfile.Kernel +
                    "&type=kernel" + " initrd=" + imageProfile.BootImage +
                    " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + _direction + " consoleblank=0" +
                    " web=" +
                    webPath + " WDS_KEY=" + wdsKey + " " +
                    globalHostArgs +
                    " " + imageProfile.KernelArguments,
                    @"imgfetch --name " + imageProfile.BootImage + " " + webPath +
                    "IpxeBoot?filename=" + imageProfile.BootImage + "&type=bootimage",
                    @"boot"
                };

                var efi32Path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac;
                string[] efi32Lines =
                {
                    @"DEFAULT cruciblewds",
                    @"LABEL cruciblewds", @"KERNEL kernels" + Path.DirectorySeparatorChar + imageProfile.Kernel,
                    @"APPEND initrd=images" + Path.DirectorySeparatorChar + imageProfile.BootImage +
                    " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + _direction + " consoleblank=0" +
                    " web=" +
                    webPath + " WDS_KEY=" + wdsKey + " " +
                    globalHostArgs +
                    " " + imageProfile.KernelArguments
                };


                var efi64Pathipxe = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                                    Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                    pxeHostMac +
                                    ".ipxe";
                string[] efi64Linesipxe =
                {
                    @"#!ipxe",
                    @"kernel " + webPath + "IpxeBoot?filename=" + imageProfile.Kernel +
                    "&type=kernel" + " initrd=" + imageProfile.BootImage +
                    " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + _direction + " consoleblank=0" +
                    " web=" +
                    webPath + " WDS_KEY=" + wdsKey + " " +
                    globalHostArgs +
                    " " + imageProfile.KernelArguments,
                    @"imgfetch --name " + imageProfile.BootImage + " " + webPath +
                    "IpxeBoot?filename=" + imageProfile.BootImage + "&type=bootimage",
                    @"boot"
                };

                var efi64Path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac;
                string[] efi64Lines =
                {
                    @"DEFAULT cruciblewds",
                    @"LABEL cruciblewds", @"KERNEL kernels" + Path.DirectorySeparatorChar + imageProfile.Kernel,
                    @"APPEND initrd=images" + Path.DirectorySeparatorChar + imageProfile.BootImage +
                    " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + _direction + " consoleblank=0" +
                    " web=" +
                    webPath + " WDS_KEY=" + wdsKey + " " +
                    globalHostArgs +
                    " " + imageProfile.KernelArguments
                };

                var efi64PathGrub = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                                    Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                    pxeHostMac +
                                    ".cfg";
                string[] efi64LinesGrub =
                {
                    @"set default=0",
                    @"set timeout=0",
                    @"menuentry ""CrucibleWDS"" --unrestricted {",
                    @"echo Please Wait While The Boot Image Is Transferred.  This May Take A Few Minutes.",
                    @"linux /kernels/" + imageProfile.Kernel + " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" +
                    _direction + " consoleblank=0" + " web=" + webPath + " WDS_KEY=" +
                    wdsKey + " " +
                    globalHostArgs + " " + imageProfile.KernelArguments,
                    @"initrd /images/" + imageProfile.BootImage,
                    @"}"
                };

                var fileOps = new FileOps();
                var host = BLL.Computer.GetComputerFromMac(Utility.PxeMacToMac(pxeHostMac));
                if (File.Exists(biosPath))
                {  
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        fileOps.MoveFile(biosPath, biosPath + ".custom");
                    }
                    else
                    {
                        //Message.Text = "The PXE File Already Exists";
                        return false;
                    }
                }

                if (File.Exists(biosPathipxe))
                {
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        fileOps.MoveFile(
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe",
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe.custom");
                    }
                    else
                    {
                        //Message.Text = "The PXE File Already Exists";
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
                        //Message.Text = "The PXE File Already Exists";
                        return false;
                    }
                }

                if (File.Exists(efi32Pathipxe))
                {
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        fileOps.MoveFile(
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe",
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe.custom");
                    }
                    else
                    {
                        //Message.Text = "The PXE File Already Exists";
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
                        //Message.Text = "The PXE File Already Exists";
                        return false;
                    }
                }

                if (File.Exists(efi64Pathipxe))
                {
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        fileOps.MoveFile(
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe",
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe.custom");
                    }
                    else
                    {
                        //Message.Text = "The PXE File Already Exists";
                        return false;
                    }
                }

                if (File.Exists(efi64PathGrub))
                {
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        fileOps.MoveFile(
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".cfg",
                            tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".cfg.custom");
                    }
                    else
                    {
                        //Message.Text = "The PXE File Already Exists";
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
                    //Message.Text = "Could Not Create PXE File";
                    return false;
                }
            } // end proxy mode
            else
            {
                if (mode == "pxelinux" || mode == "syslinux_32_efi" || mode == "syslinux_64_efi")
                {
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac;
                    string[] tmplines =
                    {
                        @"DEFAULT cruciblewds",
                        @"LABEL cruciblewds", @"KERNEL " + "kernels" + Path.DirectorySeparatorChar + imageProfile.Kernel,
                        @"APPEND initrd=" + "images" + Path.DirectorySeparatorChar + imageProfile.BootImage +
                        " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + _direction +
                        " consoleblank=0" +
                        " web=" + webPath + " WDS_KEY=" + wdsKey + " " +
                        globalHostArgs + " " + imageProfile.KernelArguments
                    };
                    lines = tmplines;
                }

                else if (mode.Contains("ipxe"))
                {
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".ipxe";

                    string[] tmplines =
                    {
                        @"#!ipxe",
                        @"kernel " + webPath + "IpxeBoot?filename=" + imageProfile.Kernel +
                        "&type=kernel" + " initrd=" + imageProfile.BootImage +
                        " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + _direction +
                        " consoleblank=0" +
                        " web=" + webPath + " WDS_KEY=" + wdsKey + " " +
                        globalHostArgs + " " + imageProfile.KernelArguments,
                        @"imgfetch --name " + imageProfile.BootImage + " " + webPath +
                        "IpxeBoot?filename=" + imageProfile.BootImage + "&type=bootimage",
                        @"boot"
                    };
                    lines = tmplines;
                }
                else if (mode.Contains("grub"))
                {
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".cfg";

                    string[] tmplines =
                    {
                        @"set default=0",
                        @"set timeout=0",
                        @"menuentry ""CrucibleWDS"" --unrestricted {",
                        @"echo Please Wait While The Boot Image Is Transferred.  This May Take A Few Minutes.",
                        @"linux /kernels/" + imageProfile.Kernel +
                        " root=/dev/ram0 rw ramdisk_size=127000 ip=dhcp imgDirection=" + _direction +
                        " consoleblank=0" +
                        " web=" + webPath + " WDS_KEY=" + wdsKey + " " +
                        globalHostArgs + " " + imageProfile.KernelArguments,
                        @"initrd /images/" + imageProfile.BootImage,
                        @"}"
                    };
                    lines = tmplines;
                }

                var fileOps = new FileOps();
                if (File.Exists(path))
                {
                    var host = new Models.Computer {Mac = Utility.PxeMacToMac(pxeHostMac)};
                    if (Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled)))
                    {
                        if (mode.Contains("ipxe"))
                            fileOps.MoveFile(
                                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                pxeHostMac + ".ipxe",
                                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                pxeHostMac + ".ipxe.custom");
                        else if (mode.Contains("grub"))
                            fileOps.MoveFile(
                                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                pxeHostMac + ".cfg",
                                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                pxeHostMac + ".cfg.custom");
                        else
                            fileOps.MoveFile(
                                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                pxeHostMac,
                                tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                pxeHostMac + ".custom");
                    }
                    else
                    {
                        //Message.Text = "The PXE File Already Exists";
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
                    //Message.Text = "Could Not Create PXE File";
                    return false;
                }
            }

            return true;
        }
    }
}