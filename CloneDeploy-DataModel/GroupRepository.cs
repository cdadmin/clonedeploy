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

        public GroupWithImage GetGroupWithImage(string searchString, int groupId)
        {
            return (from h in _context.Groups
                    join t in _context.Images on h.ImageId equals t.Id into joined
                    from p in joined.DefaultIfEmpty()
                    where h.Name.Contains(searchString) && h.Id == groupId
                    select new
                    {
                        cdGroup = h,
                        image = p
                      
                    }).AsEnumerable().Select(x => new GroupWithImage()
                    {
                        Id = x.cdGroup.Id,
                        Name = x.cdGroup.Name,
                        ClusterGroupId = x.cdGroup.ClusterGroupId,
                        Description = x.cdGroup.Description,
                        Image = x.image,
                        ImageId = x.cdGroup.ImageId,
                        ImageProfileId = x.cdGroup.ImageProfileId,
                        SetDefaultBootMenu = x.cdGroup.SetDefaultBootMenu,
                        SetDefaultProperties = x.cdGroup.SetDefaultProperties,
                        SmartCriteria = x.cdGroup.SmartCriteria,
                        Type = x.cdGroup.Type
                     

                    }).OrderBy(x => x.Name).FirstOrDefault();
        }

        public List<GroupWithImage> GetGroupsWithImage(string searchString)
        {
            return (from h in _context.Groups
                join t in _context.Images on h.ImageId equals t.Id into joined
                from p in joined.DefaultIfEmpty()
                where h.Name.Contains(searchString)
                select new
                {
                    cdGroup = h,
                    image = p

                }).AsEnumerable().Select(x => new GroupWithImage()
                {
                    Id = x.cdGroup.Id,
                    Name = x.cdGroup.Name,
                    ClusterGroupId = x.cdGroup.ClusterGroupId,
                    Description = x.cdGroup.Description,
                    Image = x.image,
                    ImageId = x.cdGroup.ImageId,
                    ImageProfileId = x.cdGroup.ImageProfileId,
                    SetDefaultBootMenu = x.cdGroup.SetDefaultBootMenu,
                    SetDefaultProperties = x.cdGroup.SetDefaultProperties,
                    SmartCriteria = x.cdGroup.SmartCriteria,
                    Type = x.cdGroup.Type


                }).OrderBy(x => x.Name).ToList();
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