using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Global;
using Models;

namespace DataAccess
{
    public class LinuxProfileDataAccess
    {
        public bool ProfileExists(LinuxProfile profile)
        {
            using (var db = new DB())
            {
                return !db.LinuxProfiles.Any(p => p.Name == profile.Name);
            }
        }
        
        public bool Create(LinuxProfile newProfile)
        {
            using (var db = new DB())
            {
                try
                {
                    db.LinuxProfiles.Add(newProfile);
                    db.SaveChanges();
                    return true;
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    return false;
                }
            }
        }

    

        public string GetTotalCount()
        {
            using (var db = new DB())
            {
                return db.LinuxProfiles.Count().ToString();
            }
        }

        public LinuxProfile Read(int id)
        {
            using (var db = new DB())
            {
                return db.LinuxProfiles.FirstOrDefault(p => p.Id == id);

            }
        }

        public List<LinuxProfile> Search(int imageId)
        {
            List<LinuxProfile> list = new List<LinuxProfile>();

            using (var db = new DB())
            {
                list.AddRange(from p in db.LinuxProfiles where p.ImageId == imageId orderby p.Name select p);
            }

            return list;
        }

        public void Update(LinuxProfile updatedProfile)
        {
            using (var db = new DB())
            {
                try
                {

                    var profile = db.LinuxProfiles.Find(updatedProfile.Id);

                    if (profile != null)
                    {
                        db.Entry(profile).CurrentValues.SetValues(updatedProfile);
                        db.SaveChanges();

                    }
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Update Image Profile.";
                    return;
                }
            }

            var history = new History
            {
                Event = "Edit",
                Type = "Host",
                TypeId = updatedProfile.Id.ToString()
            };
            history.CreateEvent();
            Utility.Message = "Successfully Updated " + updatedProfile.Name;
        }
    }
}