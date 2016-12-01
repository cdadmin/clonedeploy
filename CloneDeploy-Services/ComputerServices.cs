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
                GroupServices.UpdateAllSmartGroupsMembers();
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }
            return actionResult;
        }

        public string TotalCount()
        {

            return _uow.ComputerRepository.Count();
        }

        public string ComputerCountUser(int userId)
        {
            if (UserServices.GetUser(userId).Membership == "Administrator")
            {
                return TotalCount();
            }

            var userManagedGroups = UserGroupManagementServices.Get(userId);

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
                    computerCount += Convert.ToInt32(GroupMembershipServices.GetGroupMemberCount(managedGroup.GroupId));
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
                GroupMembershipServices.DeleteComputerMemberships(computer.Id);
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
                var room = RoomServices.GetRoom(computer.RoomId);
                if(room != null)
                dp =
                    DistributionPointServices.GetDistributionPoint(room.DistributionPointId);
            }
            else if (computer.BuildingId != -1)
            {
                var building = BuildingServices.GetBuilding(computer.BuildingId);
                if (building != null)
                dp =
                    DistributionPointServices.GetDistributionPoint(building.DistributionPointId);
            }
            else if (computer.SiteId != -1)
            {
                var site = SiteServices.GetSite(computer.SiteId);
                if (site != null)
                dp =
                    DistributionPointServices.GetDistributionPoint(site.DistributionPointId);
            }
            
            if(dp == null)
                dp = DistributionPointServices.GetPrimaryDistributionPoint();

            return dp;
        }

        public ComputerEntity GetComputer(int computerId)
        {
            var computer = _uow.ComputerRepository.GetById(computerId);
            if (computer != null)
                computer.Image = ImageServices.GetImage(computer.ImageId);
            return computer;
        }

        public ComputerEntity GetComputerFromMac(string mac)
        {

            return _uow.ComputerRepository.GetFirstOrDefault(p => p.Mac == mac);

        }

        public List<ComputerEntity> SearchComputersForUser(int userId, int limit, string searchString = "")
        {
            if(limit== 0) limit=Int32.MaxValue;
            if(UserServices.GetUser(userId).Membership == "Administrator")
                return SearchComputers(searchString,limit);

            var listOfComputers = new List<ComputerEntity>();

            var userManagedGroups = UserGroupManagementServices.Get(userId);
            if (userManagedGroups.Count == 0)
                return SearchComputers(searchString,limit);
            else
            {
                foreach (var managedGroup in userManagedGroups)
                {
                    listOfComputers.AddRange(GroupServices.GetGroupMembers(managedGroup.GroupId, searchString));
                }

                foreach (var computer in listOfComputers)
                    computer.Image = ImageServices.GetImage(computer.ImageId);

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
                GroupServices.UpdateAllSmartGroupsMembers();

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
                computer.Image = ImageServices.GetImage(computer.ImageId);

            return listOfComputers;
        }

        private ValidationResultDTO ValidateComputer(ComputerEntity computer, string type)
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

        public List<GroupMembershipEntity> GetAllComputerMemberships(int computerId)
        {

            return _uow.GroupMembershipRepository.Get(x => x.ComputerId == computerId);

        }

        public bool CreateBootFiles(ComputerEntity computer)
        {
            if (new ComputerServices().IsComputerActive(computer.Id)) return false; //Files Will Be Processed When task is done
            var bootMenu = new ComputerServices().GetComputerBootMenu(computer.Id);
            if (bootMenu == null) return false;
            var pxeMac = Utility.MacToPxeMac(computer.Mac);
            string path;

            if (Settings.ProxyDhcp == "Yes")
            {
                List<Tuple<string, string, string>> list = new List<Tuple<string, string, string>>
                {
                    Tuple.Create("bios", "", bootMenu.BiosMenu),
                    Tuple.Create("bios", ".ipxe", bootMenu.BiosMenu),
                    Tuple.Create("efi32", "",bootMenu.Efi32Menu),
                    Tuple.Create("efi32", ".ipxe", bootMenu.Efi32Menu),
                    Tuple.Create("efi64", "" , bootMenu.Efi64Menu),
                    Tuple.Create("efi64", ".ipxe", bootMenu.Efi64Menu),
                    Tuple.Create("efi64", ".cfg", bootMenu.Efi64Menu)
                };
                foreach (var tuple in list)
                {
                    path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar + tuple.Item1 +
                           Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeMac +
                           tuple.Item2;

                    if (!string.IsNullOrEmpty(tuple.Item3))
                        new FileOps().WritePath(path, tuple.Item3);


                }
            }
            else
            {
                var mode = Settings.PxeMode;
                path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                       pxeMac;

                if (mode.Contains("ipxe"))
                    path += ".ipxe";
                else if (mode.Contains("grub"))
                    path += ".cfg";

                if (!string.IsNullOrEmpty(bootMenu.BiosMenu))
                    new FileOps().WritePath(path, bootMenu.BiosMenu);

            }
            return true;
        }

        public string GetComputerNonProxyPath(ComputerEntity computer, bool isActiveOrCustom)
        {
            var mode = Settings.PxeMode;
            var pxeComputerMac = Utility.MacToPxeMac(computer.Mac);
            string path;

            var fileName = isActiveOrCustom ? pxeComputerMac : "default";

            if (mode.Contains("ipxe"))
                path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                       fileName + ".ipxe";
            else if (mode.Contains("grub"))
            {

                if (isActiveOrCustom)
                {
                    path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                      pxeComputerMac + ".cfg";
                }
                else
                {
                    path = Settings.TftpPath + "grub" + Path.DirectorySeparatorChar
                      + "grub.cfg";
                }

            }
            else
                path = Settings.TftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                       fileName;

            return path;
        }

        public string GetComputerProxyPath(ComputerEntity computer, bool isActiveOrCustom, string proxyType)
        {
            var pxeComputerMac = Utility.MacToPxeMac(computer.Mac);
            string path = null;


            var biosFile = Settings.ProxyBiosFile;
            var efi32File = Settings.ProxyEfi32File;
            var efi64File = Settings.ProxyEfi64File;

            var fileName = isActiveOrCustom ? pxeComputerMac : "default";
            switch (proxyType)
            {
                case "bios":
                    if (biosFile.Contains("ipxe"))
                    {
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName + ".ipxe";
                    }
                    else
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName;
                    break;
                case "efi32":
                    if (efi32File.Contains("ipxe"))
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName + ".ipxe";
                    else
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName;
                    break;
                case "efi64":
                    if (efi64File.Contains("ipxe"))
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName + ".ipxe";
                    else if (efi64File.Contains("grub"))
                    {
                        if (isActiveOrCustom)
                        {
                            path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                              proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                              Path.DirectorySeparatorChar + pxeComputerMac + ".cfg";

                        }
                        else
                        {
                            path = Settings.TftpPath + "grub" +
                                  Path.DirectorySeparatorChar + "grub.cfg";
                        }
                    }
                    else
                        path = Settings.TftpPath + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName;
                    break;
            }

            return path;
        }

        public List<ComputerLogEntity> SearchComputerLogs(int computerId)
        {

            return _uow.ComputerLogRepository.Get(x => x.ComputerId == computerId, orderBy: (q => q.OrderByDescending(x => x.LogTime)));

        }

        public ActionResultDTO DeleteComputerLogs(int computerId)
        {
            var computer = GetComputerLog(computerId);
            if (computer == null)
                return new ActionResultDTO() { ErrorMessage = "Computer Not Found", Id = 0 };

            var actionResult = new ActionResultDTO();

            _uow.ComputerLogRepository.DeleteRange(x => x.ComputerId == computerId);
            _uow.Save();
            actionResult.Success = true;
            actionResult.Id = computerId;



            return actionResult;
        }

        public List<ComputerMunkiEntity> GetMunkiTemplates(int computerId)
        {

            return _uow.ComputerMunkiRepository.Get(x => x.ComputerId == computerId);

        }

        public ActionResultDTO DeleteMunkiTemplates(int computerId)
        {
            var existingcomputer = new ComputerServices().GetComputer(computerId);
            if (existingcomputer == null)
                return new ActionResultDTO() { ErrorMessage = "Computer Not Found", Id = 0 };
            var actionResult = new ActionResultDTO();

            _uow.ComputerMunkiRepository.DeleteRange(x => x.ComputerId == computerId);
            _uow.Save();
            actionResult.Success = true;
            actionResult.Id = computerId;
            return actionResult;

        }

        public ComputerProxyReservationEntity GetComputerProxyReservation(int computerId)
        {

            return _uow.ComputerProxyRepository.GetFirstOrDefault(p => p.ComputerId == computerId);
        }

        public bool ToggleProxyReservation(int computerId, bool enable)
        {
            var computerServices = new ComputerServices();
            var computer = computerServices.GetComputer(computerId);
            computer.ProxyReservation = Convert.ToInt16(enable);
            computerServices.UpdateComputer(computer);
            return true;
        }

        public bool ToggleComputerBootMenu(int computerId, bool enable)
        {
            var computerServices = new ComputerServices();
            var computer = computerServices.GetComputer(computerId);
            computer.CustomBootEnabled = Convert.ToInt16(enable);
            computerServices.UpdateComputer(computer);

            if (enable)
                CreateBootFiles(computer);

            return true;
        }
    }
}