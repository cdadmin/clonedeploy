using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Helpers;

namespace DAL
{
    public class SysprepTag
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(string sysprepTagName)
        {
            return _context.SysprepTags.Any(h => h.Name == sysprepTagName);
        }

        public bool Create(Models.SysprepTag sysprepTag)
        {
            try
            {
                _context.SysprepTags.Add(sysprepTag);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Delete(int sysprepTagId)
        {
            try
            {
                var sysprepTag = _context.SysprepTags.Find(sysprepTagId);
                _context.SysprepTags.Attach(sysprepTag);
                _context.SysprepTags.Remove(sysprepTag);
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

            return _context.SysprepTags.Count().ToString();

        }

        public Models.SysprepTag Read(int sysprepTagId)
        {
            return _context.SysprepTags.FirstOrDefault(p => p.Id == sysprepTagId);
        }

        public List<Models.SysprepTag> Find(string searchString)
        {

            return (from s in _context.SysprepTags where s.OpeningTag.Contains(searchString) || s.Name.Contains(searchString) orderby s.OpeningTag select s).ToList();

        }

        public bool Update(Models.SysprepTag sysprepTag)
        {
            try
            {
                var originalSysprepTag = _context.SysprepTags.Find(sysprepTag.Id);

                if (originalSysprepTag == null) return false;
                _context.Entry(originalSysprepTag).CurrentValues.SetValues(sysprepTag);
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