using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Helpers;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CsvHelper;
using Newtonsoft.Json;

namespace CloneDeploy_Services
{
    public class ComputerServices
    {
        private readonly UnitOfWork _uow;

        public ComputerServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddComputer(ComputerEntity computer)
        {
            var actionResult = new ActionResultDTO();
            computer.Mac = Utility.FixMac(computer.Mac);
            var validationResult = ValidateComputer(computer, "new");
            if (validationResult.Success)
            {
                _uow.ComputerRepository.Insert(computer);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = computer.Id;
                Group.UpdateAllSmartGroupsMembers();
            }
            return actionResult;
        }

        public string TotalCount()
        {

            return _uow.ComputerRepository.Count();
        }

        public string ComputerCountUser(int userId)
        {
            if (User.GetUser(userId).Membership == "Administrator")
            {
                return TotalCount();
            }

            var userManagedGroups = UserGroupManagement.Get(userId);

            //If count is zero image management is not being used return total count
            if (userManagedGroups.Count == 0)
            {
                return TotalCount();
            }
            else
            {
                var computerCount = 0;
                foreach (var managedGroup in userManagedGroups)
                {
                    computerCount += Convert.ToInt32(GroupMembership.GetGroupMemberCount(managedGroup.GroupId));
                }
                return computerCount.ToString();
            }
        }


        public ActionResultDTO DeleteComputer(int id)
        {           
            var computer = GetComputer(id);
            if (computer == null) return new ActionResultDTO() {ErrorMessage = "Computer Not Found", Id = 0};

            var validationResult = ValidateComputer(computer, "delete");
            var result = new ActionResultDTO();
            if (validationResult.Success)
            {
                GroupMembership.DeleteComputerMemberships(computer.Id);
                DeleteComputerBootMenus(computer.Id);
                ComputerLog.DeleteComputerLogs(computer.Id);
                _uow.ComputerRepository.Delete(computer.Id);
                _uow.Save();
                result.Success = true;
                result.Id = computer.Id;
            }
            else
            {
                result.ErrorMessage = validationResult.ErrorMessage;
            }
            return result;
        }

        public DistributionPointEntity GetDistributionPoint(int computerId)
        {
            DistributionPointEntity dp = null;

            var computer = GetComputer(computerId);
            if (computer.RoomId != -1)
            {
                var room = Room.GetRoom(computer.RoomId);
                if(room != null)
                dp =
                    DistributionPoint.GetDistributionPoint(room.DistributionPointId);
            }
            else if (computer.BuildingId != -1)
            {
                var building = Building.GetBuilding(computer.BuildingId);
                if (building != null)
                dp =
                    DistributionPoint.GetDistributionPoint(building.DistributionPointId);
            }
            else if (computer.SiteId != -1)
            {
                var site = Site.GetSite(computer.SiteId);
                if (site != null)
                dp =
                    DistributionPoint.GetDistributionPoint(site.DistributionPointId);
            }
            
            if(dp == null)
                dp = DistributionPoint.GetPrimaryDistributionPoint();

            return dp;
        }

        public ComputerEntity GetComputer(int computerId)
        {
            var computer = _uow.ComputerRepository.GetById(computerId);
            if (computer != null)
                computer.Image = Image.GetImage(computer.ImageId);
            return computer;
        }

        public ComputerEntity GetComputerFromMac(string mac)
        {

            return _uow.ComputerRepository.GetFirstOrDefault(p => p.Mac == mac);

        }

        public List<ComputerEntity> SearchComputersForUser(int userId, int limit, string searchString = "")
        {
            if(limit== 0) limit=Int32.MaxValue;
            if(User.GetUser(userId).Membership == "Administrator")
                return SearchComputers(searchString,limit);

            var listOfComputers = new List<ComputerEntity>();

            var userManagedGroups = UserGroupManagement.Get(userId);
            if (userManagedGroups.Count == 0)
                return SearchComputers(searchString,limit);
            else
            {
                foreach (var managedGroup in userManagedGroups)
                {
                    listOfComputers.AddRange(Group.GetGroupMembers(managedGroup.GroupId, searchString));
                }

                foreach (var computer in listOfComputers)
                    computer.Image = Image.GetImage(computer.ImageId);

                return listOfComputers;
            }
        }

        public List<ComputerEntity> GetAll()
        {

            return _uow.ComputerRepository.Get();
        }

        public List<ComputerEntity> SearchComputers(string searchString, int limit)
        {

            return _uow.ComputerRepository.Search(searchString, limit);
        }

        public ActionResultDTO UpdateComputer(ComputerEntity computer)
        {
            var existingcomputer = GetComputer(computer.Id);
            if (existingcomputer == null)
                return new ActionResultDTO() {ErrorMessage = "Computer Not Found", Id = 0};

            computer.Mac = Utility.FixMac(computer.Mac);
            var actionResult = new ActionResultDTO();
            var validationResult = ValidateComputer(computer, "update");
            if (validationResult.Success)
            {
                _uow.ComputerRepository.Update(computer, computer.Id);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = computer.Id;
                Group.UpdateAllSmartGroupsMembers();

            }

            return actionResult;
        }

