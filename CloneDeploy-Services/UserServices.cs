using System;
using System.Collections.Generic;
using System.Linq;
using CloneDeploy_App.Helpers;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using Newtonsoft.Json;

namespace CloneDeploy_Services
{
    public class UserServices
    {
         private readonly UnitOfWork _uow;

        public UserServices()
        {
            _uow = new UnitOfWork();
        }

        public  ActionResultDTO AddUser(CloneDeployUserEntity user)
        {
           
                var validationResult = ValidateUser(user, true);
            var actionResult = new ActionResultDTO();
                if (validationResult.Success)
                {
                    _uow.UserRepository.Insert(user);
                    _uow.Save();
                    actionResult.Success = true;
                    actionResult.Id = user.Id;
                }
                else
                {
                    actionResult.ErrorMessage = validationResult.ErrorMessage;
                }

            return actionResult;

        }

        public  string TotalCount()
        {
            
                return _uow.UserRepository.Count();
            
        }

        public  int GetAdminCount()
        {
            
                return Convert.ToInt32(_uow.UserRepository.Count(u => u.Membership == "Administrator"));
            
        }

        public ActionResultDTO DeleteUser(int userId)
        {
            var u = GetUser(userId);
            if (u == null) return new ActionResultDTO() {ErrorMessage = "User Not Found", Id = 0};
            _uow.UserRepository.Delete(userId);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = u.Id;
            return actionResult;

        }

        public  bool IsAdmin(int userId)
        {
            var user = GetUser(userId);
            return user.Membership == "Administrator";
        }

        public  CloneDeployUserEntity GetUserByToken(string token)
        {
            
                return _uow.UserRepository.GetFirstOrDefault(x => x.Token == token);
            
        }

        public  CloneDeployUserEntity GetUserByApiId(string apiId)
        {
           
                return _uow.UserRepository.GetFirstOrDefault(x => x.ApiId == apiId);
            
        }

        public  void SendLockOutEmail(int userId)
        {
            //Mail not enabled
            if (Settings.SmtpEnabled == "0") return;

            var lockedUser = GetUser(userId);
            foreach (var user in SearchUsers("").Where(x => x.NotifyLockout == 1 && !string.IsNullOrEmpty(x.Email)))
            {
                if (user.Membership != "Administrator" && user.Id != userId) continue;
                var mail = new CloneDeploy_App.Helpers.Mail
                {
                    MailTo = user.Email,
                    Body = lockedUser.Name + " Has Been Locked For 15 Minutes Because Of Too Many Failed Login Attempts",
                    Subject = "User Locked"
                };
                mail.Send();
            }
        }

        public  CloneDeployUserEntity GetUser(int userId)
        {
           
                return _uow.UserRepository.GetById(userId);
            
        }

        public  CloneDeployUserEntity GetUser(string userName)
        {
           
                return _uow.UserRepository.GetFirstOrDefault(u => u.Name == userName);
            
        }

        public ApiObjectResponseDTO GetUserForLogin(int userId)
        {
            var result = new ApiObjectResponseDTO();
            var user = _uow.UserRepository.GetById(userId);
            if (user != null)
            {
                user.Token = string.Empty;
                user.ApiId = string.Empty;
                user.ApiKey = string.Empty;
                user.Password = string.Empty;
                user.Salt = string.Empty;
                result.Success = true;
                result.Id = user.Id;
                result.ObjectJson = JsonConvert.SerializeObject(user);
            }

            return result;


        }

        public  List<CloneDeployUserEntity> SearchUsers(string searchString = "")
        {
            
                var users = _uow.UserRepository.Get(u => u.Name.Contains(searchString));
                foreach (var user in users)
                {
                    user.UserGroup = new UserGroupServices().GetUserGroup(user.UserGroupId);
                }
                return users;
            
        }

        public  ActionResultDTO UpdateUser(CloneDeployUserEntity user)
        {
            var u = GetUser(user.Id);
            if (u == null) return new ActionResultDTO() { ErrorMessage = "User Not Found", Id = 0 };
                var validationResult = ValidateUser(user, false);
            var actionResult = new ActionResultDTO();
                if (validationResult.Success)
                {
                    _uow.UserRepository.Update(user, user.Id);
                    _uow.Save();
                    actionResult.Success = true;
                    actionResult.Id = user.Id;
                }
                else
                {
                    actionResult.ErrorMessage = validationResult.ErrorMessage;
                }

            return actionResult;

        }

        private  ValidationResultDTO ValidateUser(CloneDeployUserEntity user, bool isNewUser)
        {
            var validationResult = new ValidationResultDTO();

            if (string.IsNullOrEmpty(user.Name) || !user.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "User Name Is Not Valid";
                return validationResult;
            }

            if (isNewUser)
            {
                if (string.IsNullOrEmpty(user.Password))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "Password Is Not Valid";
                    return validationResult;
                }

              
                    if (_uow.UserRepository.Exists(h => h.Name == user.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This User Already Exists";
                        return validationResult;
                    }
                
            }
            else
            {
                
                    var originalUser = _uow.UserRepository.GetById(user.Id);
                    if (originalUser.Name != user.Name)
                    {
                        if (_uow.UserRepository.Exists(h => h.Name == user.Name))
                        {
                            validationResult.Success = false;
                            validationResult.ErrorMessage = "This User Already Exists";
                            return validationResult;
                        }
                    }
                
            }

            return validationResult;
        }

        public bool DeleteUserRights(int userId)
        {

            _uow.UserRightRepository.DeleteRange(x => x.UserId == userId);
            _uow.Save();
            return true;

        }

        public List<UserRightEntity> GetUserRights(int userId)
        {

            return _uow.UserRightRepository.Get(x => x.UserId == userId);

        }

        public bool DeleteUserImageManagements(int userId)
        {

            _uow.UserImageManagementRepository.DeleteRange(x => x.UserId == userId);
            _uow.Save();
            return true;

        }

        public List<UserImageManagementEntity> GetUserImageManagements(int userId)
        {

            return _uow.UserImageManagementRepository.Get(x => x.UserId == userId);

        }

        public bool DeleteUserGroupManagements(int userId)
        {

            _uow.UserGroupManagementRepository.DeleteRange(x => x.UserId == userId);
            _uow.Save();
            return true;

        }

        public List<UserGroupManagementEntity> GetUserGroupManagements(int userId)
        {

            return _uow.UserGroupManagementRepository.Get(x => x.UserId == userId);

        }
    }
}