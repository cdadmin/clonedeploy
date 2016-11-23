using System.Collections.Generic;

namespace BLL
{
    public static class UserGroupImageManagement
    {
        public static bool AddUserGroupImageManagements(List<CloneDeploy_Web.Models.UserGroupImageManagement> listOfImages)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                foreach (var image in listOfImages)
                    uow.UserGroupImageManagementRepository.Insert(image);

                return uow.Save();
            }
        }

        public static bool DeleteUserGroupImageManagements(int userGroupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.UserGroupImageManagementRepository.DeleteRange(x => x.UserGroupId == userGroupId);
                return uow.Save();
            }
        }

        public static List<CloneDeploy_Web.Models.UserGroupImageManagement> Get(int userGroupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserGroupImageManagementRepository.Get(x => x.UserGroupId == userGroupId);
            }
        }

        public static bool DeleteImage(int imageId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.UserGroupImageManagementRepository.DeleteRange(x => x.ImageId == imageId);
                return uow.Save();
            }
        }
    }
}