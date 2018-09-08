using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Workflows;
using CsvHelper;

namespace CloneDeploy_Services
{
    public class GroupServices
    {
        private readonly UnitOfWork _uow;

        public GroupServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddGroup(GroupEntity group, int userId)
        {
            var actionResult = new ActionResultDTO();
            var validationResult = ValidateGroup(group, true);
            if (validationResult.Success)
            {
                _uow.GroupRepository.Insert(group);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = group.Id;
                //If Group management is being used add this group to the allowed users list 
                var user = new UserServices().GetUser(userId);
                if (user.GroupManagementEnabled == 1)
                    new UserGroupManagementServices().AddUserGroupManagements(
                        new List<UserGroupManagementEntity>
                        {
                            new UserGroupManagementEntity
                            {
                                GroupId = group.Id,
                                UserId = userId
                            }
                        });
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        public bool DeleteAllBootMenusForGroup(int groupId)
        {
            _uow.GroupBootMenuRepository.DeleteRange(x => x.GroupId == groupId);
            _uow.Save();
            return true;
        }

        //check this
        public bool DeleteAllManagementsForGroup(int groupId)
        {
            _uow.UserGroupManagementRepository.DeleteRange(x => x.GroupId == groupId);
            _uow.Save();
            return true;
        }

        public bool DeleteAllMembershipsForGroup(int groupId)
        {
            _uow.GroupMembershipRepository.DeleteRange(x => x.GroupId == groupId);
            _uow.Save();
            return true;
        }

        public bool DeleteAllPropertiesForGroup(int groupId)
        {
            _uow.GroupPropertyRepository.DeleteRange(x => x.GroupId == groupId);
            _uow.Save();
            return true;
        }

        public ActionResultDTO DeleteGroup(int groupId)
        {
            var group = GetGroup(groupId);
            if (group == null)
                return new ActionResultDTO {ErrorMessage = "Group Not Found", Id = 0};
            var result = new ActionResultDTO();

            DeleteAllMembershipsForGroup(groupId);
            DeleteAllManagementsForGroup(groupId);
            DeleteAllBootMenusForGroup(groupId);
            DeleteAllPropertiesForGroup(groupId);
            _uow.GroupRepository.Delete(groupId);
            _uow.Save();
            result.Success = true;
            result.Id = group.Id;

            return result;
        }

        public bool DeleteGroupImageClassifications(int groupId)
        {
            _uow.GroupImageClassificationRepository.DeleteRange(x => x.GroupId == groupId);
            _uow.Save();
            return true;
        }

        public bool DeleteMembership(int computerId, int groupId)
        {
            _uow.GroupMembershipRepository.DeleteRange(
                g => g.ComputerId == computerId && g.GroupId == groupId);
            _uow.Save();
            return true;
        }

     

        public void ExportCsv(string path)
        {
            using (var csv = new CsvWriter(new StreamWriter(path)))
            {
                csv.Configuration.RegisterClassMap<GroupCsvMap>();
                csv.WriteRecords(SearchGroups());
            }
        }

        public ClusterGroupEntity GetClusterGroup(int groupId)
        {
            var cgServices = new ClusterGroupServices();
            var group = GetGroup(groupId);

            if (group.ClusterGroupId != -1)
            {
                var cg = cgServices.GetClusterGroup(@group.ClusterGroupId);
                return cg ?? cgServices.GetDefaultClusterGroup();
            }

            return cgServices.GetDefaultClusterGroup();
        }

        public GroupEntity GetGroup(int groupId)
        {
            var group = _uow.GroupRepository.GetById(groupId);

            return group;
        }
        public GroupEntity GetGroupByName(string groupName)
        {
            var group = _uow.GroupRepository.GetFirstOrDefault(x => x.Name == groupName);

            return group;
        }

        public GroupBootMenuEntity GetGroupBootMenu(int groupId)
        {
            return _uow.GroupBootMenuRepository.GetFirstOrDefault(p => p.GroupId == groupId);
        }

        public List<GroupImageClassificationEntity> GetGroupImageClassifications(int groupId)
        {
            return _uow.GroupImageClassificationRepository.Get(x => x.GroupId == groupId);
        }

        public string GetGroupMemberCount(int groupId)
        {
            return _uow.GroupMembershipRepository.Count(g => g.GroupId == groupId);
        }

        public List<ComputerEntity> GetGroupMembers(int groupId, string searchString = "")
        {
            return _uow.GroupRepository.GetGroupMembers(groupId, searchString);
        }

        public List<ComputerWithImage> GetGroupMembersWithImages(int groupId, string searchString = "")
        {
            return _uow.GroupRepository.GetGroupMembersWithImages(groupId, searchString);
        }

  

        public GroupPropertyEntity GetGroupProperty(int groupId)
        {
            return _uow.GroupPropertyRepository.GetFirstOrDefault(x => x.GroupId == groupId);
        }

        public string GroupCountUser(int userId)
        {
            var userServices = new UserServices();
            if (userServices.GetUser(userId).Membership == "Administrator")
            {
                return TotalCount();
            }

            var user = userServices.GetUser(userId);
            if (user.GroupManagementEnabled == 1)
            {
                var userManagedGroups = userServices.GetUserGroupManagements(userId);
                return userManagedGroups.Count.ToString();
            }
            return TotalCount();
        }

        public int ImportCsv(string csvContents, int userId)
        {
            var importCounter = 0;
            using (var csv = new CsvReader(new StringReader(csvContents)))
            {
                csv.Configuration.RegisterClassMap<GroupCsvMap>();
                var records = csv.GetRecords<GroupEntity>();
                foreach (var group in records)
                {
                    if (AddGroup(group, userId).Success)
                        importCounter++;
                }
            }
            return importCounter;
        }

        public List<GroupWithImage> SearchGroups(string searchString = "")
        {
            return _uow.GroupRepository.GetGroupsWithImage(searchString);
        }

        public List<GroupWithImage> SearchGroupsForUser(int userId, string searchString = "")
        {
            var userServices = new UserServices();
            if (userServices.GetUser(userId).Membership == "Administrator")
                return SearchGroups(searchString);

            var user = userServices.GetUser(userId);
            if (user.GroupManagementEnabled == 0)
                return SearchGroups(searchString);

            var userManagedGroups = userServices.GetUserGroupManagements(userId);
            return
                userManagedGroups.Select(
                    groupManagement => _uow.GroupRepository.GetGroupWithImage(searchString, groupManagement.GroupId))
                    .Where(@group => @group != null)
                    .ToList();
        }

        public int StartGroupUnicast(int groupId, int userId, string clientIp)
        {
            var count = 0;
            foreach (var computer in GetGroupMembersWithImages(groupId))
            {
                if (new Unicast(computer.Id, "deploy", userId, clientIp).Start().Contains("Successfully"))
                    count++;
            }
            return count;
        }

        public string TotalCount()
        {
            return _uow.GroupRepository.Count();
        }

        public void UpdateAllSmartGroupsMembers()
        {
            var groups = SearchGroups();
            foreach (var group in groups.Where(x => x.Type == "smart"))
                UpdateSmartMembership(group.Id);
        }

        public ActionResultDTO UpdateGroup(GroupEntity group)
        {
            var g = GetGroup(group.Id);
            if (g == null)
                return new ActionResultDTO {ErrorMessage = "Group Not Found", Id = 0};

            var result = new ActionResultDTO();
            var validationResult = ValidateGroup(group, false);
            if (validationResult.Success)
            {
                _uow.GroupRepository.Update(group, group.Id);
                _uow.Save();
                result.Success = true;
                result.Id = group.Id;
            }
            else
            {
                result.ErrorMessage = validationResult.ErrorMessage;
            }

            return result;
        }

        public ActionResultDTO UpdateSmartMembership(int groupId)
        {
            var group = GetGroup(groupId);
            DeleteAllMembershipsForGroup(group.Id);
            var computers = new ComputerServices().SearchComputersByName(group.SmartCriteria,int.MaxValue, group.SmartType );
            var memberships =
                computers.Select(computer => new GroupMembershipEntity {GroupId = @group.Id, ComputerId = computer.Id})
                    .ToList();
            return new GroupMembershipServices().AddMembership(memberships);
        }

        private ValidationResultDTO ValidateGroup(GroupEntity group, bool isNewGroup)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(group.Name) || !group.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Group Name Is Not Valid";
                return validationResult;
            }

            if (isNewGroup)
            {
                if (_uow.GroupRepository.Exists(h => h.Name == group.Name))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "This Group Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalGroup = _uow.GroupRepository.GetById(group.Id);
                if (originalGroup.Name != group.Name)
                {
                    if (_uow.GroupRepository.Exists(h => h.Name == group.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Group Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}