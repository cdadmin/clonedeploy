using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using Newtonsoft.Json;

namespace CloneDeploy_App.BLL
{
    public class Building
    {

        public static ActionResultEntity AddBuilding(BuildingEntity building)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateBuilding(building, true);
                if (validationResult.Success)
                {
                    uow.BuildingRepository.Insert(building);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = building.Id;
                    validationResult.Object = JsonConvert.SerializeObject(building);
                }

                return validationResult;
            }

        }

        public static  string TotalCount()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.BuildingRepository.Count();
            }
        }

        public static ActionResultEntity DeleteBuilding(int buildingId)
        {
            var actionResult = new ActionResultEntity();
            var building = GetBuilding(buildingId);
            using (var uow = new UnitOfWork())
            {
                uow.BuildingRepository.Delete(buildingId);
                actionResult.Success = uow.Save();
                actionResult.ObjectId = buildingId;
                actionResult.Object = JsonConvert.SerializeObject(building);
            }
            return actionResult;
        }

        public static BuildingEntity GetBuilding(int buildingId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.BuildingRepository.GetById(buildingId);
            }
        }

        public static List<BuildingEntity> SearchBuildings(string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                return uow.BuildingRepository.Get(searchString);
            }
        }

        public static ActionResultEntity UpdateBuilding(BuildingEntity building)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateBuilding(building, false);
                if (validationResult.Success)
                {
                    uow.BuildingRepository.Update(building, building.Id);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = building.Id;
                    validationResult.Object = JsonConvert.SerializeObject(building);
                }

                return validationResult;
            }
        }

        public static ActionResultEntity ValidateBuilding(BuildingEntity building, bool isNewBuilding)
        {
            var validationResult = new ActionResultEntity();

            if (string.IsNullOrEmpty(building.Name) || building.Name.Contains(" "))
            {
                validationResult.Success = false;
                validationResult.Message = "Building Name Is Not Valid";
                return validationResult;
            }

            if (isNewBuilding)
            {
                using (var uow = new UnitOfWork())
                {
                    if (uow.BuildingRepository.Exists(h => h.Name == building.Name))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "This Building Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new UnitOfWork())
                {
                    var originalBuilding = uow.BuildingRepository.GetById(building.Id);
                    if (originalBuilding.Name != building.Name)
                    {
                        if (uow.BuildingRepository.Exists(h => h.Name == building.Name))
                        {
                            validationResult.Success = false;
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