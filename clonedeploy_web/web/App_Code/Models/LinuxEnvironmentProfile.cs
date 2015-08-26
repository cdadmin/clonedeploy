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
    [Table("linux_profiles", Schema = "public")]
    public class LinuxEnvironmentProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_profile_id", Order = 1)]
        public int Id { get; set; }

        [Column("profile_name", Order = 2)]
        public string Name { get; set; }

        [Column("profile_description", Order = 3)]
        public string Description { get; set; }

        [Column("image_id", Order = 4)]
        public int ImageId { get; set; }

        [Column("profile_kernel", Order = 5)]
        public string Kernel { get; set; }

        [Column("profile_boot_image", Order = 6)]
        public string BootImage { get; set; }

        public bool Create()
        {
            using (var db = new DB())
            {
                try
                {
                    if (db.ImageProfiles.Any(p => p.Name == Name))
                    {
                        Utility.Message = "This Profile Already Exists";
                        return false;
                    }
                    db.ImageProfiles.Add(this);
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Create Profile.  Check The Exception Log For More Info.";
                    return false;
                }
            }

            GetProfileId();
            //var history = new History { Event = "Create", Type = "Host", Notes = Mac, TypeId = Id.ToString() };
            //history.CreateEvent();
            Utility.Message = "Successfully Created " + Name;
            return true;
        }

        private void GetProfileId()
        {
            using (var db = new DB())
            {
                var profile = db.ImageProfiles.First(p => p.Name == Name);
                Id = profile.Id;
            }
        }

        public string GetTotalCount()
        {
            using (var db = new DB())
            {
                return db.ImageProfiles.Count().ToString();
            }
        }

        public List<LinuxEnvironmentProfile> Search(int imageId)
        {
            List<LinuxEnvironmentProfile> list = new List<LinuxEnvironmentProfile>();


         
                using (var db = new DB())
                {
                    list.AddRange(from p in db.ImageProfiles where p.ImageId == imageId orderby p.Name select p);
                }
            
            return list;
        }
    }
}