using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Entities;

namespace CloneDeploy_DataModel
{
    public class RoomRepository : GenericRepository<RoomEntity>
    {
        private readonly CloneDeployDbContext _context;

        public RoomRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }

        public List<RoomWithClusterGroup> Get(string searchString)
        {
            return (from s in _context.Rooms
                join d in _context.ClusterGroups on s.DistributionPointId equals d.Id into joined
                from j in joined.DefaultIfEmpty()
                where s.Name.Contains(searchString)
                orderby s.Name
                select new
                {
                    id = s.Id,
                    name = s.Name,
                    distributionPoint = j
                }).AsEnumerable().Select(x => new RoomWithClusterGroup
                {
                    Id = x.id,
                    Name = x.name,
                    ClusterGroup = x.distributionPoint
                }).ToList();
        }
    }
}