using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Helpers;

namespace DAL
{
    public class Script
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(string scriptName)
        {
            return _context.Scripts.Any(h => h.Name == scriptName);
        }

        public bool Create(Models.Script script)
        {
            try
            {
                _context.Scripts.Add(script);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Delete(int scriptId)
        {
            try
            {
                var script = _context.Scripts.Find(scriptId);
                _context.Scripts.Attach(script);
                _context.Scripts.Remove(script);
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

            return _context.Scripts.Count().ToString();

        }

        public Models.Script Read(int scriptId)
        {
            return _context.Scripts.FirstOrDefault(p => p.Id == scriptId);
        }

        public List<Models.Script> Find(string searchString)
        {

            return (from s in _context.Scripts where s.Name.Contains(searchString) orderby s.Name select s).ToList();

        }

        public bool Update(Models.Script script)
        {
            try
            {
                var originalScript = _context.Scripts.Find(script.Id);

                if (originalScript == null) return false;
                _context.Entry(originalScript).CurrentValues.SetValues(script);
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