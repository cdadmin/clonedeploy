using System.Collections.Generic;

namespace BLL
{
    public static class UserGroupRight
    {
        public static bool AddUserGroupRights(List<Models.UserGroupRight> listOfRights)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                foreach (var right in listOfRights)
                    uow.UserGroupRightRepository.Insert(right);

                return uow.Save();
            }
        }

        public static bool DeleteUserGroupRights(int userGroupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.UserGroupRightRepository.DeleteRange(x => x.UserGroupId == userGroupId);
                return uow.Save();
            }
        }

        public static List<Models.UserGroupRight> Get(int userGroupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserGroupRightRepository.Get(x => x.UserGroupId == userGroupId);
            }
        }
    }
}