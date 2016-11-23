using System.Collections.Generic;
using CloneDeploy_Web.Models;

namespace BLL
{
    public class Building
    {
        //moved
        public static  ActionResult AddBuilding(CloneDeploy_Web.Models.Building building)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateBuilding(building, true);
                if (validationResult.Success)
                {
                    uow.BuildingRepository.Insert(building);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }

        }

        //moved
        public static  string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.BuildingRepository.Count();
            }
        }

        //moved
        public static  bool DeleteBuilding(int buildingId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.BuildingRepository.Delete(buildingId);
                return uow.Save();
            }
        }

        //moved
        public static  CloneDeploy_Web.Models.Building GetBuilding(int buildingId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.BuildingRepository.GetById(buildingId);
            }
        }

        //moved
        public static  List<CloneDeploy_Web.Models.Building> SearchBuildings(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.BuildingRepository.Get(searchString);
            }
        }

        //moved
        public static  ActionResult UpdateBuilding(CloneDeploy_Web.Models.Building building)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateBuilding(building, false);
                if (validationResult.Success)
                {
                    uow.BuildingRepository.Update(building, building.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        //move not needed
        public static  ActionResult ValidateBuilding(CloneDeploy_Web.Models.Building building, bool isNewBuilding)
        {
            var validationResult = new ActionResult();

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