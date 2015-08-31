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
    [Table("computers", Schema = "public")]
    public class Computer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_id", Order = 1)]
        public int Id { get; set; }

        [Column("computer_name", Order = 2)]
        public string Name { get; set; }

        [Column("computer_primary_mac", Order = 3)]
        public string Mac { get; set; }

        [Column("computer_description", Order = 4)]
        public string Description { get; set; }

        [Column("computer_building_id", Order = 5)]
        public int Building { get; set; }

        [Column("computer_room_id", Order = 6)]
        public int Room { get; set; }

        [Column("computer_image_id", Order = 7)]
        public int Image { get; set; }

        [Column("computer_image_profile_id", Order = 8)]
        public int ImageProfile { get; set; }

        [NotMapped]
        public string Group { get; set; }

        [NotMapped]
        public string Kernel { get; set; }

        [NotMapped]
        public string BootImage { get; set; }

        [NotMapped]
        public string Args { get; set; }

        [NotMapped]
        public string Scripts { get; set; }

        [NotMapped]
        public string CustomBootEnabled { get; set; }

        [NotMapped]
        public string PartitionScript { get; set; }
        [NotMapped]
        public string TaskId { get; set; }


        public string CheckActive()
        {
            using (var db = new DB())
            {
                var name =(from h in db.Hosts 
                           join t in db.ActiveTasks on h.Id equals t.ComputerId
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

        public List<Computer> Search(string searchString)
        {
            List<Computer> list = new List<Computer>();
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