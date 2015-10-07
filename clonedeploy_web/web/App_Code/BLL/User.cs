using System;
using System.Collections.Generic;
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

        public bool AddUser(WdsUser user)
        {
            if (_unitOfWork.UserRepository.Exists(u => u.Name == user.Name))
            {
                Message.Text = "A User With This Name Already Exists";
                return false;
            }
            user.Password = CreatePasswordHash(user.Password, user.Salt);
            _unitOfWork.UserRepository.Insert(user);
            if (_unitOfWork.Save())
            {
                Message.Text = "Successfully Created User";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create User";
                return false;
            }
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

        public void UpdateUser(WdsUser user, bool updatePassword)
        {
            user.Password = updatePassword ? CreatePasswordHash(user.Password, user.Salt) : _unitOfWork.UserRepository.GetById(user.Id).Password;

            _unitOfWork.UserRepository.Update(user,user.Id);
            if (_unitOfWork.Save())
                Message.Text = "Successfully Updated User";
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
        public bool ValidateUserData(WdsUser user)
        {
            var validated = true;
            if (string.IsNullOrEmpty(user.Name) || user.Name.Contains(" "))
            {
                validated = false;
                Message.Text = "User Name Cannot Be Empty Or Contain Spaces";
            }
            if (string.IsNullOrEmpty(user.Password) || user.Password.Contains(" "))
            {
                validated = false;
                Message.Text = "User Password Cannot Be Empty Or Contain Spaces";
            }

            return validated;
        }
    }
}