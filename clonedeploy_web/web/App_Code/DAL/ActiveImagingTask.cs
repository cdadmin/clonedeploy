using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Helpers;

namespace DAL
{
    public class ActiveImagingTask
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(int computerId)
        {
            return _context.ActiveImagingTasks.Any(h => h.ComputerId == computerId);
        }

        public Models.ActiveImagingTask Read(int activeImagingTaskId)
        {
            return _context.ActiveImagingTasks.FirstOrDefault(p => p.Id == activeImagingTaskId);
        }

        public bool Delete(int activeImagingTaskId)
        {
            try
            {
                var activeImagingTask = _context.ActiveImagingTasks.Find(activeImagingTaskId);
                _context.ActiveImagingTasks.Attach(activeImagingTask);
                _context.ActiveImagingTasks.Remove(activeImagingTask);
                _context.SaveChanges();
                return true;
            }

            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public void DeleteForMulticast(int multicastId)
        {

            var tasks = from t in _context.ActiveImagingTasks where t.MulticastId == multicastId select t;
            _context.ActiveImagingTasks.RemoveRange(tasks);
            _context.SaveChanges();

        }

        public List<Models.ActiveImagingTask> MulticastMemberStatus(int multicastId)
        {

            return
                (from t in _context.ActiveImagingTasks where t.MulticastId == multicastId orderby t.ComputerId select t)
                    .ToList();

        }

        public List<Models.Computer> MulticastComputers(int multicastId)
        {
         
            return
                (from t in _context.ActiveImagingTasks 
                 join c in _context.Computers on t.ComputerId equals c.Id 
                 where t.MulticastId == multicastId orderby t.ComputerId select c)
                    .ToList();

        }

        public List<Models.ActiveImagingTask> MulticastProgress(int multicastId)
        {

            return
                (from t in _context.ActiveImagingTasks
                    where t.MulticastId == multicastId && t.Status == "3"
                    orderby t.ComputerId
                    select t).Take(1).ToList();

        }

        public List<Models.ActiveImagingTask> ReadAll()
        {

            return _context.ActiveImagingTasks.OrderBy(t => t.Id).ToList();
        }

        public List<Models.ActiveImagingTask> ReadUnicasts()
        {

            return
                (from t in _context.ActiveImagingTasks where t.Type == "unicast" orderby t.ComputerId select t).ToList();

        }

        public bool Create(Models.ActiveImagingTask activeImagingTask)
        {
            try
            {
                _context.ActiveImagingTasks.Add(activeImagingTask);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Update(Models.ActiveImagingTask activeImagingTask)
        {
            try
            {
                var originalActiveImagingTask = _context.ActiveImagingTasks.Find(activeImagingTask.Id);

                if (originalActiveImagingTask == null) return false;
                _context.Entry(originalActiveImagingTask).CurrentValues.SetValues(activeImagingTask);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public void DeleteAll()
        {
            _context.Database.ExecuteSqlCommand("TRUNCATE TABLE activetasks;");
            _context.Database.ExecuteSqlCommand("TRUNCATE TABLE activemctasks;");
        }
        
    }
}