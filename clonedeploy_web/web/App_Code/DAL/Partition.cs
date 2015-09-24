using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Global;

namespace DAL
{
    public class Partition
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Create(Models.Partition partition)
        {
            try
            {
                _context.Partitions.Add(partition);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Delete(int partitionId)
        {
            try
            {
                var partition = _context.Partitions.Find(partitionId);
                _context.Partitions.Attach(partition);
                _context.Partitions.Remove(partition);
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
          
                return _context.Partitions.Count().ToString();
            
        }

        public Models.Partition Read(int partitionId)
        {
            return _context.Partitions.FirstOrDefault(p => p.Id == partitionId);
        }

        public List<Models.Partition> Find(int layoutId)
        {

                return (from p in _context.Partitions where p.LayoutId == layoutId orderby p.Number select p).ToList();

        }

        public bool Update(Models.Partition partition)
        {
            try
            {
                var originalPartition = _context.Partitions.Find(partition.Id);

                if (originalPartition == null) return false;
                _context.Entry(originalPartition).CurrentValues.SetValues(partition);
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