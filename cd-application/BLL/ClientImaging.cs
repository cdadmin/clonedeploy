using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BLL.DynamicClientPartition;
using Helpers;
using Models;
using Newtonsoft.Json;
using Services.Client;
using ImageProfileFileFolder = BLL.ImageProfileFileFolder;

namespace Service.Client
{
    public class Logic
    {
        public bool Authorize(string token)
        {
            if (token == Settings.UniversalToken && !string.IsNullOrEmpty(Settings.UniversalToken))
                return true;
            
            var user = BLL.User.GetUserByToken(token);
            if (user != null)
                return true;

            return false;
        }
        public string AddComputer(string name, string mac)
        {
            var computer = new Models.Computer
            {
                Name = name,
                Mac = mac,
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
                case "clobber":
                    return Settings.ClobberRequiresLogin;
                case "push":
                    return Settings.WebTaskRequiresLogin;
                case "permanent_push":
                    return Settings.WebTaskRequiresLogin;
                case "pull":
                    return Settings.WebTaskRequiresLogin;

                default:
                    return "Yes";
            }
        }

        public string CheckTaskAuth(string task, string token)
        {
            //only check ond and debug because web tasks can't even be started if user isn't authorized
            if (task == "ond" && Settings.OnDemand != "Enabled")
            {
                return "false";
            }

            if (task == "ond" && Settings.OnDemandRequiresLogin == "No")
            {
                if (token == Settings.UniversalToken && !string.IsNullOrEmpty(Settings.UniversalToken))
                    return "true";
            }
            else if (task == "ond" && Settings.OnDemandRequiresLogin == "Yes")
            {
                var user = BLL.User.GetUserByToken(token);
                if (user != null)
                {
                    if (new BLL.Authorize(user, Authorizations.AllowOnd).IsAuthorized())
                        return "true";
                }
            }
            else if (task == "debug" && Settings.DebugRequiresLogin == "No")
            {
                if (token == Settings.UniversalToken && !string.IsNullOrEmpty(Settings.UniversalToken))
                    return "true";
            }
            else if (task == "debug" && Settings.DebugRequiresLogin == "Yes")
            {
                var user = BLL.User.GetUserByToken(token);
                if (user != null)
                {
                    if (new BLL.Authorize(user, Authorizations.AllowDebug).IsAuthorized())
                        return "true";
                }
            }
            else if (task == "clobber" && Settings.ClobberRequiresLogin == "No")
            {
                if (token == Settings.UniversalToken && !string.IsNullOrEmpty(Settings.UniversalToken))
                    return "true";
            }
            else if (task == "clobber" && Settings.ClobberRequiresLogin == "Yes")
            {
                var user = BLL.User.GetUserByToken(token);
                if (user != null)
                {
                    if (new BLL.Authorize(user, Authorizations.ImageDeployTask).IsAuthorized())
                        return "true";
                }
            }
           
            return "false";
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
                var image = BLL.Image.GetImage(computer.ImageId);
                if (image != null)
                {
                    if (image.Environment == "")
                        image.Environment = "linux";
                    checkIn.ImageEnvironment = image.Environment;
                }
                
                checkIn.TaskType = computerTask.Type;
                return JsonConvert.SerializeObject(checkIn);
            }
            else
            {
                checkIn.Result = "false";
                checkIn.Message = "Could Not Update Task Status";
                return JsonConvert.SerializeObject(checkIn);
            }


        }

        public string GetMunkiBasicAuth(int profileId)
        {
            var imageProfile = BLL.ImageProfile.ReadProfile(profileId);
            var authString = imageProfile.MunkiAuthUsername + ":" + imageProfile.MunkiAuthPassword;
            return Helpers.Utility.Encode(authString);
        }

