using System.Collections.Generic;
using System.Linq;

namespace CloneDeploy_App.DAL
{
    public class RoomRepository : GenericRepository<Models.Room>
    {
        private CloneDeployDbContext _context;

        public RoomRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }

        public List<Models.Room> Get(string searchString)
        {
            return (from s in _context.Rooms
                    join d in _context.DistributionPoints on s.DistributionPointId equals d.Id into joined
                    from j in joined.DefaultIfEmpty()
                    where s.Name.Contains(searchString)
                    orderby s.Name
                    select new
                    {
                        id = s.Id,
                        name = s.Name,
                        distributionPoint = j
                    }).AsEnumerable().Select(x => new Models.Room()
                    {
                        Id = x.id,
                        Name = x.name,
                        DistributionPoint = x.distributionPoint
                    }).ToList();

        }
    }
}