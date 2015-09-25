using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Global;
using Helpers;

namespace DAL
{
    public class ImageProfilePartition
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Create(Models.ImageProfilePartition imageProfilePartition)
        {
            try
            {
                _context.ImageProfilePartitions.Add(imageProfilePartition);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Delete(int profileId)
        {
            try
            {
                _context.ImageProfilePartitions.RemoveRange(
                _context.ImageProfilePartitions.Where(x => x.ProfileId == profileId));
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public List<Models.ImageProfilePartition> Find(int searchId)
        {

            return
                (from p in _context.ImageProfilePartitions where p.ProfileId == searchId orderby p.Id select p).ToList();

        }
    }
}