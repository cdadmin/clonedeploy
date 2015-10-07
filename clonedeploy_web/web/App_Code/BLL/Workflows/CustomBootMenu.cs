using System;
using System.IO;
using Helpers;
using Models;

namespace Pxe
{
    public class CustomBootMenu
    {
        public string FileName { get; set; }
        public Computer Host { get; set; }

        /*public void MoveCustomInactiveToActive()
        {
            var tftpPath = Settings.TftpPath;
            var proxyDhcp = Settings.ProxyDhcp;
            var fileOps = new FileOps();
            var pxeHostMac = Utility.MacToPxeMac(Host.Mac);
            if (proxyDhcp == "Yes")
            {
                try
                {
                    fileOps.MoveFile(
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".ipxe.custom",
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".ipxe");
                }
                catch
                {
                    // ignored
                }
                try
                {
                    fileOps.MoveFile(
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".custom",
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac);
                }
                catch
                {
                    // ignored
                }
                try
                {
                    fileOps.MoveFile(
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".ipxe.custom",
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".ipxe");
                }
                catch
                {
                    // ignored
                }
                try
                {
                    fileOps.MoveFile(
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".custom",
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac);
                }
                catch
                {
                    // ignored
                }
                try
                {
                    fileOps.MoveFile(
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".ipxe.custom",
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".ipxe");
                }
                catch
                {
                    // ignored
                }
                try
                {
                    fileOps.MoveFile(
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".cfg.custom",
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac + ".cfg");
                }
                catch
                {
                    // ignored
                }
                try
                {
                    fileOps.MoveFile(
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".custom",
                        tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                        Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac);
                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                try
                {
                    fileOps.MoveFile(
                        tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".ipxe.custom",
                        tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".ipxe");
                }
                catch
                {
                    // ignored
                }
                try
                {
                    fileOps.MoveFile(
                        tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".cfg.custom",
                        tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".cfg");
                }
                catch
                {
                    // ignored
                }
                try
                {
                    fileOps.MoveFile(
                        tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                        ".custom",
                        tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac);
                }
                catch
                {
                    // ignored
                }
            }
        }*/

        public void RemoveCustomBootMenu()
        {
            var tftpPath = Settings.TftpPath;
            var mode = Settings.PxeMode;
            var pxeHostMac = Utility.MacToPxeMac(Host.Mac);
            var active = new BLL.ActiveImagingTask().IsComputerActive(Host.Id);
            string path;
            var fileOps = new FileOps();

            var proxyDhcp = Settings.ProxyDhcp;

            if (!active)
            {
                if (proxyDhcp == "Yes")
                {
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".ipxe";
                    fileOps.DeletePath(path);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac;
                    fileOps.DeletePath(path);

                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".ipxe";
                    fileOps.DeletePath(path);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac;
                    fileOps.DeletePath(path);

                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".ipxe";
                    fileOps.DeletePath(path);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac;
                    fileOps.DeletePath(path);

                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".cfg";
                    fileOps.DeletePath(path);
                }
                else
                {
                    if (mode.Contains("ipxe"))
                        path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeHostMac +
                               ".ipxe";
                    else if (mode.Contains("grub"))
                        path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeHostMac +
                               ".cfg";
                    else
                        path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeHostMac;
                    fileOps.DeletePath(path);
                }
            }
            else
            {
                if (proxyDhcp == "Yes")
                {
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".custom";
                    fileOps.DeletePath(path);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".custom";
                    fileOps.DeletePath(path);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".custom";
                    fileOps.DeletePath(path);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".ipxe.custom";
                    fileOps.DeletePath(path);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".ipxe.custom";
                    fileOps.DeletePath(path);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".ipxe.custom";
                    fileOps.DeletePath(path);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".cfg.custom";
                    fileOps.DeletePath(path);
                }
                else
                {
                    if (mode.Contains("ipxe"))
                        path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeHostMac +
                               ".ipxe.custom";
                    else if (mode.Contains("grub"))
                        path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeHostMac +
                               ".cfg.custom";
                    else
                        path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeHostMac +
                               ".custom";
                    fileOps.DeletePath(path);
                }
            }

            try
            {
                Host.CustomBootEnabled = "0";
                new BLL.Computer().UpdateComputer(Host);
                var history = new History
                {
                    Event = "Remove Boot Menu",
                    Type = "Host",
                    Notes = Host.Mac,
                    TypeId = Host.Id.ToString()
                };

                history.CreateEvent();
                Message.Text = "Successfully Removed Custom Boot Menu For This Host";
            }

            catch (Exception ex)
            {
                Message.Text = "Could Not Remove Custom Boot Menu.  Check The Exception Log For More Info.";
                Logger.Log(ex.Message);
            }
        }

        public void SetCustomBootMenu()
        {
            var mode = Settings.PxeMode;
            var pxeHostMac = Utility.MacToPxeMac(Host.Mac);
            var active = new BLL.ActiveImagingTask().IsComputerActive(Host.Id);
            string path;

            var proxyDhcp = Settings.ProxyDhcp;
            var tftpPath = Settings.TftpPath;
            var fileOps = new FileOps();
            if (!active)
            {
                if (proxyDhcp == "Yes")
                {
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".ipxe";
                    fileOps.WritePath(path, FileName);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac;
                    fileOps.WritePath(path, FileName);

                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".ipxe";
                    fileOps.WritePath(path, FileName);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac;
                    fileOps.WritePath(path, FileName);

                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".ipxe";
                    fileOps.WritePath(path, FileName);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".cfg";
                    fileOps.WritePath(path, FileName);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac;
                    fileOps.WritePath(path, FileName);
                }
                else
                {
                    if (mode.Contains("ipxe"))
                        path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeHostMac +
                               ".ipxe";
                    else if (mode.Contains("grub"))
                        path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeHostMac +
                               ".cfg";
                    else
                        path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeHostMac;

                    fileOps.WritePath(path, FileName);
                }
            }
            else
            {
                if (proxyDhcp == "Yes")
                {
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".custom";
                    fileOps.WritePath(path, FileName);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".custom";
                    fileOps.WritePath(path, FileName);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".custom";
                    fileOps.WritePath(path, FileName);

                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".ipxe.custom";
                    fileOps.WritePath(path, FileName);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".ipxe.custom";
                    fileOps.WritePath(path, FileName);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".ipxe.custom";
                    fileOps.WritePath(path, FileName);
                    path = tftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                           ".cfg.custom";
                    fileOps.WritePath(path, FileName);
                }
                else
                {
                    if (mode.Contains("ipxe"))
                        path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeHostMac +
                               ".ipxe.custom";
                    else if (mode.Contains("grub"))
                        path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeHostMac +
                               ".cfg.custom";
                    else
                        path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeHostMac +
                               ".custom";
                    fileOps.WritePath(path, FileName);
                }
            }

            try
            {
                Host.CustomBootEnabled = "1";
                new BLL.Computer().UpdateComputer(Host);
                var history = new History
                {
                    Event = "Set Boot Menu",
                    Type = "Host",
                    Notes = Host.Mac,
                    TypeId = Host.Id.ToString()
                };
                history.CreateEvent();
                Message.Text = "Successfully Set Custom Boot Menu For This Host";
            }

            catch (Exception ex)
            {
                Message.Text = "Could Not Set Custom Boot Menu.  Check The Exception Log For More Info.";
                Logger.Log(ex.Message);
            }
        }
    }
}