using System.Collections.Generic;
using System.Linq;
using CloneDeploy_App.Helpers;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public class ImageProfileFileFolder
    {

        public static bool AddImageProfileFileFolder(ImageProfileFileFolderEntity imageProfileFileFolder)
        {
            imageProfileFileFolder.DestinationFolder = Utility.WindowsToUnixFilePath(imageProfileFileFolder.DestinationFolder);
            if (imageProfileFileFolder.DestinationFolder.Trim().EndsWith("/") && imageProfileFileFolder.DestinationFolder.Length > 1)
            {
                char[] toRemove = { '/' };
                string trimmed = imageProfileFileFolder.DestinationFolder.TrimEnd(toRemove);
                imageProfileFileFolder.DestinationFolder = trimmed;
            }
            using (var uow = new UnitOfWork())
            {
                uow.ImageProfileFileFolderRepository.Insert(imageProfileFileFolder);
                return uow.Save();
            }
        }

        public static bool DeleteImageProfileFileFolders(int profileId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.ImageProfileFileFolderRepository.DeleteRange(x => x.ProfileId == profileId);
                return uow.Save();
            }
        }

        public static List<ImageProfileFileFolderEntity> SearchImageProfileFileFolders(int profileId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ImageProfileFileFolderRepository.Get(x => x.ProfileId == profileId, orderBy: q => q.OrderBy(t => t.Priority));
            }
        }
    }
}