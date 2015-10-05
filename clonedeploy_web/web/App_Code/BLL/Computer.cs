using System;
using System.Collections.Generic;
using BLL.Workflows;
using DAL;
using Helpers;

namespace BLL
{

    public class Computer
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public bool AddComputer(Models.Computer computer)
        {
            if (_unitOfWork.Computer.Exists(h => h.Name == computer.Name || h.Mac == computer.Mac))
            {
                Message.Text = "A Computer With This Name Already Exists";
                return false;
            }

            _unitOfWork.Computer.Insert(computer);
            if (_unitOfWork.Save())
            {
                Message.Text = "Successfully Created Computer";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Computer";
                return false;
            }

        }

        public string TotalCount()
        {
            return _unitOfWork.Computer.Count();
        }

        public string ActiveStatus(string mac)
        {
            return _unitOfWork.Computer.CheckActive(mac) != null ? "Active" : "Inactive";
        }

        public void DeleteComputer(int computerId)
        {
             _unitOfWork.Computer.Delete(computerId);
            _unitOfWork.Save();
        }

        public Models.Computer GetComputer(int computerId)
        {
            return _unitOfWork.Computer.GetById(computerId);
        }

        public Models.Computer GetComputerFromMac(string mac)
        {
            return _unitOfWork.Computer.GetFirstOrDefault(p => p.Mac == mac);

        }

        public List<Models.Computer> SearchComputers(string searchString)
        {
            return _unitOfWork.Computer.Get(w => w.Name.Contains(searchString), includeProperties:"images");
            //return _unitOfWork.Computer.Get(searchString);
        }

        public void UpdateComputer(Models.Computer computer)
        {
            _unitOfWork.Computer.Update(computer);
            if(_unitOfWork.Save())
                Message.Text = "Successfully Update Computer";
        }

    
        public bool ValidateHostData(Models.Computer computer)
        {
            var validated = true;
            if (string.IsNullOrEmpty(computer.Name) || computer.Name.Contains(" "))
            {
                validated = false;
                Message.Text = "Host Name Cannot Be Empty Or Contain Spaces";
            }
            if (string.IsNullOrEmpty(computer.Mac) || computer.Mac.Contains(" "))
            {
                validated = false;
                Message.Text = "Host Mac Cannot Be Empty Or Contain Spaces";
            }


            return validated;
        }

        public void ImportComputers()
        {
            throw new Exception("Not Implemented");
        }

        public void StartUnicast(Models.Computer computer, string direction)
        {
            switch (new Unicast().Run(computer, direction))
            {
                case "computer_error":
                    Message.Text = "The Computer No Longer Exists";
                    break;
                case "image_error":
                    Message.Text = "The Image No Longer Exists";
                    break;
                case "profile_error":
                    Message.Text = "The Image Profile No Longer Exists";
                    break;
                case "database_error":
                    Message.Text = "Could Not Create The Database Entry For This Task";
                    break;
                case "pxe_error":
                    Message.Text = "Could Not Create PXE Boot File";
                    break;
                case "arguments_error":
                    Message.Text = "Could Not Create Task Arguments";
                    break;
                case "true" :
                    Message.Text = "Successfully Started Task For " + computer.Name;
                    break;
            }
        }
    }
}