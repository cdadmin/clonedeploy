using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class SiteRepository : GenericRepository<Models.Site>
    {
        private CloneDeployDbContext _context;

        public SiteRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }

        public List<Models.Site> Get(string searchString)
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
                    }).AsEnumerable().Select(x => new Models.Site()
                    {
                        Id = x.id,
                        Name = x.name,
                        DistributionPoint = x.distributionPoint
                    }).ToList();

        }
    }
}