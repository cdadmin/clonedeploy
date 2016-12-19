using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;
using log4net;

#if __MonoCS__  
using Mono.Unix; // requires reference to  Mono.Posix.dll
#endif

namespace CloneDeploy_Services
{
    public class FilesystemServices
    {

        private static readonly ILog log = LogManager.GetLogger("ApplicationLog");
        public DpFreeSpaceDTO GetDpFreeSpace()
        {
            var primaryDp = new DistributionPointServices().GetPrimaryDistributionPoint();

            var dpFreeSpace = new DpFreeSpaceDTO();
            dpFreeSpace.dPPath = primaryDp.PhysicalPath;

            if (System.IO.Directory.Exists(primaryDp.PhysicalPath))
            {
                ulong freespace = 0;
                ulong total = 0;
                bool success = DriveFreeBytes(primaryDp.PhysicalPath, out freespace, out total);

                if (!success) return null;

                int freePercent = 0;
                int usedPercent = 0;

                if (total > 0 && freespace > 0)
                {
                    freePercent = (int)(0.5f + ((100f * Convert.ToInt64(freespace)) / Convert.ToInt64(total)));
                    usedPercent =
                        (int)(0.5f + ((100f * Convert.ToInt64(total - freespace)) / Convert.ToInt64(total)));
                }
                dpFreeSpace.freespace = freespace;
                dpFreeSpace.total = total;
                dpFreeSpace.freePercent = freePercent;
                dpFreeSpace.usedPercent = usedPercent;
            }

            return dpFreeSpace;

        }
        // if running on Mono
#if __MonoCS__     
        public static bool DriveFreeBytes(string folderName, out ulong freespace, out ulong total)
        {
            freespace = 0;
            total = 0;
            
            UnixDriveInfo[] drives = UnixDriveInfo.GetDrives();
            int idx = -1, count = -1;
            for (int i = 0; i < drives.Length; ++i)
            {
              if (folderName.StartsWith (drives[i].Name) && drives[i].Name.Length > count)
              {
                count = drives[i].Name.Length;
                idx = i;
              }
            }
            
            // Drive for path is: drives[idx].Name
            freespace = (ulong)drives[idx].AvailableFreeSpace;
            total = (ulong)drives[idx].TotalSize; 
            return true;
        }   
#else

        // using GetDiskFreeSpaceEx because this handles mountpoints, quota and UNC 
        // there are reports that old CIFS doesn't support unc-share to a mountpoint, needs Windows 2008/SMB2        
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
        out ulong lpFreeBytesAvailable,
        out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);

        public static bool DriveFreeBytes(string folderName, out ulong freespace, out ulong total)
        {
            freespace = 0;
            total = 0;
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            if (!folderName.EndsWith("\\"))
            {
                folderName += '\\';
            }

            ulong free = 0, tot = 0, dummy2 = 0;

            if (GetDiskFreeSpaceEx(folderName, out free, out tot, out dummy2))
            {
                freespace = free;
                total = tot;
                return true;
            }
            else
            {
                return false;
            }
        }
#endif
        public string GetDefaultBootMenuPath(string type)
        {
            string path = null;
            var tftpPath = Settings.TftpPath;
            var mode = Settings.PxeMode;
            var proxyDhcp = Settings.ProxyDhcp;

            if (proxyDhcp == "Yes")
            {
                var biosFile = Settings.ProxyBiosFile;
                var efi32File = Settings.ProxyEfi32File;
                var efi64File = Settings.ProxyEfi64File;
     
                if (type == "bios")
                {
                    if (biosFile.Contains("ipxe"))
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               type + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default.ipxe";
                    else
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               type + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default";
                }

                if (type == "efi32")
                {
                    if (efi32File.Contains("ipxe"))
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               type + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default.ipxe";
                    else
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               type + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default";
                }

                if (type == "efi64")
                {
                    if (efi64File.Contains("ipxe"))
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               type + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default.ipxe";
                    else if (mode.Contains("grub"))
                        path = tftpPath + "grub" + Path.DirectorySeparatorChar + "grub.cfg";
                    else
                        path = tftpPath + "proxy" + Path.DirectorySeparatorChar +
                               type + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + "default";
                }
            }
            else
            {
                if (mode.Contains("ipxe"))
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                           "default.ipxe";
                else if (mode.Contains("grub"))
                {
                    path = tftpPath + "grub" + Path.DirectorySeparatorChar + "grub.cfg";
                }
                else
                    path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar + "default";
            }
            return path;
        }

        public bool EditDefaultBootMenu(string type, string contents)
        {
            try
            {
                var path = GetDefaultBootMenuPath(type);
                using (var file = new StreamWriter(path))
                {
                    file.WriteLine(contents);
                }
                new FileOps().SetUnixPermissions(path);

                return true;
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
                return false;
            }
        }

        public bool WriteCoreScript(string type, string contents)
        {
            var path = GetServerPaths("clientScripts", type);
            try
            {
                using (var file = new StreamWriter(path))
                {
                    file.WriteLine(contents);
                }
                new FileOps().SetUnixPermissions(path);

                return true;
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
                return false;
            }
        }

        public string GetLogContents(string name)
        {
            var path = GetServerPaths("logs");
            return new FileOps().ReadAllText(path + name);
        }

        public string GetServerPaths(string type, string subType="")
        {
            var basePath = HttpContext.Current.Server.MapPath("~");
            var seperator = Path.DirectorySeparatorChar;
            switch (type)
            {
                case "iso":
                    return basePath + seperator + "private" + seperator + "client_iso" + seperator + "output" +
                           seperator;              
                case "clientScript":
                    return basePath + seperator + "private" + seperator + "clientscripts" + seperator + subType;
                case "csv":
                    return basePath + seperator + "private" + seperator + "imports" + seperator + subType;
                case "exports":
                    return basePath + seperator + "private" + seperator + "exports" + seperator;
                case "defaultTftp":
                    return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) +
                seperator + "clonedeploy" + seperator + "tftpboot" + seperator;
                case "defaultDp":
                    return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) +
                                                 seperator + "clonedeploy" +
                                                 seperator + "cd_dp";
                case "seperator":
                    return seperator.ToString();
                case "logs":
                    return basePath + seperator + "private" + seperator + "logs" + seperator;
                default:
                    return null;
            }


        }
    }

}
