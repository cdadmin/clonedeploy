using System;

namespace BLL
{
    public class UserLockout
    {
        public static void ProcessBadLogin(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var userLockout = Get(userId);
                if (userLockout == null)
                {
                    uow.UserLockoutRepository.Insert(new CloneDeploy_Web.Models.UserLockout {UserId = userId, BadLoginCount = 1});
                   
                }
                else
                {
                    userLockout.BadLoginCount += 1;
                    if (userLockout.BadLoginCount == 5)
                    {
                        userLockout.LockedUntil = DateTime.UtcNow.AddMinutes(15);
                        BLL.User.SendLockOutEmail(userId);
                    }

                    uow.UserLockoutRepository.Update(userLockout, userLockout.Id);
                }
                uow.Save();
            }
        }

        public static bool AccountIsLocked(int userId)
        {
            var userLockout = Get(userId);

            if (userLockout == null) return false;

            if (userLockout.LockedUntil == null) return false;

            if (DateTime.UtcNow > userLockout.LockedUntil)
            {
                //Lockout has expired reset lock
                DeleteUserLockouts(userId);
                return false;
            }
            else
            {
                return true;
            }


        }

        public static bool DeleteUserLockouts(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.UserLockoutRepository.DeleteRange(x => x.UserId == userId);
                return uow.Save();
            }
        }

        public static CloneDeploy_Web.Models.UserLockout Get(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserLockoutRepository.GetFirstOrDefault(x => x.UserId == userId);
            }
        }
    }
}