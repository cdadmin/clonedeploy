using DataAccess;
using Models;

namespace Logic
{
    /// <summary>
    /// Summary description for LinuxProfileLogic
    /// </summary>
    public class LinuxProfileLogic
    {
        public string AddProfile(LinuxProfile profile)
        {
            var da = new LinuxProfileDataAccess();
            if (da.ProfileExists(profile))
                return "A Profile With This Name Already Exists";
            if (da.Create(profile))
            {
                //var history = new History { Event = "Create", Type = "Host", Notes = Mac, TypeId = Id.ToString() };
                //history.CreateEvent();
                return "Successfully Created Profile";
               
            }
            else
            {
                return "Could Not Create Profile";
            }
        }

        public LinuxProfile ReadProfile(int profileId)
        {
            var da = new LinuxProfileDataAccess();
            return da.Read(profileId);
        }
    }
}