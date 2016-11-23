using System.Collections.Generic;
using CloneDeploy_App.Models;
using Newtonsoft.Json;

namespace CloneDeploy_App.BLL
{
    public class Building
    {

        public static  Models.ActionResult AddBuilding(Models.Building building)
        {
            using (var uow = new DAL.UnitOfWork())
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
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.BuildingRepository.Count();
            }
        }

        public static ActionResult DeleteBuilding(int buildingId)
        {
            var actionResult = new ActionResult();
            var building = GetBuilding(buildingId);
            using (var uow = new DAL.UnitOfWork())
            {
                uow.BuildingRepository.Delete(buildingId);
                actionResult.Success = uow.Save();
                actionResult.ObjectId = buildingId;
                actionResult.Object = JsonConvert.SerializeObject(building);
            }
            return actionResult;
        }

        public static  Models.Building GetBuilding(int buildingId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.BuildingRepository.GetById(buildingId);
            }
        }

        public static  List<Models.Building> SearchBuildings(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.BuildingRepository.Get(searchString);
            }
        }

        public static  Models.ActionResult UpdateBuilding(Models.Building building)
        {
            using (var uow = new DAL.UnitOfWork())
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

        public static  Models.ActionResult ValidateBuilding(Models.Building building, bool isNewBuilding)
        {
            var validationResult = new Models.ActionResult();

            if (string.IsNullOrEmpty(building.Name) || building.Name.Contains(" "))
            {
                validationResult.Success = false;
                validationResult.Message = "Building Name Is Not Valid";
                return validationResult;
            }

            if (isNewBuilding)
            {
                using (var uow = new DAL.UnitOfWork())
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
                using (var uow = new DAL.UnitOfWork())
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