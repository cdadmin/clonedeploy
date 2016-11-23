using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Web.Models;

namespace DAL
{
    public class SiteRepository : GenericRepository<Site>
    {
        private CloneDeployDbContext _context;

        public SiteRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }

        public List<Site> Get(string searchString)
        {
            return (from s in _context.Sites
                    join d in _context.DistributionPoints on s.DistributionPointId equals d.Id into joined
                    from j in joined.DefaultIfEmpty()
                    where s.Name.Contains(searchString)
                    orderby s.Name
                    select new
                    {
                        id = s.Id,
                        name = s.Name,
                        distributionPoint = j
                    }).AsEnumerable().Select(x => new Site()
                    {
                        Id = x.id,
                        Name = x.name,
                        DistributionPoint = x.distributionPoint
                    }).ToList();

        }
    }
}