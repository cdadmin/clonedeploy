using System.Collections.Generic;
using DAL;
using Global;
using Models;

namespace BLL
{
    public class LinuxProfile
    {
        private readonly DAL.LinuxProfile _da = new DAL.LinuxProfile();

        public bool AddProfile(Models.LinuxProfile profile)
        {
            if (_da.Exists(profile))
            {
                Utility.Message = "A Profile With This Name Already Exists";
                return false;
            }
            if (_da.Create(profile))
            {
                Utility.Message = "Successfully Created Profile";
                return true;
            }
            Utility.Message = "Could Not Create Profile";
            return false;
        }

        public Models.LinuxProfile ReadProfile(int profileId)
        {
            return _da.Read(profileId);
        }

        public List<Models.LinuxProfile> SearchProfiles(int imageId)
        {
            return _da.Find(imageId);
        }

        public void UpdateProfile(Models.LinuxProfile profile)
        {
            _da.Update(profile);
        }
    }
}