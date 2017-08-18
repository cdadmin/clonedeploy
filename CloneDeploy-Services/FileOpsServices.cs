using System;
using System.IO;
using log4net;
using Mono.Unix.Native;

namespace CloneDeploy_Services.Helpers
{
    public class FileOpsServices
    {
        private readonly ILog log = LogManager.GetLogger("ApplicationLog");

        public void Copy(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (var fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (var diSourceSubDir in source.GetDirectories())
            {
                var nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        public void DeleteAllFiles(string directoryPath)
        {
            var directories = Directory.GetDirectories(directoryPath);
            foreach (var dirPath in directories)
            {
                if (Directory.Exists(dirPath))
                    Directory.Delete(dirPath, true);
            }

            var files = Directory.GetFiles(directoryPath);
            foreach (var filePath in files)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        public bool DeletePath(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);

                return false;
            }
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public long GetDirectorySize(DirectoryInfo d)
        {
            long size = 0;
            var fis = d.GetFiles();
            foreach (var fi in fis)
            {
                size += fi.Length;
            }

            return size;
        }

        public void MoveFile(string sourceFileName, string destFileName)
        {
            if (File.Exists(destFileName))
            {
                File.Delete(destFileName);
            }

            File.Move(sourceFileName, destFileName);
        }

        public void MoveFolder(string sourceFolderName, string destFolderName)
        {
            if (Directory.Exists(destFolderName))
            {
                Directory.Delete(destFolderName, true);
            }

            Directory.Move(sourceFolderName, destFolderName);
        }

        public string ReadAllText(string path)
        {
            string fileText = null;

            try
            {
                fileText = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
                fileText = "Could Not Read File";
            }

            return fileText;
        }

        public void SetUnixPermissions(string path)
        {
            if (Environment.OSVersion.ToString().Contains("Unix"))
                Syscall.chmod(path,
                    FilePermissions.S_IWUSR | FilePermissions.S_IRGRP | FilePermissions.S_IROTH |
                    FilePermissions.S_IRUSR);
        }

        public void SetUnixPermissionsImage(string path)
        {
            if (Environment.OSVersion.ToString().Contains("Unix"))
                Syscall.chmod(path,
                    FilePermissions.S_IRWXU | FilePermissions.S_IRWXG | FilePermissions.S_IRWXO);
        }

        public bool WritePath(string path, string contents)
        {
            try
            {
                using (var file = new StreamWriter(path))
                {
                    file.WriteLine(contents);
                }
                SetUnixPermissions(path);

                return true;
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
                return false;
            }
        }
    }
}