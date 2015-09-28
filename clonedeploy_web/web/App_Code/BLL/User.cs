using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using Global;
using Helpers;

namespace BLL
{
    public class User
    {
        private readonly DAL.User _da = new DAL.User();

        public bool AddUser(Models.WdsUser user)
        {
            if (_da.Exists(user.Name))
            {
                Message.Text = "A User With This Name Already Exists";
                return false;
            }
            user.Password = CreatePasswordHash(user.Password, user.Salt);
            if (_da.Create(user))
            {
                Message.Text = "Successfully Created Computer";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Computer";
                return false;
            }
        }

        public string TotalCount()
        {
            return _da.GetTotalCount();
        }

        public int GetAdminCount()
        {
            return _da.GetAdminCount();
        }
      
        public bool DeleteUser(int userId)
        {
            return _da.Delete(userId);
        }

        public Models.WdsUser GetUser(int userId)
        {
            return _da.Read(userId);
        }

        public Models.WdsUser GetUser(string userName)
        {
            return _da.Read(userName);
        }

        public List<Models.WdsUser> SearchUsers(string searchString)
        {
            return _da.Find(searchString);
        }

        public void UpdateUser(Models.WdsUser user, bool updatePassword)
        {
            if (updatePassword)
                user.Password = CreatePasswordHash(user.Password, user.Salt);

            if (_da.Update(user,updatePassword))
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
        public bool ValidateUserData(Models.WdsUser user)
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