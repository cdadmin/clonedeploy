using System.Collections.Generic;
using System.Linq;
using DAL;
using Helpers;

namespace BLL
{
    public class DistributionPoint
    {
        private readonly DAL.UnitOfWork _unitOfWork = new UnitOfWork();

        public bool AddDistributionPoint(Models.DistributionPoint distributionPoint)
        {
            if (_unitOfWork.DistributionPointRepository.Exists(d => d.DisplayName == distributionPoint.DisplayName))
            {
                Message.Text = "A Distribution Point With This Name Already Exists";
                return false;
            }
            _unitOfWork.DistributionPointRepository.Insert(distributionPoint);
            if (_unitOfWork.Save())
            {
                Message.Text = "Successfully Created Distribution Point";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Distribution Point";
                return false;
            }
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

        public void UpdateDistributionPoint(Models.DistributionPoint distributionPoint)
        {
            _unitOfWork.DistributionPointRepository.Update(distributionPoint, distributionPoint.Id);
            if (_unitOfWork.Save())
                Message.Text = "Successfully Updated Distribution Point";
        }

      
    }
}