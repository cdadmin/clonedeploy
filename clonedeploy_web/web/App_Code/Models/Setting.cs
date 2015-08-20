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
    [Table("admin_settings")]
    public class Setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("admin_setting_id", Order = 1)]
        public string Id { get; set; }

        [Column("admin_setting_name", Order = 2)]
        public string Name { get; set; }

        [Column("admin_setting_value", Order = 3)]
        public string Value { get; set; }

        [Column("admin_setting_category", Order = 4)]
        public string Category { get; set; }

        public void ExportDatabase()
        {
            var path = HttpContext.Current.Server.MapPath("~") + "data" +
                       Path.DirectorySeparatorChar + "dbbackup" + Path.DirectorySeparatorChar;
            using (var db = new DB())
            {
               
                db.Database.ExecuteSqlCommand("COPY (select imagename,imageos,imagedesc,imageclientsize,imageclientsizecustom,checksum from images) to '" + path + "images.csv' WITH CSV HEADER;");
                db.Database.ExecuteSqlCommand("COPY (select username,userpwd,usersalt,usermembership from users) to '" + path + "users.csv' WITH CSV HEADER;");
                db.Database.ExecuteSqlCommand("COPY (select groupname,groupdesc,groupimage,groupkernel,groupbootimage,grouparguments,groupsenderargs,groupscripts from groups) to '" + path + "groups.csv' WITH CSV HEADER;");
                db.Database.ExecuteSqlCommand("COPY (select hostname,hostmac,hostimage,hostgroup,hostdesc,hostkernel,hostbootimage,hostarguments,hostscripts from hosts) to '" + path + "hosts.csv' WITH CSV HEADER;");
                Utility.Message = "Backup Complete.  Located At: " +
                                     (HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                                      Path.DirectorySeparatorChar + "dbbackup").Replace(@"\", @"\\");
            }       
        }

        public static string GetServerIpWithPort()
        {
            var ipAddress = Settings.ServerIp;
            var port = Settings.WebServerPort;
            if (port != "80" && port != "443" && !string.IsNullOrEmpty(port))
            {
                ipAddress += ":" + port;
            }

            return ipAddress;
        }

        public static string Read(string settingName)
        {
            string settingValue;
            using (var db = new DB())
            {
                var settings = db.Settings.First(s => s.Name == settingName);
                settingValue = settings.Value;
            }

            //Handle replacement of [server-ip] placeholder as well as ip addresses on different ports
            if (settingName != "Web Path" && settingName != "Nfs Upload Path" && settingName != "Nfs Deploy Path" &&
                settingName != "SMB Path") return settingValue;
            if (settingValue.Contains("[server-ip]"))
            {
                settingValue = settingValue.Replace("[server-ip]", Read("Server IP"));
            }
            return settingValue;
        }

        public static string GetValueForAdminView(string settingValue)
        {
            if (settingValue.Contains(Settings.ServerIp))
                return settingValue.Replace(Settings.ServerIp, "[server-ip]");

            return settingValue;
        }

        public bool Update(List<Setting> settings )
        {
            foreach (var setting in settings)
            {
                using (var db = new DB())
                {
                    try
                    {
                        var oldSetting = db.Settings.First(s => s.Name == setting.Name);
                        oldSetting.Value = setting.Value;
                        db.SaveChanges();
                    }
                    catch (DbUpdateException ex)
                    {
                        Logger.Log(ex.InnerException.InnerException.Message);
                        Utility.Message = "Could Not Update Settings.  Check The Exception Log For More Info.";
                        return false;
                    }
                }
            }
            Utility.Message = "Successfully Updated Settings"; 
            return true;
        }
    }
}