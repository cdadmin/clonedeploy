using System.Collections.Generic;

namespace BLL
{
    public static class UserImageManagement
    {
        //moved
        public static bool AddUserImageManagements(List<CloneDeploy_Web.Models.UserImageManagement> listOfImages)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                foreach (var image in listOfImages)
                    uow.UserImageManagementRepository.Insert(image);

                return uow.Save();
            }
        }

        //moved
        public static bool DeleteUserImageManagements(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.UserImageManagementRepository.DeleteRange(x => x.UserId == userId);
                return uow.Save();
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.UserImageManagement> Get(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserImageManagementRepository.Get(x => x.UserId == userId);
            }
        }

        //move not needed
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