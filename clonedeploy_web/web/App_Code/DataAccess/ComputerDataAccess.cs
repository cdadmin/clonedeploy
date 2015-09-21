using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using Global;
using Models;

namespace DataAccess
{

    public class ComputerDataAccess
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public string CheckActive(string mac)
        {
            var name = (from h in _context.Computers
                join t in _context.ActiveTasks on h.Id equals t.ComputerId
                where (h.Mac == mac)
                select h.Name).FirstOrDefault();
            return name;
        }

        public bool Exists(Computer computer)
        {
            return _context.Computers.Any(h => h.Name == computer.Name || h.Mac == computer.Mac);
        }

        public bool Create(Computer computer)
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
            var path = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                       Path.DirectorySeparatorChar + "csvupload" + Path.DirectorySeparatorChar;
            using (var db = new DB())
            {
                var importCount =
                    db.Database.ExecuteSqlCommand(
                        "copy hosts(hostname,hostmac,hostimage,hostgroup,hostdesc,hostkernel,hostbootimage,hostarguments,hostscripts) from '" +
                        path +
                        "hosts.csv' DELIMITER ',' csv header force not null hostimage,hostgroup,hostdesc,hostkernel,hostbootimage,hostarguments,hostscripts;");
                Utility.Message = importCount + " Host(s) Imported Successfully";
            }
        }

        public Computer Read(int computerId)
        {
            return _context.Computers.FirstOrDefault(p => p.Id == computerId);
        }

        public List<Computer> Find(string searchString)
        {
            return (from h in _context.Computers
                where h.Name.Contains(searchString) || h.Mac.Contains(searchString)
                orderby h.Name
                select h).ToList();
        }

        public Computer GetComputerFromMac(string mac)
        {
            return _context.Computers.FirstOrDefault(p => p.Mac == mac);
        }
        public bool Update(Computer computer)
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