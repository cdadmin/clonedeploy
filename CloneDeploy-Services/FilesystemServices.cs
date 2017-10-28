using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using CloneDeploy_Common;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ImageSchemaBE;
using CloneDeploy_Services.Helpers;
using log4net;


namespace CloneDeploy_Services
{
    public class FilesystemServices
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FilesystemServices));

        public DpFreeSpaceDTO GetDpFreeSpace()
        {
            var dp = new DistributionPointServices().GetPrimaryDistributionPoint();

            var dpFreeSpace = new DpFreeSpaceDTO();
            if (dp.Location == "Remote")
            {
                var basePath = @"\\" + dp.Server + @"\" + dp.ShareName;
                dpFreeSpace.dPPath = basePath;
                using (var unc = new UncServices())
                {
                    var smbPassword = new EncryptionServices().DecryptText(dp.RwPassword);
                    if (
                        unc.NetUseWithCredentials(basePath, dp.RwUsername, dp.Domain,
                            smbPassword) || unc.LastError == 1219)
                    {
                        ulong freespace = 0;
                        ulong total = 0;
                        var success = DriveFreeBytes(basePath, out freespace, out total);

                        if (!success) return null;

                        var freePercent = 0;
                        var usedPercent = 0;

                        if (total > 0 && freespace > 0)
                        {
                            freePercent = (int) (0.5f + 100f*Convert.ToInt64(freespace)/Convert.ToInt64(total));
                            usedPercent =
                                (int) (0.5f + 100f*Convert.ToInt64(total - freespace)/Convert.ToInt64(total));
                        }
                        dpFreeSpace.freespace = freespace;
                        dpFreeSpace.total = total;
                        dpFreeSpace.freePercent = freePercent;
                        dpFreeSpace.usedPercent = usedPercent;
                    }
                    else
                    {
                        log.Error("Failed to connect to " + basePath + "\r\nLastError = " + unc.LastError);
                    }
                }
            }
            else
            {
                dpFreeSpace.dPPath = dp.PhysicalPath;

                if (Directory.Exists(dp.PhysicalPath))
                {
                    //mono debugging
                    log.Info(dp.PhysicalPath + "Exists");
                    ulong freespace = 0;
                    ulong total = 0;

                     var isUnix = Environment.OSVersion.ToString().Contains("Unix");
                    bool success;
                    if (isUnix)
                    {
                        success = MonoDriveFreeBytes(dp.PhysicalPath, out freespace, out total);
                    }
                    else
                    {
                        success = DriveFreeBytes(dp.PhysicalPath, out freespace, out total);
                    }
                    

                    if (!success) return null;

                    var freePercent = 0;
                    var usedPercent = 0;

                    if (total > 0 && freespace > 0)
                    {
                        freePercent = (int) (0.5f + 100f*Convert.ToInt64(freespace)/Convert.ToInt64(total));
                        usedPercent =
                            (int) (0.5f + 100f*Convert.ToInt64(total - freespace)/Convert.ToInt64(total));
                    }
                    dpFreeSpace.freespace = freespace;
                    dpFreeSpace.total = total;
                    dpFreeSpace.freePercent = freePercent;
                    dpFreeSpace.usedPercent = usedPercent;
                }
            }

            return dpFreeSpace;
        }

        // if running on Mono
    
        public static bool MonoDriveFreeBytes(string folderName, out ulong freespace, out ulong total)
        {
            freespace = 0;
            total = 0;
            
            var drives = Mono.Unix.UnixDriveInfo.GetDrives();
            log.Info(drives.FirstOrDefault() + ": First Drive Found");
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
            return false;
        }


        public string GetDefaultBootMenuPath(string type)
        {
            string path = null;
            var tftpPath = SettingServices.GetSettingValue(SettingStrings.TftpPath);
            var mode = SettingServices.GetSettingValue(SettingStrings.PxeMode);
            var proxyDhcp = SettingServices.GetSettingValue(SettingStrings.ProxyDhcp);

            if (proxyDhcp == "Yes")
            {
                var biosFile = SettingServices.GetSettingValue(SettingStrings.ProxyBiosFile);
                var efi32File = SettingServices.GetSettingValue(SettingStrings.ProxyEfi32File);
                var efi64File = SettingServices.GetSettingValue(SettingStrings.ProxyEfi64File);

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
                new FileOpsServices().SetUnixPermissions(path);

                return true;
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }

        public bool WriteCoreScript(string type, string contents)
        {
            var path = GetServerPaths("clientScript", type);
            try
            {
                using (var file = new StreamWriter(path))
                {
                    file.WriteLine(contents);
                }
                new FileOpsServices().SetUnixPermissions(path);

                return true;
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }

        public List<string> GetLogContents(string name, int limit)
        {
            var path = GetServerPaths("logs") + name;
            return File.ReadLines(path).Reverse().Take(limit).Reverse().ToList();
        }

        public string GetServerPaths(string type, string subType = "")
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

        public bool DeleteTftpFile(string path)
        {
            if (path.StartsWith(SettingServices.GetSettingValue(SettingStrings.TftpPath)))
            {
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return false;
                }
            }
            log.Error("Could Not Delete Tftp File " + path + " It Is Not A Sub Directory Of The Base Tftp Path");
            return false;
        }

        public static List<string> GetBootImages()
        {
            var bootImagePath = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "images" +
                                Path.DirectorySeparatorChar;

            var bootImageFiles = new List<string>();
            try
            {
                var files = Directory.GetFiles(bootImagePath, "*.*");

                for (var x = 0; x < files.Length; x++)
                    bootImageFiles.Add(Path.GetFileName(files[x]));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return bootImageFiles;
        }

        public static List<string> GetKernels()
        {
            var kernelPath = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "kernels" +
                             Path.DirectorySeparatorChar;
            var result = new List<string>();
            try
            {
                var kernelFiles = Directory.GetFiles(kernelPath, "*.*");

                for (var x = 0; x < kernelFiles.Length; x++)
                    result.Add(Path.GetFileName(kernelFiles[x]));
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return result;
        }

        public static List<string> GetScripts(string type)
        {
            var scriptPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                             Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar;
            var result = new List<string>();
            try
            {
                var scriptFiles = Directory.GetFiles(scriptPath, "*.*");
                for (var x = 0; x < scriptFiles.Length; x++)
                    result.Add(Path.GetFileName(scriptFiles[x]));
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return result;
        }

        public static List<string> GetLogs()
        {
            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;

            var logFiles = Directory.GetFiles(logPath, "*.*");
            var result = new List<string>();
            for (var x = 0; x < logFiles.Length; x++)
                result.Add(Path.GetFileName(logFiles[x]));

            return result;
        }

        public bool CreateNewImageFolders(string imageName)
        {
            var primaryDp = new DistributionPointServices().GetPrimaryDistributionPoint();
            if (primaryDp.Location == "Local")
            {
                try
                {
                    Directory.CreateDirectory(primaryDp.PhysicalPath + "images" + Path.DirectorySeparatorChar +
                                              imageName);
                    new FileOpsServices().SetUnixPermissionsImage(primaryDp.PhysicalPath + "images" +
                                                                  Path.DirectorySeparatorChar + imageName);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return false;
                }
            }
            else if (primaryDp.Location == "Remote")
            {
                using (var unc = new UncServices())
                {
                    var basePath = @"\\" + primaryDp.Server + @"\" + primaryDp.ShareName;
                    var smbPassword = new EncryptionServices().DecryptText(primaryDp.RwPassword);
                    if (
                        unc.NetUseWithCredentials(basePath, primaryDp.RwUsername, primaryDp.Domain,
                            smbPassword) || unc.LastError == 1219)
                    {
                        try
                        {
                            Directory.CreateDirectory(basePath + @"\images" + @"\" +
                                                      imageName);
                            new FileOpsServices().SetUnixPermissionsImage(basePath + @"\images" +
                                                                          @"\" + imageName);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            return false;
                        }
                    }
                    else
                    {
                        log.Error("Failed to connect to " + basePath + "\r\nLastError = " + unc.LastError);
                        return false;
                    }
                }
            }
            else
            {
                log.Error("Could Not Determine Primary Distribution Point Location Type");
                return false;
            }

            return true;
        }

        public bool DeleteImageFolders(string imageName)
        {
            //Check again
            if (string.IsNullOrEmpty(imageName)) return false;

            var primaryDp = new DistributionPointServices().GetPrimaryDistributionPoint();
            if (primaryDp.Location == "Local")
            {
                try
                {
                    Directory.Delete(primaryDp.PhysicalPath + "images" + Path.DirectorySeparatorChar + imageName,
                        true);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return false;
                }
            }
            else if (primaryDp.Location == "Remote")
            {
                using (var unc = new UncServices())
                {
                    var basePath = @"\\" + primaryDp.Server + @"\" + primaryDp.ShareName;
                    var smbPassword = new EncryptionServices().DecryptText(primaryDp.RwPassword);
                    if (
                        unc.NetUseWithCredentials(basePath, primaryDp.RwUsername, primaryDp.Domain,
                            smbPassword) || unc.LastError == 1219)
                    {
                        try
                        {
                            Directory.Delete(basePath + @"\images" + @"\" +
                                             imageName, true);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            return false;
                        }
                    }
                    else
                    {
                        log.Error("Failed to connect to " + basePath + "\r\nLastError = " + unc.LastError);
                        return false;
                    }
                }
            }
            else
            {
                log.Error("Could Not Determine Primary Distribution Point Location Type");
                return false;
            }

            return true;
        }

        public List<ImageFileInfo> GetPartitionFileSize(string imageName, string hd, string partition)
        {
            var primaryDp = new DistributionPointServices().GetPrimaryDistributionPoint();
            var imageFileInfo = new ImageFileInfo();
            if (primaryDp.Location == "Local")
            {
                try
                {
                    var imageFile =
                        Directory.GetFiles(
                            primaryDp.PhysicalPath + "images" + Path.DirectorySeparatorChar + imageName +
                            Path.DirectorySeparatorChar + "hd" + hd +
                            Path.DirectorySeparatorChar,
                            "part" + partition + ".*").FirstOrDefault();

                    var fi = new FileInfo(imageFile);
                    imageFileInfo = new ImageFileInfo
                    {
                        FileName = fi.Name,
                        FileSize = (fi.Length/1024f/1024f).ToString("#.##") + " MB"
                    };
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return null;
                }
            }
            else if (primaryDp.Location == "Remote")
            {
                using (var unc = new UncServices())
                {
                    var basePath = @"\\" + primaryDp.Server + @"\" + primaryDp.ShareName;
                    var smbPassword = new EncryptionServices().DecryptText(primaryDp.RwPassword);
                    if (
                        unc.NetUseWithCredentials(basePath, primaryDp.RwUsername, primaryDp.Domain,
                            smbPassword) || unc.LastError == 1219)
                    {
                        try
                        {
                            var imageFile =
                                Directory.GetFiles(basePath + @"\images\" + imageName +
                                                   @"\" + "hd" + hd + @"\", "part" + partition + ".*").FirstOrDefault();

                            var fi = new FileInfo(imageFile);
                            imageFileInfo = new ImageFileInfo
                            {
                                FileName = fi.Name,
                                FileSize = (fi.Length/1024f/1024f).ToString("#.##") + " MB"
                            };
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            return null;
                        }
                    }
                    else
                    {
                        log.Error("Failed to connect to " + basePath + "\r\nLastError = " + unc.LastError);
                        return null;
                    }
                }
            }
            else
            {
                log.Error("Could Not Determine Primary Distribution Point Location Type");
                return null;
            }

            return new List<ImageFileInfo> {imageFileInfo};
        }

        public string GetHdFileSize(string imageName, string hd)
        {
            var primaryDp = new DistributionPointServices().GetPrimaryDistributionPoint();
            if (primaryDp.Location == "Local")
            {
                try
                {
                    var imagePath = primaryDp.PhysicalPath + "images" + Path.DirectorySeparatorChar + imageName +
                                    Path.DirectorySeparatorChar + "hd" + hd;
                    var size = new FileOpsServices().GetDirectorySize(new DirectoryInfo(imagePath))/1024f/1024f/1024f;
                    return Math.Abs(size) < 0.1f ? "< 100M" : size.ToString("#.##") + " GB";
                }
                catch
                {
                    return "N/A";
                }
            }
            if (primaryDp.Location == "Remote")
            {
                using (var unc = new UncServices())
                {
                    var basePath = @"\\" + primaryDp.Server + @"\" + primaryDp.ShareName;
                    var smbPassword = new EncryptionServices().DecryptText(primaryDp.RwPassword);
                    if (
                        unc.NetUseWithCredentials(basePath, primaryDp.RwUsername, primaryDp.Domain,
                            smbPassword) || unc.LastError == 1219)
                    {
                        try
                        {
                            var imagePath = basePath + @"\images\" + imageName + @"\hd" + hd;
                            var size = new FileOpsServices().GetDirectorySize(new DirectoryInfo(imagePath))/1024f/1024f/
                                       1024f;
                            return Math.Abs(size) < 0.1f ? "< 100M" : size.ToString("#.##") + " GB";
                        }
                        catch
                        {
                            return "N/A";
                        }
                    }
                    log.Error("Failed to connect to " + basePath + "\r\nLastError = " + unc.LastError);
                    return "N/A";
                }
            }
            log.Error("Could Not Determine Primary Distribution Point Location Type");
            return "N/A";
        }

        public string ReadSchemaFile(string imageName)
        {
            var primaryDp = new DistributionPointServices().GetPrimaryDistributionPoint();
            var schemaText = "";
            if (primaryDp.Location == "Local")
            {
                try
                {
                    var path = primaryDp.PhysicalPath + "images" + Path.DirectorySeparatorChar +
                               imageName + Path.DirectorySeparatorChar + "schema";

                    using (var reader = new StreamReader(path))
                    {
                        schemaText = reader.ReadLine() ?? "";
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Could Not Read Schema File On The Primary Distribution Point");
                    log.Error(ex.Message);
                }
            }
            else if (primaryDp.Location == "Remote")
            {
                using (var unc = new UncServices())
                {
                    var basePath = @"\\" + primaryDp.Server + @"\" + primaryDp.ShareName;
                    var smbPassword = new EncryptionServices().DecryptText(primaryDp.RwPassword);
                    if (
                        unc.NetUseWithCredentials(basePath, primaryDp.RwUsername, primaryDp.Domain,
                            smbPassword) || unc.LastError == 1219)
                    {
                        try
                        {
                            var path = basePath + @"\images\" + imageName + @"\schema";

                            using (var reader = new StreamReader(path))
                            {
                                schemaText = reader.ReadLine() ?? "";
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Could Not Read Schema File On The Primary Distribution Point");
                            log.Error(ex.Message);
                        }
                    }
                    else
                    {
                        log.Error("Could Not Read Schema File On The Primary Distribution Point");
                        log.Error("Failed to connect to " + basePath + "\r\nLastError = " + unc.LastError);
                    }
                }
            }
            else
            {
                log.Error("Could Not Determine Primary Distribution Point Location Type");
            }

            return schemaText;
        }

        public bool RenameImageFolder(string oldName, string newName)
        {
            if (string.IsNullOrEmpty(oldName)) return false;
            var primaryDp = new DistributionPointServices().GetPrimaryDistributionPoint();
            if (primaryDp.Location == "Local")
            {
                try
                {
                    var imagePath = primaryDp.PhysicalPath + "images" + Path.DirectorySeparatorChar;
                    Directory.Move(imagePath + oldName, imagePath + newName);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return false;
                }
            }
            else if (primaryDp.Location == "Remote")
            {
                using (var unc = new UncServices())
                {
                    var basePath = @"\\" + primaryDp.Server + @"\" + primaryDp.ShareName;
                    var smbPassword = new EncryptionServices().DecryptText(primaryDp.RwPassword);
                    if (
                        unc.NetUseWithCredentials(basePath, primaryDp.RwUsername, primaryDp.Domain,
                            smbPassword) || unc.LastError == 1219)
                    {
                        try
                        {
                            var imagePath = basePath + @"\images\";
                            Directory.Move(imagePath + oldName, imagePath + newName);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            return false;
                        }
                    }
                    else
                    {
                        log.Error("Failed to connect to " + basePath + "\r\nLastError = " + unc.LastError);
                        return false;
                    }
                }
            }
            else
            {
                log.Error("Could Not Determine Primary Distribution Point Location Type");
                return false;
            }

            return true;
        }

        public string GetFileNameWithFullPath(string imageName, string schemaHdNumber, string partitionNumber,
            string extension)
        {
            var primaryDp = new DistributionPointServices().GetPrimaryDistributionPoint();

            var filePath = "";
            if (primaryDp.Location == "Local")
            {
                var imagePath = primaryDp.PhysicalPath + "images" +
                                Path.DirectorySeparatorChar + imageName + Path.DirectorySeparatorChar + "hd" +
                                schemaHdNumber;
                try
                {
                    filePath =
                        Directory.GetFiles(
                            imagePath + Path.DirectorySeparatorChar, "part" + partitionNumber + "." + extension + ".*")
                            .FirstOrDefault();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
            else if (primaryDp.Location == "Remote")
            {
                using (var unc = new UncServices())
                {
                    var basePath = @"\\" + primaryDp.Server + @"\" + primaryDp.ShareName;
                    var smbPassword = new EncryptionServices().DecryptText(primaryDp.RwPassword);
                    if (
                        unc.NetUseWithCredentials(basePath, primaryDp.RwUsername, primaryDp.Domain,
                            smbPassword) || unc.LastError == 1219)
                    {
                        var imagePath = basePath + @"\images\" + imageName + @"\hd" +
                                        schemaHdNumber;
                        try
                        {
                            filePath =
                                Directory.GetFiles(
                                    imagePath + @"\", "part" + partitionNumber + "." + extension + ".*")
                                    .FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }
                    }
                    else
                    {
                        log.Error("Failed to connect to " + basePath + "\r\nLastError = " + unc.LastError);
                    }
                }
            }
            else
            {
                log.Error("Could Not Determine Primary Distribution Point Location Type");
            }

            return filePath;
        }

        public string GetLVMFileNameWithFullPath(string imageName, string schemaHdNumber, string vgName, string lvName,
            string extension)
        {
            var primaryDp = new DistributionPointServices().GetPrimaryDistributionPoint();

            var filePath = "";
            if (primaryDp.Location == "Local")
            {
                var imagePath = primaryDp.PhysicalPath + "images" +
                                Path.DirectorySeparatorChar + imageName + Path.DirectorySeparatorChar + "hd" +
                                schemaHdNumber;
                try
                {
                    filePath =
                        Directory.GetFiles(
                            imagePath + Path.DirectorySeparatorChar,
                            vgName + "-" + lvName + "." + extension + ".*")
                            .FirstOrDefault();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
            else if (primaryDp.Location == "Remote")
            {
                using (var unc = new UncServices())
                {
                    var basePath = @"\\" + primaryDp.Server + @"\" + primaryDp.ShareName;
                    var smbPassword = new EncryptionServices().DecryptText(primaryDp.RwPassword);
                    if (
                        unc.NetUseWithCredentials(basePath, primaryDp.RwUsername, primaryDp.Domain,
                            smbPassword) || unc.LastError == 1219)
                    {
                        var imagePath = basePath + @"\images\" + imageName + @"\hd" +
                                        schemaHdNumber;
                        try
                        {
                            filePath =
                                Directory.GetFiles(
                                    imagePath + @"\",
                                    vgName + "-" + lvName + "." + extension + ".*")
                                    .FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }
                    }
                    else
                    {
                        log.Error("Failed to connect to " + basePath + "\r\nLastError = " + unc.LastError);
                    }
                }
            }
            else
            {
                log.Error("Could Not Determine Primary Distribution Point Location Type");
            }

            return filePath;
        }
    }
}