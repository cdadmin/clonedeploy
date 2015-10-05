using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Helpers;

namespace DAL
{
    public class ActiveMulticastSession
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(Models.ActiveMulticastSession activeMulticast )
        {
            return _context.ActiveMulticastSessions.Any(h => h.Name == activeMulticast.Name);
        }

        public bool Create(Models.ActiveMulticastSession activeMulticastSession)
        {
            try
            {
                _context.ActiveMulticastSessions.Add(activeMulticastSession);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public Models.ActiveMulticastSession Read(int multicastId)
        {
            return _context.ActiveMulticastSessions.FirstOrDefault(p => p.Id == multicastId);
        }
     
        public bool Delete(int multicastId)
        {

            try
            {
                var activeMulticastSession = _context.ActiveMulticastSessions.Find(multicastId);
                _context.ActiveMulticastSessions.Attach(activeMulticastSession);
                _context.ActiveMulticastSessions.Remove(activeMulticastSession);
                _context.SaveChanges();
                return true;
            }

            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Update(Models.ActiveMulticastSession activeMulticastSession)
        {
            try
            {
                var originalActiveMulticastSession = _context.ActiveMulticastSessions.Find(activeMulticastSession.Id);

                if (originalActiveMulticastSession == null) return false;
                _context.Entry(originalActiveMulticastSession).CurrentValues.SetValues(activeMulticastSession);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public List<Models.ActiveMulticastSession> ReadMulticasts()
        {

            return _context.ActiveMulticastSessions.OrderBy(t => t.Name).ToList();

        }


    }
}