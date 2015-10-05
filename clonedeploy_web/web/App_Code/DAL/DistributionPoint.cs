using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Helpers;

namespace DAL
{
    public class DistributionPoint
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(string distributionPointName)
        {
            return _context.DistributionPoints.Any(h => h.DisplayName == distributionPointName);
        }

        public bool Create(Models.DistributionPoint distributionPoint)
        {
            try
            {
                _context.DistributionPoints.Add(distributionPoint);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Delete(int distributionPointId)
        {
            try
            {
                var distributionPoint = _context.DistributionPoints.Find(distributionPointId);
                _context.DistributionPoints.Attach(distributionPoint);
                _context.DistributionPoints.Remove(distributionPoint);
                _context.SaveChanges();
                return true;
            }

            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public string GetTotalCount()
        {

            return _context.DistributionPoints.Count().ToString();

        }

        public Models.DistributionPoint Read(int distributionPointId)
        {
            return _context.DistributionPoints.FirstOrDefault(p => p.Id == distributionPointId);
        }

        public List<Models.DistributionPoint> Find(string searchString)
        {

            return (from s in _context.DistributionPoints where s.DisplayName.Contains(searchString) orderby s.DisplayName select s).ToList();

        }

        public bool Update(Models.DistributionPoint distributionPoint)
        {
            try
            {
                var originalDistributionPoint = _context.DistributionPoints.Find(distributionPoint.Id);

                if (originalDistributionPoint == null) return false;
                _context.Entry(originalDistributionPoint).CurrentValues.SetValues(distributionPoint);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }
    }
}