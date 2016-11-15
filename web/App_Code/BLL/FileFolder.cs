using System.Collections.Generic;
using System.Linq;
using Helpers;

namespace BLL
{
    public class FileFolder
    {

        public static Models.ActionResult AddFileFolder(Models.FileFolder fileFolder)
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

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.FileFolderRepository.Count();
            }
        }

        public static bool DeleteFileFolder(int FileFolderId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.FileFolderRepository.Delete(FileFolderId);
                return uow.Save();
            }
        }

        public static Models.FileFolder GetFileFolder(int FileFolderId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.FileFolderRepository.GetById(FileFolderId);
            }
        }

        public static List<Models.FileFolder> SearchFileFolders(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    uow.FileFolderRepository.Get(
                        s => s.Name.Contains(searchString), orderBy: (q => q.OrderBy(t => t.Name)));
            }
        }

        public static Models.ActionResult UpdateFileFolder(Models.FileFolder fileFolder)
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

        public static Models.ActionResult ValidateTemplate(Models.FileFolder fileFolder, bool isNewTemplate)
        {
            var validationResult = new Models.ActionResult();

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