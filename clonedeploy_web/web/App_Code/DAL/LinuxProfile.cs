using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Helpers;

namespace DAL
{
    public class LinuxProfile
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(Models.LinuxProfile profile)
        {
            return _context.LinuxProfiles.Any(p => p.Name == profile.Name);
        }

        public bool Create(Models.LinuxProfile newProfile)
        {
            try
            {
                _context.LinuxProfiles.Add(newProfile);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }

        public Models.LinuxProfile Read(int id)
        {
            return _context.LinuxProfiles.FirstOrDefault(p => p.Id == id);
        }

        public List<Models.LinuxProfile> Find(int imageId)
        {
            return (from p in _context.LinuxProfiles where p.ImageId == imageId orderby p.Name select p).ToList();
        }

        public bool Update(Models.LinuxProfile updatedProfile)
        {
            try
            {
                var profile = _context.LinuxProfiles.Find(updatedProfile.Id);

                if (profile == null) return false;
                _context.Entry(profile).CurrentValues.SetValues(updatedProfile);
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