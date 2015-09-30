using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpers;

namespace BLL
{
    public class Building
    {
        private readonly DAL.Building _da = new DAL.Building();

        public bool AddBuilding(Models.Building building)
        {
            if (_da.Exists(building.Name))
            {
                Message.Text = "A Building With This Name Already Exists";
                return false;
            }
            if (_da.Create(building))
            {
                Message.Text = "Successfully Created Building";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Building";
                return false;
            }
        }

        public string TotalCount()
        {
            return _da.GetTotalCount();
        }

        public bool DeleteBuilding(int buildingId)
        {
            return _da.Delete(buildingId);
        }

        public Models.Building GetBuilding(int buildingId)
        {
            return _da.Read(buildingId);
        }

        public List<Models.Building> SearchBuildings(string searchString)
        {
            return _da.Find(searchString);
        }

        public void UpdateBuilding(Models.Building building)
        {
            if (_da.Update(building))
                Message.Text = "Successfully Updated Building";
        }

      
    }
}