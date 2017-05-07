using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;

namespace CloneDeploy_Services
{
    public class FileFolderServices
    {
        private readonly UnitOfWork _uow;

        public FileFolderServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddFileFolder(FileFolderEntity fileFolder)
        {
            var actionResult = new ActionResultDTO();
            var validationResult = ValidateFileFolder(fileFolder, true);
            if (validationResult.Success)
            {
                _uow.FileFolderRepository.Insert(fileFolder);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = fileFolder.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        public ActionResultDTO DeleteFileFolder(int FileFolderId)
        {
            var fileFolder = GetFileFolder(FileFolderId);
            if (fileFolder == null)
                return new ActionResultDTO {ErrorMessage = "File Folder Not Found", Id = 0};
            var actionResult = new ActionResultDTO();


            _uow.FileFolderRepository.Delete(FileFolderId);
            _uow.Save();
            actionResult.Success = true;
            actionResult.Id = fileFolder.Id;


            return actionResult;
        }

        public FileFolderEntity GetFileFolder(int FileFolderId)
        {
            return _uow.FileFolderRepository.GetById(FileFolderId);
        }

        public List<FileFolderEntity> SearchFileFolders(string searchString = "")
        {
            return
                _uow.FileFolderRepository.Get(
                    s => s.Name.Contains(searchString), q => q.OrderBy(t => t.Name));
        }

        public string TotalCount()
        {
            return _uow.FileFolderRepository.Count();
        }

        public ActionResultDTO UpdateFileFolder(FileFolderEntity fileFolder)
        {
            var ff = GetFileFolder(fileFolder.Id);
            if (ff == null)
                return new ActionResultDTO {ErrorMessage = "File Folder Not Found", Id = 0};
            var actionResult = new ActionResultDTO();

            var validationResult = ValidateFileFolder(fileFolder, false);
            if (validationResult.Success)
            {
                _uow.FileFolderRepository.Update(fileFolder, fileFolder.Id);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = fileFolder.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        private ValidationResultDTO ValidateFileFolder(FileFolderEntity fileFolder, bool isNewTemplate)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(fileFolder.Name))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Name Is Not Valid";
                return validationResult;
            }

            if (fileFolder.Path.Trim().EndsWith("/") || fileFolder.Path.Trim().EndsWith(@"\"))
            {
                char[] toRemove = {'/', '\\'};
                var trimmed = fileFolder.Path.TrimEnd(toRemove);
                fileFolder.Path = trimmed;
            }

            if (fileFolder.Path.Trim().StartsWith("/") || fileFolder.Path.Trim().StartsWith(@"\"))
            {
                char[] toRemove = {'/', '\\'};
                var trimmed = fileFolder.Path.TrimStart(toRemove);
                fileFolder.Path = trimmed;
            }

            fileFolder.Path = Utility.WindowsToUnixFilePath(fileFolder.Path);

            if (isNewTemplate)
            {
                if (_uow.FileFolderRepository.Exists(h => h.Name == fileFolder.Name))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "File / Folder Name Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalTemplate = _uow.FileFolderRepository.GetById(fileFolder.Id);
                if (originalTemplate.Name != fileFolder.Name)
                {
                    if (_uow.FileFolderRepository.Exists(h => h.Name == fileFolder.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "File / Folder Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}