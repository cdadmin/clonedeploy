﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Helpers;

namespace CloneDeploy_Services
{
    public class DistributionPointServices
    {
        private readonly UnitOfWork _uow;

        public DistributionPointServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddDistributionPoint(DistributionPointEntity distributionPoint)
        {
            distributionPoint.RoPassword = new EncryptionServices().EncryptText(distributionPoint.RoPassword);
            distributionPoint.RwPassword = new EncryptionServices().EncryptText(distributionPoint.RwPassword);
            var validationResult = ValidateDistributionPoint(distributionPoint, true);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.DistributionPointRepository.Insert(distributionPoint);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = distributionPoint.Id;
            }

            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }
            return actionResult;
        }

        public ActionResultDTO DeleteDistributionPoint(int distributionPointId)
        {
            var actionResult = new ActionResultDTO();
            var dp = GetDistributionPoint(distributionPointId);
            if (dp == null)
                return new ActionResultDTO {ErrorMessage = "Distribution Point Not Found", Id = 0};

            _uow.DistributionPointRepository.Delete(distributionPointId);
            _uow.Save();
            actionResult.Success = true;
            actionResult.Id = dp.Id;

            return actionResult;
        }

        public DistributionPointEntity GetDistributionPoint(int distributionPointId)
        {
            return _uow.DistributionPointRepository.GetById(distributionPointId);
        }

        public DistributionPointEntity GetPrimaryDistributionPoint()
        {
            return _uow.DistributionPointRepository.GetFirstOrDefault(x => x.IsPrimary == 1);
        }

        public List<DistributionPointEntity> SearchDistributionPoints(string searchString = "")
        {
            return _uow.DistributionPointRepository.Get(d => d.DisplayName.Contains(searchString),
                q => q.OrderBy(d => d.DisplayName));
        }

        public string TotalCount()
        {
            return _uow.DistributionPointRepository.Count();
        }

        public ActionResultDTO UpdateDistributionPoint(DistributionPointEntity distributionPoint)
        {
            var actionResult = new ActionResultDTO();
            var dp = GetDistributionPoint(distributionPoint.Id);
            if (dp == null)
                return new ActionResultDTO {ErrorMessage = "Distribution Point Not Found", Id = 0};

            if (string.IsNullOrEmpty(distributionPoint.RwPassword))
                distributionPoint.RwPassword = dp.RwPassword;
            else
                distributionPoint.RwPassword = new EncryptionServices().EncryptText(distributionPoint.RwPassword);
            if (string.IsNullOrEmpty(distributionPoint.RoPassword))
                distributionPoint.RoPassword = dp.RoPassword;
            else
                distributionPoint.RoPassword = new EncryptionServices().EncryptText(distributionPoint.RoPassword);

            var validationResult = ValidateDistributionPoint(distributionPoint, false);
            if (validationResult.Success)
            {
                _uow.DistributionPointRepository.Update(distributionPoint, distributionPoint.Id);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = distributionPoint.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        private ValidationResultDTO ValidateDistributionPoint(DistributionPointEntity distributionPoint,
            bool isNewDistributionPoint)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (Convert.ToBoolean(distributionPoint.IsPrimary))
                if (!distributionPoint.PhysicalPath.Trim().EndsWith(Path.DirectorySeparatorChar.ToString()))
                    distributionPoint.PhysicalPath += Path.DirectorySeparatorChar;

            if (string.IsNullOrEmpty(distributionPoint.DisplayName) || distributionPoint.DisplayName.Contains(" "))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Distribution Point Name Is Not Valid";
                return validationResult;
            }

            if (isNewDistributionPoint)
            {
                var primaryDp = GetPrimaryDistributionPoint();
                if (primaryDp != null && Convert.ToBoolean(distributionPoint.IsPrimary))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "There Can Only Be One Primary Distribution Point";
                    return validationResult;
                }

                if (_uow.DistributionPointRepository.Exists(h => h.DisplayName == distributionPoint.DisplayName))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "This Distribution Point Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalDistributionPoint = _uow.DistributionPointRepository.GetById(distributionPoint.Id);
                var primaryDp = GetPrimaryDistributionPoint();
                if (primaryDp != null)
                {
                    if ((primaryDp.Id != distributionPoint.Id) && distributionPoint.IsPrimary == 1)
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "There Can Only Be One Primary Distribution Point";
                        return validationResult;
                    }
                }
                if (originalDistributionPoint.DisplayName != distributionPoint.DisplayName)
                {
                    if (_uow.DistributionPointRepository.Exists(h => h.DisplayName == distributionPoint.DisplayName))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Distribution Point Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}