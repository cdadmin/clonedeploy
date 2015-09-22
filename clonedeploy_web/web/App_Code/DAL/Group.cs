using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using Global;
using Models;

namespace DAL
{

    public class Group
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(Models.Group group)
        {
            return _context.Groups.Any(g => g.Name == group.Name);
        }

        public bool Create(Models.Group group)
        {
            try
            {
                _context.Groups.Add(group);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public bool Delete(int groupId)
        {
            try
            {
                var group = _context.Groups.Find(groupId);
                _context.Groups.Attach(group);
                _context.Groups.Remove(group);
                _context.SaveChanges();
                return true;
            }

            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

    

     

        public string GetMemberCount()
        {
            return "n/a";
        }

        public string GetTotalCount()
        {
            return _context.Groups.Count().ToString();
        }

        public void Import()
        {
            var path = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                         Path.DirectorySeparatorChar + "csvupload" + Path.DirectorySeparatorChar;
            using (var db = new DB())
            {
                var importCount = db.Database.ExecuteSqlCommand("copy groups(groupname,groupdesc,groupimage,groupkernel,groupbootimage,grouparguments,groupsenderargs,groupscripts) from '" + path + "hosts.csv' DELIMITER ',' csv header FORCE NOT NULL groupdesc,groupimage,groupkernel,groupbootimage,grouparguments,groupsenderargs,groupscripts;");
                Utility.Message = importCount + " Group(s) Imported Successfully";
            }
        }

        public Models.Group Read(int groupId)
        {
            return _context.Groups.FirstOrDefault(p => p.Id == groupId);
        }

        public List<Models.Group> Find(string searchString)
        {


            return (from g in _context.Groups where g.Name.Contains(searchString) orderby g.Name select g).ToList();

        }

        public List<Models.Computer> SearchSmartHosts(string searchString)
        {
         
            return new List<Models.Computer>();
        }

        public bool Update(Models.Group group)
        {
            try
            {
                var originalGroup = _context.Groups.Find(group.Id);

                if (originalGroup == null) return false;
                _context.Entry(originalGroup).CurrentValues.SetValues(group);
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