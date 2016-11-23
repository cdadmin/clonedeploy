using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Web.Models;
using Helpers;

namespace BLL
{
    public class FileFolder
    {
        //moved
        public static ActionResult AddFileFolder(CloneDeploy_Web.Models.FileFolder fileFolder)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateTemplate(fileFolder, true);
                if (validationResult.Success)
                {
                    uow.FileFolderRepository.Insert(fileFolder);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        //moved
        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.FileFolderRepository.Count();
            }
        }

        //moved
        public static bool DeleteFileFolder(int FileFolderId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.FileFolderRepository.Delete(FileFolderId);
                return uow.Save();
            }
        }

        //moved
        public static CloneDeploy_Web.Models.FileFolder GetFileFolder(int FileFolderId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.FileFolderRepository.GetById(FileFolderId);
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.FileFolder> SearchFileFolders(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    uow.FileFolderRepository.Get(
                        s => s.Name.Contains(searchString), orderBy: (q => q.OrderBy(t => t.Name)));
            }
        }

        //moved
        public static ActionResult UpdateFileFolder(CloneDeploy_Web.Models.FileFolder fileFolder)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateTemplate(fileFolder, false);
                if (validationResult.Success)
                {
                    uow.FileFolderRepository.Update(fileFolder, fileFolder.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        //move not needed
        public static ActionResult ValidateTemplate(CloneDeploy_Web.Models.FileFolder fileFolder, bool isNewTemplate)
        {
            var validationResult = new ActionResult();

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
                using (var uow = new DAL.UnitOfWork())
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
                using (var uow = new DAL.UnitOfWork())
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