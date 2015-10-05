using System.Collections.Generic;
using Helpers;

namespace BLL
{
    public class DistributionPoint
    {
        private readonly DAL.DistributionPoint _da = new DAL.DistributionPoint();

        public bool AddDistributionPoint(Models.DistributionPoint distributionPoint)
        {
            if (_da.Exists(distributionPoint.DisplayName))
            {
                Message.Text = "A Distribution Point With This Name Already Exists";
                return false;
            }
            if (_da.Create(distributionPoint))
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
            return _da.GetTotalCount();
        }

        public bool DeleteDistributionPoint(int distributionPointId)
        {
            return _da.Delete(distributionPointId);
        }

        public Models.DistributionPoint GetDistributionPoint(int distributionPointId)
        {
            return _da.Read(distributionPointId);
        }

        public List<Models.DistributionPoint> SearchDistributionPoints(string searchString)
        {
            return _da.Find(searchString);
        }

        public void UpdateDistributionPoint(Models.DistributionPoint distributionPoint)
        {
            if (_da.Update(distributionPoint))
                Message.Text = "Successfully Updated Distribution Point";
        }

      
    }
}