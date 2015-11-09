using System;
using System.IO;
using System.Linq;
using DAL;
using Global;
using Helpers;
using Models;
using Pxe;

namespace Services.Client
{
    public class Global
    {
        public string IsLoginRequired(string task)
        {
            switch (task)
            {
                case "ond":
                    return Settings.OnDemandRequiresLogin;
                case "debug":
                    return Settings.DebugRequiresLogin;

                case "register":
                    return Settings.RegisterRequiresLogin;

                case "push":
                    return Settings.WebTaskRequiresLogin;

                case "pull":
                    return Settings.WebTaskRequiresLogin;

                default:
                    return "Yes";
            }
        }

        /*
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();
        public string CheckIn(string mac)
        {
            using (var db = new DB())
            {
                if (!_context.Computers.Any(h => h.Mac.ToLower() == mac.ToLower()))
                    return "checkInResult=\"The Host Was Not Found In The Database\"";

                var task = (from h in _context.Computers
                    join t in db.ActiveTasks on h.Id equals t.ComputerId
                    where (h.Mac.ToLower() == mac.ToLower())
                    select t).FirstOrDefault();

                if (task == null)
                    return "checkInResult=\"A Task Is Not Running For This Host\"";

                task.Status = "1";
                new BLL.ActiveImagingTask().UpdateActiveImagingTask(task);

                return "checkInResult=Success " + task.Arguments;

            }
          
        }

        public string CheckOut(string mac, string direction, string imgName)
        {
            string result = null;
          
            try
            {
                using (var db = new DB())
                {
                    var task = (from h in _context.Computers
                                join t in db.ActiveTasks on h.Id equals t.ComputerId
                                where (h.Mac.ToLower() == mac.ToLower())
                                select t).FirstOrDefault();
                    db.ActiveTasks.Remove(task);
                    db.SaveChanges();
                }
                var pxeHostMac = "01-" + mac.ToLower().Replace(':', '-');
                new PxeFileOps().CleanPxeBoot(pxeHostMac);

                if (direction == "pull")
                {
                    var xferMode = Settings.ImageTransferMode;
                    if (xferMode != "udp+http" && xferMode != "smb" && xferMode != "smb+http")
                    {
                        new FileOps().MoveFolder(Settings.ImageHoldPath + imgName,
                            Settings.ImageStorePath + imgName);
                        if ((Directory.Exists(Settings.ImageStorePath + imgName)) &&
                            (!Directory.Exists(Settings.ImageHoldPath + imgName)))
                        {
                            try
                            {
                                Directory.CreateDirectory(Settings.ImageHoldPath + imgName);
                                // for next upload
                                result = "Success";
                            }
                            catch (Exception ex)
                            {
                                Logger.Log(ex.Message);
                                result =
                                    "Could Not Recreate Directory,  You Must Create It Before You Can Upload Again";
                            }
                        }
                        else
                            result = "Could Not Move Image From Hold To Store.  You Must Do It Manually.";
                    }
                }
            }
            catch (Exception ex)
            {
                result = "Could Not Check Out.  Check The Exception Log For More Info";
                Logger.Log(ex.ToString());
            }

            return result;
        }

        public string GetSmbCredentials(string credential)
        {
            var xferMode = Settings.ImageTransferMode;
            if (xferMode != "smb" && xferMode != "smb+http")
            {
                Logger.Log("An Attempt Was Made To Access SMB Credentials But Current Image Transfer Mode Is Not SMB");
                return "";
            }


            switch (credential)
            {
                case "username":
                    return Settings.SmbUserName;
                case "password":
                    return Settings.SmbPassword;
            }

            return "";
        }

      

       
         * */
    }
}