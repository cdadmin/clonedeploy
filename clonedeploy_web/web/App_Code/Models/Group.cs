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
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using Global;


namespace Models
{
    [Table("groups", Schema = "public")]
    public class Group
    {
        public Group()
        {
            Members = new List<Host>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("groupid", Order = 1)]
        public string Id { get; set; }

        [Column("groupname", Order = 2)]
        public string Name { get; set; }

        [Column("groupdesc", Order = 3)]
        public string Description { get; set; }

        [Column("groupimage", Order = 4)]
        public string Image { get; set; }

        [Column("groupkernel", Order = 5)]
        public string Kernel { get; set; }

        [Column("groupbootimage", Order = 6)]
        public string BootImage { get; set; }

        [Column("grouparguments", Order = 7)]
        public string Args { get; set; }

        [Column("groupsenderargs", Order = 8)]
        public string SenderArgs { get; set; }

        [Column("groupscripts", Order = 9)]
        public string Scripts { get; set; }

        [Column("grouptype", Order = 10)]
        public string Type { get; set; }

        [Column("groupexpression", Order = 11)]
        public string Expression { get; set; }

        [NotMapped] 
        public List<Host> Members { get; set; }

        public void Create()
        {
            var affectedRows = 0;
            using (var db = new DB())
            {
                try
                {
                    if (db.Groups.Any(g => g.Name == Name))
                    {
                        Utility.Message = "This Group already exists";
                        return;
                    }
                    else
                    {
                        db.Groups.Add(this);
                        affectedRows = db.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Create Group.  Check The Exception Log For More Info.";
                    return;
                }
            }

            if (affectedRows != 1) return;
            GetGroupId();
            var history = new History
            {
                Event = "Create",
                Type = "Group",
                TypeId = Id
            };
            history.CreateEvent();

            if (UpdateHosts(true))
                Utility.Message = "Successfully Created Group " + Name + " With " + Members.Count + " Hosts";
        }

        public void Delete()
        {
            using (var db = new DB())
            {
                try
                {
                    db.Groups.Attach(this);
                    db.Groups.Remove(this);
                    db.SaveChanges();
                }

                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Delete Group.  Check The Exception Log For More Info.";
                    return;
                }

                Utility.Message = "Successfully Deleted " + Name;
                var history = new History { Event = "Delete", Type = "Group", TypeId = Id };
                history.CreateEvent();
            }
        }

        public void GetGroupId()
        {
            using (var db = new DB())
            {
                var group = db.Groups.First(g => g.Name == Name);
                Id = group.Id;
            }          
        }

        public List<Host> GroupMembers()
        {
            var hosts = new List<Host>();

            switch (Type)
            {
                case "standard":
                    using (var db = new DB())
                    {
                        hosts.AddRange(from h in db.Hosts where h.Group == Name orderby h.Name select h);
                    }
                    break;
                case "smart":
                    if (string.IsNullOrEmpty(Expression)) return hosts;
                    hosts.AddRange(SearchSmartHosts(Expression));
                    break;
            }

            return hosts;
        }

        public string GetMemberCount()
        {
            using (var db = new DB())
            {
                return db.Hosts.Count(h => h.Group == Name).ToString();
            }
        }

        public string GetTotalCount()
        {
            using (var db = new DB())
            {
                return db.Groups.Count().ToString();
            }
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

        public void Read()
        {
            if (string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name))
                GetGroupId();

            using (var db = new DB())
            {
                var group = db.Groups.First(g => g.Id == Id);
                Name = group.Name;
                Image = group.Image;
                Description = group.Description;
                Kernel = group.Kernel;
                BootImage = group.BootImage;
                Args = group.Args;
                SenderArgs = group.SenderArgs;
                Scripts = group.Scripts;
                Type = group.Type;
                Expression = group.Expression;

            }        
        }

