using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Helpers;

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

        public void Import()
        {
            throw new Exception("Not Implemented");
        }

        public List<Models.Computer> Get(string searchString)
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
                        image = p
                    }).AsEnumerable().Select(x => new Models.Computer()
                    {
                        Id = x.id,
                        Name = x.name,
                        Mac = x.mac,
                        Image = x.image
                    }).ToList();
        }

        public List<Models.Computer> GetComputersWithoutGroup()
        {
            var list = (from c in _context.Computers

            join g in _context.GroupMemberships on c.Id equals g.ComputerId into joined

            from j in joined.DefaultIfEmpty()

            where j == null

            select c).ToList();
           
            return list;
        } 
    }
}