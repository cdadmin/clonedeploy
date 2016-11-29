using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public static class UserRight
    {
        public static bool AddUserRights(List<UserRightEntity> listOfRights)
        {
            using (var uow = new UnitOfWork())
            {
                foreach (var right in listOfRights)
                    uow.UserRightRepository.Insert(right);

                return uow.Save();
            }
        }

        public static bool DeleteUserRights(int userId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.UserRightRepository.DeleteRange(x => x.UserId == userId);
                return uow.Save();
            }
        }

        public static List<UserRightEntity> Get(int userId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.UserRightRepository.Get(x => x.UserId == userId);
            }
        }
    }
}