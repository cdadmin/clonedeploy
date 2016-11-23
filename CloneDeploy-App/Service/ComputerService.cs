using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloneDeploy_App.DAL;
using CloneDeploy_App.Helpers;
using CloneDeploy_App.Models;
using CsvHelper;
using Newtonsoft.Json;
using DistributionPoint = CloneDeploy_App.BLL.DistributionPoint;

namespace CloneDeploy_App.Service
{

    public class ComputerService
    {
        private UnitOfWork _uow;

        public ComputerService()
        {
            _uow = new UnitOfWork();
        }

        public Models.ActionResult AddComputer(Models.Computer computer)
        {

            computer.Mac = Utility.FixMac(computer.Mac);
            var validationResult = ValidateComputer(computer, "new");
            if (validationResult.Success)
            {

                _uow.ComputerRepository.Insert(computer);
                validationResult.Success = _uow.Save();

                if (validationResult.Success)
                {
                    validationResult.ObjectId = computer.Id;
                    validationResult.Object = JsonConvert.SerializeObject(computer);
                    BLL.Group.UpdateAllSmartGroupsMembers();
                }

            }

            return validationResult;
        }

        public string TotalCount()
        {
            return _uow.ComputerRepository.Count();
        }

        public ApiDTO ComputerCountUser(int userId)
        {
            var apiDTO = new ApiDTO();
            if (BLL.User.GetUser(userId).Membership == "Administrator")
            {
                apiDTO.Value = TotalCount();
                return apiDTO;
            }

            var userManagedGroups = BLL.UserGroupManagement.Get(userId);

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
                    computerCount += Convert.ToInt32(BLL.GroupMembership.GetGroupMemberCount(managedGroup.GroupId));
                }
                apiDTO.Value = computerCount.ToString();
                return apiDTO;
            }
        }


        public Models.ActionResult DeleteComputer(int id)
        {
            var computer = GetComputer(id);
            if (computer == null)
            {
                var message = string.Format("Could Not Delete Computer With Id {0}.  The Computer Was not Found", id);
                Logger.Log(message);
                return new ActionResult() { Success = false, Message = message, ObjectId = id };
            }
           
                var validationResult = ValidateComputer(computer, "delete");
                if (validationResult.Success)
                {
                    BLL.GroupMembership.DeleteComputerMemberships(computer.Id);
                    BLL.ComputerBootMenu.DeleteComputerBootMenus(computer.Id);
                    BLL.ComputerLog.DeleteComputerLogs(computer.Id);
                    _uow.ComputerRepository.Delete(computer.Id);
                    validationResult.Success = _uow.Save();
                    validationResult.ObjectId = computer.Id;
                    validationResult.Object = JsonConvert.SerializeObject(computer);
                }
                return validationResult;
            

        }

        public Models.DistributionPoint GetDistributionPoint(Models.Computer computer)
        {
            Models.DistributionPoint dp = null;

            if (computer.RoomId != -1)
            {
                var room = BLL.Room.GetRoom(computer.RoomId);
                if (room != null)
                    dp =
                        BLL.DistributionPoint.GetDistributionPoint(room.DistributionPointId);
            }
            else if (computer.BuildingId != -1)
            {
                var building = BLL.Building.GetBuilding(computer.BuildingId);
                if (building != null)
                    dp =
                        BLL.DistributionPoint.GetDistributionPoint(building.DistributionPointId);
            }
            else if (computer.SiteId != -1)
            {
                var site = BLL.Site.GetSite(computer.SiteId);
                if (site != null)
                    dp =
                        BLL.DistributionPoint.GetDistributionPoint(site.DistributionPointId);
            }

            if (dp == null)
                dp = DistributionPoint.GetPrimaryDistributionPoint();

            return dp;
        }

        public Models.Computer GetComputer(int computerId)
        {
           
                var computer = _uow.ComputerRepository.GetById(computerId);
                if (computer != null)
                    computer.Image = BLL.Image.GetImage(computer.ImageId);
                return computer;
            
        }

        public Models.Computer GetComputerFromMac(string mac)
        {
            return _uow.ComputerRepository.GetFirstOrDefault(p => p.Mac == mac);
        }

        public List<Models.Computer> SearchComputersForUser(int userId, int limit, string searchString = "")
        {
            if (limit == 0) limit = Int32.MaxValue;
            if (BLL.User.GetUser(userId).Membership == "Administrator")
                return SearchComputers(searchString, limit);

            var listOfComputers = new List<Models.Computer>();

            var userManagedGroups = BLL.UserGroupManagement.Get(userId);
            if (userManagedGroups.Count == 0)
                return SearchComputers(searchString, limit);
            else
            {
                foreach (var managedGroup in userManagedGroups)
                {
                    listOfComputers.AddRange(BLL.Group.GetGroupMembers(managedGroup.GroupId, searchString));
                }

                foreach (var computer in listOfComputers)
                    computer.Image = BLL.Image.GetImage(computer.ImageId);

                return listOfComputers;
            }
        }

        public List<Models.Computer> GetAll()
        {

            return _uow.ComputerRepository.Get();

        }

        public  List<Models.Computer> SearchComputers(string searchString, int limit)
        {
           
                return _uow.ComputerRepository.Search(searchString, limit);
            
        }

        public Models.ActionResult UpdateComputer(Models.Computer computer)
        {
            var existingcomputer = GetComputer(computer.Id);
            if (existingcomputer == null)
            {
                var message = string.Format("Could Not Update Computer With Id {0}.  The Computer Was not Found",
                    computer.Id);
                Logger.Log(message);
                return new ActionResult() {Success = false, Message = message, ObjectId = computer.Id};
            }


            computer.Mac = Utility.FixMac(computer.Mac);
            var validationResult = ValidateComputer(computer, "update");
            if (validationResult.Success)
            {
                _uow.ComputerRepository.Update(computer, computer.Id);
                validationResult.Success = _uow.Save();
                validationResult.ObjectId = computer.Id;
                validationResult.Object = JsonConvert.SerializeObject(computer);
                if (validationResult.Success) BLL.Group.UpdateAllSmartGroupsMembers();
            }

            return validationResult;

        }

        public IEnumerable<Models.Computer> ComputersWithCustomBootMenu()
        {

            return _uow.ComputerRepository.Get(x => x.CustomBootEnabled == 1);


        }

        public int ImportCsv(string path)
        {
            var importCounter = 0;
            using (var csv = new CsvReader(new StreamReader(path)))
            {
                csv.Configuration.RegisterClassMap<Models.ComputerCsvMap>();
                var records = csv.GetRecords<Models.Computer>();
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
                csv.Configuration.RegisterClassMap<Models.ComputerCsvMap>();
                csv.WriteRecords(GetAll());
            }
        }

        public List<Models.Computer> ComputersWithoutGroup(int limit,string searchString="")
        {
            var listOfComputers = _uow.ComputerRepository.GetComputersWithoutGroup(searchString, limit);

            
            foreach (var computer in listOfComputers)
                computer.Image = BLL.Image.GetImage(computer.ImageId);

            return listOfComputers;
        }

        public static Models.ActionResult ValidateComputer(Models.Computer computer, string type)
        {
            var validationResult = new Models.ActionResult { Success = false };
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
                    using (var uow = new DAL.UnitOfWork())
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
                    using (var uow = new DAL.UnitOfWork())
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
                if (BLL.ActiveImagingTask.IsComputerActive(computer.Id))
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