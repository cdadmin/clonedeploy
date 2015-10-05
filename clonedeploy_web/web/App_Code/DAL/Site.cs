using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Helpers;

namespace DAL
{
    public class Site
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(string siteName)
        {
            return _context.Sites.Any(h => h.Name == siteName);
        }

        public bool Create(Models.Site site)
        {
            try
            {
                _context.Sites.Add(site);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Delete(int siteId)
        {
            try
            {
                var site = _context.Sites.Find(siteId);
                _context.Sites.Attach(site);
                _context.Sites.Remove(site);
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

            return _context.Sites.Count().ToString();

        }

        public Models.Site Read(int siteId)
        {
            return _context.Sites.FirstOrDefault(p => p.Id == siteId);
        }

        public List<Models.Site> Find(string searchString)
        {
            return (from s in _context.Sites
                join d in _context.DistributionPoints on s.DistributionPoint equals d.Id into joined
                from j in joined.DefaultIfEmpty()
                where s.Name.Contains(searchString)
                orderby s.Name
                select new
                {
                    id = s.Id,
                    name = s.Name,
                    distributionPoint = s.DistributionPoint,
                    dpName = j.DisplayName
                }).AsEnumerable().Select(x => new Models.Site()
                {
                    Id = x.id,
                    Name = x.name,
                    DistributionPoint = x.distributionPoint,
                    DpName = x.dpName
                }).ToList();

           

        }

        public bool Update(Models.Site site)
        {
            try
            {
                var originalSite = _context.Sites.Find(site.Id);

                if (originalSite == null) return false;
                _context.Entry(originalSite).CurrentValues.SetValues(site);
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