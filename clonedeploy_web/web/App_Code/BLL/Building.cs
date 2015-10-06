using System.Collections.Generic;
using DAL;
using Helpers;

namespace BLL
{
    public class Building
    {
        private readonly DAL.UnitOfWork _unitOfWork = new UnitOfWork();

        public bool AddBuilding(Models.Building building)
        {
            if (_unitOfWork.BuildingRepository.Exists(t => t.Name == building.Name))
            
            {
                Message.Text = "A Building With This Name Already Exists";
                return false;
            }
            _unitOfWork.BuildingRepository.Insert(building);
            if (_unitOfWork.Save())
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
            return _unitOfWork.BuildingRepository.Count();
        }

        public bool DeleteBuilding(int buildingId)
        {
            _unitOfWork.BuildingRepository.Delete(buildingId);
            return _unitOfWork.Save();
        }

        public Models.Building GetBuilding(int buildingId)
        {
            return _unitOfWork.BuildingRepository.GetById(buildingId);
        }

        public List<Models.Building> SearchBuildings(string searchString)
        {
            return _unitOfWork.BuildingRepository.Get(b => b.Name.Contains(searchString), includeProperties: "dp");
        }

        public void UpdateBuilding(Models.Building building)
        {
            _unitOfWork.BuildingRepository.Update(building, building.Id);
            if (_unitOfWork.Save())
                Message.Text = "Successfully Updated Building";
        }

      
    }
}