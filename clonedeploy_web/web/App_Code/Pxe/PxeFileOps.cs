using System;
using System.IO;
using BLL;
using Global;
using Models;

namespace Pxe
{
    public class PxeFileOps
    {
        public bool CleanPxeBoot(string pxeHostMac)
        {
            var host = new BLL.Computer().GetComputerFromMac(Utility.PxeMacToMac(pxeHostMac));
            var mode = Settings.PxeMode;
            var isCustomBootTemplate = Convert.ToBoolean(Convert.ToInt16(host.CustomBootEnabled));
            var proxyDhcp = Settings.ProxyDhcp;
            var fileOps = new FileOps();
            if (proxyDhcp == "Yes")
            {
                try
                {
                    File.Delete(Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac);
                    if (isCustomBootTemplate)
                        fileOps.MoveFile(
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".custom",
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac);


                    File.Delete(Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                                ".ipxe");
                    if (isCustomBootTemplate)
                        fileOps.MoveFile(
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe.custom",
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "bios" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe");


                    File.Delete(Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac);
                    if (isCustomBootTemplate)
                        fileOps.MoveFile(
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".custom",
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac);


                    File.Delete(Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                                ".ipxe");
                    if (isCustomBootTemplate)
                        fileOps.MoveFile(
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe.custom",
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi32" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe");


                    File.Delete(Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac);
                    if (isCustomBootTemplate)
                        fileOps.MoveFile(
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".custom",
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac);


                    File.Delete(Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                                ".ipxe");
                    if (isCustomBootTemplate)
                        fileOps.MoveFile(
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe.custom",
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe");

                    File.Delete(Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                                Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                                ".cfg");
                    if (isCustomBootTemplate)
                        fileOps.MoveFile(
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".cfg.custom",
                            Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + "efi64" +
                            Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".cfg");


                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.ToString());
                    Utility.Message = "Could Not Delete PXE File";
                    return false;
                }
            }
            try
            {
                if (mode.Contains("ipxe"))
                {
                    File.Delete(Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                pxeHostMac + ".ipxe");
                    if (isCustomBootTemplate)
                        fileOps.MoveFile(
                            Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe.custom",
                            Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".ipxe");
                }

                else if (mode.Contains("grub"))
                {
                    File.Delete(Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                pxeHostMac + ".cfg");
                    if (isCustomBootTemplate)
                        fileOps.MoveFile(
                            Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".cfg.custom",
                            Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".cfg");
                }
                else
                {
                    File.Delete(Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                                pxeHostMac);
                    if (isCustomBootTemplate)
                        fileOps.MoveFile(
                            Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac +
                            ".custom",
                            Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeHostMac);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                Utility.Message = "Could Not Delete PXE File";
                return false;
            }
        }

        public void CopyPxeFiles(string mode)
        {
            var proxyDhcp = Settings.ProxyDhcp;
            string biosFile = null;
            string efi32File = null;
            string efi64File = null;
            var sourcePath = Settings.TftpPath + "static" + Path.DirectorySeparatorChar;
            string destinationPath;
            var fileOps = new FileOps();
            if (proxyDhcp == "Yes")
            {
                destinationPath = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar;
                biosFile = Settings.ProxyBiosFile;
                efi32File = Settings.ProxyEfi32File;
                efi64File = Settings.ProxyEfi64File;
            }
            else
                destinationPath = Settings.TftpPath;
            try
            {
                if (proxyDhcp == "Yes")
                {
                    string type;


                    switch (biosFile)
                    {
                        case "ipxe":
                            type = "bios";
                            File.Copy(
                                sourcePath + "ipxe" + Path.DirectorySeparatorChar + "proxy" +
                                Path.DirectorySeparatorChar + "ipxe" + Path.DirectorySeparatorChar + "undionly.kpxe",
                                destinationPath + type + Path.DirectorySeparatorChar + "pxeboot.0", true);

                            fileOps.SetUnixPermissions(destinationPath + type + Path.DirectorySeparatorChar +
                                                       "pxeboot.0");

                            break;
                        case "lpxelinux":
                            File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "lpxelinux.0",
                                destinationPath + "bios" + Path.DirectorySeparatorChar + "pxeboot.0", true);
                            File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "ldlinux.c32",
                                destinationPath + "bios" + Path.DirectorySeparatorChar + "ldlinux.c32", true);
                            File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "libcom32.c32",
                                destinationPath + "bios" + Path.DirectorySeparatorChar + "libcom32.c32", true);
                            File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "libutil.c32",
                                destinationPath + "bios" + Path.DirectorySeparatorChar + "libutil.c32", true);
                            File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "vesamenu.c32",
                                destinationPath + "bios" + Path.DirectorySeparatorChar + "vesamenu.c32", true);

                            fileOps.SetUnixPermissions(destinationPath + "bios" + Path.DirectorySeparatorChar +
                                                       "ldlinux.c32");
                            fileOps.SetUnixPermissions(destinationPath + "bios" + Path.DirectorySeparatorChar +
                                                       "libcom32.c32");
                            fileOps.SetUnixPermissions(destinationPath + "bios" + Path.DirectorySeparatorChar +
                                                       "libutil.c32");
                            fileOps.SetUnixPermissions(destinationPath + "bios" + Path.DirectorySeparatorChar +
                                                       "vesamenu.c32");

                            break;
                        case "pxelinux":
                            File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "pxelinux.0",
                                destinationPath + "bios" + Path.DirectorySeparatorChar + "pxeboot.0", true);
                            File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "ldlinux.c32",
                                destinationPath + "bios" + Path.DirectorySeparatorChar + "ldlinux.c32", true);
                            File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "libcom32.c32",
                                destinationPath + "bios" + Path.DirectorySeparatorChar + "libcom32.c32", true);
                            File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "libutil.c32",
                                destinationPath + "bios" + Path.DirectorySeparatorChar + "libutil.c32", true);
                            File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "vesamenu.c32",
                                destinationPath + "bios" + Path.DirectorySeparatorChar + "vesamenu.c32", true);

                            fileOps.SetUnixPermissions(destinationPath + "bios" + Path.DirectorySeparatorChar +
                                                       "pxeboot.0");
                            fileOps.SetUnixPermissions(destinationPath + "bios" + Path.DirectorySeparatorChar +
                                                       "ldlinux.c32");
                            fileOps.SetUnixPermissions(destinationPath + "bios" + Path.DirectorySeparatorChar +
                                                       "libcom32.c32");
                            fileOps.SetUnixPermissions(destinationPath + "bios" + Path.DirectorySeparatorChar +
                                                       "libutil.c32");
                            fileOps.SetUnixPermissions(destinationPath + "bios" + Path.DirectorySeparatorChar +
                                                       "vesamenu.c32");

                            break;
                    }
                    switch (efi32File)
                    {
                        case "ipxe_32_efi":
                            type = "efi32";
                            File.Copy(
                                sourcePath + "ipxe" + Path.DirectorySeparatorChar + "proxy" +
                                Path.DirectorySeparatorChar + "ipxe_efi_32" + Path.DirectorySeparatorChar + "ipxe.efi",
                                destinationPath + type + Path.DirectorySeparatorChar + "pxeboot.0", true);


                            fileOps.SetUnixPermissions(destinationPath + type + Path.DirectorySeparatorChar +
                                                       "pxeboot.0");

                            break;
                        case "ipxe_32_efi_snp":
                            type = "efi32";
                            File.Copy(
                                sourcePath + "ipxe" + Path.DirectorySeparatorChar + "proxy" +
                                Path.DirectorySeparatorChar + "ipxe_efi_32" + Path.DirectorySeparatorChar + "snp.efi",
                                destinationPath + type + Path.DirectorySeparatorChar + "pxeboot.0", true);


                            fileOps.SetUnixPermissions(destinationPath + type + Path.DirectorySeparatorChar +
                                                       "pxeboot.0");

                            break;
                        case "ipxe_32_efi_snponly":
                            type = "efi32";
                            File.Copy(
                                sourcePath + "ipxe" + Path.DirectorySeparatorChar + "proxy" +
                                Path.DirectorySeparatorChar + "ipxe_efi_32" + Path.DirectorySeparatorChar +
                                "snponly.efi", destinationPath + type + Path.DirectorySeparatorChar + "pxeboot.0", true);


                            fileOps.SetUnixPermissions(destinationPath + type + Path.DirectorySeparatorChar +
                                                       "pxeboot.0");

                            break;
                        case "syslinux_32_efi":
                            File.Copy(sourcePath + "syslinux_efi_32" + Path.DirectorySeparatorChar + "syslinux.efi",
                                destinationPath + "efi32" + Path.DirectorySeparatorChar + "pxeboot.0", true);
                            File.Copy(sourcePath + "syslinux_efi_32" + Path.DirectorySeparatorChar + "ldlinux.e32",
                                destinationPath + "efi32" + Path.DirectorySeparatorChar + "ldlinux.e32", true);
                            File.Copy(sourcePath + "syslinux_efi_32" + Path.DirectorySeparatorChar + "libcom32.c32",
                                destinationPath + "efi32" + Path.DirectorySeparatorChar + "libcom32.c32", true);
                            File.Copy(sourcePath + "syslinux_efi_32" + Path.DirectorySeparatorChar + "libutil.c32",
                                destinationPath + "efi32" + Path.DirectorySeparatorChar + "libutil.c32", true);
                            File.Copy(sourcePath + "syslinux_efi_32" + Path.DirectorySeparatorChar + "vesamenu.c32",
                                destinationPath + "efi32" + Path.DirectorySeparatorChar + "vesamenu.c32", true);

                            fileOps.SetUnixPermissions(destinationPath + "efi32" + Path.DirectorySeparatorChar +
                                                       "pxeboot.0");
                            fileOps.SetUnixPermissions(destinationPath + "efi32" + Path.DirectorySeparatorChar +
                                                       "ldlinux.e32");
                            fileOps.SetUnixPermissions(destinationPath + "efi32" + Path.DirectorySeparatorChar +
                                                       "libcom32.c32");
                            fileOps.SetUnixPermissions(destinationPath + "efi32" + Path.DirectorySeparatorChar +
                                                       "libutil.c32");
                            fileOps.SetUnixPermissions(destinationPath + "efi32" + Path.DirectorySeparatorChar +
                                                       "vesamenu.c32");

                            break;
                    }
                    switch (efi64File)
                    {
                        case "ipxe_64_efi":
                            type = "efi64";
                            File.Copy(
                                sourcePath + "ipxe" + Path.DirectorySeparatorChar + "proxy" +
                                Path.DirectorySeparatorChar + "ipxe_efi_64" + Path.DirectorySeparatorChar + "ipxe.efi",
                                destinationPath + type + Path.DirectorySeparatorChar + "pxeboot.0", true);


                            fileOps.SetUnixPermissions(destinationPath + type + Path.DirectorySeparatorChar +
                                                       "pxeboot.0");

                            break;
                        case "ipxe_64_efi_snp":
                            type = "efi64";
                            File.Copy(
                                sourcePath + "ipxe" + Path.DirectorySeparatorChar + "proxy" +
                                Path.DirectorySeparatorChar + "ipxe_efi_64" + Path.DirectorySeparatorChar + "snp.efi",
                                destinationPath + type + Path.DirectorySeparatorChar + "pxeboot.0", true);


                            fileOps.SetUnixPermissions(destinationPath + type + Path.DirectorySeparatorChar +
                                                       "pxeboot.0");

                            break;
                        case "ipxe_64_efi_snponly":
                            type = "efi64";
                            File.Copy(
                                sourcePath + "ipxe" + Path.DirectorySeparatorChar + "proxy" +
                                Path.DirectorySeparatorChar + "ipxe_efi_64" + Path.DirectorySeparatorChar +
                                "snponly.efi", destinationPath + type + Path.DirectorySeparatorChar + "pxeboot.0", true);


                            fileOps.SetUnixPermissions(destinationPath + type + Path.DirectorySeparatorChar +
                                                       "pxeboot.0");

                            break;
                        case "syslinux_64_efi":
                            File.Copy(sourcePath + "syslinux_efi_64" + Path.DirectorySeparatorChar + "syslinux.efi",
                                destinationPath + "efi64" + Path.DirectorySeparatorChar + "pxeboot.0", true);
                            File.Copy(sourcePath + "syslinux_efi_64" + Path.DirectorySeparatorChar + "ldlinux.e64",
                                destinationPath + "efi64" + Path.DirectorySeparatorChar + "ldlinux.e64", true);
                            File.Copy(sourcePath + "syslinux_efi_64" + Path.DirectorySeparatorChar + "libcom32.c32",
                                destinationPath + "efi64" + Path.DirectorySeparatorChar + "libcom32.c32", true);
                            File.Copy(sourcePath + "syslinux_efi_64" + Path.DirectorySeparatorChar + "libutil.c32",
                                destinationPath + "efi64" + Path.DirectorySeparatorChar + "libutil.c32", true);
                            File.Copy(sourcePath + "syslinux_efi_64" + Path.DirectorySeparatorChar + "vesamenu.c32",
                                destinationPath + "efi64" + Path.DirectorySeparatorChar + "vesamenu.c32", true);

                            fileOps.SetUnixPermissions(destinationPath + "efi64" + Path.DirectorySeparatorChar +
                                                       "pxeboot.0");
                            fileOps.SetUnixPermissions(destinationPath + "efi64" + Path.DirectorySeparatorChar +
                                                       "ldlinux.e64");
                            fileOps.SetUnixPermissions(destinationPath + "efi64" + Path.DirectorySeparatorChar +
                                                       "libcom32.c32");
                            fileOps.SetUnixPermissions(destinationPath + "efi64" + Path.DirectorySeparatorChar +
                                                       "libutil.c32");
                            fileOps.SetUnixPermissions(destinationPath + "efi64" + Path.DirectorySeparatorChar +
                                                       "vesamenu.c32");

                            break;
                        case "grub_64_efi":
                            File.Copy(sourcePath + "grub" + Path.DirectorySeparatorChar + "bootx64.efi",
                                destinationPath + "efi64" + Path.DirectorySeparatorChar + "pxeboot.0", true);
                            File.Copy(sourcePath + "grub" + Path.DirectorySeparatorChar + "grubx64.efi",
                                destinationPath + "efi64" + Path.DirectorySeparatorChar + "grubx64.efi", true);


                            fileOps.SetUnixPermissions(destinationPath + "efi64" + Path.DirectorySeparatorChar +
                                                       "pxeboot.0");
                            fileOps.SetUnixPermissions(destinationPath + "efi64" + Path.DirectorySeparatorChar +
                                                       "grubx64.efi");

                            break;
                    }
                }
                else
                {
                    if (mode.Contains("ipxe"))
                    {
                        switch (mode)
                        {
                            case "ipxe":
                                File.Copy(
                                    sourcePath + "ipxe" + Path.DirectorySeparatorChar + "normal" +
                                    Path.DirectorySeparatorChar + "ipxe" + Path.DirectorySeparatorChar + "undionly.kpxe",
                                    destinationPath + "pxeboot.0", true);
                                break;
                            case "ipxe_32_efi":
                                File.Copy(
                                    sourcePath + "ipxe" + Path.DirectorySeparatorChar + "normal" +
                                    Path.DirectorySeparatorChar + "ipxe_efi_32" + Path.DirectorySeparatorChar +
                                    "ipxe.efi", destinationPath + "pxeboot.0", true);
                                break;
                            case "ipxe_64_efi":
                                File.Copy(
                                    sourcePath + "ipxe" + Path.DirectorySeparatorChar + "normal" +
                                    Path.DirectorySeparatorChar + "ipxe_efi_64" + Path.DirectorySeparatorChar +
                                    "ipxe.efi", destinationPath + "pxeboot.0", true);
                                break;
                            case "ipxe_32_efi_snp":
                                File.Copy(
                                    sourcePath + "ipxe" + Path.DirectorySeparatorChar + "normal" +
                                    Path.DirectorySeparatorChar + "ipxe_efi_32" + Path.DirectorySeparatorChar +
                                    "snp.efi", destinationPath + "pxeboot.0", true);
                                break;
                            case "ipxe_64_efi_snp":
                                File.Copy(
                                    sourcePath + "ipxe" + Path.DirectorySeparatorChar + "normal" +
                                    Path.DirectorySeparatorChar + "ipxe_efi_64" + Path.DirectorySeparatorChar +
                                    "snp.efi", destinationPath + "pxeboot.0", true);
                                break;
                            case "ipxe_32_efi_snponly":
                                File.Copy(
                                    sourcePath + "ipxe" + Path.DirectorySeparatorChar + "normal" +
                                    Path.DirectorySeparatorChar + "ipxe_efi_32" + Path.DirectorySeparatorChar +
                                    "snponly.efi", destinationPath + "pxeboot.0", true);
                                break;
                            case "ipxe_64_efi_snponly":
                                File.Copy(
                                    sourcePath + "ipxe" + Path.DirectorySeparatorChar + "normal" +
                                    Path.DirectorySeparatorChar + "ipxe_efi_64" + Path.DirectorySeparatorChar +
                                    "snponly.efi", destinationPath + "pxeboot.0", true);
                                break;
                        }


                        fileOps.SetUnixPermissions(destinationPath + "pxeboot.0");
                    }
                    else
                        switch (mode)
                        {
                            case "lpxelinux":
                                File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "lpxelinux.0",
                                    destinationPath + "pxeboot.0", true);
                                File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "ldlinux.c32",
                                    destinationPath + "ldlinux.c32", true);
                                File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "libcom32.c32",
                                    destinationPath + "libcom32.c32", true);
                                File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "libutil.c32",
                                    destinationPath + "libutil.c32", true);
                                File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "vesamenu.c32",
                                    destinationPath + "vesamenu.c32", true);

                                fileOps.SetUnixPermissions(destinationPath + "pxeboot.0");
                                fileOps.SetUnixPermissions(destinationPath + "ldlinux.c32");
                                fileOps.SetUnixPermissions(destinationPath + "libcom32.c32");
                                fileOps.SetUnixPermissions(destinationPath + "libutil.c32");
                                fileOps.SetUnixPermissions(destinationPath + "vesamenu.c32");

                                break;
                            case "pxelinux":

                                File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "pxelinux.0",
                                    destinationPath + "pxeboot.0", true);
                                File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "ldlinux.c32",
                                    destinationPath + "ldlinux.c32", true);
                                File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "libcom32.c32",
                                    destinationPath + "libcom32.c32", true);
                                File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "libutil.c32",
                                    destinationPath + "libutil.c32", true);
                                File.Copy(sourcePath + "pxelinux" + Path.DirectorySeparatorChar + "vesamenu.c32",
                                    destinationPath + "vesamenu.c32", true);

                                fileOps.SetUnixPermissions(destinationPath + "pxeboot.0");
                                fileOps.SetUnixPermissions(destinationPath + "ldlinux.c32");
                                fileOps.SetUnixPermissions(destinationPath + "libcom32.c32");
                                fileOps.SetUnixPermissions(destinationPath + "libutil.c32");
                                fileOps.SetUnixPermissions(destinationPath + "vesamenu.c32");

                                break;
                            case "syslinux_32_efi":

                                File.Copy(
                                    sourcePath + "syslinux_efi_32" + Path.DirectorySeparatorChar + "syslinux.efi",
                                    destinationPath + "pxeboot.0", true);
                                File.Copy(sourcePath + "syslinux_efi_32" + Path.DirectorySeparatorChar + "ldlinux.e32",
                                    destinationPath + "ldlinux.e32", true);
                                File.Copy(
                                    sourcePath + "syslinux_efi_32" + Path.DirectorySeparatorChar + "libcom32.c32",
                                    destinationPath + "libcom32.c32", true);
                                File.Copy(sourcePath + "syslinux_efi_32" + Path.DirectorySeparatorChar + "libutil.c32",
                                    destinationPath + "libutil.c32", true);
                                File.Copy(
                                    sourcePath + "syslinux_efi_32" + Path.DirectorySeparatorChar + "vesamenu.c32",
                                    destinationPath + "vesamenu.c32", true);

                                fileOps.SetUnixPermissions(destinationPath + "pxeboot.0");
                                fileOps.SetUnixPermissions(destinationPath + "ldlinux.e32");
                                fileOps.SetUnixPermissions(destinationPath + "libcom32.c32");
                                fileOps.SetUnixPermissions(destinationPath + "libutil.c32");
                                fileOps.SetUnixPermissions(destinationPath + "vesamenu.c32");

                                break;
                            case "syslinux_64_efi":

                                File.Copy(
                                    sourcePath + "syslinux_efi_64" + Path.DirectorySeparatorChar + "syslinux.efi",
                                    destinationPath + "pxeboot.0", true);
                                File.Copy(sourcePath + "syslinux_efi_64" + Path.DirectorySeparatorChar + "ldlinux.e64",
                                    destinationPath + "ldlinux.e64", true);
                                File.Copy(
                                    sourcePath + "syslinux_efi_64" + Path.DirectorySeparatorChar + "libcom32.c32",
                                    destinationPath + "libcom32.c32", true);
                                File.Copy(sourcePath + "syslinux_efi_64" + Path.DirectorySeparatorChar + "libutil.c32",
                                    destinationPath + "libutil.c32", true);
                                File.Copy(
                                    sourcePath + "syslinux_efi_64" + Path.DirectorySeparatorChar + "vesamenu.c32",
                                    destinationPath + "vesamenu.c32", true);

                                fileOps.SetUnixPermissions(destinationPath + "pxeboot.0");
                                fileOps.SetUnixPermissions(destinationPath + "ldlinux.e64");
                                fileOps.SetUnixPermissions(destinationPath + "libcom32.c32");
                                fileOps.SetUnixPermissions(destinationPath + "libutil.c32");
                                fileOps.SetUnixPermissions(destinationPath + "vesamenu.c32");

                                break;
                            case "grub_64_efi":

                                File.Copy(sourcePath + "grub" + Path.DirectorySeparatorChar + "bootx64.efi",
                                    destinationPath + "pxeboot.0", true);
                                File.Copy(sourcePath + "grub" + Path.DirectorySeparatorChar + "grubx64.efi",
                                    destinationPath + "grubx64.efi", true);


                                fileOps.SetUnixPermissions(destinationPath + "pxeboot.0");
                                fileOps.SetUnixPermissions(destinationPath + "grubx64.efi");

                                break;
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
            }
        }

        public string GetDefaultMenuPath(string proxyMode)
        {
            var mode = Settings.PxeMode;
            var proxyDhcp = Settings.ProxyDhcp;
            string path;
            if (proxyDhcp == "Yes")
            {
                var biosFile = Settings.ProxyBiosFile;

                if (biosFile.Contains("ipxe"))
                    path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                           proxyMode + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                           Path.DirectorySeparatorChar + "default.ipxe";
                else
                    path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                           proxyMode + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                           Path.DirectorySeparatorChar + "default";
            }
            else
            {
                if (mode.Contains("ipxe"))
                    path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                           "default.ipxe";
                else
                    path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                           "default";
            }
            return path;
        }

        public string GetHostNonProxyPath(Models.Computer host, bool isActiveOrCustom)
        {
            var mode = Settings.PxeMode;
            var pxeHostMac = Utility.MacToPxeMac(host.Mac);
            string path;

            var fileName = isActiveOrCustom ? pxeHostMac : "default";

            if (mode.Contains("ipxe"))
                path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                       fileName + ".ipxe";
            else if (mode.Contains("grub"))
            {
                fileName = isActiveOrCustom ? pxeHostMac : "grub";
                path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                       fileName + ".cfg";
            }
            else
                path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                       fileName;

            return path;
        }

        public string GetHostProxyPath(Models.Computer host, bool isActiveOrCustom, string proxyType)
        {
            var pxeHostMac = Utility.MacToPxeMac(host.Mac);
            string path = null;


            var biosFile = Settings.ProxyBiosFile;
            var efi32File = Settings.ProxyEfi32File;
            var efi64File = Settings.ProxyEfi64File;

            var fileName = isActiveOrCustom ? pxeHostMac : "default";
            switch (proxyType)
            {
                case "bios":
                    if (biosFile.Contains("ipxe"))
                    {
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName + ".ipxe";
                    }
                    else
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName;
                    break;
                case "efi32":
                    if (efi32File.Contains("ipxe"))
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName + ".ipxe";
                    else
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName;
                    break;
                case "efi64":
                    if (efi64File.Contains("ipxe"))
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName + ".ipxe";
                    else if (efi64File.Contains("grub"))
                    {
                        fileName = isActiveOrCustom ? pxeHostMac : "grub";
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName + ".cfg";
                    }
                    else
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName;
                    break;
            }

            return path;
        }
    }
}