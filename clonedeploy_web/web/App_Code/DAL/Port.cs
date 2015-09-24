using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Global;

namespace DAL
{
    public class Port
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Create(Models.Port port)
        {

            try
            {
                _context.Ports.Add(port);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public Models.Port GetLastUsedPort()
        {
            return _context.Ports.OrderByDescending(p => p.Id).FirstOrDefault();
        
        }
    }
}