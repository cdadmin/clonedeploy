/*  
    CrucibleWDS A Windows Deployment Solution
    Copyright (C) 2011  Jon Dolny

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/.
 */


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.IdentityModel.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Global;

namespace Models
{
    [Table("hosts", Schema = "public")]
    public class Host
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("hostid", Order = 1)]
        public int Id { get; set; }

        [Column("hostname", Order = 2)]
        public string Name { get; set; }

        [Column("hostmac", Order = 3)]
        public string Mac { get; set; }

        [Column("hostimage", Order = 4)]
        public string Image { get; set; }

        [Column("hostgroup", Order = 5)]
        public string Group { get; set; }

        [Column("hostdesc", Order = 6)]
        public string Description { get; set; }

        [Column("hostkernel", Order = 7)]
        public string Kernel { get; set; }

        [Column("hostbootimage", Order = 8)]
        public string BootImage { get; set; }

        [Column("hostarguments", Order = 9)]
        public string Args { get; set; }

        [Column("hostscripts", Order = 10)]
        public string Scripts { get; set; }

        [Column("custombootenabled", Order = 11)]
        public string CustomBootEnabled { get; set; }

        [Column("partitionscript", Order = 12)]
        public string PartitionScript { get; set; }

        [NotMapped]
        public string TaskId { get; set; }


        public string CheckActive()
        {
            using (var db = new DB())
            {
                var name =(from h in db.Hosts 
                           join t in db.ActiveTasks on h.Name equals t.Name
                           where (h.Mac == Mac)
                           select h.Name).FirstOrDefault();
                return name != null ? "Active" : "Inactive";
            }
        }

        public bool Create()
        {
            using (var db = new DB())
            {
                try
                {
                    if (db.Hosts.Any(h => h.Name == Name) || db.Hosts.Any(h => h.Mac == Mac))
                    {
                        Utility.Message = "This host already exists";
                        return false;
                    }
                    db.Hosts.Add(this);
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Create Host.  Check The Exception Log For More Info.";
                    return false;
                }
            }

            GetHostId();
            var history = new History { Event = "Create", Type = "Host", Notes = Mac, TypeId = Id.ToString() };
            history.CreateEvent();
            Utility.Message = "Successfully Created " + Name;
            return true;
        }



        public void Delete()
        {
            using (var db = new DB())
            {
                try
                {
                    db.Hosts.Attach(this);
                    db.Hosts.Remove(this);
                    db.SaveChanges();                 
                }

                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Delete Host.  Check The Exception Log For More Info.";
                    return;
                }

                Utility.Message = "Successfully Deleted " + Name;
                var history = new History { Event = "Delete", Type = "Host", Notes = Name, TypeId = Id.ToString() };
                history.CreateEvent();
            }
        }

        private void GetHostId()
        {
            using (var db = new DB())
            {
                var host = db.Hosts.First(h => h.Mac.ToUpper() == Mac.ToUpper());
                Id = host.Id;
            }          
        }

        public string GetTotalCount()
        {
            using (var db = new DB())
            {
                return db.Hosts.Count().ToString();
            }
        }

        public void Import()
        {
            var path = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                        Path.DirectorySeparatorChar + "csvupload" + Path.DirectorySeparatorChar;
            using (var db = new DB())
            {
                var importCount = db.Database.ExecuteSqlCommand("copy hosts(hostname,hostmac,hostimage,hostgroup,hostdesc,hostkernel,hostbootimage,hostarguments,hostscripts) from '" + path + "hosts.csv' DELIMITER ',' csv header force not null hostimage,hostgroup,hostdesc,hostkernel,hostbootimage,hostarguments,hostscripts;");
                Utility.Message = importCount + " Host(s) Imported Successfully";
            }       
        }

        public void Read()
        {
            if (string.IsNullOrEmpty(Id.ToString()))
                GetHostId();

            using (var db = new DB())
            {
                var host = db.Hosts.FirstOrDefault(h => h.Id == Id);
                if (host == null) return;
                Name = host.Name;
                Mac = host.Mac;
                Image = host.Image;
                Group = host.Group;
                Description = host.Description;
                Kernel = host.Kernel;
                BootImage = host.BootImage;
                Args = host.Args;
                Scripts = host.Scripts;
                CustomBootEnabled = host.CustomBootEnabled;
                PartitionScript = host.PartitionScript;
            }
        }

        public List<Host> Search(string searchString)
        {
            List<Host> list = new List<Host>();
            var user = new WdsUser { Name = HttpContext.Current.User.Identity.Name };
            user.Read();

            if (!string.IsNullOrEmpty(user.GroupManagement) && user.Membership == "User")
            {
                var listManagementGroups = user.GroupManagement.Split(' ').ToList();

                foreach (var id in listManagementGroups)
                {
                    var mgmtgroup = new Group { Id = id };
                    mgmtgroup.Read();
                    using (var db = new DB())
                    {
                        list.AddRange(from h in db.Hosts where (h.Name.Contains(searchString) || h.Mac.Contains(searchString)) && h.Group == mgmtgroup.Name orderby h.Name select h);
                    }
                }

                list = list.OrderBy(h => h.Name).ToList();
            }
            else
            {
                using (var db = new DB())
                {
                    list.AddRange(from h in db.Hosts where h.Name.Contains(searchString) || h.Mac.Contains(searchString) orderby h.Name select h);
                }
            }
            return list;
        }

        public void Update()
        {
            using (var db = new DB())
            {
                try
                {
                    var host = db.Hosts.Find(Id);
                    if (host != null)
                    {
                        host.Name = Name;
                        host.Mac = Mac;
                        host.Image = Image;
                        host.Group = Group;
                        host.Description = Description;
                        host.Kernel = Kernel;
                        host.BootImage = BootImage;
                        host.Args = Args;
                        host.Scripts = Scripts;
                        host.CustomBootEnabled = CustomBootEnabled;
                        host.PartitionScript = PartitionScript;
                        db.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Update Host.  Check The Exception Log For More Info.";
                    return;
                }
            }

            var history = new History
            {
                Event = "Edit",
                Type = "Host",
                Notes = Mac,
                TypeId = Id.ToString()
            };
            history.CreateEvent();
            Utility.Message = "Successfully Updated " + Name;
        }

        public bool ValidateHostData()
        {
            var validated = true;
            if (string.IsNullOrEmpty(Name) || Name.Contains(" "))
            {
                validated = false;
                Utility.Message = "Host Name Cannot Be Empty Or Contain Spaces";
            }
            if (string.IsNullOrEmpty(Mac) || Mac.Contains(" "))
            {
                validated = false;
                Utility.Message = "Host Mac Cannot Be Empty Or Contain Spaces";
            }
            if (Kernel == "Select Kernel")
            {
                validated = false;
                Utility.Message = "You Must Select A Kernel";
            }
            if (BootImage == "Select Boot Image")
            {
                validated = false;
                Utility.Message = "You Must Select A Boot Image";
            }

            return validated;
        }
    }
}