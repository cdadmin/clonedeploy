using System;
using System.IO;
using Helpers;

namespace BLL.Workflows
{
    public class CopyPxeBinaries
    {
        private const string BootFile = "pxeboot.0";
        private readonly string[] _ipxeBiosFiles = {"undionly.kpxe"};
        private readonly string[] _ipxeEfiFiles = {"ipxe.efi", "snp.efi", "snponly.efi"};

        private readonly string[] _syslinuxBiosFiles =
        {
            "ldlinux.c32", "libcom32.c32", "libutil.c32", "vesamenu.c32", 
            "pxelinux.0"
        };

        private readonly string[] _syslinuxEfi32Files =
        {
            "ldlinux.e32", "libcom32.c32", "libutil.c32", "vesamenu.c32",
            "syslinux.efi"
        };

        private readonly string[] _syslinuxEfi64Files =
        {
            "ldlinux.e64", "libcom32.c32", "libutil.c32", "vesamenu.c32",
            "syslinux.efi"
        };

        private readonly string[] _grubEfi64Files = {"bootx64.efi", "grubx64.efi"};

        private readonly string[] _winPEBiosFiles = { "bootmgr.exe", "pxeboot.n12" };

        private readonly string[] _winPEEfiFiles = { "bootmgfw.efi" };

        private readonly string _sourceRootPath = Settings.TftpPath + "static" + Path.DirectorySeparatorChar;

        public bool CopyFiles()
        {
            return Settings.ProxyDhcp == "Yes" ? CopyFilesForProxy() : CopyFilesForNonProxy();
        }

        private bool CopyFilesForProxy()
        {
            var biosFile = Settings.ProxyBiosFile;
            var efi32File = Settings.ProxyEfi32File;
            var efi64File = Settings.ProxyEfi64File;

            foreach (var file in _ipxeBiosFiles)
            {
                if (!CopyCommand("ipxe", "proxy", "ipxe", "bios", file, file)) return false;
                if(biosFile == "ipxe" && file == "undionly.kpxe")
                    if (!CopyCommand("ipxe", "proxy", "ipxe", "bios", file, BootFile))return false;
            }
            foreach (var file in _ipxeEfiFiles)
            {
                if (!CopyCommand("ipxe", "proxy", "ipxe_efi_32", "efi32", file, file)) return false;
                if ((efi32File == "ipxe_efi" && file == "ipxe.efi") ||
                    (efi32File == "ipxe_efi_snp" && file == "snp.efi")
                    || (efi32File == "ipxe_efi_snponly" && file == "snponly.efi"))
                    if (!CopyCommand("ipxe", "proxy", "ipxe_efi_32", "efi32", file, BootFile)) return false;

            }
            foreach (var file in _ipxeEfiFiles)
            {
                if (!CopyCommand("ipxe", "proxy", "ipxe_efi_64", "efi64", file, file)) return false;
                 if ((efi64File == "ipxe_efi" && file == "ipxe.efi") ||
                    (efi64File == "ipxe_efi_snp" && file == "snp.efi")
                    || (efi64File == "ipxe_efi_snponly" && file == "snponly.efi"))
                     if (!CopyCommand("ipxe", "proxy", "ipxe_efi_64", "efi64", file, BootFile)) return false;
            }

            foreach (var file in _syslinuxBiosFiles)
            {
                if (!CopyCommand("syslinux", "proxy", "pxelinux", "bios", file, file)) return false;
                if(biosFile == "pxelinux" && file == "pxelinux.0")
                    if (!CopyCommand("syslinux", "proxy", "pxelinux", "bios", file, BootFile)) return false;
            }

            foreach (var file in _syslinuxEfi32Files)
            {
                if (!CopyCommand("syslinux", "proxy", "syslinux_efi_32", "efi32", file, file)) return false;
                if (efi32File == "syslinux" && file == "syslinux.efi")
                    if (!CopyCommand("syslinux", "proxy", "syslinux_efi_32", "efi32", file, BootFile)) return false;
            }

            foreach (var file in _syslinuxEfi64Files)
            {
                if (!CopyCommand("syslinux", "proxy", "syslinux_efi_64", "efi64", file, file)) return false;
                if (efi64File == "syslinux" && file == "syslinux.efi")
                    if (!CopyCommand("syslinux", "proxy", "syslinux_efi_64", "efi64", file, BootFile)) return false;
            }

            foreach (var file in _grubEfi64Files)
            {
                if (!CopyCommand("grub", "", "", "efi64", file, file)) return false;
                if (!CopyCommand("grub", "", "", "", file, file)) return false;
                if(efi64File == "grub" && file == "bootx64.efi")
                    if (!CopyCommand("grub", "", "", "efi64", file, BootFile)) return false;
            }

            if (
                new Helpers.FileOps().FileExists(Settings.TftpPath + Path.DirectorySeparatorChar + "boot" +
                                                 Path.DirectorySeparatorChar + "boot.sdi"))
            {
                foreach (var file in _winPEEfiFiles)
                {
                    if (!CopyCommand("winpe", "", "winpe_efi_64", "efi64", file, file)) return false;
                    if (!CopyCommand("winpe", "", "winpe_efi_32", "efi32", file, file)) return false;
                    if (efi64File == "winpe_efi")
                    {
                        if (!CopyCommand("winpe", "", "winpe_efi_64", "efi64", file, BootFile)) return false;
                    }
                    if (efi32File == "winpe_efi")
                    {
                        if (!CopyCommand("winpe", "", "winpe_efi_32", "efi32", file, BootFile)) return false;
                    }

                }
                foreach (var file in _winPEBiosFiles)
                {
                    if (file == "bootmgr.exe")
                    {
                        if (!CopyCommand("winpe", "", "winpe", "", file, file)) return false;
                    }
                    if (!CopyCommand("winpe", "", "winpe", "bios", file, file)) return false;
                
                    if (biosFile == "winpe" && file == "pxeboot.n12")
                    {
                        if (!CopyCommand("winpe", "", "winpe", "bios", file, BootFile)) return false;
                    }
                }
            }

            return true;
        }

