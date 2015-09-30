using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Global;
using Helpers;

namespace DAL
{
    public class Building
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(string buildingName)
        {
            return _context.Buildings.Any(h => h.Name == buildingName);
        }

        public bool Create(Models.Building building)
        {
            try
            {
                _context.Buildings.Add(building);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Delete(int buildingId)
        {
            try
            {
                var building = _context.Buildings.Find(buildingId);
                _context.Buildings.Attach(building);
                _context.Buildings.Remove(building);
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

            return _context.Buildings.Count().ToString();

        }

        public Models.Building Read(int buildingId)
        {
            return _context.Buildings.FirstOrDefault(p => p.Id == buildingId);
        }

        public List<Models.Building> Find(string searchString)
        {

            return (from s in _context.Buildings where s.Name.Contains(searchString) orderby s.Name select s).ToList();

        }

        public bool Update(Models.Building building)
        {
            try
            {
                var originalBuilding = _context.Buildings.Find(building.Id);

                if (originalBuilding == null) return false;
                _context.Entry(originalBuilding).CurrentValues.SetValues(building);
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