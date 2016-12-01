using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class UserGroupServices
    {
         private readonly UnitOfWork _uow;

        public UserGroupServices()
        {
            _uow = new UnitOfWork();
        }

        public  ActionResultDTO AddUserGroup(CloneDeployUserGroupEntity userGroup)
        {
          
                var validationResult = ValidateUserGroup(userGroup, true);
            var actionResult = new ActionResultDTO();
                if (validationResult.Success)
                {
                    _uow.UserGroupRepository.Insert(userGroup);
                    _uow.Save();
                    actionResult.Success = true;
                    actionResult.Id = userGroup.Id;
                }
                else
                {
                    actionResult.ErrorMessage = validationResult.ErrorMessage;
                }

            return actionResult;

        }

        public  string TotalCount()
        {
         
                return _uow.UserGroupRepository.Count();
            
        }

        public  string MemberCount(int userGroupId)
        {
            
                return _uow.UserRepository.Count(x => x.UserGroupId == userGroupId);
               
            
        }

        public  ActionResultDTO DeleteUserGroup(int userGroupId)
        {
            var ug = GetUserGroup(userGroupId);
            if (ug == null) return new ActionResultDTO() { ErrorMessage = "User Group Not Found", Id = 0 };

            var groupMembers = GetGroupMembers(userGroupId);
            foreach (var groupMember in groupMembers)
            {
                groupMember.UserGroupId = -1;
                new UserServices().UpdateUser(groupMember);
            }

           
                _uow.UserGroupRepository.Delete(userGroupId);
                _uow.Save();
                var actionResult = new ActionResultDTO();
                actionResult.Success = true;
                actionResult.Id = ug.Id;
                return actionResult;
            
        }


        public  CloneDeployUserGroupEntity GetUserGroup(int userGroupId)
        {
            
                return _uow.UserGroupRepository.GetById(userGroupId);
            
        }


        public  List<CloneDeployUserGroupEntity> SearchUserGroups(string searchString = "")
        {
           
                return _uow.UserGroupRepository.Get(u => u.Name.Contains(searchString));
            
        }

        public  ActionResultDTO UpdateUser(CloneDeployUserGroupEntity userGroup)
        {
            var ug = GetUserGroup(userGroup.Id);
            if (ug == null) return new ActionResultDTO() { ErrorMessage = "User Group Not Found", Id = 0 };
            var actionResult = new ActionResultDTO();
                var validationResult = ValidateUserGroup(userGroup, false);
                if (validationResult.Success)
                {
                    _uow.UserGroupRepository.Update(userGroup, userGroup.Id);
                    _uow.Save();
                    actionResult.Success = true;
                    actionResult.Id = userGroup.Id;
                }
                else
                {
                    actionResult.ErrorMessage = validationResult.ErrorMessage;
                }

            return actionResult;

        }


        public  List<CloneDeployUserGroupEntity> GetLdapGroups()
        {
            
                return _uow.UserGroupRepository.Get(x => x.IsLdapGroup == 1);
            
        }

        public  List<CloneDeployUserEntity> GetGroupMembers(int userGroupId, string searchString = "")
        {
          
                return _uow.UserRepository.Get(x => x.UserGroupId == userGroupId && x.Name.Contains(searchString),orderBy: (q => q.OrderBy(p => p.Name)));
            
        }

        public bool UpdateAllGroupMembersAcls(CloneDeployUserGroupEntity userGroup)
        {
            var rights = UserGroupRightServices.Get(userGroup.Id);     
           
            foreach (var user in GetGroupMembers(userGroup.Id))
            {
                var userRights =
                    rights.Select(right => new UserRightEntity { UserId = user.Id, Right = right.Right }).ToList();
                UserRightServices.DeleteUserRights(user.Id);
                UserRightServices.AddUserRights(userRights);
            }

            return true;
        }

        public  bool UpdateAllGroupMembersGroupMgmt(CloneDeployUserGroupEntity userGroup)
        {        
            var groupManagement = UserGroupGroupManagementServices.Get(userGroup.Id);

            foreach (var user in GetGroupMembers(userGroup.Id))
            {
                var userGroupManagement =
                    groupManagement.Select(g => new UserGroupManagementEntity { GroupId = g.GroupId, UserId = user.Id })
                        .ToList();
                UserGroupManagementServices.DeleteUserGroupManagements(user.Id);
                UserGroupManagementServices.AddUserGroupManagements(userGroupManagement);    
            }

            return true;
        }

        public  bool UpdateAllGroupMembersImageMgmt(CloneDeployUserGroupEntity userGroup)
        {
            var imageManagement = UserGroupImageManagementServices.Get(userGroup.Id);

            foreach (var user in GetGroupMembers(userGroup.Id))
            {
                var userImageManagement =
                    imageManagement.Select(g => new UserImageManagementEntity { ImageId = g.ImageId, UserId = user.Id })
                        .ToList();
                UserImageManagementServices.DeleteUserImageManagements(user.Id);
                UserImageManagementServices.AddUserImageManagements(userImageManagement);
            }

            return true;
        }

        public  bool AddNewGroupMember(CloneDeployUserGroupEntity userGroup, CloneDeployUserEntity user)
        {
            user.Membership = userGroup.Membership;
            user.UserGroupId = userGroup.Id;
            new UserServices().UpdateUser(user);


            var rights = UserGroupRightServices.Get(userGroup.Id);
            var groupManagement = UserGroupGroupManagementServices.Get(userGroup.Id);
            var imageManagement = UserGroupImageManagementServices.Get(userGroup.Id);

            var userRights =
                   rights.Select(right => new UserRightEntity { UserId = user.Id, Right = right.Right }).ToList();
            UserRightServices.DeleteUserRights(user.Id);
            UserRightServices.AddUserRights(userRights);


            var userGroupManagement =
                groupManagement.Select(g => new UserGroupManagementEntity { GroupId = g.GroupId, UserId = user.Id })
                    .ToList();
            UserGroupManagementServices.DeleteUserGroupManagements(user.Id);
            UserGroupManagementServices.AddUserGroupManagements(userGroupManagement);


            var userImageManagement =
                imageManagement.Select(g => new UserImageManagementEntity { ImageId = g.ImageId, UserId = user.Id })
                    .ToList();
            UserImageManagementServices.DeleteUserImageManagements(user.Id);
            UserImageManagementServices.AddUserImageManagements(userImageManagement);

            return true;
        }



        private  ValidationResultDTO ValidateUserGroup(CloneDeployUserGroupEntity userGroup, bool isNewUserGroup)
        {
            var validationResult = new ValidationResultDTO();

            if (isNewUserGroup)
            {
               
                    if (_uow.UserGroupRepository.Exists(h => h.Name == userGroup.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This User Group Already Exists";
                        return validationResult;
                    }
                
            }
            else
            {
               
                    var originalUserGroup = _uow.UserGroupRepository.GetById(userGroup.Id);
                    if (originalUserGroup.Name != userGroup.Name)
                    {
                        if (_uow.UserGroupRepository.Exists(h => h.Name == userGroup.Name))
                        {
                            validationResult.Success = false;
                            validationResult.ErrorMessage = "This User Group Already Exists";
                            return validationResult;
                        }
                    }
                
            }

            return validationResult;
        }

        public bool DeleteUserGroupRights(int userGroupId)
        {

            _uow.UserGroupRightRepository.DeleteRange(x => x.UserGroupId == userGroupId);
            _uow.Save();
            return true;

        }

        public List<UserGroupRightEntity> GetUserGroupRights(int userGroupId)
        {

            return _uow.UserGroupRightRepository.Get(x => x.UserGroupId == userGroupId);

        }

        public bool DeleteUserGroupGroupManagements(int userGroupId)
        {

            _uow.UserGroupGroupManagementRepository.DeleteRange(x => x.UserGroupId == userGroupId);
            _uow.Save();
            return true;

        }

        public List<UserGroupGroupManagementEntity> GetUserGroupGroupManagements(int userGroupId)
        {

            return _uow.UserGroupGroupManagementRepository.Get(x => x.UserGroupId == userGroupId);

        }

        public bool DeleteUserGroupImageManagements(int userGroupId)
        {

            _uow.UserGroupImageManagementRepository.DeleteRange(x => x.UserGroupId == userGroupId);
            _uow.Save();
            return true;

        }

        public List<UserGroupImageManagementEntity> GetUserGroupImageManagements(int userGroupId)
        {

            return _uow.UserGroupImageManagementRepository.Get(x => x.UserGroupId == userGroupId);

        }
    }
}