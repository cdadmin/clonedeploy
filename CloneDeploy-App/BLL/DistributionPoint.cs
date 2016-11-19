using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CloneDeploy_App.BLL
{
    public class DistributionPoint
    {

        public static Models.ActionResult AddDistributionPoint(Models.DistributionPoint distributionPoint)       
        {

            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateDistributionPoint(distributionPoint, true);
                if (validationResult.Success)
                {
                    uow.DistributionPointRepository.Insert(distributionPoint);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.DistributionPointRepository.Count();
            }
        }

        public static bool DeleteDistributionPoint(int distributionPointId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.DistributionPointRepository.Delete(distributionPointId);
                return uow.Save();
            }
        }

        public static Models.DistributionPoint GetDistributionPoint(int distributionPointId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.DistributionPointRepository.GetById(distributionPointId);
            }
        }

        public static List<Models.DistributionPoint> SearchDistributionPoints(string searchString)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.DistributionPointRepository.Get(d => d.DisplayName.Contains(searchString),
                    orderBy: (q => q.OrderBy(d => d.DisplayName)));
            }
        }

        public static Models.DistributionPoint GetPrimaryDistributionPoint()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.DistributionPointRepository.GetFirstOrDefault(x => x.IsPrimary == 1);
            }
        }

        public static Models.ActionResult UpdateDistributionPoint(Models.DistributionPoint distributionPoint)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateDistributionPoint(distributionPoint, false);
                if (validationResult.Success)
                {
                    uow.DistributionPointRepository.Update(distributionPoint, distributionPoint.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static Models.ActionResult ValidateDistributionPoint(Models.DistributionPoint distributionPoint, bool isNewDistributionPoint)
        {
            var validationResult = new Models.ActionResult();

            

            if(Convert.ToBoolean(distributionPoint.IsPrimary))
                if (!distributionPoint.PhysicalPath.Trim().EndsWith(Path.DirectorySeparatorChar.ToString()))
                    distributionPoint.PhysicalPath += Path.DirectorySeparatorChar;

            if (string.IsNullOrEmpty(distributionPoint.DisplayName) || distributionPoint.DisplayName.Contains(" "))
            {
                validationResult.Success = false;
                validationResult.Message = "Distribution Point Name Is Not Valid";
                return validationResult;
            }

            if (isNewDistributionPoint)
            {
                var primaryDp = GetPrimaryDistributionPoint();
                if (primaryDp != null && Convert.ToBoolean(distributionPoint.IsPrimary))
                {
                    validationResult.Success = false;
                    validationResult.Message = "There Can Only Be One Primary Distribution Point";
                    return validationResult;
                }

                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.DistributionPointRepository.Exists(h => h.DisplayName == distributionPoint.DisplayName))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "This Distribution Point Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    var originalDistributionPoint = uow.DistributionPointRepository.GetById(distributionPoint.Id);
                    if (originalDistributionPoint.DisplayName != distributionPoint.DisplayName)
                    {
                        if (uow.DistributionPointRepository.Exists(h => h.DisplayName == distributionPoint.DisplayName))
                        {
                            validationResult.Success = false;
                            validationResult.Message = "This Distribution Point Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }
      
    }
}