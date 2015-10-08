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
        private readonly DAL.UnitOfWork _unitOfWork;

        public User()
        {
            _unitOfWork = new UnitOfWork();
        }

        public Models.ValidationResult AddUser(WdsUser user)
        {
            var validationResult = ValidateUser(user, true);
            if (validationResult.IsValid)
            {
                user.Password = CreatePasswordHash(user.Password, user.Salt);
                _unitOfWork.UserRepository.Insert(user);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
        }

        public string TotalCount()
        {
            return _unitOfWork.UserRepository.Count();
        }

        public int GetAdminCount()
        {
            return Convert.ToInt32(_unitOfWork.UserRepository.Count(u => u.Membership == "Administrator"));
        }
      
        public bool DeleteUser(int userId)
        {
            _unitOfWork.UserRepository.Delete(userId);
            return _unitOfWork.Save();
        }

        public WdsUser GetUser(int userId)
        {
            return _unitOfWork.UserRepository.GetById(userId);
        }

        public WdsUser GetUser(string userName)
        {
            return _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Name == userName);
        }

        public List<WdsUser> SearchUsers(string searchString)
        {
            return _unitOfWork.UserRepository.Get(u => u.Name.Contains(searchString));
        }

        public Models.ValidationResult UpdateUser(WdsUser user, bool updatePassword)
        {
            var validationResult = ValidateUser(user, true);
            if (validationResult.IsValid)
            {
                user.Password = updatePassword ? CreatePasswordHash(user.Password, user.Salt) : _unitOfWork.UserRepository.GetById(user.Id).Password;
                _unitOfWork.UserRepository.Update(user, user.Id);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
        }

        public string CreatePasswordHash(string pwd, string salt)
        {
            var saltAndPwd = string.Concat(pwd, salt);
            var hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");
            return hashedPwd;
        }

        public string CreateSalt(int byteSize)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[byteSize];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public void ImportUsers()
        {
            throw new Exception("Not Implemented");
        }

        public Models.ValidationResult ValidateUser(Models.WdsUser user, bool isNewUser)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(user.Name) || user.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
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