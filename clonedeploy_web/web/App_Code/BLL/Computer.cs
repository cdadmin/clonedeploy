﻿using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Workflows;
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
                var validationResult = ValidateComputer(computer, true);
                if (validationResult.IsValid)
                {
                    uow.ComputerRepository.Insert(computer);
                    validationResult.IsValid = uow.Save();
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

        public static bool DeleteComputer(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ComputerRepository.Delete(computerId);
                return uow.Save();
            }
        }

        public static Models.DistributionPoint GetDistributionPoint(Models.Computer computer)
        {
            Models.DistributionPoint dp;

            if (computer.RoomId != -1)
                dp =
                    DistributionPoint.GetDistributionPoint(Room.GetRoom(computer.RoomId).DistributionPointId);
            else if (computer.BuildingId != -1)
                dp =
                    DistributionPoint.GetDistributionPoint(Building.GetBuilding(computer.BuildingId).DistributionPointId);
            else if (computer.SiteId != -1)
                dp =
                    DistributionPoint.GetDistributionPoint(Site.GetSite(computer.SiteId).DistributionPointId);
            else
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

        public static List<Models.Computer> SearchComputersForUser(int userId, string searchString = "")
        {
            if(BLL.User.GetUser(userId).Membership == "Administrator")
                return SearchComputers(searchString);

            var listOfComputers = new List<Models.Computer>();

            var userManagedGroups = BLL.UserGroupManagement.Get(userId);
            if (userManagedGroups.Count == 0)
                return SearchComputers(searchString);
            else
            {
                foreach (var managedGroup in userManagedGroups)
                    listOfComputers.AddRange(BLL.Group.GetGroupMembers(managedGroup.GroupId, searchString));

                foreach (var computer in listOfComputers)
                    computer.Image = BLL.Image.GetImage(computer.ImageId);

                return listOfComputers;
            }
        }

        public static List<Models.Computer> SearchComputers(string searchString)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerRepository.Get(searchString);
            }
        }

        public static Models.ValidationResult UpdateComputer(Models.Computer computer)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                computer.Mac = Utility.FixMac(computer.Mac);
                var validationResult = ValidateComputer(computer, false);
                if (validationResult.IsValid)
                {
                    uow.ComputerRepository.Update(computer, computer.Id);
                    validationResult.IsValid = uow.Save();
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

        public static List<Models.Computer> ComputersWithoutGroup()
        {
            var listOfComputers = new List<Models.Computer>();
            using (var uow = new DAL.UnitOfWork())
            {
                listOfComputers = uow.ComputerRepository.GetComputersWithoutGroup();
                
            }
            foreach (var computer in listOfComputers)
                computer.Image = BLL.Image.GetImage(computer.ImageId);

            return listOfComputers;
        } 
    
        public static Models.ValidationResult ValidateComputer(Models.Computer computer, bool isNewComputer)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(computer.Name) || !computer.Name.All(c => char.IsLetterOrDigit(c) || c=='_'))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Computer Name Is Not Valid";
                return validationResult;
            }
            
            if (string.IsNullOrEmpty(computer.Mac) || !computer.Mac.All(c => char.IsLetterOrDigit(c) || c == ':' || c == '-')
                && (computer.Mac.Length == 12 && !computer.Mac.All(char.IsLetterOrDigit)) )
            {
                validationResult.IsValid = false;
                validationResult.Message = "Computer Mac Is Not Valid";
                return validationResult;
            }


            if (isNewComputer)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.ComputerRepository.Exists(h => h.Name == computer.Name || h.Mac == computer.Mac))
                    {
                        validationResult.IsValid = false;
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
                            validationResult.IsValid = false;
                            validationResult.Message = "This Computer Already Exists";
                            return validationResult;
                        }
                    }
                    else if (originalComputer.Mac != computer.Mac)
                    {
                        if (uow.ComputerRepository.Exists(h => h.Mac == computer.Mac))
                        {
                            validationResult.IsValid = false;
                            validationResult.Message = "This Computer Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }

        public static void ImportComputers()
        {
            throw new NotImplementedException();
        }

        public static Models.ValidationResult StartUnicast(Models.Computer computer, string direction)
        {
            var result = new Models.ValidationResult {IsValid = false};
            switch (new Unicast(computer,direction).Start())
            {
                case "active":
                    result.Message = "";
                    break;

                case "computer_error":
                    result.Message = "";
                    break;
                case "image_error":
                    result.Message = "";
                    break;
                case "profile_error":
                    result.Message = "";
                    break;
                case "database_error":
                    result.Message = "";
                    break;
                case "pxe_error":
                    result.Message = "";
                    break;
                case "arguments_error":
                    result.Message = "";
                    break;
                case "true" :
                    result.IsValid = true;
                    result.Message = "Successfully Started Task For " + computer.Name;
                    break;
            }
            return result;
        }
    }
}