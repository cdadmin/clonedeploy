using System.Collections.Generic;
using System.Linq;
using Helpers;

namespace BLL
{
    public class FileFolder
    {

        public static Models.ValidationResult AddFileFolder(Models.FileFolder fileFolder)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateTemplate(fileFolder, true);
                if (validationResult.IsValid)
                {
                    uow.FileFolderRepository.Insert(fileFolder);
                    validationResult.IsValid = uow.Save();
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

        public static Models.ValidationResult UpdateFileFolder(Models.FileFolder fileFolder)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateTemplate(fileFolder, false);
                if (validationResult.IsValid)
                {
                    uow.FileFolderRepository.Update(fileFolder, fileFolder.Id);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }

        public static Models.ValidationResult ValidateTemplate(Models.FileFolder fileFolder, bool isNewTemplate)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(fileFolder.Name))
            {
                validationResult.IsValid = false;
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
                        validationResult.IsValid = false;
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
                            validationResult.IsValid = false;
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