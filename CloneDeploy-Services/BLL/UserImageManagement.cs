using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public static class UserImageManagement
    {
        public static bool AddUserImageManagements(List<UserImageManagementEntity> listOfImages)
        {
            using (var uow = new UnitOfWork())
            {
                foreach (var image in listOfImages)
                    uow.UserImageManagementRepository.Insert(image);

                return uow.Save();
            }
        }

        public static bool DeleteUserImageManagements(int userId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.UserImageManagementRepository.DeleteRange(x => x.UserId == userId);
                return uow.Save();
            }
        }

        public static List<UserImageManagementEntity> Get(int userId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.UserImageManagementRepository.Get(x => x.UserId == userId);
            }
        }

        public static bool DeleteImage(int imageId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.UserImageManagementRepository.DeleteRange(x => x.ImageId == imageId);
                return uow.Save();
            }
        }
    }
}