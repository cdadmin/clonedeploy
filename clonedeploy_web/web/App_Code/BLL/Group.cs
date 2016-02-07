using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Models;

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

        public static string GroupCountUser(int userId)
        {
            if (BLL.User.GetUser(userId).Membership == "Administrator")
                return TotalCount();

            var userManagedGroups = BLL.UserGroupManagement.Get(userId);

            //If count is zero image management is not being used return total count
            return userManagedGroups.Count == 0 ? TotalCount() : userManagedGroups.Count.ToString();
        }


      
        public static Models.ValidationResult DeleteGroup(int groupId)
        {
            var result = new ValidationResult();
            using (var uow = new DAL.UnitOfWork())
            {
                BLL.GroupMembership.DeleteAllMembershipsForGroup(groupId);
                BLL.UserGroupManagement.DeleteGroup(groupId);
                BLL.GroupBootMenu.DeleteGroup(groupId);
                BLL.GroupProperty.DeleteGroup(groupId);
                uow.GroupRepository.Delete(groupId);
                result.IsValid = uow.Save();
                return result;
            }
        }

        public static Models.Group GetGroup(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var group = uow.GroupRepository.GetById(groupId);
                if (group != null)
                    group.Image = BLL.Image.GetImage(group.ImageId);
                return group;
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
                    var listOfGroups = userManagedGroups.Select(managedGroup => uow.GroupRepository.GetFirstOrDefault(i => i.Name.Contains(searchString) && i.Id == managedGroup.GroupId)).ToList();
                    foreach (var group in listOfGroups)
                        group.Image = BLL.Image.GetImage(group.ImageId);
                    return listOfGroups;
                    
                }
            }
        }

        public static List<Models.Group> SearchGroups(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var listOfGroups = uow.GroupRepository.Get(g => g.Name.Contains(searchString));
                foreach (var group in listOfGroups)
                    group.Image = BLL.Image.GetImage(group.ImageId);
                return listOfGroups;
            }
        }

        public static bool UpdateSmartMembership(Models.Group group)
        {
            BLL.GroupMembership.DeleteAllMembershipsForGroup(group.Id);
            var computers = BLL.Computer.SearchComputers(group.SmartCriteria,Int32.MaxValue);
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

        public static int StartGroupUnicast(Models.Group group, int userId)
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
                csv.Configuration.RegisterClassMap<Models.GroupCsvMap>();
                var records = csv.GetRecords<Models.Group>();
                foreach (var group in records)
                {
                    if (AddGroup(group,userId).IsValid)
                        importCounter++;
                }
            }
            return importCounter;
        }

        public static void ExportCsv(string path)
        {
            using (var csv = new CsvWriter(new StreamWriter(path)))
            {
                csv.Configuration.RegisterClassMap<Models.GroupCsvMap>();
                csv.WriteRecords(SearchGroups());
            }
        }

        public static List<Models.Computer> GetGroupMembers(int groupId, string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupRepository.GetGroupMembers(groupId, searchString);
            }
        }

        public static void UpdateAllSmartGroupsMembers()
        {
            var groups = SearchGroups();
            foreach (var group in groups.Where(x => x.Type == "smart"))
                UpdateSmartMembership(group);
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