using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CsvHelper;
using Newtonsoft.Json;

namespace CloneDeploy_App.BLL
{
    public class Group
    {

        public static ActionResultEntity AddGroup(GroupEntity group, int userId)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateGroup(group, true);
                if (validationResult.Success)
                {
                    uow.GroupRepository.Insert(group);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = group.Id;
                    validationResult.Object = JsonConvert.SerializeObject(group);
                    //If Group management is being used add this group to the allowed users list 
                    var userManagedGroups = BLL.UserGroupManagement.Get(userId);
                    if (userManagedGroups.Count > 0)
                        BLL.UserGroupManagement.AddUserGroupManagements(
                            new List<UserGroupManagementEntity>
                            {
                                new UserGroupManagementEntity
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
            using (var uow = new UnitOfWork())
            {
                return uow.GroupRepository.Count();
            }
        }

        public static ApiDTO GroupCountUser(int userId)
        {
            var apiDTO = new ApiDTO();
            if (BLL.User.GetUser(userId).Membership == "Administrator")
            {
                apiDTO.Value = TotalCount();
                return apiDTO;
            }

            var userManagedGroups = BLL.UserGroupManagement.Get(userId);

            //If count is zero image management is not being used return total count
            apiDTO.Value = userManagedGroups.Count == 0 ? TotalCount() : userManagedGroups.Count.ToString();
            return apiDTO;
        }


        public static ActionResultEntity DeleteGroup(int groupId)
        {
            var group = GetGroup(groupId);
            var result = new ActionResultEntity();
            using (var uow = new UnitOfWork())
            {
                BLL.GroupMembership.DeleteAllMembershipsForGroup(groupId);
                BLL.UserGroupManagement.DeleteGroup(groupId);
                BLL.GroupBootMenu.DeleteGroup(groupId);
                BLL.GroupProperty.DeleteGroup(groupId);
                uow.GroupRepository.Delete(groupId);
                result.Success = uow.Save();
                result.ObjectId = group.Id;
                result.Object = JsonConvert.SerializeObject(group);
                return result;
            }
        }

        public static GroupEntity GetGroup(int groupId)
        {
            using (var uow = new UnitOfWork())
            {
                var group = uow.GroupRepository.GetById(groupId);
                if (group != null)
                    group.Image = BLL.Image.GetImage(group.ImageId);
                return group;
            }
        }

        public static List<GroupEntity> SearchGroupsForUser(int userId, string searchString = "")
        {
            if (BLL.User.GetUser(userId).Membership == "Administrator")
                return SearchGroups(searchString);

            var userManagedGroups = BLL.UserGroupManagement.Get(userId);
            if (userManagedGroups.Count == 0)
                return SearchGroups(searchString);

            else
            {
                using (var uow = new UnitOfWork())
                {
                    var listOfGroups = userManagedGroups.Select(managedGroup => uow.GroupRepository.GetFirstOrDefault(i => i.Name.Contains(searchString) && i.Id == managedGroup.GroupId)).ToList();
                    foreach (var group in listOfGroups)
                        group.Image = BLL.Image.GetImage(group.ImageId);
                    return listOfGroups;
                    
                }
            }
        }

        public static List<GroupEntity> SearchGroups(string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                var listOfGroups = uow.GroupRepository.Get(g => g.Name.Contains(searchString));
                foreach (var group in listOfGroups)
                    group.Image = BLL.Image.GetImage(group.ImageId);
                return listOfGroups;
            }
        }

        public static bool UpdateSmartMembership(GroupEntity group)
        {
            BLL.GroupMembership.DeleteAllMembershipsForGroup(group.Id);
            var computers = BLL.Computer.SearchComputers(group.SmartCriteria,Int32.MaxValue);
            var memberships = computers.Select(computer => new GroupMembershipEntity { GroupId = @group.Id, ComputerId = computer.Id }).ToList();
            return BLL.GroupMembership.AddMembership(memberships);
        }

        public static ActionResultEntity UpdateGroup(GroupEntity group)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateGroup(group, false);
                if (validationResult.Success)
                {
                    uow.GroupRepository.Update(group, group.Id);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = group.Id;
                    validationResult.Object = JsonConvert.SerializeObject(group);
                }

                return validationResult;
            }
        }

        public static int StartGroupUnicast(GroupEntity group, int userId)
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
                var records = csv.GetRecords<GroupEntity>();
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

        public static List<ComputerEntity> GetGroupMembers(int groupId, string searchString = "")
        {
            using (var uow = new UnitOfWork())
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

        public static ActionResultEntity ValidateGroup(GroupEntity group, bool isNewGroup)
        {
            var validationResult = new ActionResultEntity();

            if (string.IsNullOrEmpty(group.Name) || !group.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.Message = "Group Name Is Not Valid";
                return validationResult;
            }

            if (isNewGroup)
            {
                using (var uow = new UnitOfWork())
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
                using (var uow = new UnitOfWork())
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