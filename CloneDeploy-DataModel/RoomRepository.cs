using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Entities;

namespace CloneDeploy_DataModel
{
    public class RoomRepository : GenericRepository<RoomEntity>
    {
        private CloneDeployDbContext _context;

        public RoomRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }

        public List<RoomEntity> Get(string searchString)
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
                    }).AsEnumerable().Select(x => new RoomEntity()
                    {
                        Id = x.id,
                        Name = x.name,
                        DistributionPoint = x.distributionPoint
                    }).ToList();

        }
    }
}