        private bool CopyFilesForNonProxy()
        {
            var pxeMode = Settings.PxeMode;
            switch (pxeMode)
            {
                case "ipxe":
                    if (!CopyCommand("ipxe", "normal", "ipxe", "", "undionly.kpxe", BootFile)) return false;
                    break;
                case "ipxe_efi32":
                    if (!CopyCommand("ipxe", "normal", "ipxe_efi_32", "", "ipxe.efi", BootFile)) return false;
                    break;
                case "ipxe_efi_snp32":
                    if (!CopyCommand("ipxe", "normal", "ipxe_efi_32", "", "snp.efi", BootFile)) return false;
                    break;
                case "ipxe_efi_snponly32":
                    if (!CopyCommand("ipxe", "normal", "ipxe_efi_32", "", "snponly.efi", BootFile)) return false;
                    break;
                case "ipxe_efi64":
                    if (!CopyCommand("ipxe", "normal", "ipxe_efi_64", "", "ipxe.efi", BootFile)) return false;
                    break;
                case "ipxe_efi_snp64":
                    if (!CopyCommand("ipxe", "normal", "ipxe_efi_64", "", "snp.efi", BootFile)) return false;
                    break;
                case "ipxe_efi_snponly64":
                    if (!CopyCommand("ipxe", "normal", "ipxe_efi_64", "", "snponly.efi", BootFile)) return false;
                    break;
                case "pxelinux":
                    foreach (var file in _syslinuxBiosFiles)
                    {
                        if (file == "pxelinux.0")
                        {
                            if (!CopyCommand("syslinux", "normal", "pxelinux", "", file, BootFile)) return false;
                        }
                        else
                        {
                            if (!CopyCommand("syslinux", "normal", "pxelinux", "", file, file)) return false;
                        }
                    }
                    break;
                case "syslinux_efi32":
                    foreach (var file in _syslinuxEfi32Files)
                    {
                        if (file == "syslinux.efi")
                        {
                            if (!CopyCommand("syslinux", "normal", "syslinux_efi_32", "", file, BootFile)) return false;
                        }
                        else
                        {
                            if (!CopyCommand("syslinux", "normal", "syslinux_efi_32", "", file, file)) return false;
                        }
                    }
                    break;
                case "syslinux_efi64":
                    foreach (var file in _syslinuxEfi64Files)
                    {
                        if (file == "syslinux.efi")
                        {
                            if (!CopyCommand("syslinux", "normal", "syslinux_efi_64", "", file, BootFile)) return false;
                        }
                        else
                        {
                            if (!CopyCommand("syslinux", "normal", "syslinux_efi_64", "", file, file)) return false;
                        }
                    }
                    break;
                case "grub":
                    foreach (var file in _grubEfi64Files)
                    {
                        if (file == "bootx64.efi")
                        {
                            if (!CopyCommand("grub", "", "", "", file, BootFile)) return false;
                        }
                        else
                        {
                            if (!CopyCommand("grub", "", "", "", file, file)) return false;
                        }
                    }
                    break;
                case "winpe_bios32":
                    try
                    {
                        File.Copy(Settings.TftpPath + "boot" + Path.DirectorySeparatorChar + "BCDx86",
                            Settings.TftpPath + "boot" + Path.DirectorySeparatorChar + "BCD",true);
                        new FileOps().SetUnixPermissions(Settings.TftpPath + "boot" + Path.DirectorySeparatorChar + "BCD");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message);
                        return false;
                    }
                    foreach (var file in _winPEBiosFiles)
                    {
                        if (file == "pxeboot.n12")
                        {
                            if (!CopyCommand("winpe", "", "winpe", "", file, BootFile)) return false;
                        }
                        else
                        {
                            if (!CopyCommand("winpe", "", "winpe", "", file, file)) return false;
                        }
                    }
                    break;
                case "winpe_bios64":
                    try
                    {
                        File.Copy(Settings.TftpPath + "boot" + Path.DirectorySeparatorChar + "BCDx64",
                            Settings.TftpPath + "boot" + Path.DirectorySeparatorChar + "BCD",true);
                        new FileOps().SetUnixPermissions(Settings.TftpPath + "boot" + Path.DirectorySeparatorChar + "BCD");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message);
                        return false;
                    }
                    foreach (var file in _winPEBiosFiles)
                    {
                        if (file == "pxeboot.n12")
                        {
                            if (!CopyCommand("winpe", "", "winpe", "", file, BootFile)) return false;
                        }
                        else
                        {
                            if (!CopyCommand("winpe", "", "winpe", "", file, file)) return false;
                        }
                    }
                    break;
                case "winpe_efi32":
                    try
                    {
                        File.Copy(Settings.TftpPath + "boot" + Path.DirectorySeparatorChar + "BCDx86",
                            Settings.TftpPath + "boot" + Path.DirectorySeparatorChar + "BCD",true);
                        new FileOps().SetUnixPermissions(Settings.TftpPath + "boot" + Path.DirectorySeparatorChar + "BCD");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message);
                        return false;
                    }
                    if (!CopyCommand("winpe", "", "winpe_efi_32", "", "bootmgfw.efi", BootFile)) return false;
                    break;
                case "winpe_efi64":
                    try
                    {
                        File.Copy(Settings.TftpPath + "boot" + Path.DirectorySeparatorChar + "BCDx64",
                            Settings.TftpPath + "boot" + Path.DirectorySeparatorChar + "BCD",true);
                        new FileOps().SetUnixPermissions(Settings.TftpPath + "boot" + Path.DirectorySeparatorChar + "BCD");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message);
                        return false;
                    }
                    if (!CopyCommand("winpe", "", "winpe_efi_64", "", "bootmgfw.efi", BootFile)) return false;
                    break;
            }
            return true;
        }

        private bool CopyCommand(string pxeType, string mode, string sArch, string dArch, string sName, string dName)
        {
            var destinationRootPath = Settings.TftpPath;
            if (mode == "proxy" || (pxeType == "grub" && dArch == "efi64") || (pxeType == "winpe" && dArch == "efi64") || (pxeType == "winpe" && dArch == "efi32") || pxeType == "winpe" && dArch == "bios")
                destinationRootPath += "proxy" + Path.DirectorySeparatorChar;
            try
            {
                File.Copy(
                  _sourceRootPath + pxeType + Path.DirectorySeparatorChar + mode +
                  Path.DirectorySeparatorChar + sArch + Path.DirectorySeparatorChar + sName,
                  destinationRootPath + dArch + Path.DirectorySeparatorChar + dName, true);

                new FileOps().SetUnixPermissions(destinationRootPath + dArch + Path.DirectorySeparatorChar + dName);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return false;           
            }
            return true;

        }
    }
}