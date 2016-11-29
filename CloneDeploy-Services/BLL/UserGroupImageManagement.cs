using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public static class UserGroupImageManagement
    {
        public static bool AddUserGroupImageManagements(List<UserGroupImageManagementEntity> listOfImages)
        {
            using (var uow = new UnitOfWork())
            {
                foreach (var image in listOfImages)
                    uow.UserGroupImageManagementRepository.Insert(image);

                return uow.Save();
            }
        }

        public static bool DeleteUserGroupImageManagements(int userGroupId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.UserGroupImageManagementRepository.DeleteRange(x => x.UserGroupId == userGroupId);
                return uow.Save();
            }
        }

        public static List<UserGroupImageManagementEntity> Get(int userGroupId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.UserGroupImageManagementRepository.Get(x => x.UserGroupId == userGroupId);
            }
        }

        public static bool DeleteImage(int imageId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.UserGroupImageManagementRepository.DeleteRange(x => x.ImageId == imageId);
                return uow.Save();
            }
        }
    }
}