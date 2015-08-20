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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using Global;

namespace Models
{
    [Table("clonedeploy_users", Schema = "public")]
    public class WdsUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_id", Order = 1)]
        public string Id { get; set; }

        [Column("clonedeploy_username", Order = 2)]
        public string Name { get; set; }

        [Column("clonedeploy_user_pwd", Order = 3)]
        public string Password { get; set; }

        [Column("clonedeploy_user_salt", Order = 4)]
        public string Salt { get; set; }

        [Column("clonedeploy_user_role", Order = 5)]
        public string Membership { get; set; }

        [NotMapped]
        public string GroupManagement { get; set; }

        [NotMapped]
        public string OndAccess { get; set; }

        [NotMapped]
        public string DebugAccess { get; set; }

        [NotMapped]
        public string DiagAccess { get; set; }
        
        public void Create()
        {
            using (var db = new DB())
            {
                try
                {                 
                    if (db.Users.Any(u => u.Name.ToLower() == Name.ToLower()))
                    {
                        Utility.Message = "This user already exists";
                        return;
                    }
                    Password = CreatePasswordHash(Password, Salt);
                    db.Users.Add(this);
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Create User.  Check The Exception Log For More Info.";
                    return;
                }
            }

            GetId();
            var history = new History
            {
                Event = "Create",
                Type = "User",
                TypeId = Id
            };
            history.CreateEvent();
            Utility.Message = "Successfully Created " + Name;
        }

        public string CreatePasswordHash(string pwd, string salt)
        {
            var saltAndPwd = string.Concat(pwd, salt);
            var hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");
            return hashedPwd;
        }

        public string CreateSalt(int byteSize)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[byteSize];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public void Delete()
        {
            using (var db = new DB())
            {
                try
                {
                    db.Users.Attach(this);
                    db.Users.Remove(this);
                    db.SaveChanges();
                }

                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Delete User.  Check The Exception Log For More Info.";
                    return;
                }

                Utility.Message = "Successfully Deleted " + Name;
                var history = new History
                {
                    Event = "Delete",
                    Type = "User",
                    TypeId = Id
                };
                history.CreateEvent();
            }
        }

        public int GetAdminCount()
        {
            using (var db = new DB())
            {
                return db.Users.Count(u => u.Membership == "Administrator");
            }
        }

        public void GetId()
        {
            using (var db = new DB())
            {
                var WDSUser = db.Users.FirstOrDefault(u => u.Name == Name);
                if(WDSUser != null)
                    Id = WDSUser.Id;
            }          
        }

        public string GetTotalCount()
        {
            using (var db = new DB())
            {
                return db.Users.Count().ToString();
            }
        }

        public void Import()
        {
            var path = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                       Path.DirectorySeparatorChar + "csvupload" + Path.DirectorySeparatorChar + "users.csv"; 
            using (var db = new DB())
            {
                var importCount = db.Database.ExecuteSqlCommand("copy users(username,userpwd,usersalt,usermembership) from '" + path + "' DELIMITER ',' csv header;");
                Utility.Message = importCount + " User(s) Imported Successfully";
            }       
        }

        public void Read()
        {
            if (string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name))
                GetId();

            using (var db = new DB())
            {
                var wdsUser = db.Users.FirstOrDefault(u => u.Id == Id);
                if (wdsUser != null)
                {
                    Name = wdsUser.Name;
                    Membership = wdsUser.Membership;
                    GroupManagement = wdsUser.GroupManagement;
                    OndAccess = wdsUser.OndAccess;
                    DebugAccess = wdsUser.DebugAccess;
                    DiagAccess = wdsUser.DiagAccess;
                    Password = wdsUser.Password;
                    Salt = wdsUser.Salt;
                }
            }
        }

        public List<WdsUser> Search(string searchString)
        {
            List<WdsUser> list = new List<WdsUser>();
            using (var db = new DB())
            {
                list.AddRange(from u in db.Users where u.Name.Contains(searchString) orderby u.Name select u);
            }
         
            return list;
        }

        public void Update(bool updatePassword)
        {
            using (var db = new DB())
            {
                try
                {
                    var wdsUser = db.Users.Find(Id);
                    if (wdsUser != null)
                    {
                        if (updatePassword)
                        {
                            wdsUser.Password = CreatePasswordHash(Password, Salt);
                            wdsUser.Salt = Salt;
                        }
                        wdsUser.Name = Name;
                        wdsUser.Membership = Membership;
                        wdsUser.GroupManagement = GroupManagement;
                        wdsUser.OndAccess = OndAccess;
                        wdsUser.DiagAccess = DiagAccess;
                        wdsUser.DebugAccess = DebugAccess;
                        db.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Update User.  Check The Exception Log For More Info.";
                    return;
                }

                var history = new History
                {
                    Event = "Edit",
                    Type = "User",
                    TypeId = Id
                };
                history.CreateEvent();
                Utility.Message = "Successfully Updated " + Name;
            }
        }

        public bool ValidateUserData()
        {
            var validated = true;
            if (string.IsNullOrEmpty(Name) || Name.Contains(" "))
            {
                validated = false;
                Utility.Message = "User Name Cannot Be Empty Or Contain Spaces";
            }
            if (string.IsNullOrEmpty(Password) || Password.Contains(" "))
            {
                validated = false;
                Utility.Message = "User Password Cannot Be Empty Or Contain Spaces";
            }

            return validated;
        }
    }
}