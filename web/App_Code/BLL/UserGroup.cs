using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Models;

namespace BLL
{
    public class UserGroup
    {

        public static Models.ValidationResult AddUserGroup(CloneDeployUserGroup userGroup)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateUser(userGroup, true);
                if (validationResult.IsValid)
                {
                    uow.UserGroupRepository.Insert(userGroup);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserGroupRepository.Count();
            }
        }

        public static string MemberCount(int userGroupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserRepository.Count(x => x.UserGroupId == userGroupId);
               
            }
        }

        public static bool DeleteUserGroup(int userGroupId)
        {
            var groupMembers = GetGroupMembers(userGroupId);
            foreach (var groupMember in groupMembers)
            {
                groupMember.UserGroupId = -1;
                BLL.User.UpdateUser(groupMember);
            }

            using (var uow = new DAL.UnitOfWork())
            {
                uow.UserGroupRepository.Delete(userGroupId);
                return uow.Save();
            }
        }


        public static CloneDeployUserGroup GetUserGroup(int userGroupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserGroupRepository.GetById(userGroupId);
            }
        }

      
        public static List<CloneDeployUserGroup> SearchUserGroups(string searchString)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserGroupRepository.Get(u => u.Name.Contains(searchString));
            }
        }

        public static Models.ValidationResult UpdateUser(CloneDeployUserGroup userGroup)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateUser(userGroup, false);
                if (validationResult.IsValid)
                {
                    uow.UserGroupRepository.Update(userGroup, userGroup.Id);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }


        public static List<Models.CloneDeployUserGroup> GetLdapGroups()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserGroupRepository.Get(x => x.IsLdapGroup == 1);
            }
        }

        public static List<Models.CloneDeployUser> GetGroupMembers(int userGroupId, string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserRepository.Get(x => x.UserGroupId == userGroupId && x.Name.Contains(searchString),orderBy: (q => q.OrderBy(p => p.Name)));
            }
        }

        public static void UpdateAllGroupMembersAcls(CloneDeployUserGroup userGroup)
        {
            var rights = BLL.UserGroupRight.Get(userGroup.Id);     
           
            foreach (var user in GetGroupMembers(userGroup.Id))
            {
                var userRights =
                    rights.Select(right => new Models.UserRight {UserId = user.Id, Right = right.Right}).ToList();
                BLL.UserRight.DeleteUserRights(user.Id);
                BLL.UserRight.AddUserRights(userRights);
            }
        }

        public static void UpdateAllGroupMembersGroupMgmt(CloneDeployUserGroup userGroup)
        {        
            var groupManagement = BLL.UserGroupGroupManagement.Get(userGroup.Id);

            foreach (var user in GetGroupMembers(userGroup.Id))
            {
                var userGroupManagement =
                    groupManagement.Select(g => new Models.UserGroupManagement { GroupId = g.GroupId, UserId = user.Id })
                        .ToList();
                BLL.UserGroupManagement.DeleteUserGroupManagements(user.Id);
                BLL.UserGroupManagement.AddUserGroupManagements(userGroupManagement);    
            }
        }

        public static void UpdateAllGroupMembersImageMgmt(CloneDeployUserGroup userGroup)
        {
            var imageManagement = BLL.UserGroupImageManagement.Get(userGroup.Id);

            foreach (var user in GetGroupMembers(userGroup.Id))
            {
                var userImageManagement =
                    imageManagement.Select(g => new Models.UserImageManagement { ImageId = g.ImageId, UserId = user.Id })
                        .ToList();
                BLL.UserImageManagement.DeleteUserImageManagements(user.Id);
                BLL.UserImageManagement.AddUserImageManagements(userImageManagement);
            }
        }

        public static void AddNewGroupMember(CloneDeployUserGroup userGroup, CloneDeployUser user)
        {
            user.Membership = userGroup.Membership;
            user.UserGroupId = userGroup.Id;
            BLL.User.UpdateUser(user);


            var rights = BLL.UserGroupRight.Get(userGroup.Id);
            var groupManagement = BLL.UserGroupGroupManagement.Get(userGroup.Id);
            var imageManagement = BLL.UserGroupImageManagement.Get(userGroup.Id);

            var userRights =
                   rights.Select(right => new Models.UserRight { UserId = user.Id, Right = right.Right }).ToList();
            BLL.UserRight.DeleteUserRights(user.Id);
            BLL.UserRight.AddUserRights(userRights);


            var userGroupManagement =
                groupManagement.Select(g => new Models.UserGroupManagement { GroupId = g.GroupId, UserId = user.Id })
                    .ToList();
            BLL.UserGroupManagement.DeleteUserGroupManagements(user.Id);
            BLL.UserGroupManagement.AddUserGroupManagements(userGroupManagement);


            var userImageManagement =
                imageManagement.Select(g => new Models.UserImageManagement { ImageId = g.ImageId, UserId = user.Id })
                    .ToList();
            BLL.UserImageManagement.DeleteUserImageManagements(user.Id);
            BLL.UserImageManagement.AddUserImageManagements(userImageManagement);
        }

       

        public static Models.ValidationResult ValidateUser(Models.CloneDeployUserGroup userGroup, bool isNewUserGroup)
        {
            var validationResult = new Models.ValidationResult();

            if (isNewUserGroup)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.UserGroupRepository.Exists(h => h.Name == userGroup.Name))
                    {
                        validationResult.IsValid = false;
                        validationResult.Message = "This User Group Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    var originalUserGroup = uow.UserGroupRepository.GetById(userGroup.Id);
                    if (originalUserGroup.Name != userGroup.Name)
                    {
                        if (uow.UserGroupRepository.Exists(h => h.Name == userGroup.Name))
                        {
                            validationResult.IsValid = false;
                            validationResult.Message = "This User Group Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }
    }
}