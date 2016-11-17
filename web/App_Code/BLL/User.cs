using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Models;
using Newtonsoft.Json;

namespace BLL
{
    public class User
    {

        public static Models.ActionResult AddUser(CloneDeployUser user)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateUser(user, true);
                if (validationResult.Success)
                {
                    uow.UserRepository.Insert(user);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserRepository.Count();
            }
        }

        public static int GetAdminCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return Convert.ToInt32(uow.UserRepository.Count(u => u.Membership == "Administrator"));
            }
        }
      
        public static bool DeleteUser(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.UserRepository.Delete(userId);
                return uow.Save();
            }
        }

        public static bool IsAdmin(int userId)
        {
            var user = GetUser(userId);
            return user.Membership == "Administrator";
        }

        public static Models.CloneDeployUser GetUserByToken(string token)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserRepository.GetFirstOrDefault(x => x.Token == token);
            }
        }

        public static Models.CloneDeployUser GetUserByApiId(string apiId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserRepository.GetFirstOrDefault(x => x.ApiId == apiId);
            }
        }

        public static void SendLockOutEmail(int userId)
        {
            //Mail not enabled
            if (Settings.SmtpEnabled == "0") return;

            var lockedUser = GetUser(userId);
            foreach (var user in SearchUsers("").Where(x => x.NotifyLockout == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                if (user.Membership != "Administrator" && user.Id != userId) continue;
                var mail = new Helpers.Mail
                {
                    MailTo = user.Email,
                    Body = lockedUser.Name + " Has Been Locked For 15 Minutes Because Of Too Many Failed Login Attempts",
                    Subject = "User Locked"
                };
                mail.Send();
            }
        }

        public static CloneDeployUser GetUser(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserRepository.GetById(userId);
            }
        }

        public static CloneDeployUser GetUser(string userName)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserRepository.GetFirstOrDefault(u => u.Name == userName);
            }
        }

        public static ActionResult GetUserForLogin(int userId)
        {
            var actionResult = new ActionResult();
            using (var uow = new DAL.UnitOfWork())
            {
                var user = uow.UserRepository.GetById(userId);
                if (user != null)
                {
                    user.Token = string.Empty;
                    user.ApiId = string.Empty;
                    user.ApiKey = string.Empty;
                    user.Password = string.Empty;
                    user.Salt = string.Empty;             
                    actionResult.ObjectId = user.Id;
                    actionResult.Success = true;
                    actionResult.Object = JsonConvert.SerializeObject(user);
                }
                else
                {
                    actionResult.Success = false;
                    actionResult.Message = "Could Not Find User";
                }

                

                return actionResult;
            }
           
        }

        public static List<CloneDeployUser> SearchUsers(string searchString="")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var users = uow.UserRepository.Get(u => u.Name.Contains(searchString));
                foreach (var user in users)
                {
                    user.UserGroup = BLL.UserGroup.GetUserGroup(user.UserGroupId);
                }
                return users;
            }
        }

        public static Models.ActionResult UpdateUser(CloneDeployUser user)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateUser(user, false);
                if (validationResult.Success)
                {
                    uow.UserRepository.Update(user, user.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static Models.ActionResult ValidateUser(Models.CloneDeployUser user, bool isNewUser)
        {
            var validationResult = new Models.ActionResult();

            if (string.IsNullOrEmpty(user.Name) || !user.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.Message = "User Name Is Not Valid";
                return validationResult;
            }

            if (isNewUser)
            {
                if (string.IsNullOrEmpty(user.Password))
                {
                    validationResult.Success = false;
                    validationResult.Message = "Password Is Not Valid";
                    return validationResult;
                }

                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.UserRepository.Exists(h => h.Name == user.Name))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "This User Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    var originalUser = uow.UserRepository.GetById(user.Id);
                    if (originalUser.Name != user.Name)
                    {
                        if (uow.UserRepository.Exists(h => h.Name == user.Name))
                        {
                            validationResult.Success = false;
                            validationResult.Message = "This User Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }
    }
}