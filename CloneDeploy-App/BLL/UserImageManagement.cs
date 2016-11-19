using System.Collections.Generic;

namespace CloneDeploy_App.BLL
{
    public static class UserImageManagement
    {
        public static bool AddUserImageManagements(List<Models.UserImageManagement> listOfImages)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                foreach (var image in listOfImages)
                    uow.UserImageManagementRepository.Insert(image);

                return uow.Save();
            }
        }

        public static bool DeleteUserImageManagements(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.UserImageManagementRepository.DeleteRange(x => x.UserId == userId);
                return uow.Save();
            }
        }

        public static List<Models.UserImageManagement> Get(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserImageManagementRepository.Get(x => x.UserId == userId);
            }
        }

        public static bool DeleteImage(int imageId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.UserImageManagementRepository.DeleteRange(x => x.ImageId == imageId);
                return uow.Save();
            }
        }
    }
}