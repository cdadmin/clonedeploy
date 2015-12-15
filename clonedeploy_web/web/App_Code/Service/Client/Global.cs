using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Forms;
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

        public void UpdateProgress(int computerId, string progress, string progressType)
        {
            var task = BLL.ActiveImagingTask.GetTask(computerId);
            if (progressType == "wim")
            {
                task.Elapsed = progress;
                task.Remaining = "";
                task.Completed = "";
                task.Rate = "";
            }
            else
            {
                var values = progress.Split('*').ToList();
                task.Elapsed = values[1];
                task.Remaining = values[2];
                task.Completed = values[3];
                task.Rate = values[4];
            }

            BLL.ActiveImagingTask.UpdateActiveImagingTask(task);
        }

        public void UpdateProgressPartition(int computerId, string partition)
        {
            var task = BLL.ActiveImagingTask.GetTask(computerId);
            task.Partition = partition;
            task.Elapsed = "Please Wait...";
            task.Remaining = "";
            task.Completed = "";
            task.Rate = "";
            BLL.ActiveImagingTask.UpdateActiveImagingTask(task);
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
        
        public void CheckOut(string mac)
        {
            var computer = BLL.Computer.GetComputerFromMac(mac);
            var computerTask = BLL.ActiveImagingTask.GetTask(computer.Id);
            BLL.ActiveImagingTask.DeleteActiveImagingTask(computerTask.Id);    
        }

        public void UploadLog(int computerId, string logContents, string subType)
        {
            if (computerId == -1)
            {
                return;
            }
            else
            {
                var computerLog = new Models.ComputerLog
                {
                    ComputerId = computerId,
                    Contents = logContents,
                    Type = "image",
                    SubType = subType
                };
                BLL.ComputerLog.AddComputerLog(computerLog);
            }
        }

        public string OnDemandTaskArguments(string mac, int profileId, string taskType)
        {
            var computer = BLL.Computer.GetComputerFromMac(mac);
            var imageProfile = BLL.ImageProfile.ReadProfile(profileId);
            return new BLL.Workflows.CreateTaskArguments(computer, imageProfile, taskType).Run();
            
        }

        public string UploadFile(string fileName, string imagePath, string fileType, HttpFileCollection files)
        {

            try
            {
                string fullPath;

               
            

                if (files.Count == 1 && files[0].ContentLength > 0 && !string.IsNullOrEmpty(fileName))
                {
                    var binaryWriteArray = new
                        byte[files[0].InputStream.Length];
                    files[0].InputStream.Read(binaryWriteArray, 0, (int)files[0].InputStream.Length);

                    var str = System.Text.Encoding.Default.GetString(binaryWriteArray);

                 
                }
                return "File Was Not Posted Successfully";
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return "Check The Exception Log For More Info";
            }
        }

        public string CheckQueue(int computerId)
        {
            //Check if already part of the queue
            var thisComputerTask = BLL.ActiveImagingTask.GetTask(computerId);
            if (thisComputerTask.Status == "2")
            {
                //Check if the queue is open yet
                var inUse = BLL.ActiveImagingTask.GetCurrentQueue();
                var totalCapacity = Convert.ToInt32(Settings.QueueSize);
                if (inUse < totalCapacity)
                {
                    //queue is open, is this computer next
                    var firstTaskInQueue = BLL.ActiveImagingTask.GetNextComputerInQueue();
                    if (firstTaskInQueue.ComputerId == computerId)
                    {
                        ChangeStatusInProgress(computerId);
                        return "true";
                    }
                    else
                    {
                        //not time for this computer yet
                        var queuePosition = BLL.ActiveImagingTask.GetQueuePosition(computerId);
                        return "false" + queuePosition;
                    }
                }
                else
                {
                    //queue not open yet
                    var queuePosition = BLL.ActiveImagingTask.GetQueuePosition(computerId);
                    return "false" + queuePosition;
                }
            }
            else
            {
                //New computer checking queue for the first time

                var inUse = BLL.ActiveImagingTask.GetCurrentQueue();
                var totalCapacity = Convert.ToInt32(Settings.QueueSize);
                if (inUse < totalCapacity)
                {
                    ChangeStatusInProgress(computerId);

                    return "true";

                }
                else
                {
                    //place into queue
                    var lastQueuedTask = BLL.ActiveImagingTask.GetLastQueuedTask();
                    if (lastQueuedTask == null)
                        thisComputerTask.QueuePosition = 1;
                    else
                        thisComputerTask.QueuePosition = lastQueuedTask.QueuePosition + 1;
                    thisComputerTask.Status = "2";
                    BLL.ActiveImagingTask.UpdateActiveImagingTask(thisComputerTask);

                    var queuePosition = BLL.ActiveImagingTask.GetQueuePosition(computerId);
                    return "false" + queuePosition;

                }
            }

        }
        /*
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