using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Global;
using Helpers;

namespace DAL
{
    public class Room
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(string roomName)
        {
            return _context.Rooms.Any(h => h.Name == roomName);
        }

        public bool Create(Models.Room room)
        {
            try
            {
                _context.Rooms.Add(room);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Delete(int roomId)
        {
            try
            {
                var room = _context.Rooms.Find(roomId);
                _context.Rooms.Attach(room);
                _context.Rooms.Remove(room);
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

            return _context.Rooms.Count().ToString();

        }

        public Models.Room Read(int roomId)
        {
            return _context.Rooms.FirstOrDefault(p => p.Id == roomId);
        }

        public List<Models.Room> Find(string searchString)
        {
            return (from s in _context.Rooms
                join d in _context.DistributionPoints on s.DistributionPoint equals d.Id into joined
                from j in joined.DefaultIfEmpty()
                where s.Name.Contains(searchString)
                orderby s.Name
                select new
                {
                    id = s.Id,
                    name = s.Name,
                    distributionPoint = s.DistributionPoint,
                    dpName = j.DisplayName
                }).AsEnumerable().Select(x => new Models.Room()
                {
                    Id = x.id,
                    Name = x.name,
                    DistributionPoint = x.distributionPoint,
                    DpName = x.dpName
                }).ToList();
        }

        public bool Update(Models.Room room)
        {
            try
            {
                var originalRoom = _context.Rooms.Find(room.Id);

                if (originalRoom == null) return false;
                _context.Entry(originalRoom).CurrentValues.SetValues(room);
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