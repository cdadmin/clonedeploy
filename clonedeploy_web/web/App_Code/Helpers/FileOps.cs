using System;
using System.IO;
using Mono.Unix.Native;

namespace Helpers
{
    public class FileOps
    {
        public string ReadAllText(string path)
        {
            string fileText = null;

            try
            {
                fileText = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                fileText = "Could Not Read File";
            }

            return fileText;
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
                Logger.Log(ex.Message);
                throw new Exception("Could Not Remove Custom Boot Menu.  Check The Exception Log For More Info.");
             
                return false;
            }
        }

        public long GetDirectorySize(DirectoryInfo d)
        {
            long size = 0;
            var fis = d.GetFiles();
            foreach (var fi in fis)
            {
                size += fi.Length;
            }

            return (size);
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

        public void RenameFolder(string oldName, string newName)
        {
            try
            {
                var imagePath = Settings.ImageStorePath;
                if (Directory.Exists(imagePath + oldName))
                    Directory.Move(imagePath + oldName, imagePath + newName);
            }
            catch (Exception ex)
            {
               Logger.Log(ex.Message);
               throw;
            }
        }

        public void SetUnixPermissions(string path)
        {
            if (Environment.OSVersion.ToString().Contains("Unix"))
                Syscall.chmod(path,
                    (FilePermissions.S_IWUSR | FilePermissions.S_IRGRP | FilePermissions.S_IROTH |
                     FilePermissions.S_IRUSR));
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
                
                //Message.Text = "Could Not Set Custom Boot Menu.  Check The Exception Log For More Info.";
                Logger.Log(ex.Message);
                return false;
            }
        }
    }
}