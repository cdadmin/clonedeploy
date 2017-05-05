using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Entities;

namespace CloneDeploy_DataModel
{

    public class GroupRepository : GenericRepository<GroupEntity>
    {
        private CloneDeployDbContext _context;

        public GroupRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }

        public List<ComputerEntity> GetGroupMembers(int searchGroupId, string searchString)
        {

            return (from h in _context.Computers
                    join g in _context.GroupMemberships on h.Id equals g.ComputerId
                    where (g.GroupId == searchGroupId) && (h.Name.Contains(searchString) || h.Mac.Contains(searchString))
                    select h).ToList();
        }

        public List<ComputerWithImage> GetGroupMembersWithImages(int searchGroupId, string searchString)
        {

            return (from h in _context.Computers
                    join g in _context.GroupMemberships on h.Id equals g.ComputerId
                    join t in _context.Images on h.ImageId equals t.Id into joined
                    from p in joined.DefaultIfEmpty()
                    where (g.GroupId == searchGroupId) && (h.Name.Contains(searchString) || h.Mac.Contains(searchString))
                    select new
                    {
                        id = h.Id,
                        name = h.Name,
                        mac = h.Mac,
                        image = p,
                        profileId = h.ImageProfileId,
                        site = h.SiteId,
                        building = h.BuildingId,
                        room = h.RoomId
                    }).AsEnumerable().Select(x => new ComputerWithImage()
                    {
                        Id = x.id,
                        Name = x.name,
                        Mac = x.mac,
                        Image = x.image,
                        ImageProfileId = x.profileId,
                        SiteId = x.site,
                        BuildingId = x.building,
                        RoomId = x.room

                    }).OrderBy(x => x.Name).ToList();
        }
    
    }
}