using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public static class UserGroupManagement
    {
        public static bool AddUserGroupManagements(List<UserGroupManagementEntity> listOfGroups)
        {
            using (var uow = new UnitOfWork())
            {
                foreach (var group in listOfGroups)
                    uow.UserGroupManagementRepository.Insert(group);

                return uow.Save();
            }
        }

        public static bool DeleteUserGroupManagements(int userId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.UserGroupManagementRepository.DeleteRange(x => x.UserId == userId);
                return uow.Save();
            }
        }

        public static bool DeleteGroup(int groupId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.UserGroupManagementRepository.DeleteRange(x => x.GroupId == groupId);
                return uow.Save();
            }
        }

        public static List<UserGroupManagementEntity> Get(int userId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.UserGroupManagementRepository.Get(x => x.UserId == userId);
            }
        }
    }
}