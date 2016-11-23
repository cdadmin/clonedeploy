using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Web.Models;

namespace BLL
{
    public class UserGroup
    {

        public static ActionResult AddUserGroup(CloneDeployUserGroup userGroup)
        {
            using (var uow = new DAL.UnitOfWork())
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

        public static ActionResult UpdateUser(CloneDeployUserGroup userGroup)
        {
            using (var uow = new DAL.UnitOfWork())
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


        public static List<CloneDeployUserGroup> GetLdapGroups()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserGroupRepository.Get(x => x.IsLdapGroup == 1);
            }
        }

        public static List<CloneDeployUser> GetGroupMembers(int userGroupId, string searchString = "")
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
                    rights.Select(right => new CloneDeploy_Web.Models.UserRight {UserId = user.Id, Right = right.Right}).ToList();
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
                    groupManagement.Select(g => new CloneDeploy_Web.Models.UserGroupManagement { GroupId = g.GroupId, UserId = user.Id })
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
                    imageManagement.Select(g => new CloneDeploy_Web.Models.UserImageManagement { ImageId = g.ImageId, UserId = user.Id })
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
                   rights.Select(right => new CloneDeploy_Web.Models.UserRight { UserId = user.Id, Right = right.Right }).ToList();
            BLL.UserRight.DeleteUserRights(user.Id);
            BLL.UserRight.AddUserRights(userRights);


            var userGroupManagement =
                groupManagement.Select(g => new CloneDeploy_Web.Models.UserGroupManagement { GroupId = g.GroupId, UserId = user.Id })
                    .ToList();
            BLL.UserGroupManagement.DeleteUserGroupManagements(user.Id);
            BLL.UserGroupManagement.AddUserGroupManagements(userGroupManagement);


            var userImageManagement =
                imageManagement.Select(g => new CloneDeploy_Web.Models.UserImageManagement { ImageId = g.ImageId, UserId = user.Id })
                    .ToList();
            BLL.UserImageManagement.DeleteUserImageManagements(user.Id);
            BLL.UserImageManagement.AddUserImageManagements(userImageManagement);
        }

       

        public static ActionResult ValidateUser(CloneDeployUserGroup userGroup, bool isNewUserGroup)
        {
            var validationResult = new ActionResult();

            if (isNewUserGroup)
            {
                using (var uow = new DAL.UnitOfWork())
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
                using (var uow = new DAL.UnitOfWork())
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