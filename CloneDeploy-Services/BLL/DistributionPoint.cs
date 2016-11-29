using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using Newtonsoft.Json;

namespace CloneDeploy_App.BLL
{
    public class DistributionPoint
    {

        public static ActionResultEntity AddDistributionPoint(DistributionPointEntity distributionPoint)       
        {

            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateDistributionPoint(distributionPoint, true);
                if (validationResult.Success)
                {
                    uow.DistributionPointRepository.Insert(distributionPoint);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = distributionPoint.Id;
                    validationResult.Object = JsonConvert.SerializeObject(distributionPoint);
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.DistributionPointRepository.Count();
            }
        }

        public static ActionResultEntity DeleteDistributionPoint(int distributionPointId)
        {
            var actionResult = new ActionResultEntity();
            var dp = GetDistributionPoint(distributionPointId);
            
            using (var uow = new UnitOfWork())
            {
                uow.DistributionPointRepository.Delete(distributionPointId);
                actionResult.Success = uow.Save();
                actionResult.ObjectId = dp.Id;
                actionResult.Object = JsonConvert.SerializeObject(dp);
            }

            return actionResult;
            
        }

        public static DistributionPointEntity GetDistributionPoint(int distributionPointId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.DistributionPointRepository.GetById(distributionPointId);
            }
        }

        public static List<DistributionPointEntity> SearchDistributionPoints(string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                return uow.DistributionPointRepository.Get(d => d.DisplayName.Contains(searchString),
                    orderBy: (q => q.OrderBy(d => d.DisplayName)));
            }
        }

        public static DistributionPointEntity GetPrimaryDistributionPoint()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.DistributionPointRepository.GetFirstOrDefault(x => x.IsPrimary == 1);
            }
        }

        public static ActionResultEntity UpdateDistributionPoint(DistributionPointEntity distributionPoint)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateDistributionPoint(distributionPoint, false);
                if (validationResult.Success)
                {
                    uow.DistributionPointRepository.Update(distributionPoint, distributionPoint.Id);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = distributionPoint.Id;
                    validationResult.Object = JsonConvert.SerializeObject(distributionPoint);
                }

                return validationResult;
            }
        }

        public static ActionResultEntity ValidateDistributionPoint(DistributionPointEntity distributionPoint, bool isNewDistributionPoint)
        {
            var validationResult = new ActionResultEntity();

            

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

                using (var uow = new UnitOfWork())
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
                using (var uow = new UnitOfWork())
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