using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;

namespace CloneDeploy_Services
{
    public class ImageProfileFileFolderService
    {
        private readonly UnitOfWork _uow;

        public ImageProfileFileFolderService()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddImageProfileFileFolder(ImageProfileFileFolderEntity imageProfileFileFolder)
        {
            imageProfileFileFolder.DestinationFolder = Utility.WindowsToUnixFilePath(imageProfileFileFolder.DestinationFolder);
            if (imageProfileFileFolder.DestinationFolder.Trim().EndsWith("/") && imageProfileFileFolder.DestinationFolder.Length > 1)
            {
                char[] toRemove = { '/' };
                string trimmed = imageProfileFileFolder.DestinationFolder.TrimEnd(toRemove);
                imageProfileFileFolder.DestinationFolder = trimmed;
            }
           
                _uow.ImageProfileFileFolderRepository.Insert(imageProfileFileFolder);
                _uow.Save();
                var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = imageProfileFileFolder.Id;
                return actionResult;
            

        }

       

        
    }
}