        public string DistributionPoint(int dpId, string task)
        {
            var smb = new Services.Client.SMB();
            var dp = BLL.DistributionPoint.GetDistributionPoint(dpId);
            smb.SharePath = "//" + ParameterReplace.Between(dp.Server) + "/" + dp.ShareName;
            smb.Domain = dp.Domain;
            if (task == "pull")
            {
                smb.Username = dp.RwUsername;
                smb.Password = new Helpers.Encryption().DecryptText(dp.RwPassword);
            }
            else
            {
                smb.Username = dp.RoUsername;
                smb.Password = new Helpers.Encryption().DecryptText(dp.RoPassword);
            }
            
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
            var profile = BLL.ImageProfile.ReadProfile(profileId);
            if (string.IsNullOrEmpty(profile.Image.Name)) return;
            //Remove existing custom deploy schema, it may not match newly updated image
            profile.CustomSchema = string.Empty;
            BLL.ImageProfile.UpdateProfile(profile);
            try
            {
                if (Directory.Exists(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + profile.Image.Name))
                    Directory.Delete(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + profile.Image.Name, true);
                Directory.CreateDirectory(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + profile.Image.Name);
                new FileOps().SetUnixPermissionsImage(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + profile.Image.Name);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        public void ErrorEmail(int computerId, string error)
        {
            var computerTask = BLL.ActiveImagingTask.GetTask(computerId);
            BLL.ActiveImagingTask.SendTaskErrorEmail(computerTask,error);
        }

        public void CheckOut(int computerId)
        {
            var computerTask = BLL.ActiveImagingTask.GetTask(computerId);
            BLL.ActiveImagingTask.DeleteActiveImagingTask(computerTask.Id);
            if(computerTask.Type == "unicast")
                BLL.ActiveImagingTask.SendTaskCompletedEmail(computerTask);
        }

        public void PermanentTaskCheckOut(int computerId)
        {
            var computerTask = BLL.ActiveImagingTask.GetTask(computerId);
            computerTask.Status = "0";
            computerTask.Partition = "";
            computerTask.Completed = "";
            computerTask.Elapsed = "";
            computerTask.Rate = "";
            computerTask.Remaining = "";
            BLL.ActiveImagingTask.UpdateActiveImagingTask(computerTask);
           
            BLL.ActiveImagingTask.SendTaskCompletedEmail(computerTask);
        }

        public void UploadLog(int computerId, string logContents, string subType, string computerMac)
        {
            var computerLog = new Models.ComputerLog
            {
                ComputerId = computerId,
                Contents = logContents,
                Type = "image",
                SubType = subType,
                Mac = computerMac
            };
            BLL.ComputerLog.AddComputerLog(computerLog);

        }

        public string CheckQueue(int computerId)
        {
            var queueStatus = new Services.Client.QueueStatus();

            //Check if already part of the queue
            var thisComputerTask = BLL.ActiveImagingTask.GetTask(computerId);
            if (thisComputerTask.Status == "2")
            {
                //Check if the queue is open yet
                var inUse = BLL.ActiveImagingTask.GetCurrentQueue(thisComputerTask.Type);
                var totalCapacity = Convert.ToInt32(Settings.QueueSize);
                if (inUse < totalCapacity)
                {
                    //queue is open, is this computer next
                    var firstTaskInQueue = BLL.ActiveImagingTask.GetNextComputerInQueue(thisComputerTask.Type);
                    if (firstTaskInQueue.ComputerId == computerId)
                    {
                        ChangeStatusInProgress(computerId);
                        queueStatus.Result = "true";
                        queueStatus.Position = "0";
                        return JsonConvert.SerializeObject(queueStatus);
                    }
                    else
                    {
                        //not time for this computer yet
                        queueStatus.Result = "false";
                        queueStatus.Position = BLL.ActiveImagingTask.GetQueuePosition(computerId); 
                        return JsonConvert.SerializeObject(queueStatus);
                    }
                }
                else
                {
                    //queue not open yet
                    queueStatus.Result = "false";
                    queueStatus.Position = BLL.ActiveImagingTask.GetQueuePosition(computerId);
                    return JsonConvert.SerializeObject(queueStatus);
                }
            }
            else
            {
                //New computer checking queue for the first time

                var inUse = BLL.ActiveImagingTask.GetCurrentQueue(thisComputerTask.Type);
                var totalCapacity = Convert.ToInt32(Settings.QueueSize);
                if (inUse < totalCapacity)
                {
                    ChangeStatusInProgress(computerId);

                    queueStatus.Result = "true";
                    queueStatus.Position = "0";
                    return JsonConvert.SerializeObject(queueStatus);

                }
                else
                {
                    //place into queue
                    var lastQueuedTask = BLL.ActiveImagingTask.GetLastQueuedTask(thisComputerTask.Type);
                    if (lastQueuedTask == null)
                        thisComputerTask.QueuePosition = 1;
                    else
                        thisComputerTask.QueuePosition = lastQueuedTask.QueuePosition + 1;
                    thisComputerTask.Status = "2";
                    BLL.ActiveImagingTask.UpdateActiveImagingTask(thisComputerTask);

                    queueStatus.Result = "false";
                    queueStatus.Position = BLL.ActiveImagingTask.GetQueuePosition(computerId);
                    return JsonConvert.SerializeObject(queueStatus);

                }
            }

        }

        public string CheckHdRequirements(int profileId, int clientHdNumber, string newHdSize, string imageSchemaDrives, int clientLbs)
        {
            var result = new Services.Client.HardDriveSchema();
            
            var imageProfile = BLL.ImageProfile.ReadProfile(profileId);
            var partitionHelper = new ClientPartitionHelper(imageProfile);
            var imageSchema = partitionHelper.GetImageSchema();

            if (clientHdNumber > imageSchema.HardDrives.Count())
            {
                result.IsValid = "false";
                result.Message = "No Image Exists To Download To This Hard Drive.  There Are More" +
                                 "Hard Drive's Than The Original Image";

                return JsonConvert.SerializeObject(result);
            }

            var listSchemaDrives = new List<int>();
            if(!string.IsNullOrEmpty(imageSchemaDrives))
                listSchemaDrives.AddRange(imageSchemaDrives.Split(' ').Select(hd => Convert.ToInt32(hd)));         
            result.SchemaHdNumber = partitionHelper.NextActiveHardDrive(listSchemaDrives,clientHdNumber);
            
            if (result.SchemaHdNumber == -1)
            {
                result.IsValid = "false";
                result.Message = "No Active Hard Drive Images Were Found To Deploy.";
                return JsonConvert.SerializeObject(result);
            }

            var newHdBytes = Convert.ToInt64(newHdSize);
            var minimumSize = partitionHelper.HardDrive(result.SchemaHdNumber,newHdBytes);

            if (clientLbs != 0) //if zero should be from the osx imaging environment or winpe
            {
                if (clientLbs != imageSchema.HardDrives[result.SchemaHdNumber].Lbs)
                {
                    Logger.Log("Error: The Logical Block Size Of This Hard Drive " + clientLbs +
                               " Does Not Match The Original Image " + imageSchema.HardDrives[result.SchemaHdNumber].Lbs);


                    result.IsValid = "false";
                    result.Message = "The Logical Block Size Of This Hard Drive " + clientLbs +
                                     " Does Not Match The Original Image " +
                                     imageSchema.HardDrives[result.SchemaHdNumber].Lbs;
                    return JsonConvert.SerializeObject(result);
                }
            }

            if (minimumSize > newHdBytes)
            {
                Logger.Log("Error:  " + newHdBytes / 1024 / 1024 +
                           " MB Is Less Than The Minimum Required HD Size For This Image(" +
                           minimumSize / 1024 / 1024 + " MB)");

                result.IsValid = "false";
                result.Message = newHdBytes/1024/1024 +
                                 " MB Is Less Than The Minimum Required HD Size For This Image(" +
                                 minimumSize/1024/1024 + " MB)";
                return JsonConvert.SerializeObject(result);
            }
            if (minimumSize == newHdBytes)
            {
                result.IsValid = "original";
                result.PhysicalPartitions = partitionHelper.GetActivePartitions(result.SchemaHdNumber, imageProfile);
                result.PhysicalPartitionCount = partitionHelper.GetActivePartitionCount(result.SchemaHdNumber);
                result.PartitionType = imageSchema.HardDrives[result.SchemaHdNumber].Table;
                result.BootPartition = imageSchema.HardDrives[result.SchemaHdNumber].Boot;
                result.UsesLvm = partitionHelper.CheckForLvm(result.SchemaHdNumber);
                result.Guid = imageSchema.HardDrives[result.SchemaHdNumber].Guid;
                return JsonConvert.SerializeObject(result);
            }

            result.IsValid = "true";
            result.PhysicalPartitions = partitionHelper.GetActivePartitions(result.SchemaHdNumber, imageProfile);
            result.PhysicalPartitionCount = partitionHelper.GetActivePartitionCount(result.SchemaHdNumber);
            result.PartitionType = imageSchema.HardDrives[result.SchemaHdNumber].Table;
            result.BootPartition = imageSchema.HardDrives[result.SchemaHdNumber].Boot;
            result.UsesLvm = partitionHelper.CheckForLvm(result.SchemaHdNumber);
            result.Guid = imageSchema.HardDrives[result.SchemaHdNumber].Guid;
            return JsonConvert.SerializeObject(result);

        }

        public string GetOriginalLvm(int profileId, string clientHd, string hdToGet, string partitionPrefix)
        {
            string result = null;

            var imageProfile = BLL.ImageProfile.ReadProfile(profileId);
            var hdNumberToGet = Convert.ToInt32(hdToGet);
            var partitionHelper = new ClientPartitionHelper(imageProfile);
            var imageSchema = partitionHelper.GetImageSchema();
            foreach (var part in from part in imageSchema.HardDrives[hdNumberToGet].Partitions
                                 where part.Active
                                 where part.VolumeGroup != null
                                 where part.VolumeGroup.LogicalVolumes != null
                                 select part)
            {
                result = "pvcreate -u " + part.Uuid + " --norestorefile -yf " +
                         clientHd + partitionPrefix + part.VolumeGroup.PhysicalVolume[part.VolumeGroup.PhysicalVolume.Length - 1] + "\r\n";
                result += "vgcreate " + part.VolumeGroup.Name + " " + clientHd + partitionPrefix +
                          part.VolumeGroup.PhysicalVolume[part.VolumeGroup.PhysicalVolume.Length - 1] + " -yf" + "\r\n";
                result += "echo \"" + part.VolumeGroup.Uuid + "\" >>/tmp/vg-" + part.VolumeGroup.Name +
                          "\r\n";
                foreach (var lv in part.VolumeGroup.LogicalVolumes.Where(lv => lv.Active))
                {
                    result += "lvcreate --yes -L " + lv.Size + "s -n " + lv.Name + " " +
                              lv.VolumeGroup + "\r\n";
                    result += "echo \"" + lv.Uuid + "\" >>/tmp/" + lv.VolumeGroup + "-" +
                              lv.Name + "\r\n";
                }
                result += "vgcfgbackup -f /tmp/lvm-" + part.VolumeGroup.Name + "\r\n";
            }

            return result;
        }

        public string CheckForCancelledTask(int computerId)
        {
            return BLL.ActiveImagingTask.IsComputerActive(computerId) ? "false" : "true";
        }

        public string GetCustomScript(int scriptId)
        {
            var script = BLL.Script.GetScript(scriptId);
            return script.Contents;
        }

        public string GetSysprepTag(int tagId, string imageEnvironment)
        {
            var tag = BLL.SysprepTag.GetSysprepTag(tagId);
            tag.OpeningTag = Utility.EscapeCharacter(tag.OpeningTag, new[] {">", "<"});
            tag.ClosingTag = Utility.EscapeCharacter(tag.ClosingTag, new[] {">", "<", "/"});
            tag.Contents = Utility.EscapeCharacter(tag.Contents, new[] {">", "<", "/", "\""});

            var a = tag.Contents.Replace("\r", imageEnvironment == "win" ? "" : "\\r");
            var b = a.Replace("\n", "\\n");

            if (b.Length >= 5)
            {
                if (b.Substring(b.Length - 5) == "\\r\\n'")
                    b = b.Substring(0, b.Length - 1) + "\\r\\n'";
            }
            tag.Contents = b;
            return JsonConvert.SerializeObject(tag);
        }

        public string GetFileCopySchema(int profileId)
        {
            var fileFolderSchema = new Services.Client.FileFolderCopySchema() {FilesAndFolders = new List<FileFolderCopy>()};
            var counter = 0;
            foreach (var profileFileFolder in ImageProfileFileFolder.SearchImageProfileFileFolders(profileId))
            {
                counter++;
                var fileFolder = BLL.FileFolder.GetFileFolder(profileFileFolder.FileFolderId);

                var clientFileFolder = new Services.Client.FileFolderCopy();
                clientFileFolder.SourcePath = fileFolder.Path;
                clientFileFolder.DestinationFolder = profileFileFolder.DestinationFolder;
                clientFileFolder.DestinationPartition = profileFileFolder.DestinationPartition;
                clientFileFolder.FolderCopyType = profileFileFolder.FolderCopyType;

                fileFolderSchema.FilesAndFolders.Add(clientFileFolder);
            }
            fileFolderSchema.Count = counter.ToString();
            return JsonConvert.SerializeObject(fileFolderSchema);
        }

        public string MulticastCheckout(string portBase)
        {
            string result = null;
            var mcTask = BLL.ActiveMulticastSession.GetFromPort(Convert.ToInt32(portBase));
               
            if (mcTask != null)
            {
                var prsRunning = true;

                if (Environment.OSVersion.ToString().Contains("Unix"))
                {
                    try
                    {
                        var prs = Process.GetProcessById(Convert.ToInt32(mcTask.Pid));
                        if (prs.HasExited)
                        {
                            prsRunning = false;
                        }
                    }
                    catch
                    {
                        prsRunning = false;
                    }
                }
                else
                {
                    try
                    {
                        Process.GetProcessById(Convert.ToInt32(mcTask.Pid));
                    }
                    catch
                    {
                        prsRunning = false;
                    }
                }
                if (!prsRunning)
                {
                    if (BLL.ActiveMulticastSession.Delete(mcTask.Id))
                    {
                        result = "Success";
                        BLL.ActiveMulticastSession.SendMulticastCompletedEmail(mcTask);
                    }
                }
                else
                    result = "Cannot Close Session, It Is Still In Progress";
            }
            else
                result = "Session Is Already Closed";

            return result;
        }

        public string GetCustomPartitionScript(int profileId)
        {
            return BLL.ImageProfile.ReadProfile(profileId).CustomPartitionScript;
        }

        public string GetOnDemandArguments(string mac, int objectId, string task)
        {
            ImageProfile imageProfile;
            var computer = BLL.Computer.GetComputerFromMac(mac);
            if (task == "push" || task == "pull")
            {
                imageProfile = BLL.ImageProfile.ReadProfile(objectId);
                return new BLL.Workflows.CreateTaskArguments(computer, imageProfile, task).Run();
            }
            else //Multicast
            {
                var multicast = BLL.ActiveMulticastSession.GetFromPort(objectId);
                imageProfile = BLL.ImageProfile.ReadProfile(multicast.ImageProfileId);
                return new BLL.Workflows.CreateTaskArguments(computer, imageProfile, task).Run(objectId.ToString());
            }

            
        }

        public string ImageList(string environment,int userId = 0)
        {
            var images = BLL.Image.GetOnDemandImageList(userId);
            if (environment == "winpe")
            {
                images = images.Where(x => x.Environment == "winpe").ToList();
                var imageList = new List<Services.Client.WinPEImageList>();
                foreach (var image in images)
                {
                    var winpeImage = new Services.Client.WinPEImageList();
                    winpeImage.ImageId = image.Id.ToString();
                    winpeImage.ImageName = image.Name;
                    imageList.Add(winpeImage);
                }
                return JsonConvert.SerializeObject(imageList);
            }
            else
            {
                var imageList = new Services.Client.ImageList {Images = new List<string>()};              
                if (environment == "macOS")
                    images = images.Where(x => x.Environment == "macOS").ToList();
                else if (environment == "linux")
                    images = images.Where(x => x.Environment != "macOS" && x.Environment != "winpe").ToList();
                foreach (var image in images)
                    imageList.Images.Add(image.Id + " " + image.Name);

                if (imageList.Images.Count == 0)
                    imageList.Images.Add(-1 + " " + "No_Images_Found");
                return JsonConvert.SerializeObject(imageList);
            }
        }

        public string ImageProfileList(int imageId)
        {
            var selectedImage = BLL.Image.GetImage(imageId);
            if (selectedImage.Environment == "winpe")
            {
                var imageProfileList = new Services.Client.WinPEProfileList { ImageProfiles = new List<WinPEProfile>() };
                int profileCounter = 0;
                foreach (var imageProfile in BLL.ImageProfile.SearchProfiles(Convert.ToInt32(imageId)).OrderBy(x => x.Name))
                {
                    profileCounter++;
                    var winpeProfile = new Services.Client.WinPEProfile();
                    winpeProfile.ProfileId = imageProfile.Id.ToString();
                    winpeProfile.ProfileName = imageProfile.Name;
                    imageProfileList.ImageProfiles.Add(winpeProfile);
                    
                    if (profileCounter == 1)
                        imageProfileList.FirstProfileId = imageProfile.Id.ToString();
                }
                imageProfileList.Count = profileCounter.ToString();
                return JsonConvert.SerializeObject(imageProfileList);
            
            }
            else
            {
                var imageProfileList = new Services.Client.ImageProfileList {ImageProfiles = new List<string>()};

                int profileCounter = 0;
                foreach (var imageProfile in BLL.ImageProfile.SearchProfiles(Convert.ToInt32(imageId)))
                {
                    profileCounter++;
                    imageProfileList.ImageProfiles.Add(imageProfile.Id + " " + imageProfile.Name);
                    if (profileCounter == 1)
                        imageProfileList.FirstProfileId = imageProfile.Id.ToString();
                }

                imageProfileList.Count = profileCounter.ToString();
                return JsonConvert.SerializeObject(imageProfileList);
            }
            
        }

        public string MulicastSessionList(string environment)
        {
            if (environment == "winpe")
            {
                var multicastList = new List<Services.Client.WinPEMulticastList>();
                foreach (var multicast in BLL.ActiveMulticastSession.GetOnDemandList())
                {
                    var multicastSession = new Services.Client.WinPEMulticastList();
                    multicastSession.Port = multicast.Port.ToString();
                    multicastSession.Name = multicast.Name;
                    multicastList.Add(multicastSession);
                }
                return JsonConvert.SerializeObject(multicastList);
            }
            else
            {
                var multicastList = new Services.Client.MulticastList() {Multicasts = new List<string>()};

                foreach (var multicast in BLL.ActiveMulticastSession.GetOnDemandList())
                {
                    multicastList.Multicasts.Add(multicast.Port + " " + multicast.Name);
                }

                return JsonConvert.SerializeObject(multicastList);
            }
        }

        public string AddImage(string imageName)
        {
            var image = new Models.Image()
            {
                Name = imageName,
                Environment = "linux",
                Type = "Block",
                Enabled = 1,
                IsVisible = 1,
                Os = "",
                Description = ""
            };
            var result = BLL.Image.AddImage(image);
            if (result.Success)
                result.Message = image.Id.ToString();

            return JsonConvert.SerializeObject(result);
        }

        public string AddImageWinPEEnv(string imageName)
        {
            var image = new Models.Image()
            {
                Name = imageName,
                Environment = "winpe",
                Type = "File",
                Enabled = 1,
                IsVisible = 1,
                Os = "",
                Description = ""
            };
            var result = BLL.Image.AddImage(image);
            if (result.Success)
                result.Message = image.Id.ToString();

            return JsonConvert.SerializeObject(result);
        }

        public string AddImageOsxEnv(string imageName)
        {
            var image = new Models.Image()
            {
                Name = imageName,
                Environment = "macOS",
                Type = "Block",
                OsxType = "thick",
                Enabled = 1,
                IsVisible = 1,
                Os = "",
                Description = ""

            };
            var result = BLL.Image.AddImage(image);
            if (result.Success)
                result.Message = image.Id.ToString();

            return JsonConvert.SerializeObject(result);
        }

        public string GetProxyReservation(string mac)
        {
            var bootClientReservation = new Services.Client.ProxyReservation();

            var computer = BLL.Computer.GetComputerFromMac(mac);
            if (computer == null)
            {
                bootClientReservation.BootFile = "NotFound";
                return JsonConvert.SerializeObject(bootClientReservation);
            }
            if (computer.ProxyReservation == 0)
            {
                bootClientReservation.BootFile = "NotEnabled";
                return JsonConvert.SerializeObject(bootClientReservation);
            }

            var computerReservation = BLL.ComputerProxyReservation.GetComputerProxyReservation(computer.Id);
            

            bootClientReservation.NextServer = Helpers.ParameterReplace.Between(computerReservation.NextServer);
            switch (computerReservation.BootFile)
            {
                case "bios_pxelinux":
                    bootClientReservation.BootFile = @"proxy/bios/pxelinux.0";
                    break;
                case "bios_ipxe":
                    bootClientReservation.BootFile = @"proxy/bios/undionly.kpxe";
                    break;
                case "bios_x86_winpe":
                    bootClientReservation.BootFile = @"proxy/bios/pxeboot.n12";
                    bootClientReservation.BcdFile = @"/boot/BCDx86";
                    break;
                case "bios_x64_winpe":
                    bootClientReservation.BootFile = @"proxy/bios/pxeboot.n12";
                    bootClientReservation.BcdFile = @"/boot/BCDx64";
                    break;
                case "efi_x86_syslinux":
                    bootClientReservation.BootFile = @"proxy/efi32/syslinux.efi";
                    break;
                case "efi_x86_ipxe":
                    bootClientReservation.BootFile = @"proxy/efi32/ipxe.efi";
                    break;
                case "efi_x86_winpe":
                    bootClientReservation.BootFile = @"proxy/efi32/bootmgfw.efi";
                    bootClientReservation.BcdFile = @"/boot/BCDx86";
                    break;
                case "efi_x64_syslinux":
                    bootClientReservation.BootFile = @"proxy/efi64/syslinux.efi";
                    break;
                case "efi_x64_ipxe":
                    bootClientReservation.BootFile = @"proxy/efi64/ipxe.efi";
                    break;
                case "efi_x64_winpe":
                    bootClientReservation.BootFile = @"proxy/efi64/bootmgfw.efi";
                    bootClientReservation.BcdFile = @"/boot/BCDx64";
                    break;
                case "efi_x64_grub":
                    bootClientReservation.BootFile = @"proxy/efi64/bootx64.efi";
                    break;
            }

            return JsonConvert.SerializeObject(bootClientReservation);
        }

       
    }
}
