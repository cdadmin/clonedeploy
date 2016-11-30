using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public static class UserGroupRight
    {
        public static bool AddUserGroupRights(List<UserGroupRightEntity> listOfRights)
        {
            using (var uow = new UnitOfWork())
            {
                foreach (var right in listOfRights)
                    uow.UserGroupRightRepository.Insert(right);
                uow.Save();
                return true;
            }
        }

        public static bool DeleteUserGroupRights(int userGroupId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.UserGroupRightRepository.DeleteRange(x => x.UserGroupId == userGroupId);
                uow.Save();
                return true;
            }
        }

        public static List<UserGroupRightEntity> Get(int userGroupId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.UserGroupRightRepository.Get(x => x.UserGroupId == userGroupId);
            }
        }
    }
}