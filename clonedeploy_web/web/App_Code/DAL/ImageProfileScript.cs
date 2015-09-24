using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Global;

namespace DAL
{
    public class ImageProfileScript
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Create(Models.ImageProfileScript imageProfileScript)
        {
            try
            {
                _context.ImageProfileScripts.Add(imageProfileScript);
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
                _context.ImageProfileScripts.RemoveRange(_context.ImageProfileScripts.Where(x => x.ProfileId == profileId));
                _context.SaveChanges();
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public List<Models.ImageProfileScript> Find(int profileId)
        {
            return (from p in _context.ImageProfileScripts join s in _context.Scripts on p.ScriptId equals s.Id where p.ProfileId == profileId orderby s.Priority ascending, s.Name ascending select p).ToList();
        }
    }
}