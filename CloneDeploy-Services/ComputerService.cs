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

    public class ComputerService
    {
        private readonly UnitOfWork _uow;

        public ComputerService()
        {
            _uow = new UnitOfWork();
        }


        public ActionResultEntity AddComputer(ComputerEntity computer)
        {
            using (var uow = new UnitOfWork())
            {
                computer.Mac = Utility.FixMac(computer.Mac);
                var validationResult = ValidateComputer(computer, "new");
                if (validationResult.Success)
                {
                    uow.ComputerRepository.Insert(computer);
                    validationResult.Success = uow.Save();
                    if (validationResult.Success)
                    {
                        validationResult.ObjectId = computer.Id;
                        validationResult.Object = JsonConvert.SerializeObject(computer);
                        Group.UpdateAllSmartGroupsMembers();
                    }

                }

                return validationResult;
            }
        }

        public string TotalCount()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ComputerRepository.Count();
            }
        }

        public ApiDTO ComputerCountUser(int userId)
        {
            var apiDTO = new ApiDTO();
            if (User.GetUser(userId).Membership == "Administrator")
            {
                apiDTO.Value = TotalCount();
                return apiDTO;
            }

            var userManagedGroups = UserGroupManagement.Get(userId);

            //If count is zero image management is not being used return total count
            if (userManagedGroups.Count == 0)
            {
                apiDTO.Value = TotalCount();
                return apiDTO;
            }
            else
            {
                var computerCount = 0;
                foreach (var managedGroup in userManagedGroups)
                {
                    computerCount += Convert.ToInt32(GroupMembership.GetGroupMemberCount(managedGroup.GroupId));
                }
                apiDTO.Value = computerCount.ToString();
                return apiDTO;
            }
        }


        public ActionResultEntity DeleteComputer(int id)
        {
            var computer = GetComputer(id);
            if (computer == null)
            {
                var message = string.Format("Could Not Delete Computer With Id {0}.  The Computer Was not Found",id);
                Logger.Log(message);
                return new ActionResultEntity() { Success = false, Message = message, ObjectId = id };
            }
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateComputer(computer, "delete");
                if (validationResult.Success)
                {
                    GroupMembership.DeleteComputerMemberships(computer.Id);
                    ComputerBootMenu.DeleteComputerBootMenus(computer.Id);
                    ComputerLog.DeleteComputerLogs(computer.Id);
                    uow.ComputerRepository.Delete(computer.Id);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = computer.Id;
                    validationResult.Object = JsonConvert.SerializeObject(computer);
                }
                return validationResult;
            }

        }

        public DistributionPointEntity GetDistributionPoint(ComputerEntity computer)
        {
            DistributionPointEntity dp = null;

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
            using (var uow = new UnitOfWork())
            {
                var computer = uow.ComputerRepository.GetById(computerId);
                if (computer != null)
                    computer.Image = Image.GetImage(computer.ImageId);
                return computer;
            }
        }

        public ComputerEntity GetComputerFromMac(string mac)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ComputerRepository.GetFirstOrDefault(p => p.Mac == mac);
            }
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
            using (var uow = new UnitOfWork())
            {
                return uow.ComputerRepository.Get();
            }
        }

        public List<ComputerEntity> SearchComputers(string searchString, int limit)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ComputerRepository.Search(searchString, limit);
            }
        }

        public ActionResultEntity UpdateComputer(ComputerEntity computer)
        {
            var existingcomputer = GetComputer(computer.Id);
            if (existingcomputer == null)
            {
                var message = string.Format("Could Not Update Computer With Id {0}.  The Computer Was not Found", computer.Id);
                Logger.Log(message);
                return new ActionResultEntity() { Success = false, Message = message, ObjectId = computer.Id };
            }

            using (var uow = new UnitOfWork())
            {
                computer.Mac = Utility.FixMac(computer.Mac);
                var validationResult = ValidateComputer(computer, "update");
                if (validationResult.Success)
                {
                    uow.ComputerRepository.Update(computer, computer.Id);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = computer.Id;
                    validationResult.Object = JsonConvert.SerializeObject(computer);
                    if (validationResult.Success) Group.UpdateAllSmartGroupsMembers();
                }

                return validationResult;
            }
        }

        public IEnumerable<ComputerEntity> ComputersWithCustomBootMenu()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ComputerRepository.Get(x => x.CustomBootEnabled == 1);

            } 
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
                    if (AddComputer(computer).Success)
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

        public List<ComputerEntity> ComputersWithoutGroup(int limit, string searchString="")
        {
            List<ComputerEntity> listOfComputers;
            using (var uow = new UnitOfWork())
            {
                listOfComputers = uow.ComputerRepository.GetComputersWithoutGroup(searchString, limit);
                
            }
            foreach (var computer in listOfComputers)
                computer.Image = Image.GetImage(computer.ImageId);

            return listOfComputers;
        }

        public ActionResultEntity ValidateComputer(ComputerEntity computer, string type)
        {
            var validationResult = new ActionResultEntity { Success = false };
            if (type == "new" || type == "update")
            {
                if (string.IsNullOrEmpty(computer.Name) || computer.Name.Any(c => c == ' '))
                {
                    validationResult.Message = "Computer Name Is Not Valid";
                    return validationResult;
                }

                if (string.IsNullOrEmpty(computer.Mac) ||
                    !computer.Mac.All(c => char.IsLetterOrDigit(c) || c == ':' || c == '-')
                    && (computer.Mac.Length == 12 && !computer.Mac.All(char.IsLetterOrDigit)))
                {
                    validationResult.Message = "Computer Mac Is Not Valid";
                    return validationResult;
                }

                if (type == "new")
                {
                    using (var uow = new UnitOfWork())
                    {
                        if (uow.ComputerRepository.Exists(h => h.Name == computer.Name))
                        {
                            validationResult.Message = "A Computer With This Name Already Exists";
                            return validationResult;
                        }
                        if (uow.ComputerRepository.Exists(h => h.Mac == computer.Mac))
                        {
                            validationResult.Message = "A Computer With This MAC Already Exists";
                            return validationResult;
                        }
                    }
                }
                else
                {
                    using (var uow = new UnitOfWork())
                    {
                        var originalComputer = uow.ComputerRepository.GetById(computer.Id);
                        if (originalComputer.Name != computer.Name)
                        {
                            if (uow.ComputerRepository.Exists(h => h.Name == computer.Name))
                            {
                                validationResult.Message = "A Computer With This Name Already Exists";
                                return validationResult;
                            }
                        }
                        else if (originalComputer.Mac != computer.Mac)
                        {
                            if (uow.ComputerRepository.Exists(h => h.Mac == computer.Mac))
                            {
                                validationResult.Message = "A Computer With This MAC Already Exists";
                                return validationResult;
                            }
                        }
                    }
                }
            }

            if (type == "delete")
            {
                if (ActiveImagingTask.IsComputerActive(computer.Id))
                {
                    validationResult.Message = "A Computer Cannot Be Deleted While It Has An Active Task";
                    return validationResult;
                }

            }

            validationResult.Success = true;
            return validationResult;
        }
    }
}