        public IEnumerable<ComputerEntity> ComputersWithCustomBootMenu()
        {

            return _uow.ComputerRepository.Get(x => x.CustomBootEnabled == 1);


        }

        public int ImportCsv(string path)
        {
            var importCounter = 0;
            using (var csv = new CsvReader(new StreamReader(path)))
            {
                csv.Configuration.RegisterClassMap<ComputerCsvMap>();
                var records = csv.GetRecords<ComputerEntity>();
                foreach (var computer in records)
                {
                    if (AddComputer(computer).Success )
                        importCounter++;
                }
            }
            return importCounter;
        }

        public void ExportCsv(string path)
        {
            using (var csv = new CsvWriter(new StreamWriter(path)))
            {
                csv.Configuration.RegisterClassMap<ComputerCsvMap>();
                csv.WriteRecords(GetAll());
            }
        }

        public List<ComputerEntity> ComputersWithoutGroup(int limit, string searchString = "")
        {
            var listOfComputers = _uow.ComputerRepository.GetComputersWithoutGroup(searchString, limit);
            foreach (var computer in listOfComputers)
                computer.Image = Image.GetImage(computer.ImageId);

            return listOfComputers;
        }

        public ValidationResultDTO ValidateComputer(ComputerEntity computer, string type)
        {
            var validationResult = new ValidationResultDTO() { Success = false };
            if (type == "new" || type == "update")
            {
                if (string.IsNullOrEmpty(computer.Name) || computer.Name.Any(c => c == ' '))
                {
                    validationResult.ErrorMessage = "Computer Name Is Not Valid";
                    return validationResult;
                }

                if (string.IsNullOrEmpty(computer.Mac) ||
                    !computer.Mac.All(c => char.IsLetterOrDigit(c) || c == ':' || c == '-')
                    && (computer.Mac.Length == 12 && !computer.Mac.All(char.IsLetterOrDigit)))
                {
                    validationResult.ErrorMessage = "Computer Mac Is Not Valid";
                    return validationResult;
                }

                if (type == "new")
                {
                    
                        if (_uow.ComputerRepository.Exists(h => h.Name == computer.Name))
                        {
                            validationResult.ErrorMessage = "A Computer With This Name Already Exists";
                            return validationResult;
                        }
                        if (_uow.ComputerRepository.Exists(h => h.Mac == computer.Mac))
                        {
                            validationResult.ErrorMessage = "A Computer With This MAC Already Exists";
                            return validationResult;
                        }
                    
                }
                else
                {
                 
                        var originalComputer = _uow.ComputerRepository.GetById(computer.Id);
                        if (originalComputer.Name != computer.Name)
                        {
                            if (_uow.ComputerRepository.Exists(h => h.Name == computer.Name))
                            {
                                validationResult.ErrorMessage = "A Computer With This Name Already Exists";
                                return validationResult;
                            }
                        }
                        else if (originalComputer.Mac != computer.Mac)
                        {
                            if (_uow.ComputerRepository.Exists(h => h.Mac == computer.Mac))
                            {
                                validationResult.ErrorMessage = "A Computer With This MAC Already Exists";
                                return validationResult;
                            }
                        }
                    
                }
            }

            if (type == "delete")
            {
                if (IsComputerActive(computer.Id))
                {
                    validationResult.ErrorMessage = "A Computer Cannot Be Deleted While It Has An Active Task";
                    return validationResult;
                }

            }

            validationResult.Success = true;
            return validationResult;
        }

        public bool IsComputerActive(int computerId)
        {
            return _uow.ActiveImagingTaskRepository.Exists(a => a.ComputerId == computerId);
        }

        public ActiveImagingTaskEntity GetTaskForComputer(int computerId)
        {
           
                return _uow.ActiveImagingTaskRepository.GetFirstOrDefault(x => x.ComputerId == computerId);
            
        }

        public string GetQueuePosition(int computerId)
        {
            var computerTask = GetTaskForComputer(computerId);
           
                return
                    _uow.ActiveImagingTaskRepository.Count(
                        x => x.Status == "2" && x.QueuePosition < computerTask.QueuePosition);
            
        }

        public ComputerBootMenuEntity GetComputerBootMenu(int computerId)
        {
            
                return _uow.ComputerBootMenuRepository.GetFirstOrDefault(p => p.ComputerId == computerId);
            
        }

        public ActionResultDTO DeleteComputerBootMenus(int computerId)
        {
            var actionResult = new ActionResultDTO();
          

            using (var uow = new UnitOfWork())
            {
                uow.ComputerBootMenuRepository.DeleteRange(x => x.ComputerId == computerId);
                uow.Save();
                actionResult.Success = true;
                actionResult.Id = computerId;
            }
            return actionResult;

        }
    }
}