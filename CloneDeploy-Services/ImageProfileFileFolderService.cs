using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

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
            imageProfileFileFolder.DestinationFolder =
                StringManipulationServices.WindowsToUnixFilePath(imageProfileFileFolder.DestinationFolder);
            if (imageProfileFileFolder.DestinationFolder.Trim().EndsWith("/") &&
                imageProfileFileFolder.DestinationFolder.Length > 1)
            {
                char[] toRemove = {'/'};
                var trimmed = imageProfileFileFolder.DestinationFolder.TrimEnd(toRemove);
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