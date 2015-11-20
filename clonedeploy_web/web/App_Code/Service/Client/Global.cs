using System;
using System.IO;
using System.Linq;
using DAL;
using Global;
using Helpers;
using Models;
using Newtonsoft.Json;
using Pxe;
using Services.Client;

namespace Service.Client
{
    public class Global
    {
        public string AddComputer(string name, string mac, string imageId, string imageProfileId)
        {
            var computer = new Models.Computer
            {
                Name = name,
                Mac = mac,
                ImageId = Convert.ToInt32(imageId),
                ImageProfile = Convert.ToInt32(imageProfileId)

            };
            var result = BLL.Computer.AddComputer(computer);
            return JsonConvert.SerializeObject(result);
        }

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


        public string CheckIn(string computerMac)
        {
            var checkIn = new Services.Client.CheckIn();

            var computer = BLL.Computer.GetComputerFromMac(computerMac);
            if (computer == null)
            {
                checkIn.Result = "false";
                checkIn.Message = "This Computer Was Not Found";
                return JsonConvert.SerializeObject(checkIn);
            }

            var computerTask = BLL.ActiveImagingTask.GetTask(computer.Id);
            if (computerTask == null)
            {
                checkIn.Result = "false";
                checkIn.Message = "An Active Task Was Not Found For This Computer";
                return JsonConvert.SerializeObject(checkIn);
            }

            computerTask.Status = "1";
            if (BLL.ActiveImagingTask.UpdateActiveImagingTask(computerTask))
            {
                checkIn.Result = "true";
                checkIn.TaskArguments = computerTask.Arguments;
                return JsonConvert.SerializeObject(checkIn);
            }
            else
            {
                checkIn.Result = "false";
                checkIn.Message = "Could Not Update Task Status";
                return JsonConvert.SerializeObject(checkIn);
            }


        }

        public string DistributionPoint(int dpId)
        {
            var smb = new Services.Client.SMB();
            var dp = BLL.DistributionPoint.GetDistributionPoint(dpId);
            smb.SharePath = "//" + ParameterReplace.Between(dp.Server) + "/" + dp.ShareName;
            smb.Domain = dp.Domain;
            smb.Username = dp.Username;
            smb.Password = dp.Password;
            return JsonConvert.SerializeObject(smb);


        }

        public void ChangeStatusInProgress(int computerId)
        {
            var computerTask = BLL.ActiveImagingTask.GetTask(computerId);
            computerTask.Status = "3";
            BLL.ActiveImagingTask.UpdateActiveImagingTask(computerTask);
        }

        public void DeleteImage(int profileId)
        {
            var image = BLL.ImageProfile.ReadProfile(profileId).Image;
            try
            {
                if (Directory.Exists(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + image.Name))
                    Directory.Delete(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + image.Name, true);
                Directory.CreateDirectory(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + image.Name);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }
        /*
       

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