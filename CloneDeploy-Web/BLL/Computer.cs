using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloneDeploy_Web.Models;
using CsvHelper;
using Helpers;
using Newtonsoft.Json;

namespace BLL
{

    public class Computer
    {
        //moved
        public static ActionResult AddComputer(CloneDeploy_Web.Models.Computer computer)
        {
            using (var uow = new DAL.UnitOfWork())
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
                        BLL.Group.UpdateAllSmartGroupsMembers();
                    }

                }

                return validationResult;
            }
        }

        //moved
        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerRepository.Count();
            }
        }

        //moved
        public static ApiDTO ComputerCountUser(int userId)
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

        //moved
        public static ActionResult DeleteComputer(int id)
        {
            var computer = GetComputer(id);
            if (computer == null)
            {
                var message = string.Format("Could Not Delete Computer With Id {0}.  The Computer Was not Found",id);
                Logger.Log(message);
                return new ActionResult() {Success = false, Message = message, ObjectId = id};
            }
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateComputer(computer, "delete");
                if (validationResult.Success)
                {
                    BLL.GroupMembership.DeleteComputerMemberships(computer.Id);
                    BLL.ComputerBootMenu.DeleteComputerBootMenus(computer.Id);
                    BLL.ComputerLog.DeleteComputerLogs(computer.Id);
                    uow.ComputerRepository.Delete(computer.Id);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = computer.Id;
                    validationResult.Object = JsonConvert.SerializeObject(computer);
                }
                return validationResult;
            }

        }

        //move not needed
        public static CloneDeploy_Web.Models.DistributionPoint GetDistributionPoint(CloneDeploy_Web.Models.Computer computer)
        {
            CloneDeploy_Web.Models.DistributionPoint dp = null;

            if (computer.RoomId != -1)
            {
                var room = BLL.Room.GetRoom(computer.RoomId);
                if(room != null)
                dp =
                    DistributionPoint.GetDistributionPoint(room.DistributionPointId);
            }
            else if (computer.BuildingId != -1)
            {
                var building = BLL.Building.GetBuilding(computer.BuildingId);
                if (building != null)
                dp =
                    DistributionPoint.GetDistributionPoint(building.DistributionPointId);
            }
            else if (computer.SiteId != -1)
            {
                var site = BLL.Site.GetSite(computer.SiteId);
                if (site != null)
                dp =
                    DistributionPoint.GetDistributionPoint(site.DistributionPointId);
            }
            
            if(dp == null)
                dp = DistributionPoint.GetPrimaryDistributionPoint();

            return dp;
        }

        //moved
        public static CloneDeploy_Web.Models.Computer GetComputer(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var computer = uow.ComputerRepository.GetById(computerId);
                if (computer != null)
                    computer.Image = BLL.Image.GetImage(computer.ImageId);
                return computer;
            }
        }

        //moved
        public static CloneDeploy_Web.Models.Computer GetComputerFromMac(string mac)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerRepository.GetFirstOrDefault(p => p.Mac == mac);
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.Computer> SearchComputersForUser(int userId,int limit, string searchString = "")
        {
            if(limit== 0) limit=Int32.MaxValue;
            if(BLL.User.GetUser(userId).Membership == "Administrator")
                return SearchComputers(searchString,limit);

            var listOfComputers = new List<CloneDeploy_Web.Models.Computer>();

            var userManagedGroups = BLL.UserGroupManagement.Get(userId);
            if (userManagedGroups.Count == 0)
                return SearchComputers(searchString,limit);
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

        //move not needed
        public static List<CloneDeploy_Web.Models.Computer> GetAll()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerRepository.Get();
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.Computer> SearchComputers(string searchString, int limit)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerRepository.Search(searchString, limit);
            }
        }

        //moved
        public static ActionResult UpdateComputer(CloneDeploy_Web.Models.Computer computer)
        {
            var existingcomputer = GetComputer(computer.Id);
            if (existingcomputer == null)
            {
                var message = string.Format("Could Not Update Computer With Id {0}.  The Computer Was not Found", computer.Id);
                Logger.Log(message);
                return new ActionResult() { Success = false, Message = message, ObjectId = computer.Id };
            }

            using (var uow = new DAL.UnitOfWork())
            {
                computer.Mac = Utility.FixMac(computer.Mac);
                var validationResult = ValidateComputer(computer, "update");
                if (validationResult.Success)
                {
                    uow.ComputerRepository.Update(computer, computer.Id);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = computer.Id;
                    validationResult.Object = JsonConvert.SerializeObject(computer);
                    if (validationResult.Success) BLL.Group.UpdateAllSmartGroupsMembers();
                }

                return validationResult;
            }
        }

        //move not needed
        public static IEnumerable<CloneDeploy_Web.Models.Computer> ComputersWithCustomBootMenu()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerRepository.Get(x => x.CustomBootEnabled == 1);

            } 
        }

        public static int ImportCsv(string path)
        {
            var importCounter = 0;
            using (var csv = new CsvReader(new StreamReader(path)))
            {
                csv.Configuration.RegisterClassMap<ComputerCsvMap>();
                var records = csv.GetRecords<CloneDeploy_Web.Models.Computer>();
                foreach (var computer in records)
                {
                    if (AddComputer(computer).Success)
                        importCounter++;
                }
            }
            return importCounter;
        }

        public static void ExportCsv(string path)
        {
            using (var csv = new CsvWriter(new StreamWriter(path)))
            {
                csv.Configuration.RegisterClassMap<ComputerCsvMap>();
                csv.WriteRecords(GetAll());
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.Computer> ComputersWithoutGroup(string searchString, int limit)
        {
            List<CloneDeploy_Web.Models.Computer> listOfComputers;
            using (var uow = new DAL.UnitOfWork())
            {
                listOfComputers = uow.ComputerRepository.GetComputersWithoutGroup(searchString, limit);
                
            }
            foreach (var computer in listOfComputers)
                computer.Image = BLL.Image.GetImage(computer.ImageId);

            return listOfComputers;
        }

        //move not needed
        public static ActionResult ValidateComputer(CloneDeploy_Web.Models.Computer computer, string type)
        {
            var validationResult = new ActionResult {Success = false};
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