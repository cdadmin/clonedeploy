using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Helpers;

namespace DAL
{
    public class Computer : GenericRepository<Models.Computer>
    {
        private CloneDeployDbContext _context;

        public Computer(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }

        public string CheckActive(string mac)
        {
            var name = (from h in _context.Computers
                join t in _context.ActiveImagingTasks on h.Id equals t.ComputerId
                where (h.Mac == mac)
                select h.Name).FirstOrDefault();
            return name;
        }

     

      

      
     

        public void Import()
        {
           throw new Exception("Not Implemented");
        }

      

        public List<Models.Computer> Get(string searchString)
        {
            return (from h in _context.Computers
             join t in _context.Images on h.Image equals t.Id into joined
             from p in joined.DefaultIfEmpty()
             where h.Name.Contains(searchString) || h.Mac.Contains(searchString)
             select new 
             {
                 id = h.Id,
                 name = h.Name,
                 mac = h.Mac,
                 imageName = p.Name
             }).AsEnumerable().Select(x => new Models.Computer()
             {
                 Id = x.id,
                 Name = x.name,
                 Mac = x.mac,
                 ImageName = x.imageName
             }).ToList();
        }  
    }
}