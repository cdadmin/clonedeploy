using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using CloneDeploy_Entities;

namespace CloneDeploy_DataModel
{
    public class ComputerRepository : GenericRepository<ComputerEntity>
    {
        private readonly CloneDeployDbContext _context;

        public ComputerRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }

        public ComputerWithImage GetComputerWithImage(int computerId)
        {
            return (from h in _context.Computers
                    join t in _context.Images on h.ImageId equals t.Id into joinimage
                    from p in joinimage.DefaultIfEmpty()
                    where h.Id == computerId
                    select new
                    {
                        id = h.Id,
                        name = h.Name,
                        mac = h.Mac,
                        image = p,
                        profileId = h.ImageProfileId,
                        site = h.SiteId,
                        building = h.BuildingId,
                        room = h.RoomId,
                        alternateip = h.AlternateServerIpId
                    }).AsEnumerable().Select(x => new ComputerWithImage
                    {
                        Id = x.id,
                        Name = x.name,
                        Mac = x.mac,
                        Image = x.image,
                        ImageProfileId = x.profileId,
                        SiteId = x.site,
                        BuildingId = x.building,
                        RoomId = x.room,
                        AlternateServerIpId = x.alternateip
                    }).FirstOrDefault();
        }

        public List<ComputerEntity> GetComputersWithoutGroup(string searchString, int limit = int.MaxValue)
        {
            var list = (from c in _context.Computers.Where(x => x.Name.Contains(searchString))
                join g in _context.GroupMemberships on c.Id equals g.ComputerId into joined
                from j in joined.DefaultIfEmpty()
                where j == null
                select c).OrderBy(x => x.Name).Take(limit).ToList();

            return list;
        }


        public List<ComputerWithImage> Search(string searchString, int limit = int.MaxValue)
        {
            return (from h in _context.Computers
                join t in _context.Images on h.ImageId equals t.Id into joinimage
                join i in _context.ImageProfiles on h.ImageProfileId equals i.Id into joinprofile
                from p in joinimage.DefaultIfEmpty()
                from z in joinprofile.DefaultIfEmpty()
                where h.Name.Contains(searchString) || h.Mac.Contains(searchString)
                select new
                {
                    id = h.Id,
                    name = h.Name,
                    mac = h.Mac,
                    image = p,
                    imageProfile = z,
                    profileId = h.ImageProfileId,
                    site = h.SiteId,
                    building = h.BuildingId,
                    room = h.RoomId,
                    altIp = h.AlternateServerIpId
                }).AsEnumerable().Select(x => new ComputerWithImage
                {
                    Id = x.id,
                    Name = x.name,
                    Mac = x.mac,
                    Image = x.image,
                    ImageProfile = x.imageProfile,
                    ImageProfileId = x.profileId,
                    SiteId = x.site,
                    BuildingId = x.building,
                    RoomId = x.room,
                    AlternateServerIpId = x.altIp
                }).OrderBy(x => x.Name).Take(limit).ToList();
        }

        public List<ComputerWithImage> SearchByName(string searchString, int limit = int.MaxValue)
        {
            return (from h in _context.Computers
                join t in _context.Images on h.ImageId equals t.Id into joined
                from p in joined.DefaultIfEmpty()
                where h.Name.Contains(searchString)
                select new
                {
                    id = h.Id,
                    name = h.Name,
                    mac = h.Mac,
                    image = p,
                    profileId = h.ImageProfileId,
                    site = h.SiteId,
                    building = h.BuildingId,
                    room = h.RoomId,
                    altIp = h.AlternateServerIpId
                }).AsEnumerable().Select(x => new ComputerWithImage
                {
                    Id = x.id,
                    Name = x.name,
                    Mac = x.mac,
                    Image = x.image,
                    ImageProfileId = x.profileId,
                    SiteId = x.site,
                    BuildingId = x.building,
                    RoomId = x.room,
                    AlternateServerIpId = x.altIp
                }).OrderBy(x => x.Name).Take(limit).ToList();
        }
    }
}