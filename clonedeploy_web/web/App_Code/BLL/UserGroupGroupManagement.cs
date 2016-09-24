using System.Collections.Generic;

namespace BLL
{
    public static class UserGroupGroupManagement
    {
        public static bool AddUserGroupGroupManagements(List<Models.UserGroupGroupManagement> listOfGroups)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                foreach (var group in listOfGroups)
                    uow.UserGroupGroupManagementRepository.Insert(group);

                return uow.Save();
            }
        }

        public static bool DeleteUserGroupGroupManagements(int userGroupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.UserGroupGroupManagementRepository.DeleteRange(x => x.UserGroupId == userGroupId);
                return uow.Save();
            }
        }

        public static bool DeleteGroup(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.UserGroupGroupManagementRepository.DeleteRange(x => x.GroupId == groupId);
                return uow.Save();
            }
        }

        public static List<Models.UserGroupGroupManagement> Get(int userGroupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserGroupGroupManagementRepository.Get(x => x.UserGroupId == userGroupId);
            }
        }
    }
}