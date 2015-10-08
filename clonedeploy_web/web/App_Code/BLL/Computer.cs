using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Workflows;
using DAL;
using Helpers;
using Models;

namespace BLL
{

    public class Computer
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public Models.ValidationResult AddComputer(Models.Computer computer)
        {
            var validationResult = ValidateComputer(computer, true);
            if (validationResult.IsValid)
            {
                _unitOfWork.ComputerRepository.Insert(computer);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
        }

        public string TotalCount()
        {
            return _unitOfWork.ComputerRepository.Count();
        }

        public bool DeleteComputer(int computerId)
        {
            _unitOfWork.ComputerRepository.Delete(computerId);
            return _unitOfWork.Save();
        }

        public Models.Computer GetComputer(int computerId)
        {
            return _unitOfWork.ComputerRepository.GetById(computerId);
        }

        public Models.Computer GetComputerFromMac(string mac)
        {
            return _unitOfWork.ComputerRepository.GetFirstOrDefault(p => p.Mac == mac);
        }

        public List<Models.Computer> SearchComputers(string searchString)
        {
            return _unitOfWork.ComputerRepository.Get(w => w.Name.Contains(searchString), includeProperties:"images");
        }

        public Models.ValidationResult UpdateComputer(Models.Computer computer)
        {
            var validationResult = ValidateComputer(computer, false);
            if (validationResult.IsValid)
            {
                _unitOfWork.ComputerRepository.Update(computer, computer.Id);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
        }

    
        public Models.ValidationResult ValidateComputer(Models.Computer computer, bool isNewComputer)
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
                if (_unitOfWork.ComputerRepository.Exists(h => h.Name == computer.Name || h.Mac == computer.Mac))
                {
                    validationResult.IsValid = false;
                    validationResult.Message = "This Computer Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalComputer = _unitOfWork.ComputerRepository.GetById(computer.Id);
                if (originalComputer.Name != computer.Name || originalComputer.Mac != computer.Mac)
                {
                    if (_unitOfWork.ComputerRepository.Exists(h => h.Name == computer.Name || h.Mac == computer.Mac))
                    {
                        validationResult.IsValid = false;
                        validationResult.Message = "This Computer Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }

        public void ImportComputers()
        {
            throw new NotImplementedException();
        }

        public void StartUnicast(Models.Computer computer, string direction)
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