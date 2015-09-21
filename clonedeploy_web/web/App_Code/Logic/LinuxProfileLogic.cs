using System.Collections.Generic;
using DataAccess;
using Global;
using Models;

namespace Logic
{
    public class LinuxProfileLogic
    {
        private readonly LinuxProfileDataAccess _da = new LinuxProfileDataAccess();

        public bool AddProfile(LinuxProfile profile)
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

        public LinuxProfile ReadProfile(int profileId)
        {
            return _da.Read(profileId);
        }

        public List<LinuxProfile> SearchProfiles(int imageId)
        {
            return _da.Find(imageId);
        }

        public void UpdateProfile(LinuxProfile profile)
        {
            _da.Update(profile);
        }
    }
}