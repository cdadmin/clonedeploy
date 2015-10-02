using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using Global;
using Helpers;
using Models;

namespace DAL
{

    public class Computer
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public string CheckActive(string mac)
        {
            var name = (from h in _context.Computers
                join t in _context.ActiveImagingTasks on h.Id equals t.ComputerId
                where (h.Mac == mac)
                select h.Name).FirstOrDefault();
            return name;
        }

        public bool Exists(Models.Computer computer)
        {
            return _context.Computers.Any(h => h.Name == computer.Name || h.Mac == computer.Mac);
        }

        public bool Create(Models.Computer computer)
        {
            try
            {
                _context.Computers.Add(computer);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Delete(int computerId)
        {
            try
            {
                var computer = _context.Computers.Find(computerId);
                _context.Computers.Attach(computer);
                _context.Computers.Remove(computer);
                _context.SaveChanges();
                return true;
            }

            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public string GetTotalCount()
        {
            return _context.Computers.Count().ToString();
        }

        public void Import()
        {
           throw new Exception("Not Implemented");
        }

        public Models.Computer Read(int computerId)
        {
            return _context.Computers.FirstOrDefault(p => p.Id == computerId);
        }

        public List<Models.Computer> Find(string searchString)
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

        public Models.Computer GetComputerFromMac(string mac)
        {
            return _context.Computers.FirstOrDefault(p => p.Mac == mac);
        }
        public bool Update(Models.Computer computer)
        {
            try
            {
                var originalComputer = _context.Computers.Find(computer.Id);

                if (originalComputer == null) return false;
                _context.Entry(originalComputer).CurrentValues.SetValues(computer);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }
    }
}