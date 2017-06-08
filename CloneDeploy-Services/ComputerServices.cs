using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloneDeploy_ApiCalls;
using CloneDeploy_Common;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;
using CsvHelper;

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
            computer.Mac = StringManipulationServices.FixMac(computer.Mac);
            var validationResult = ValidateComputer(computer, "new");
            if (validationResult.Success)
            {
                _uow.ComputerRepository.Insert(computer);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = computer.Id;
            }
            else
            {
                actionResult.Success = false;
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }
            return actionResult;
        }

        public void AddComputerToSmartGroups(ComputerEntity computer)
        {
            var groups = new GroupServices().SearchGroups();
            foreach (var group in groups.Where(x => x.Type == "smart"))
            {
                var computers =
                    SearchComputersByName(group.SmartCriteria, int.MaxValue).Where(x => x.Name == computer.Name);
                var memberships =
                    computers.Select(comp => new GroupMembershipEntity {GroupId = @group.Id, ComputerId = comp.Id})
                        .ToList();
                new GroupMembershipServices().AddMembership(memberships);
            }
        }

        public string ComputerCountUser(int userId)
        {
            var userServices = new UserServices();
            if (userServices.GetUser(userId).Membership == "Administrator")
            {
                return TotalCount();
            }

            var userManagedGroups = userServices.GetUserGroupManagements(userId);

            //If count is zero image management is not being used return total count
            if (userManagedGroups.Count == 0)
            {
                return TotalCount();
            }
            var computerCount = 0;
            foreach (var managedGroup in userManagedGroups)
            {
                computerCount += Convert.ToInt32(new GroupServices().GetGroupMemberCount(managedGroup.GroupId));
            }
            return computerCount.ToString();
        }

        public IEnumerable<ComputerEntity> ComputersWithCustomBootMenu()
        {
            return _uow.ComputerRepository.Get(x => x.CustomBootEnabled == 1);
        }

        public List<ComputerEntity> ComputersWithoutGroup(int limit, string searchString = "")
        {
            var listOfComputers = _uow.ComputerRepository.GetComputersWithoutGroup(searchString, limit);
            return listOfComputers;
        }

        public bool CreateBootFiles(int id)
        {
            var computer = GetComputer(id);
            if (new ComputerServices().IsComputerActive(computer.Id))
                return false; //Files Will Be Processed When task is done
            var bootMenu = new ComputerServices().GetComputerBootMenu(computer.Id);
            if (bootMenu == null) return false;
            var pxeMac = StringManipulationServices.MacToPxeMac(computer.Mac);
            string path;

            if (SettingServices.GetSettingValue(SettingStrings.ProxyDhcp) == "Yes")
            {
                var list = new List<Tuple<string, string, string>>
                {
                    Tuple.Create("bios", "", bootMenu.BiosMenu),
                    Tuple.Create("bios", ".ipxe", bootMenu.BiosMenu),
                    Tuple.Create("efi32", "", bootMenu.Efi32Menu),
                    Tuple.Create("efi32", ".ipxe", bootMenu.Efi32Menu),
                    Tuple.Create("efi64", "", bootMenu.Efi64Menu),
                    Tuple.Create("efi64", ".ipxe", bootMenu.Efi64Menu),
                    Tuple.Create("efi64", ".cfg", bootMenu.Efi64Menu)
                };

                if (SettingServices.ServerIsNotClustered)
                {
                    foreach (var tuple in list)
                    {
                        path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "proxy" + Path.DirectorySeparatorChar + tuple.Item1 +
                               Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeMac +
                               tuple.Item2;

                        if (!string.IsNullOrEmpty(tuple.Item3))
                            new FileOpsServices().WritePath(path, tuple.Item3);
                    }
                }
                else
                {
                    if (SettingServices.TftpServerRole)
                    {
                        foreach (var tuple in list)
                        {
                            path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "proxy" + Path.DirectorySeparatorChar + tuple.Item1 +
                                   Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeMac +
                                   tuple.Item2;

                            if (!string.IsNullOrEmpty(tuple.Item3))
                                new FileOpsServices().WritePath(path, tuple.Item3);
                        }
                    }

                    var secondaryServers =
                        new SecondaryServerServices().SearchSecondaryServers().Where(x => x.TftpRole == 1);
                    foreach (var server in secondaryServers)
                    {
                        var tftpPath =
                            new APICall(new SecondaryServerServices().GetToken(server.Name))
                                .SettingApi.GetSetting("Tftp Path").Value;
                        foreach (var tuple in list)
                        {
                            path = tftpPath + "proxy" + Path.DirectorySeparatorChar + tuple.Item1 +
                                   Path.DirectorySeparatorChar + "pxelinux.cfg" + Path.DirectorySeparatorChar + pxeMac +
                                   tuple.Item2;

                            new APICall(new SecondaryServerServices().GetToken(server.Name))
                                .ServiceAccountApi.WriteTftpFile(new TftpFileDTO
                                {
                                    Path = path,
                                    Contents = tuple.Item3
                                });
                        }
                    }
                }
            }
            else
            {
                var mode = SettingServices.GetSettingValue(SettingStrings.PxeMode);
                path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                       pxeMac;

                if (SettingServices.ServerIsNotClustered)
                {
                    if (mode.Contains("ipxe"))
                        path += ".ipxe";
                    else if (mode.Contains("grub"))
                        path += ".cfg";

                    if (!string.IsNullOrEmpty(bootMenu.BiosMenu))
                        new FileOpsServices().WritePath(path, bootMenu.BiosMenu);
                }
                else
                {
                    if (SettingServices.TftpServerRole)
                    {
                        path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeMac;
                        if (mode.Contains("ipxe"))
                            path += ".ipxe";
                        else if (mode.Contains("grub"))
                            path += ".cfg";

                        if (!string.IsNullOrEmpty(bootMenu.BiosMenu))
                            new FileOpsServices().WritePath(path, bootMenu.BiosMenu);
                    }
                    var secondaryServers =
                        new SecondaryServerServices().SearchSecondaryServers().Where(x => x.TftpRole == 1);
                    foreach (var server in secondaryServers)
                    {
                        var tftpPath =
                            new APICall(new SecondaryServerServices().GetToken(server.Name))
                                .SettingApi.GetSetting("Tftp Path").Value;
                        path = tftpPath + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                               pxeMac;

                        if (mode.Contains("ipxe"))
                            path += ".ipxe";
                        else if (mode.Contains("grub"))
                            path += ".cfg";

                        new APICall(new SecondaryServerServices().GetToken(server.Name))
                            .ServiceAccountApi.WriteTftpFile(new TftpFileDTO
                            {
                                Path = path,
                                Contents = bootMenu.BiosMenu
                            });
                    }
                }
            }
            return true;
        }


        public ActionResultDTO DeleteComputer(int id)
        {
            var computer = GetComputer(id);
            if (computer == null) return new ActionResultDTO {ErrorMessage = "Computer Not Found", Id = 0};

            var validationResult = ValidateComputer(computer, "delete");
            var result = new ActionResultDTO();
            if (validationResult.Success)
            {
                DeleteComputerMemberships(computer.Id);
                DeleteComputerBootMenus(computer.Id);
                DeleteComputerLogs(computer.Id);
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

        public ActionResultDTO DeleteComputerLogs(int computerId)
        {
            var computer = GetComputer(computerId);
            if (computer == null)
                return new ActionResultDTO {ErrorMessage = "Computer Not Found", Id = 0};

            var actionResult = new ActionResultDTO();

            _uow.ComputerLogRepository.DeleteRange(x => x.ComputerId == computerId);
            _uow.Save();
            actionResult.Success = true;
            actionResult.Id = computerId;


            return actionResult;
        }

        public bool DeleteComputerMemberships(int computerId)
        {
            _uow.GroupMembershipRepository.DeleteRange(x => x.ComputerId == computerId);
            _uow.Save();
            return true;
        }

        public ActionResultDTO DeleteMunkiTemplates(int computerId)
        {
            var existingcomputer = new ComputerServices().GetComputer(computerId);
            if (existingcomputer == null)
                return new ActionResultDTO {ErrorMessage = "Computer Not Found", Id = 0};
            var actionResult = new ActionResultDTO();

            _uow.ComputerMunkiRepository.DeleteRange(x => x.ComputerId == computerId);
            _uow.Save();
            actionResult.Success = true;
            actionResult.Id = computerId;
            return actionResult;
        }

        public void ExportCsv(string path)
        {
            using (var csv = new CsvWriter(new StreamWriter(path)))
            {
                csv.Configuration.RegisterClassMap<ComputerCsvMap>();
                csv.WriteRecords(GetAll());
            }
        }

        public List<ComputerEntity> GetAll()
        {
            return _uow.ComputerRepository.Get();
        }

        public ComputerWithImage GetWithImage(int computerId)
        {
            return _uow.ComputerRepository.GetComputerWithImage(computerId);
        }

        public List<GroupMembershipEntity> GetAllComputerMemberships(int computerId)
        {
            return _uow.GroupMembershipRepository.Get(x => x.ComputerId == computerId);
        }

        public ClusterGroupEntity GetClusterGroup(int computerId)
        {
            ClusterGroupEntity cg = null;
            var cgServices = new ClusterGroupServices();
            var computer = GetComputer(computerId);

            if (computer.ClusterGroupId != -1)
            {
                cg = cgServices.GetClusterGroup(computer.ClusterGroupId);
                return cg ?? cgServices.GetDefaultClusterGroup();
            }
            if (computer.RoomId != -1)
            {
                var room = new RoomServices().GetRoom(computer.RoomId);
                if (room != null)
                {
                    if (room.ClusterGroupId != -1)
                    {
                        cg =
                            cgServices.GetClusterGroup(room.ClusterGroupId);
                        return cg ?? cgServices.GetDefaultClusterGroup();
                    }
                }
            }
            if (computer.BuildingId != -1)
            {
                var building = new BuildingServices().GetBuilding(computer.BuildingId);
                if (building != null)
                {
                    if (building.ClusterGroupId != -1)
                    {
                        cg =
                            cgServices.GetClusterGroup(building.ClusterGroupId);
                        return cg ?? cgServices.GetDefaultClusterGroup();
                    }
                }
            }
            if (computer.SiteId != -1)
            {
                var site = new SiteServices().GetSite(computer.SiteId);
                if (site != null)
                {
                    if (site.ClusterGroupId != -1)
                    {
                        cg =
                            cgServices.GetClusterGroup(site.ClusterGroupId);
                        return cg ?? cgServices.GetDefaultClusterGroup();
                    }
                }
            }

            return cgServices.GetDefaultClusterGroup();
        }


        public ComputerEntity GetComputer(int computerId)
        {
            var computer = _uow.ComputerRepository.GetById(computerId);
            return computer;
        }

        public ComputerBootMenuEntity GetComputerBootMenu(int computerId)
        {
            return _uow.ComputerBootMenuRepository.GetFirstOrDefault(p => p.ComputerId == computerId);
        }

        public ComputerEntity GetComputerFromMac(string mac)
        {
            return _uow.ComputerRepository.GetFirstOrDefault(p => p.Mac == mac);
        }

        public ComputerEntity GetComputerFromName(string name)
        {
            return _uow.ComputerRepository.GetFirstOrDefault(p => p.Name == name);
        }

        public ComputerEntity GetComputerFromClientIdentifier(string clientIdentifier)
        {
            return _uow.ComputerRepository.GetFirstOrDefault(p => p.ClientIdentifier == clientIdentifier);
        }

        public List<AuditLogEntity> GetComputerAuditLogs(int computerId,int limit)
        {
            if (limit == 0) limit = int.MaxValue;
            return _uow.AuditLogRepository.Get(x => x.ObjectType == "Computer" && x.ObjectId == computerId).OrderByDescending(x => x.Id).Take(limit).ToList();
        }

        public string GetComputerNonProxyPath(int computerId, bool isActiveOrCustom)
        {
            var computer = GetComputer(computerId);
            var mode = SettingServices.GetSettingValue(SettingStrings.PxeMode);
            var pxeComputerMac = StringManipulationServices.MacToPxeMac(computer.Mac);
            string path;

            var fileName = isActiveOrCustom ? pxeComputerMac : "default";

            if (mode.Contains("ipxe"))
                path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                       fileName + ".ipxe";
            else if (mode.Contains("grub"))
            {
                if (isActiveOrCustom)
                {
                    path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                           pxeComputerMac + ".cfg";
                }
                else
                {
                    path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "grub" + Path.DirectorySeparatorChar
                           + "grub.cfg";
                }
            }
            else
                path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "pxelinux.cfg" + Path.DirectorySeparatorChar +
                       fileName;

            return path;
        }

        public string GetComputerProxyPath(int computerId, bool isActiveOrCustom, string proxyType)
        {
            var computer = GetComputer(computerId);
            var pxeComputerMac = StringManipulationServices.MacToPxeMac(computer.Mac);
            string path = null;


            var biosFile = SettingServices.GetSettingValue(SettingStrings.ProxyBiosFile);
            var efi32File = SettingServices.GetSettingValue(SettingStrings.ProxyEfi32File);
            var efi64File = SettingServices.GetSettingValue(SettingStrings.ProxyEfi64File);

            var fileName = isActiveOrCustom ? pxeComputerMac : "default";
            switch (proxyType)
            {
                case "bios":
                    if (biosFile.Contains("ipxe"))
                    {
                        path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName + ".ipxe";
                    }
                    else
                        path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName;
                    break;
                case "efi32":
                    if (efi32File.Contains("ipxe"))
                        path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName + ".ipxe";
                    else
                        path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName;
                    break;
                case "efi64":
                    if (efi64File.Contains("ipxe"))
                        path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName + ".ipxe";
                    else if (efi64File.Contains("grub"))
                    {
                        if (isActiveOrCustom)
                        {
                            path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "proxy" + Path.DirectorySeparatorChar +
                                   proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                                   Path.DirectorySeparatorChar + pxeComputerMac + ".cfg";
                        }
                        else
                        {
                            path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "grub" +
                                   Path.DirectorySeparatorChar + "grub.cfg";
                        }
                    }
                    else
                        path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "proxy" + Path.DirectorySeparatorChar +
                               proxyType + Path.DirectorySeparatorChar + "pxelinux.cfg" +
                               Path.DirectorySeparatorChar + fileName;
                    break;
            }

            return path;
        }

        public ComputerProxyReservationEntity GetComputerProxyReservation(int computerId)
        {
            return _uow.ComputerProxyRepository.GetFirstOrDefault(p => p.ComputerId == computerId);
        }

        public List<ComputerMunkiEntity> GetMunkiTemplates(int computerId)
        {
            return _uow.ComputerMunkiRepository.Get(x => x.ComputerId == computerId);
        }

        public string GetQueuePosition(int computerId)
        {
            var computerTask = GetTaskForComputer(computerId);

            return
                _uow.ActiveImagingTaskRepository.Count(
                    x => x.Status == "2" && x.QueuePosition < computerTask.QueuePosition);
        }

        public ActiveImagingTaskEntity GetTaskForComputer(int computerId)
        {
            return _uow.ActiveImagingTaskRepository.GetFirstOrDefault(x => x.ComputerId == computerId);
        }

        public int ImportCsv(string csvContents)
        {
            var importCounter = 0;
            using (var csv = new CsvReader(new StringReader(csvContents)))
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

        public bool IsComputerActive(int computerId)
        {
            return _uow.ActiveImagingTaskRepository.Exists(a => a.ComputerId == computerId);
        }

        public List<ComputerLogEntity> SearchComputerLogs(int computerId)
        {
            return _uow.ComputerLogRepository.Get(x => x.ComputerId == computerId,
                q => q.OrderByDescending(x => x.LogTime));
        }

        public List<ComputerWithImage> SearchComputers(string searchString, int limit)
        {
            return _uow.ComputerRepository.Search(searchString, limit);
        }

        public List<ComputerWithImage> SearchComputersByName(string searchString, int limit)
        {
            return _uow.ComputerRepository.SearchByName(searchString, limit);
        }


        public List<ComputerWithImage> SearchComputersForUser(int userId, int limit, string searchString = "")
        {
            var userServices = new UserServices();
            if (limit == 0) limit = int.MaxValue;
            if (userServices.GetUser(userId).Membership == "Administrator")
                return SearchComputers(searchString, limit);

            var listOfComputers = new List<ComputerWithImage>();

            var userManagedGroups = userServices.GetUserGroupManagements(userId);
            if (userManagedGroups.Count == 0)
                return SearchComputers(searchString, limit);
            foreach (var managedGroup in userManagedGroups)
            {
                listOfComputers.AddRange(new GroupServices().GetGroupMembersWithImages(managedGroup.GroupId,
                    searchString));
            }


            return listOfComputers;
        }

        public List<ComputerWithImage> SearchComputersForUserByName(int userId, int limit, string searchString = "")
        {
            var userServices = new UserServices();
            if (limit == 0) limit = int.MaxValue;

            if (userServices.GetUser(userId).Membership == "Administrator")
                return SearchComputers(searchString, limit);

            var listOfComputers = new List<ComputerWithImage>();

            var userManagedGroups = userServices.GetUserGroupManagements(userId);
            if (userManagedGroups.Count == 0)
                return SearchComputersByName(searchString, limit);
            foreach (var managedGroup in userManagedGroups)
            {
                listOfComputers.AddRange(new GroupServices().GetGroupMembersWithImages(managedGroup.GroupId,
                    searchString));
            }


            return listOfComputers;
        }

        public bool ToggleComputerBootMenu(int computerId, bool enable)
        {
            var computerServices = new ComputerServices();
            var computer = computerServices.GetComputer(computerId);
            computer.CustomBootEnabled = Convert.ToInt16(enable);
            computerServices.UpdateComputer(computer);

            if (enable)
                CreateBootFiles(computer.Id);

            return true;
        }

        public bool ToggleProxyReservation(int computerId, bool enable)
        {
            var computerServices = new ComputerServices();
            var computer = computerServices.GetComputer(computerId);
            computer.ProxyReservation = Convert.ToInt16(enable);
            computerServices.UpdateComputer(computer);
            return true;
        }

        public string TotalCount()
        {
            return _uow.ComputerRepository.Count();
        }

        public ActionResultDTO UpdateComputer(ComputerEntity computer)
        {
            var existingcomputer = GetComputer(computer.Id);
            if (existingcomputer == null)
                return new ActionResultDTO {ErrorMessage = "Computer Not Found", Id = 0};

            computer.Mac = StringManipulationServices.FixMac(computer.Mac);
            var actionResult = new ActionResultDTO();
            var validationResult = ValidateComputer(computer, "update");
            if (validationResult.Success)
            {
                _uow.ComputerRepository.Update(computer, computer.Id);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = computer.Id;
            }
            else
            {
                actionResult.Success = false;
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        private ValidationResultDTO ValidateComputer(ComputerEntity computer, string type)
        {
            var validationResult = new ValidationResultDTO {Success = false};
            if (type == "new" || type == "update")
            {
                if (string.IsNullOrEmpty(computer.Name) || computer.Name.Any(c => c == ' '))
                {
                    validationResult.ErrorMessage = "Computer Name Is Not Valid";
                    return validationResult;
                }

                if (string.IsNullOrEmpty(computer.Mac) ||
                    !computer.Mac.All(c => char.IsLetterOrDigit(c) || c == ':' || c == '-') && computer.Mac.Length == 12 &&
                    !computer.Mac.All(char.IsLetterOrDigit))
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
                        var existingComputer = GetComputerFromMac(computer.Mac);
                        if ((string.IsNullOrEmpty(existingComputer.ClientIdentifier) || string.IsNullOrEmpty(computer.ClientIdentifier)) || existingComputer.ClientIdentifier == computer.ClientIdentifier)
                        {
                            validationResult.ErrorMessage = "Duplicate MAC Addresses Are Only Allowed If Each Computer Has A Unique Client Identifier";
                            return validationResult;
                        }
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
                            var existingComputer = GetComputerFromMac(computer.Mac);
                            if ((string.IsNullOrEmpty(existingComputer.ClientIdentifier) || string.IsNullOrEmpty(computer.ClientIdentifier)) || existingComputer.ClientIdentifier == computer.ClientIdentifier)
                            {
                                validationResult.ErrorMessage = "Duplicate MAC Addresses Are Only Allowed If Each Computer Has A Unique Client Identifier";
                                return validationResult;
                            }                           
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
    }
}