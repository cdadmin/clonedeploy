using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Workflows;
using DAL;
using Helpers;
using Models;
using Tasks;

namespace BLL
{
    public class Group
    {

        public static Models.ValidationResult AddGroup(Models.Group group, int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateGroup(group, true);
                if (validationResult.IsValid)
                {
                    uow.GroupRepository.Insert(group);
                    validationResult.IsValid = uow.Save();

                    //If Group management is being used add this group to the allowed users list 
                    var userManagedGroups = BLL.UserGroupManagement.Get(userId);
                    if (userManagedGroups.Count > 0)
                        BLL.UserGroupManagement.AddUserGroupManagements(
                            new List<Models.UserGroupManagement>
                            {
                                new Models.UserGroupManagement
                                {
                                    GroupId = group.Id,
                                    UserId = userId
                                }
                            });
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupRepository.Count();
            }
        }

      
        public static bool DeleteGroup(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.GroupRepository.Delete(groupId);
                return uow.Save();
            }
        }

        public static Models.Group GetGroup(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupRepository.GetById(groupId);
            }
        }

        public static List<Models.Group> SearchGroupsForUser(int userId, string searchString = "")
        {
            if (BLL.User.GetUser(userId).Membership == "Administrator")
                return SearchGroups(searchString);

            var userManagedGroups = BLL.UserGroupManagement.Get(userId);
            if (userManagedGroups.Count == 0)
                return SearchGroups(searchString);

            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    return userManagedGroups.Select(managedGroup => uow.GroupRepository.GetFirstOrDefault(i => i.Name.Contains(searchString) && i.Id == managedGroup.GroupId)).ToList();
                }
            }
        }

        public static List<Models.Group> SearchGroups(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupRepository.Get(g => g.Name.Contains(searchString));
            }
        }

        public static bool UpdateSmartMembership(Models.Group group)
        {
            BLL.GroupMembership.DeleteAllMembershipsForGroup(group.Id);
            var computers = BLL.Computer.SearchComputers(group.SmartCriteria);
            var memberships = computers.Select(computer => new Models.GroupMembership {GroupId = @group.Id, ComputerId = computer.Id}).ToList();
            return BLL.GroupMembership.AddMembership(memberships);
        }

        public static Models.ValidationResult UpdateGroup(Models.Group group)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateGroup(group, false);
                if (validationResult.IsValid)
                {
                    uow.GroupRepository.Update(group, group.Id);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }

        public static void StartMulticast(Models.Group group)
        {
            var multicast = new Multicast(group);
            multicast.Create();
        }

        public static void StartGroupUnicast(Models.Group group)
        {
            var image = BLL.Image.GetImage(group.Image);

            if (BLL.Image.Check_Checksum(image))
            {
                var count = 0;

                foreach (var host in GetGroupMembers(group.Id, ""))
                {
                    new BLL.Workflows.Unicast(host, "push").Start();
                    count++;
                }
                //Message.Text = "Started " + count + " Tasks";
                var history = new History
                {
                    Event = "Unicast",
                    Type = "Group",
                    TypeId = group.Id.ToString()
                };
                history.CreateEvent();
            }
        }

        public static void ImportGroups()
        {
            throw new Exception("Not Implemented");
        }

       

        public static List<Models.Computer> GetGroupMembers(int groupId, string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupRepository.GetGroupMembers(groupId, searchString);
            }
        }

        public static Models.ValidationResult ValidateGroup(Models.Group group, bool isNewGroup)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(group.Name) || !group.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Group Name Is Not Valid";
                return validationResult;
            }

            if (isNewGroup)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.GroupRepository.Exists(h => h.Name == group.Name))
                    {
                        validationResult.IsValid = false;
                        validationResult.Message = "This Group Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    var originalGroup = uow.GroupRepository.GetById(group.Id);
                    if (originalGroup.Name != group.Name)
                    {
                        if (uow.GroupRepository.Exists(h => h.Name == group.Name))
                        {
                            validationResult.IsValid = false;
                            validationResult.Message = "This Group Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }
    }
}