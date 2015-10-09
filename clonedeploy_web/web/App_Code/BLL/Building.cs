using System.Collections.Generic;
using DAL;
using Helpers;

namespace BLL
{
    public class Building
    {

        public static  Models.ValidationResult AddBuilding(Models.Building building)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateBuilding(building, true);
                if (validationResult.IsValid)
                {
                    uow.BuildingRepository.Insert(building);
                    validationResult.IsValid = uow.Save();
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

        public static  bool DeleteBuilding(int buildingId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.BuildingRepository.Delete(buildingId);
                return uow.Save();
            }
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

        public static  Models.ValidationResult UpdateBuilding(Models.Building building)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateBuilding(building, false);
                if (validationResult.IsValid)
                {
                    uow.BuildingRepository.Update(building, building.Id);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }

        public static  Models.ValidationResult ValidateBuilding(Models.Building building, bool isNewBuilding)
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