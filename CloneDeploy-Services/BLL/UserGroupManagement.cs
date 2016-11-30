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
                uow.Save();
                return true;
            }
        }

        public static bool DeleteUserGroupManagements(int userId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.UserGroupManagementRepository.DeleteRange(x => x.UserId == userId);
                uow.Save();
                return true;
            }
        }

        public static bool DeleteGroup(int groupId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.UserGroupManagementRepository.DeleteRange(x => x.GroupId == groupId);
                uow.Save();
                return true;
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