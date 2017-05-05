using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class BuildingServices
    {
        private readonly UnitOfWork _uow;

        public BuildingServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddBuilding(BuildingEntity building)
        {

            var validationResult = ValidateBuilding(building, true);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.BuildingRepository.Insert(building);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = building.Id;
            }

            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }
            return actionResult;


        }

        public   string TotalCount()
        {
            
                return _uow.BuildingRepository.Count();
            
        }

        public ActionResultDTO DeleteBuilding(int buildingId)
        {
            var actionResult = new ActionResultDTO();
            var building = GetBuilding(buildingId);
            if (building == null)
                return new ActionResultDTO() {ErrorMessage = "Building Not Found", Id = 0};

            _uow.BuildingRepository.Delete(buildingId);
            _uow.Save();
            actionResult.Success = true;
            actionResult.Id = buildingId;

            return actionResult;
        }

        public  BuildingEntity GetBuilding(int buildingId)
        {
           
                return _uow.BuildingRepository.GetById(buildingId);
            
        }

        public  List<BuildingWithClusterGroup> SearchBuildings(string searchString = "")
        {
            
                return _uow.BuildingRepository.Get(searchString);
            
        }

        public  ActionResultDTO UpdateBuilding(BuildingEntity building)
        {
            var actionResult = new ActionResultDTO();
            var existingBuilding = GetBuilding(building.Id);
            if (existingBuilding == null)

                return new ActionResultDTO() { ErrorMessage = "Building Not Found", Id = 0 };
                var validationResult = ValidateBuilding(building, false);
                if (validationResult.Success)
                {
                    _uow.BuildingRepository.Update(building, building.Id);
                    _uow.Save();
                    actionResult.Success = true;
                    actionResult.Id = building.Id;
                }
                else
                {
                    actionResult.ErrorMessage = validationResult.ErrorMessage;
                }
            return actionResult;

        }

        private  ValidationResultDTO ValidateBuilding(BuildingEntity building, bool isNewBuilding)
        {
            var validationResult = new ValidationResultDTO() { Success = true };

            if (string.IsNullOrEmpty(building.Name) || building.Name.Contains(" "))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Building Name Is Not Valid";
                return validationResult;
            }

            if (isNewBuilding)
            {
                
                    if (_uow.BuildingRepository.Exists(h => h.Name == building.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Building Already Exists";
                        return validationResult;
                    }
                
            }
            else
            {
               
                    var originalBuilding = _uow.BuildingRepository.GetById(building.Id);
                    if (originalBuilding.Name != building.Name)
                    {
                        if (_uow.BuildingRepository.Exists(h => h.Name == building.Name))
                        {
                            validationResult.Success = false;
                            validationResult.ErrorMessage = "This Building Already Exists";
                            return validationResult;
                        }
                    }
                
            }

            return validationResult;
        }
      
    }
}