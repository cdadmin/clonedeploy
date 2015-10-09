using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Workflows;

namespace BLL
{

    public class Computer
    {
        public static Models.ValidationResult AddComputer(Models.Computer computer)
        {
            using (var uow = new DAL.UnitOfWork())
            {
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

        public static Models.Computer GetComputer(int computerId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerRepository.GetById(computerId);
            }
        }

        public static Models.Computer GetComputerFromMac(string mac)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ComputerRepository.GetFirstOrDefault(p => p.Mac == mac);
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
                var validationResult = ValidateComputer(computer, false);
                if (validationResult.IsValid)
                {
                    uow.ComputerRepository.Update(computer, computer.Id);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
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
            
            if (string.IsNullOrEmpty(computer.Mac) || computer.Mac.Contains(" "))
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
                    if (originalComputer.Name != computer.Name || originalComputer.Mac != computer.Mac)
                    {
                        if (uow.ComputerRepository.Exists(h => h.Name == computer.Name || h.Mac == computer.Mac))
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

        public static void StartUnicast(Models.Computer computer, string direction)
        {
            switch (new Unicast().Run(computer, direction))
            {
                case "computer_error":
                    //Message.Text = "The Computer No Longer Exists";
                    break;
                case "image_error":
                    //Message.Text = "The Image No Longer Exists";
                    break;
                case "profile_error":
                    //Message.Text = "The Image Profile No Longer Exists";
                    break;
                case "database_error":
                    //Message.Text = "Could Not Create The Database Entry For This Task";
                    break;
                case "pxe_error":
                    //Message.Text = "Could Not Create PXE Boot File";
                    break;
                case "arguments_error":
                    //Message.Text = "Could Not Create Task Arguments";
                    break;
                case "true" :
                    //Message.Text = "Successfully Started Task For " + computer.Name;
                    break;
            }
        }
    }
}