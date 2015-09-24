using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Global;

namespace DAL
{
    public class PartitionLayout
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(string partitionLayoutName)
        {
            return _context.PartitionLayouts.Any(h => h.Name == partitionLayoutName);
        }

        public bool Create(Models.PartitionLayout partitionLayout)
        {
            try
            {
                _context.PartitionLayouts.Add(partitionLayout);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Delete(int partitionLayoutId)
        {
            try
            {
                var partitionLayout = _context.PartitionLayouts.Find(partitionLayoutId);
                _context.PartitionLayouts.Attach(partitionLayout);
                _context.PartitionLayouts.Remove(partitionLayout);
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

                return _context.PartitionLayouts.Count().ToString();

        }

        public Models.PartitionLayout Read(int partitionLayoutId)
        {
            return _context.PartitionLayouts.FirstOrDefault(p => p.Id == partitionLayoutId);
        }

        public List<Models.PartitionLayout> Find(string searchString)
        {

            return
                (from p in _context.PartitionLayouts where p.Name.Contains(searchString) orderby p.Name select p).ToList
                    ();

        }

        public bool Update(Models.PartitionLayout partitionLayout)
        {
            try
            {
                var originalPartitionLayout = _context.PartitionLayouts.Find(partitionLayout.Id);

                if (originalPartitionLayout == null) return false;
                _context.Entry(originalPartitionLayout).CurrentValues.SetValues(partitionLayout);
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