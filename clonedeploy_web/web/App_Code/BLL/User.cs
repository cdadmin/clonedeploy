using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Security;
using DAL;
using Helpers;
using Models;

namespace BLL
{
    public class User
    {

        public static Models.ValidationResult AddUser(CloneDeployUser user)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateUser(user, true);
                if (validationResult.IsValid)
                {
                    user.Password = Helpers.Utility.CreatePasswordHash(user.Password, user.Salt);
                    uow.UserRepository.Insert(user);
                    validationResult.IsValid = uow.Save();
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

        public static List<CloneDeployUser> SearchUsers(string searchString)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserRepository.Get(u => u.Name.Contains(searchString));
            }
        }

        public static Models.ValidationResult UpdateUser(CloneDeployUser user, bool updatePassword)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateUser(user, false);
                if (validationResult.IsValid)
                {
                    user.Password = updatePassword
                        ? Helpers.Utility.CreatePasswordHash(user.Password, user.Salt)
                        : uow.UserRepository.GetById(user.Id).Password;
                    uow.UserRepository.Update(user, user.Id);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }

        public static Models.ValidationResult ValidateUser(Models.CloneDeployUser user, bool isNewUser)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(user.Name) || !user.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.IsValid = false;
                validationResult.Message = "User Name Is Not Valid";
                return validationResult;
            }

            if (isNewUser)
            {
                if (string.IsNullOrEmpty(user.Password))
                {
                    validationResult.IsValid = false;
                    validationResult.Message = "Password Is Not Valid";
                    return validationResult;
                }

                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.UserRepository.Exists(h => h.Name == user.Name))
                    {
                        validationResult.IsValid = false;
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
                            validationResult.IsValid = false;
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