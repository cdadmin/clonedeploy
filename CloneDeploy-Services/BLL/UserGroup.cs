using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;

namespace CloneDeploy_App.BLL
{
    public class UserGroup
    {

        public static ActionResultEntity AddUserGroup(CloneDeployUserGroupEntity userGroup)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateUser(userGroup, true);
                if (validationResult.Success)
                {
                    uow.UserGroupRepository.Insert(userGroup);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.UserGroupRepository.Count();
            }
        }

        public static string MemberCount(int userGroupId)
        {
            using (var uow = new UnitOfWork())
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

            using (var uow = new UnitOfWork())
            {
                uow.UserGroupRepository.Delete(userGroupId);
                return uow.Save();
            }
        }


        public static CloneDeployUserGroupEntity GetUserGroup(int userGroupId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.UserGroupRepository.GetById(userGroupId);
            }
        }


        public static List<CloneDeployUserGroupEntity> SearchUserGroups(string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                return uow.UserGroupRepository.Get(u => u.Name.Contains(searchString));
            }
        }

        public static ActionResultEntity UpdateUser(CloneDeployUserGroupEntity userGroup)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateUser(userGroup, false);
                if (validationResult.Success)
                {
                    uow.UserGroupRepository.Update(userGroup, userGroup.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }


        public static List<CloneDeployUserGroupEntity> GetLdapGroups()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.UserGroupRepository.Get(x => x.IsLdapGroup == 1);
            }
        }

        public static List<CloneDeployUserEntity> GetGroupMembers(int userGroupId, string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                return uow.UserRepository.Get(x => x.UserGroupId == userGroupId && x.Name.Contains(searchString),orderBy: (q => q.OrderBy(p => p.Name)));
            }
        }

        public static void UpdateAllGroupMembersAcls(CloneDeployUserGroupEntity userGroup)
        {
            var rights = BLL.UserGroupRight.Get(userGroup.Id);     
           
            foreach (var user in GetGroupMembers(userGroup.Id))
            {
                var userRights =
                    rights.Select(right => new UserRightEntity { UserId = user.Id, Right = right.Right }).ToList();
                BLL.UserRight.DeleteUserRights(user.Id);
                BLL.UserRight.AddUserRights(userRights);
            }
        }

        public static void UpdateAllGroupMembersGroupMgmt(CloneDeployUserGroupEntity userGroup)
        {        
            var groupManagement = BLL.UserGroupGroupManagement.Get(userGroup.Id);

            foreach (var user in GetGroupMembers(userGroup.Id))
            {
                var userGroupManagement =
                    groupManagement.Select(g => new UserGroupManagementEntity { GroupId = g.GroupId, UserId = user.Id })
                        .ToList();
                BLL.UserGroupManagement.DeleteUserGroupManagements(user.Id);
                BLL.UserGroupManagement.AddUserGroupManagements(userGroupManagement);    
            }
        }

        public static void UpdateAllGroupMembersImageMgmt(CloneDeployUserGroupEntity userGroup)
        {
            var imageManagement = BLL.UserGroupImageManagement.Get(userGroup.Id);

            foreach (var user in GetGroupMembers(userGroup.Id))
            {
                var userImageManagement =
                    imageManagement.Select(g => new UserImageManagementEntity { ImageId = g.ImageId, UserId = user.Id })
                        .ToList();
                BLL.UserImageManagement.DeleteUserImageManagements(user.Id);
                BLL.UserImageManagement.AddUserImageManagements(userImageManagement);
            }
        }

        public static void AddNewGroupMember(CloneDeployUserGroupEntity userGroup, CloneDeployUserEntity user)
        {
            user.Membership = userGroup.Membership;
            user.UserGroupId = userGroup.Id;
            BLL.User.UpdateUser(user);


            var rights = BLL.UserGroupRight.Get(userGroup.Id);
            var groupManagement = BLL.UserGroupGroupManagement.Get(userGroup.Id);
            var imageManagement = BLL.UserGroupImageManagement.Get(userGroup.Id);

            var userRights =
                   rights.Select(right => new UserRightEntity { UserId = user.Id, Right = right.Right }).ToList();
            BLL.UserRight.DeleteUserRights(user.Id);
            BLL.UserRight.AddUserRights(userRights);


            var userGroupManagement =
                groupManagement.Select(g => new UserGroupManagementEntity { GroupId = g.GroupId, UserId = user.Id })
                    .ToList();
            BLL.UserGroupManagement.DeleteUserGroupManagements(user.Id);
            BLL.UserGroupManagement.AddUserGroupManagements(userGroupManagement);


            var userImageManagement =
                imageManagement.Select(g => new UserImageManagementEntity { ImageId = g.ImageId, UserId = user.Id })
                    .ToList();
            BLL.UserImageManagement.DeleteUserImageManagements(user.Id);
            BLL.UserImageManagement.AddUserImageManagements(userImageManagement);
        }



        public static ActionResultEntity ValidateUser(CloneDeployUserGroupEntity userGroup, bool isNewUserGroup)
        {
            var validationResult = new ActionResultEntity();

            if (isNewUserGroup)
            {
                using (var uow = new UnitOfWork())
                {
                    if (uow.UserGroupRepository.Exists(h => h.Name == userGroup.Name))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "This User Group Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new UnitOfWork())
                {
                    var originalUserGroup = uow.UserGroupRepository.GetById(userGroup.Id);
                    if (originalUserGroup.Name != userGroup.Name)
                    {
                        if (uow.UserGroupRepository.Exists(h => h.Name == userGroup.Name))
                        {
                            validationResult.Success = false;
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