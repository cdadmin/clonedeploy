using System.Collections.Generic;
using System.Linq;
using DAL;
using Helpers;

namespace BLL
{
    public class DistributionPoint
    {
        private readonly DAL.UnitOfWork _unitOfWork = new UnitOfWork();

        public Models.ValidationResult AddDistributionPoint(Models.DistributionPoint distributionPoint)
        {
            var validationResult = ValidateDistributionPoint(distributionPoint, true);
            if (validationResult.IsValid)
            {
                _unitOfWork.DistributionPointRepository.Insert(distributionPoint);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
        }

        public string TotalCount()
        {
            return _unitOfWork.DistributionPointRepository.Count();
        }

        public bool DeleteDistributionPoint(int distributionPointId)
        {
            _unitOfWork.DistributionPointRepository.Delete(distributionPointId);
            return _unitOfWork.Save();         
        }

        public Models.DistributionPoint GetDistributionPoint(int distributionPointId)
        {
            return _unitOfWork.DistributionPointRepository.GetById(distributionPointId);
        }

        public List<Models.DistributionPoint> SearchDistributionPoints(string searchString)
        {
            return _unitOfWork.DistributionPointRepository.Get(d => d.DisplayName.Contains(searchString), orderBy: (q => q.OrderBy(d => d.DisplayName)));
        }

        public Models.ValidationResult UpdateDistributionPoint(Models.DistributionPoint distributionPoint)
        {
            var validationResult = ValidateDistributionPoint(distributionPoint, false);
            if (validationResult.IsValid)
            {
                _unitOfWork.DistributionPointRepository.Update(distributionPoint, distributionPoint.Id);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;

        }

        public Models.ValidationResult ValidateDistributionPoint(Models.DistributionPoint distributionPoint, bool isNewDistributionPoint)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(distributionPoint.DisplayName) || distributionPoint.DisplayName.Contains(" "))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Distribution Point Name Is Not Valid";
                return validationResult;
            }

            if (isNewDistributionPoint)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.DistributionPointRepository.Exists(h => h.DisplayName == distributionPoint.DisplayName))
                    {
                        validationResult.IsValid = false;
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
                            validationResult.IsValid = false;
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