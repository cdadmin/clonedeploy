using System;
using System.Collections.Generic;
using DataAccess;
using Global;
using Models;

namespace Logic
{
    /// <summary>
    /// Summary description for ComputerLogic
    /// </summary>
    public class ComputerLogic
    {
        private readonly ComputerDataAccess _da = new ComputerDataAccess();

        public bool AddComputer(Computer computer)
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
        public void DeleteComputer(int computerId)
        {
            _da.Delete(computerId);
        }

        public Computer GetComputer(int computerId)
        {
            return _da.Read(computerId);
        }

        public Computer GetComputerFromMac(string mac)
        {
            return _da.GetComputerFromMac(mac);
        }

        public List<Computer> SearchComputers(string searchString)
        {
            return _da.Find(searchString);
        }

        public void UpdateComputer(Computer computer)
        {
            _da.Update(computer);
        }

        public bool ValidateHostData(Computer computer)
        {
            var validated = true;
            if (string.IsNullOrEmpty(computer.Name) || computer.Name.Contains(" "))
            {
                validated = false;
                Utility.Message = "Host Name Cannot Be Empty Or Contain Spaces";
            }
            if (string.IsNullOrEmpty(computer.Mac) || computer.Mac.Contains(" "))
            {
                validated = false;
                Utility.Message = "Host Mac Cannot Be Empty Or Contain Spaces";
            }


            return validated;
        }

        public void ImportComputers()
        {
            throw new Exception("Not Implemented");
        }
    }
}