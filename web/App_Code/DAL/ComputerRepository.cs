using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class ComputerRepository : GenericRepository<Models.Computer>
    {
        private CloneDeployDbContext _context;

        public ComputerRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }

        public List<Models.Computer> Search(string searchString, int limit=Int32.MaxValue)
        {
            return (from h in _context.Computers
                    join t in _context.Images on h.ImageId equals t.Id into joined
                    from p in joined.DefaultIfEmpty()
                    where h.Name.Contains(searchString) || h.Mac.Contains(searchString)
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
                    }).AsEnumerable().Select(x => new Models.Computer()
                    {
                        Id = x.id,
                        Name = x.name,
                        Mac = x.mac,
                        Image = x.image,
                        ImageProfileId = x.profileId,
                        SiteId = x.site,
                        BuildingId = x.building,
                        RoomId = x.room
                        
                    }).OrderBy(x => x.Name).Take(limit).ToList();
        }

        public List<Models.Computer> SearchByName(string searchString, int limit = Int32.MaxValue)
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
                        room = h.RoomId
                    }).AsEnumerable().Select(x => new Models.Computer()
                    {
                        Id = x.id,
                        Name = x.name,
                        Mac = x.mac,
                        Image = x.image,
                        ImageProfileId = x.profileId,
                        SiteId = x.site,
                        BuildingId = x.building,
                        RoomId = x.room

                    }).OrderBy(x => x.Name).Take(limit).ToList();
        }

        public List<Models.Computer> GetComputersWithoutGroup(string searchString, int limit=Int32.MaxValue)
        {
            var list = (from c in _context.Computers.Where(x => x.Name.Contains(searchString))

            join g in _context.GroupMemberships on c.Id equals g.ComputerId into joined

            from j in joined.DefaultIfEmpty()

            where j == null

            select c).OrderBy(x => x.Name).Take(limit).ToList();
           
            return list;
        } 
    }
}