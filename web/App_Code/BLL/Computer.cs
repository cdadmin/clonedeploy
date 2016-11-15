using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Helpers;

namespace BLL
{

    public class Computer
    {
        public static Models.ValidationResult AddComputer(Models.Computer computer)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                computer.Mac = Utility.FixMac(computer.Mac);
                var validationResult = ValidateComputer(computer, "new");
                if (validationResult.IsValid)
                {
                    
                    uow.ComputerRepository.Insert(computer);
                    validationResult.IsValid = uow.Save();
                    if (validationResult.IsValid)
                    {
                        validationResult.ObjectId = computer.Id;
                        BLL.Group.UpdateAllSmartGroupsMembers();
                    }

                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerRepository.Count();
            }
        }

        public static string ComputerCountUser(int userId)
        {
            if (BLL.User.GetUser(userId).Membership == "Administrator")
                return TotalCount();

            var userManagedGroups = BLL.UserGroupManagement.Get(userId);

            //If count is zero image management is not being used return total count
            if (userManagedGroups.Count == 0)
                return TotalCount();
            else
            {
                var computerCount = 0;
                foreach (var managedGroup in userManagedGroups)
                {
                    computerCount += Convert.ToInt32(BLL.GroupMembership.GetGroupMemberCount(managedGroup.GroupId));
                }
                return computerCount.ToString();
            }
        }


        public static Models.ValidationResult DeleteComputer(int id)
        {
            var computer = GetComputer(id);
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateComputer(computer, "delete");
                if (validationResult.IsValid)
                {
                    BLL.GroupMembership.DeleteComputerMemberships(computer.Id);
                    BLL.ComputerBootMenu.DeleteComputerBootMenus(computer.Id);
                    BLL.ComputerLog.DeleteComputerLogs(computer.Id);
                    uow.ComputerRepository.Delete(computer.Id);
                    validationResult.IsValid = uow.Save();
                }
                return validationResult;
            }

        }

        public static Models.DistributionPoint GetDistributionPoint(Models.Computer computer)
        {
            Models.DistributionPoint dp = null;

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

        public static Models.Computer GetComputer(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var computer = uow.ComputerRepository.GetById(computerId);
                if (computer != null)
                    computer.Image = BLL.Image.GetImage(computer.ImageId);
                return computer;
            }
        }

        public static Models.Computer GetComputerFromMac(string mac)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerRepository.GetFirstOrDefault(p => p.Mac == mac);
            }
        }

        public static List<Models.Computer> SearchComputersForUser(int userId,int limit, string searchString = "")
        {

            if(BLL.User.GetUser(userId).Membership == "Administrator")
                return SearchComputers(searchString,limit);

            var listOfComputers = new List<Models.Computer>();

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

        public static List<Models.Computer> GetAll()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerRepository.Get();
            }
        }

        public static List<Models.Computer> SearchComputers(string searchString, int limit)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerRepository.Search(searchString, limit);
            }
        }

        public static Models.ValidationResult UpdateComputer(Models.Computer computer)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                computer.Mac = Utility.FixMac(computer.Mac);
                var validationResult = ValidateComputer(computer, "update");
                if (validationResult.IsValid)
                {
                    uow.ComputerRepository.Update(computer, computer.Id);
                    validationResult.IsValid = uow.Save();
                    if (validationResult.IsValid) BLL.Group.UpdateAllSmartGroupsMembers();
                }

                return validationResult;
            }
        }

        public static IEnumerable<Models.Computer> ComputersWithCustomBootMenu()
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
                csv.Configuration.RegisterClassMap<Models.ComputerCsvMap>();
                var records = csv.GetRecords<Models.Computer>();
                foreach (var computer in records)
                {
                    if (AddComputer(computer).IsValid)
                        importCounter++;
                }
            }
            return importCounter;
        }

        public static void ExportCsv(string path)
        {
            using (var csv = new CsvWriter(new StreamWriter(path)))
            {
                csv.Configuration.RegisterClassMap<Models.ComputerCsvMap>();
                csv.WriteRecords(GetAll());
            }
        }

        public static List<Models.Computer> ComputersWithoutGroup(string searchString, int limit)
        {
            List<Models.Computer> listOfComputers;
            using (var uow = new DAL.UnitOfWork())
            {
                listOfComputers = uow.ComputerRepository.GetComputersWithoutGroup(searchString, limit);
                
            }
            foreach (var computer in listOfComputers)
                computer.Image = BLL.Image.GetImage(computer.ImageId);

            return listOfComputers;
        }

        public static Models.ValidationResult ValidateComputer(Models.Computer computer, string type)
        {
            var validationResult = new Models.ValidationResult {IsValid = false};
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
                        if (uow.ComputerRepository.Exists(h => h.Name == computer.Name || h.Mac == computer.Mac))
                        {
                            validationResult.Message = "This Computer Already Exists";
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
                                validationResult.Message = "This Computer Already Exists";
                                return validationResult;
                            }
                        }
                        else if (originalComputer.Mac != computer.Mac)
                        {
                            if (uow.ComputerRepository.Exists(h => h.Mac == computer.Mac))
                            {
                                validationResult.Message = "This Computer Already Exists";
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

            validationResult.IsValid = true;
            return validationResult;
        }

      

       
    }
}