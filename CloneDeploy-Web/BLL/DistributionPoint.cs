using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloneDeploy_Web.Models;

namespace BLL
{
    public class DistributionPoint
    {
        //moved
        public static ActionResult AddDistributionPoint(CloneDeploy_Web.Models.DistributionPoint distributionPoint)       
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

        //moved
        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.DistributionPointRepository.Count();
            }
        }

        //moved
        public static bool DeleteDistributionPoint(int distributionPointId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.DistributionPointRepository.Delete(distributionPointId);
                return uow.Save();
            }
        }

        //moved
        public static CloneDeploy_Web.Models.DistributionPoint GetDistributionPoint(int distributionPointId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.DistributionPointRepository.GetById(distributionPointId);
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.DistributionPoint> SearchDistributionPoints(string searchString)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.DistributionPointRepository.Get(d => d.DisplayName.Contains(searchString),
                    orderBy: (q => q.OrderBy(d => d.DisplayName)));
            }
        }

        //moved
        public static CloneDeploy_Web.Models.DistributionPoint GetPrimaryDistributionPoint()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.DistributionPointRepository.GetFirstOrDefault(x => x.IsPrimary == 1);
            }
        }

        //moved
        public static ActionResult UpdateDistributionPoint(CloneDeploy_Web.Models.DistributionPoint distributionPoint)
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

        //move not needed
        public static ActionResult ValidateDistributionPoint(CloneDeploy_Web.Models.DistributionPoint distributionPoint, bool isNewDistributionPoint)
        {
            var validationResult = new ActionResult();

            

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