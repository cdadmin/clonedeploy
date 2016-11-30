using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public static class UserGroupGroupManagement
    {
        public static bool AddUserGroupGroupManagements(List<UserGroupGroupManagementEntity> listOfGroups)
        {
            using (var uow = new UnitOfWork())
            {
                foreach (var group in listOfGroups)
                    uow.UserGroupGroupManagementRepository.Insert(group);
                uow.Save();
                return true;
            }
        }

        public static bool DeleteUserGroupGroupManagements(int userGroupId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.UserGroupGroupManagementRepository.DeleteRange(x => x.UserGroupId == userGroupId);
                uow.Save();
                return true;
            }
        }

        public static bool DeleteGroup(int groupId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.UserGroupGroupManagementRepository.DeleteRange(x => x.GroupId == groupId);
                uow.Save();
                return true;
            }
        }

        public static List<UserGroupGroupManagementEntity> Get(int userGroupId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.UserGroupGroupManagementRepository.Get(x => x.UserGroupId == userGroupId);
            }
        }
    }
}