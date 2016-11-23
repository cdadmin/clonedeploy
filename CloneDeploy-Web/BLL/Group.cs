using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloneDeploy_Web.Models;
using CsvHelper;

namespace BLL
{
    public class Group
    {
        //moved
        public static ActionResult AddGroup(CloneDeploy_Web.Models.Group group, int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateGroup(group, true);
                if (validationResult.Success)
                {
                    uow.GroupRepository.Insert(group);
                    validationResult.Success = uow.Save();

                    //If Group management is being used add this group to the allowed users list 
                    var userManagedGroups = BLL.UserGroupManagement.Get(userId);
                    if (userManagedGroups.Count > 0)
                        BLL.UserGroupManagement.AddUserGroupManagements(
                            new List<CloneDeploy_Web.Models.UserGroupManagement>
                            {
                                new CloneDeploy_Web.Models.UserGroupManagement
                                {
                                    GroupId = group.Id,
                                    UserId = userId
                                }
                            });
                }

                return validationResult;
            }
        }

        //move not needed
        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupRepository.Count();
            }
        }

        //moved
        public static string GroupCountUser(int userId)
        {
            if (BLL.User.GetUser(userId).Membership == "Administrator")
                return TotalCount();

            var userManagedGroups = BLL.UserGroupManagement.Get(userId);

            //If count is zero image management is not being used return total count
            return userManagedGroups.Count == 0 ? TotalCount() : userManagedGroups.Count.ToString();
        }

        //moved
        public static ActionResult DeleteGroup(int groupId)
        {
            var result = new ActionResult();
            using (var uow = new DAL.UnitOfWork())
            {
                BLL.GroupMembership.DeleteAllMembershipsForGroup(groupId);
                BLL.UserGroupManagement.DeleteGroup(groupId);
                BLL.GroupBootMenu.DeleteGroup(groupId);
                BLL.GroupProperty.DeleteGroup(groupId);
                uow.GroupRepository.Delete(groupId);
                result.Success = uow.Save();
                return result;
            }
        }

        //moved
        public static CloneDeploy_Web.Models.Group GetGroup(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var group = uow.GroupRepository.GetById(groupId);
                if (group != null)
                    group.Image = BLL.Image.GetImage(group.ImageId);
                return group;
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.Group> SearchGroupsForUser(int userId, string searchString = "")
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
                    var listOfGroups = userManagedGroups.Select(managedGroup => uow.GroupRepository.GetFirstOrDefault(i => i.Name.Contains(searchString) && i.Id == managedGroup.GroupId)).ToList();
                    foreach (var group in listOfGroups)
                        group.Image = BLL.Image.GetImage(group.ImageId);
                    return listOfGroups;
                    
                }
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.Group> SearchGroups(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var listOfGroups = uow.GroupRepository.Get(g => g.Name.Contains(searchString));
                foreach (var group in listOfGroups)
                    group.Image = BLL.Image.GetImage(group.ImageId);
                return listOfGroups;
            }
        }

        //moved
        public static bool UpdateSmartMembership(CloneDeploy_Web.Models.Group group)
        {
            BLL.GroupMembership.DeleteAllMembershipsForGroup(group.Id);
            var computers = BLL.Computer.SearchComputers(group.SmartCriteria,Int32.MaxValue);
            var memberships = computers.Select(computer => new CloneDeploy_Web.Models.GroupMembership {GroupId = @group.Id, ComputerId = computer.Id}).ToList();
            return BLL.GroupMembership.AddMembership(memberships);
        }

        //moved
        public static ActionResult UpdateGroup(CloneDeploy_Web.Models.Group group)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateGroup(group, false);
                if (validationResult.Success)
                {
                    uow.GroupRepository.Update(group, group.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        //moved
        public static int StartGroupUnicast(CloneDeploy_Web.Models.Group group, int userId)
        {
            var count = 0;
            foreach (var computer in GetGroupMembers(group.Id))
            {
                if(new BLL.Workflows.Unicast(computer, "push",userId).Start() == "true")
                count++;
            }
            return count;
        }

        public static int ImportCsv(string path, int userId)
        {
            var importCounter = 0;
            using (var csv = new CsvReader(new StreamReader(path)))
            {
                csv.Configuration.RegisterClassMap<GroupCsvMap>();
                var records = csv.GetRecords<CloneDeploy_Web.Models.Group>();
                foreach (var group in records)
                {
                    if (AddGroup(group,userId).Success)
                        importCounter++;
                }
            }
            return importCounter;
        }

        public static void ExportCsv(string path)
        {
            using (var csv = new CsvWriter(new StreamWriter(path)))
            {
                csv.Configuration.RegisterClassMap<GroupCsvMap>();
                csv.WriteRecords(SearchGroups());
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.Computer> GetGroupMembers(int groupId, string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupRepository.GetGroupMembers(groupId, searchString);
            }
        }

        //move not needed
        public static void UpdateAllSmartGroupsMembers()
        {
            var groups = SearchGroups();
            foreach (var group in groups.Where(x => x.Type == "smart"))
                UpdateSmartMembership(group);
        }

        //move not needed
        public static ActionResult ValidateGroup(CloneDeploy_Web.Models.Group group, bool isNewGroup)
        {
            var validationResult = new ActionResult();

            if (string.IsNullOrEmpty(group.Name) || !group.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.Message = "Group Name Is Not Valid";
                return validationResult;
            }

            if (isNewGroup)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.GroupRepository.Exists(h => h.Name == group.Name))
                    {
                        validationResult.Success = false;
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
                            validationResult.Success = false;
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