using System;
using System.Collections.Generic;
using DAL;
using Global;
using Helpers;
using Models;

namespace BLL
{

    public class Computer
    {
        private readonly DAL.Computer _da = new DAL.Computer();

        public bool AddComputer(Models.Computer computer)
        {
            if (_da.Exists(computer))
            {
                Message.Text = "A Computer With This Name Already Exists";
                return false;
            }
            if (_da.Create(computer))
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
            return _da.GetTotalCount();
        }

        public string ActiveStatus(string mac)
        {
            return _da.CheckActive(mac) != null ? "Active" : "Inactive";
        }
        public bool DeleteComputer(int computerId)
        {
            return _da.Delete(computerId);
        }

        public Models.Computer GetComputer(int computerId)
        {
            return _da.Read(computerId);
        }

        public Models.Computer GetComputerFromMac(string mac)
        {
            return _da.GetComputerFromMac(mac);
        }

        public List<Models.Computer> SearchComputers(string searchString)
        {
            return _da.Find(searchString);
        }

        public void UpdateComputer(Models.Computer computer)
        {
            if (_da.Update(computer))
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
            switch (new Workflows.Unicast().Run(computer, direction))
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