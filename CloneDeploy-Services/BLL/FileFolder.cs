using System.Collections.Generic;
using System.Linq;
using CloneDeploy_App.Helpers;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using Newtonsoft.Json;

namespace CloneDeploy_App.BLL
{
    public class FileFolder
    {

        public static ActionResultEntity AddFileFolder(FileFolderEntity fileFolder)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateTemplate(fileFolder, true);
                if (validationResult.Success)
                {
                    uow.FileFolderRepository.Insert(fileFolder);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = fileFolder.Id;
                    validationResult.Object = JsonConvert.SerializeObject(fileFolder);
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.FileFolderRepository.Count();
            }
        }

        public static ActionResultEntity DeleteFileFolder(int FileFolderId)
        {
            var actionResult = new ActionResultEntity();
            var fileFolder = GetFileFolder(FileFolderId);
            using (var uow = new UnitOfWork())
            {
                uow.FileFolderRepository.Delete(FileFolderId);
                actionResult.Success = uow.Save();
                actionResult.ObjectId = fileFolder.Id;
                actionResult.Object = JsonConvert.SerializeObject(fileFolder);
            }

            return actionResult;
            
        }

        public static FileFolderEntity GetFileFolder(int FileFolderId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.FileFolderRepository.GetById(FileFolderId);
            }
        }

        public static List<FileFolderEntity> SearchFileFolders(string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                return
                    uow.FileFolderRepository.Get(
                        s => s.Name.Contains(searchString), orderBy: (q => q.OrderBy(t => t.Name)));
            }
        }

        public static ActionResultEntity UpdateFileFolder(FileFolderEntity fileFolder)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateTemplate(fileFolder, false);
                if (validationResult.Success)
                {
                    uow.FileFolderRepository.Update(fileFolder, fileFolder.Id);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = fileFolder.Id;
                    validationResult.Object = JsonConvert.SerializeObject(fileFolder);
                }

                return validationResult;
            }
        }

        public static ActionResultEntity ValidateTemplate(FileFolderEntity fileFolder, bool isNewTemplate)
        {
            var validationResult = new ActionResultEntity();

            if (string.IsNullOrEmpty(fileFolder.Name))
            {
                validationResult.Success = false;
                validationResult.Message = "Name Is Not Valid";
                return validationResult;
            }

            if (fileFolder.Path.Trim().EndsWith("/") || fileFolder.Path.Trim().EndsWith(@"\"))
            {
                char[] toRemove = { '/', '\\' };
                string trimmed = fileFolder.Path.TrimEnd(toRemove);
                fileFolder.Path = trimmed;
            }

            if (fileFolder.Path.Trim().StartsWith("/") || fileFolder.Path.Trim().StartsWith(@"\"))
            {
                char[] toRemove = { '/', '\\' };
                string trimmed = fileFolder.Path.TrimStart(toRemove);
                fileFolder.Path = trimmed;
            }

            fileFolder.Path = Utility.WindowsToUnixFilePath(fileFolder.Path);

            if (isNewTemplate)
            {
                using (var uow = new UnitOfWork())
                {
                    if (uow.FileFolderRepository.Exists(h => h.Name == fileFolder.Name))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "File / Folder Name Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new UnitOfWork())
                {
                    var originalTemplate = uow.FileFolderRepository.GetById(fileFolder.Id);
                    if (originalTemplate.Name != fileFolder.Name)
                    {
                        if (uow.FileFolderRepository.Exists(h => h.Name == fileFolder.Name))
                        {
                            validationResult.Success = false;
                            validationResult.Message = "File / Folder Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }

    }
}