        public void RemoveGroupHosts()
        {
            foreach (var host in Members)
            {
                using (var db = new DB())
                {
                    try
                    {
                        var newHost = db.Hosts.Find(host.Id);            
                        newHost.Group = "";
                        db.SaveChanges();

                    }
                    catch (DbUpdateException ex)
                    {
                        Logger.Log(ex.InnerException.InnerException.Message);
                        Utility.Message = "Could Not Update Host.  Check The Exception Log For More Info.";
                        return;
                    }
                }
            }

            Utility.Message += "Successfully Removed " + Members.Count + " Host(s) From " + Name + "";    
        }

        public List<Group> Search(string searchString)
        {
            List<Group> list = new List<Group>();
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
                        list.AddRange(from g in db.Groups where g.Name.Contains(searchString) && g.Name == mgmtgroup.Name orderby g.Name select g);
                    }

                }
            }
            else
            {
                using (var db = new DB())
                {
                    list.AddRange(from g in db.Groups where g.Name.Contains(searchString) orderby g.Name select g);
                }
            }
       
            return list;
        }

        public List<Host> SearchSmartHosts(string searchString)
        {
            /*
            using (var db = new DB())
            {
                return
                    db.Hosts.SqlQuery(
                        "SELECT cast(hostid as text) as Id, hostname as Name, hostmac as Mac, hostimage as Image, hostgroup as Group, hostdesc as Description, hostkernel as Kernel, hostbootimage as BootImage, hostarguments as Args, hostscripts as Scripts, custombootenabled as CustomBootEnabled FROM public.hosts WHERE hostname ~* @searchstring ORDER BY hostname;",
                        new NpgsqlParameter("searchstring", searchString)).ToList();
            }
             * */
            return new List<Host>();
        }

        public bool Update()
        {
            using (var db = new DB())
            {
                try
                {
                    var group = db.Groups.Find(Id);
                    if (group != null)
                    {
                        group.Name = Name;
                        group.Image = Image;
                        group.Description = Description;
                        group.Kernel = Kernel;
                        group.BootImage = BootImage;
                        group.Args = Args;
                        group.Scripts = Scripts;
                        group.SenderArgs = SenderArgs;
                        group.Type = Type;
                        group.Expression = Expression;
                        db.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Update Group.  Check The Exception Log For More Info.";
                    return false;
                }
                var history = new History
                {
                    Event = "Edit",
                    Type = "Group",
                    TypeId = Id
                };
                history.CreateEvent();
            }
            return true;
        }

        public bool UpdateHosts(bool isAdd)
        {
            foreach (var host in Members)
            {
                using (var db = new DB())
                {
                    try
                    {
                        var newHost = db.Hosts.Find(host.Id);
                        if (Type == "standard")
                            newHost.Group = Name;
                        newHost.Image = Image;
                        newHost.Kernel = Kernel;
                        newHost.BootImage = BootImage;
                        newHost.Args = Args;
                        newHost.Scripts = Scripts;
                        db.SaveChanges();

                    }
                    catch (DbUpdateException ex)
                    {
                        Logger.Log(ex.InnerException.InnerException.Message);
                        Utility.Message = "Could Not Update Host.  Check The Exception Log For More Info.";
                        return false;
                    }
                }

                var history = new History
                {
                    Event = "Edit",
                    Type = "Host",
                    Notes = "Via Group Update " + Name,
                    EventUser = HttpContext.Current.User.Identity.Name,
                    TypeId = host.Id.ToString()
                };
                history.CreateEvent();

                if (Type == "standard")
                {
                    if (isAdd)
                        Utility.Message += "Successfully Added " + Members.Count + " Host(s) To " + Name + "<br>";
                }
            }
            return true;
        }

        public bool ValidateGroupData()
        {
            var validated = true;
            if (string.IsNullOrEmpty(Name) || Name.Contains(" "))
            {
                validated = false;
                Utility.Message = "Group Name Cannot Be Empty Or Contain Spaces";
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