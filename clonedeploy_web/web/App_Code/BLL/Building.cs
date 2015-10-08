using System.Collections.Generic;
using DAL;
using Helpers;

namespace BLL
{
    public class Building
    {
        private readonly DAL.UnitOfWork _unitOfWork = new UnitOfWork();

        public Models.ValidationResult AddBuilding(Models.Building building)
        {
            var validationResult = ValidateBuilding(building, true);
            if (validationResult.IsValid)
            {
                _unitOfWork.BuildingRepository.Insert(building);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
          
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

        public Models.ValidationResult UpdateBuilding(Models.Building building)
        {
            var validationResult = ValidateBuilding(building, false);
            if (validationResult.IsValid)
            {
                _unitOfWork.BuildingRepository.Update(building, building.Id);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
        }


        public Models.ValidationResult ValidateBuilding(Models.Building building, bool isNewBuilding)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(building.Name) || building.Name.Contains(" "))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Building Name Is Not Valid";
                return validationResult;
            }

            if (isNewBuilding)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.BuildingRepository.Exists(h => h.Name == building.Name))
                    {
                        validationResult.IsValid = false;
                        validationResult.Message = "This Building Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    var originalBuilding = uow.BuildingRepository.GetById(building.Id);
                    if (originalBuilding.Name != building.Name)
                    {
                        if (uow.BuildingRepository.Exists(h => h.Name == building.Name))
                        {
                            validationResult.IsValid = false;
                            validationResult.Message = "This Building Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }
      
    }
}