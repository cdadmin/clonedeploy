using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CloneDeploy_ApiCalls;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Entities.DTOs.ClientImaging;
using CloneDeploy_Services.Helpers;
using CloneDeploy_Services.Workflows;
using log4net;
using Newtonsoft.Json;

namespace CloneDeploy_Services
{
    public class ClientImagingServices
    {
        private readonly ILog log = LogManager.GetLogger("ApplicationLog");
        public bool Authorize(string token)
        {
            if (token == Settings.UniversalToken && !string.IsNullOrEmpty(Settings.UniversalToken))
                return true;
            
            var user = new UserServices().GetUserByToken(token);
            if (user != null)
                return true;

            return false;
        }
        public string AddComputer(string name, string mac)
        {
            var computer = new ComputerEntity
            {
                Name = name,
                Mac = mac,
            };
            var result = new ComputerServices().AddComputer(computer);
            return JsonConvert.SerializeObject(result);
        }

        public void UpdateProgress(int computerId, string progress, string progressType)
        {
            var task = new ComputerServices().GetTaskForComputer(computerId);
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

            new ActiveImagingTaskServices().UpdateActiveImagingTask(task);
        }

        public void UpdateProgressPartition(int computerId, string partition)
        {
            var task = new ComputerServices().GetTaskForComputer(computerId);
            task.Partition = partition;
            task.Elapsed = "Please Wait...";
            task.Remaining = "";
            task.Completed = "";
            task.Rate = "";
            new ActiveImagingTaskServices().UpdateActiveImagingTask(task);
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
            var userServices = new UserServices();
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
                var user = userServices.GetUserByToken(token);
                if (user != null)
                {
                    if (new AuthorizationServices(user.Id, Authorizations.AllowOnd).IsAuthorized())
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
                var user = userServices.GetUserByToken(token);
                if (user != null)
                {
                    if (new AuthorizationServices(user.Id, Authorizations.AllowDebug).IsAuthorized())
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
                var user = userServices.GetUserByToken(token);
                if (user != null)
                {
                    if (new AuthorizationServices(user.Id, Authorizations.ImageDeployTask).IsAuthorized())
                        return "true";
                }
            }
           
            return "false";
        }

        public string CheckIn(string computerMac)
        {
            var checkIn = new CheckIn();
            var computerServices = new ComputerServices();
            var computer = computerServices.GetComputerFromMac(computerMac);
            if (computer == null)
            {
                checkIn.Result = "false";
                checkIn.Message = "This Computer Was Not Found";
                return JsonConvert.SerializeObject(checkIn);
            }

            var computerTask = computerServices.GetTaskForComputer(computer.Id);
            if (computerTask == null)
            {
                checkIn.Result = "false";
                checkIn.Message = "An Active Task Was Not Found For This Computer";
                return JsonConvert.SerializeObject(checkIn);
            }

            var imageServerIdentifier = new Workflows.GetImageServer(computer).Run();

            computerTask.Status = "1";
            computerTask.ImageServer = imageServerIdentifier;

            if (new ActiveImagingTaskServices().UpdateActiveImagingTask(computerTask))
            {
                checkIn.Result = "true";
               
                var image = new ImageServices().GetImage(computer.ImageId);
                if (image != null)
                {
                    if (image.Environment == "")
                        image.Environment = "linux";
                    checkIn.ImageEnvironment = image.Environment;
                }
                
                if(image.Environment == "winpe")
                    checkIn.TaskArguments = computerTask.Arguments + " image_server_identifier=\"" + imageServerIdentifier + "\"\r\n";
                else
                    checkIn.TaskArguments = computerTask.Arguments + " image_server_identifier=\"" + imageServerIdentifier + "\"";
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
            var imageProfile = new ImageProfileServices().ReadProfile(profileId);
            var authString = imageProfile.MunkiAuthUsername + ":" + imageProfile.MunkiAuthPassword;
            return Helpers.Utility.Encode(authString);
        }

        public string DistributionPoint(string serverIdentifier, string task)
        {
      
            var smb = new SMB();
          
            ImageShareDTO imageShare;
            if (Settings.OperationMode == "Single" || serverIdentifier == Settings.ServerIdentifier)
            {
                imageShare = new SettingServices().GetImageShare();
            }
            else
            {
                imageShare = new APICall(new SecondaryServerServices().GetApiToken(serverIdentifier)).SettingApi.GetImageShareSettings();              
            }

            smb.SharePath = "//" + Utility.Between(imageShare.Server) + "/" + imageShare.Name;
            smb.Domain = imageShare.Domain;
            if (task == "pull")
            {
                smb.Username = imageShare.ReadWriteUser;
                smb.Password = imageShare.ReadOnlyPassword;
            }
            else
            {
                smb.Username = imageShare.ReadOnlyUser;
                smb.Password = imageShare.ReadOnlyPassword;
            }
            
            return JsonConvert.SerializeObject(smb);
        }

        public void ChangeStatusInProgress(int computerId)
        {
            var computerTask = new ComputerServices().GetTaskForComputer(computerId);
            computerTask.Status = "3";
            new ActiveImagingTaskServices().UpdateActiveImagingTask(computerTask);
        }

        public void DeleteImage(int profileId)
        {
            var profile = new ImageProfileServices().ReadProfile(profileId);
            if (string.IsNullOrEmpty(profile.Image.Name)) return;
            //Remove existing custom deploy schema, it may not match newly updated image
            profile.CustomSchema = string.Empty;
            new ImageProfileServices().UpdateProfile(profile);
            try
            {
                if (Directory.Exists(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + profile.Image.Name))
                    Directory.Delete(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + profile.Image.Name, true);
                Directory.CreateDirectory(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + profile.Image.Name);
                new FileOps().SetUnixPermissionsImage(Settings.PrimaryStoragePath + "images" + Path.DirectorySeparatorChar + profile.Image.Name);
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
        }

        public void ErrorEmail(int computerId, string error)
        {
            var computerTask = new ComputerServices().GetTaskForComputer(computerId);
            new ActiveImagingTaskServices().SendTaskErrorEmail(computerTask,error);
        }

        public void CheckOut(int computerId)
        {
            var computerTask = new ComputerServices().GetTaskForComputer(computerId);
            var activeImagingTaskServices = new ActiveImagingTaskServices();
            activeImagingTaskServices.DeleteActiveImagingTask(computerTask.Id);
            if(computerTask.Type == "unicast")
                activeImagingTaskServices.SendTaskCompletedEmail(computerTask);
        }

        public void PermanentTaskCheckOut(int computerId)
        {
            var computerTask = new ComputerServices().GetTaskForComputer(computerId);
            var activeImagingTaskServices = new ActiveImagingTaskServices();
            computerTask.Status = "0";
            computerTask.Partition = "";
            computerTask.Completed = "";
            computerTask.Elapsed = "";
            computerTask.Rate = "";
            computerTask.Remaining = "";
            activeImagingTaskServices.UpdateActiveImagingTask(computerTask);
           
            activeImagingTaskServices.SendTaskCompletedEmail(computerTask);
        }

        public void UploadLog(int computerId, string logContents, string subType, string computerMac)
        {
            var computerLog = new ComputerLogEntity
            {
                ComputerId = computerId,
                Contents = logContents,
                Type = "image",
                SubType = subType,
                Mac = computerMac
            };
            new ComputerLogServices().AddComputerLog(computerLog);

        }

        public string CheckQueue(int computerId)
        {
            var queueStatus = new QueueStatus();
            var activeImagingTaskServices = new ActiveImagingTaskServices();

            //Check if already part of the queue
            var thisComputerTask = new ComputerServices().GetTaskForComputer(computerId);
            if (thisComputerTask.Status == "2")
            {
                //Check if the queue is open yet
                var inUse = activeImagingTaskServices.GetCurrentQueue(thisComputerTask.Type);
                var totalCapacity = Convert.ToInt32(Settings.QueueSize);
                if (inUse < totalCapacity)
                {
                    //queue is open, is this computer next
                    var firstTaskInQueue = activeImagingTaskServices.GetNextComputerInQueue(thisComputerTask.Type);
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
                        queueStatus.Position = new ComputerServices().GetQueuePosition(computerId); 
                        return JsonConvert.SerializeObject(queueStatus);
                    }
                }
                else
                {
                    //queue not open yet
                    queueStatus.Result = "false";
                    queueStatus.Position = new ComputerServices().GetQueuePosition(computerId);
                    return JsonConvert.SerializeObject(queueStatus);
                }
            }
            else
            {
                //New computer checking queue for the first time

                var inUse = activeImagingTaskServices.GetCurrentQueue(thisComputerTask.Type);
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
                    var lastQueuedTask = activeImagingTaskServices.GetLastQueuedTask(thisComputerTask.Type);
                    if (lastQueuedTask == null)
                        thisComputerTask.QueuePosition = 1;
                    else
                        thisComputerTask.QueuePosition = lastQueuedTask.QueuePosition + 1;
                    thisComputerTask.Status = "2";
                    activeImagingTaskServices.UpdateActiveImagingTask(thisComputerTask);

                    queueStatus.Result = "false";
                    queueStatus.Position = new ComputerServices().GetQueuePosition(computerId);
                    return JsonConvert.SerializeObject(queueStatus);

                }
            }

        }

        public string CheckHdRequirements(int profileId, int clientHdNumber, string newHdSize, string imageSchemaDrives, int clientLbs)
        {
            var result = new HardDriveSchema();
            
            var imageProfile = new ImageProfileServices().ReadProfile(profileId);
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
                    log.Debug("Error: The Logical Block Size Of This Hard Drive " + clientLbs +
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
                log.Debug("Error:  " + newHdBytes / 1024 / 1024 +
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

            var imageProfile = new ImageProfileServices().ReadProfile(profileId);
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
            return new ComputerServices().IsComputerActive(computerId) ? "false" : "true";
        }

        public string GetCustomScript(int scriptId)
        {
            var script = new ScriptServices().GetScript(scriptId);
            return script.Contents;
        }

        public string GetSysprepTag(int tagId, string imageEnvironment)
        {
            var tag = new SysprepTagServices().GetSysprepTag(tagId);
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
            var fileFolderSchema = new FileFolderCopySchema() {FilesAndFolders = new List<FileFolderCopy>()};
            var counter = 0;
            foreach (var profileFileFolder in new ImageProfileServices().SearchImageProfileFileFolders(profileId))
            {
                counter++;
                var fileFolder = new FileFolderServices().GetFileFolder(profileFileFolder.FileFolderId);

                var clientFileFolder = new FileFolderCopy();
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
            var activeMulticastSessionServices = new ActiveMulticastSessionServices();
            var mcTask = activeMulticastSessionServices.GetFromPort(Convert.ToInt32(portBase));
               
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
                    if (activeMulticastSessionServices.Delete(mcTask.Id).Success)
                    {
                        result = "Success";
                        activeMulticastSessionServices.SendMulticastCompletedEmail(mcTask);
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
            return new ImageProfileServices().ReadProfile(profileId).CustomPartitionScript;
        }

        public string GetOnDemandArguments(string mac, int objectId, string task)
        {
            ImageProfileEntity imageProfile;
            var computer = new ComputerServices().GetComputerFromMac(mac);
            var arguments = "";
            if (task == "push" || task == "pull")
            {
                imageProfile = new ImageProfileServices().ReadProfile(objectId);
                arguments = new CreateTaskArguments(computer, imageProfile, task).Run();
            }
            else //Multicast
            {
                var multicast = new ActiveMulticastSessionServices().GetFromPort(objectId);
                imageProfile = new ImageProfileServices().ReadProfile(multicast.ImageProfileId);
                arguments = new CreateTaskArguments(computer, imageProfile, task).Run(objectId.ToString());
            }

            var imageServerIdentifier = new Workflows.GetImageServer(computer).Run();
            if (imageProfile.Image.Environment == "winpe")
                arguments += " image_server_identifier=\"" + imageServerIdentifier + "\"\r\n";
            else
                arguments += " image_server_identifier=\"" + imageServerIdentifier + "\"";

            return arguments;

        }

        public string ImageList(string environment,int userId = 0)
        {
            var images = new ImageServices().GetOnDemandImageList(userId);
            if (environment == "winpe")
            {
                images = images.Where(x => x.Environment == "winpe").ToList();
                var imageList = new List<WinPEImageList>();
                foreach (var image in images)
                {
                    var winpeImage = new WinPEImageList();
                    winpeImage.ImageId = image.Id.ToString();
                    winpeImage.ImageName = image.Name;
                    imageList.Add(winpeImage);
                }
                return JsonConvert.SerializeObject(imageList);
            }
            else
            {
                var imageList = new ImageList {Images = new List<string>()};              
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
            var imageServices = new ImageServices();
            var selectedImage = imageServices.GetImage(imageId);
            if (selectedImage.Environment == "winpe")
            {
                var imageProfileList = new WinPEProfileList { ImageProfiles = new List<WinPEProfile>() };
                int profileCounter = 0;
                foreach (var imageProfile in imageServices.SearchProfiles(Convert.ToInt32(imageId)).OrderBy(x => x.Name))
                {
                    profileCounter++;
                    var winpeProfile = new WinPEProfile();
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
                var imageProfileList = new ImageProfileList {ImageProfiles = new List<string>()};

                int profileCounter = 0;
                foreach (var imageProfile in imageServices.SearchProfiles(Convert.ToInt32(imageId)))
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
                var multicastList = new List<WinPEMulticastList>();
                foreach (var multicast in new ActiveMulticastSessionServices().GetOnDemandList())
                {
                    var multicastSession = new WinPEMulticastList();
                    multicastSession.Port = multicast.Port.ToString();
                    multicastSession.Name = multicast.Name;
                    multicastList.Add(multicastSession);
                }
                return JsonConvert.SerializeObject(multicastList);
            }
            else
            {
                var multicastList = new MulticastList() {Multicasts = new List<string>()};

                foreach (var multicast in new ActiveMulticastSessionServices().GetOnDemandList())
                {
                    multicastList.Multicasts.Add(multicast.Port + " " + multicast.Name);
                }

                return JsonConvert.SerializeObject(multicastList);
            }
        }

        public string AddImage(string imageName)
        {
            var image = new ImageEntity()
            {
                Name = imageName,
                Environment = "linux",
                Type = "Block",
                Enabled = 1,
                IsVisible = 1,
                Os = "",
                Description = ""
            };
            var result = new ImageServices().AddImage(image);
            if (result.Success)
                result.ErrorMessage = image.Id.ToString();

            return JsonConvert.SerializeObject(result);
        }

        public string AddImageWinPEEnv(string imageName)
        {
            var image = new ImageEntity()
            {
                Name = imageName,
                Environment = "winpe",
                Type = "File",
                Enabled = 1,
                IsVisible = 1,
                Os = "",
                Description = ""
            };
            var result = new ImageServices().AddImage(image);
            if (result.Success)
                result.ErrorMessage = image.Id.ToString();

            return JsonConvert.SerializeObject(result);
        }

        public string AddImageOsxEnv(string imageName)
        {
            var image = new ImageEntity()
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
            var result = new ImageServices().AddImage(image);
            if (result.Success)
                result.ErrorMessage = image.Id.ToString();

            return JsonConvert.SerializeObject(result);
        }

        public string GetProxyReservation(string mac)
        {
            var bootClientReservation = new ProxyReservation();

            var computer = new ComputerServices().GetComputerFromMac(mac);
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

            var computerReservation = new ComputerServices().GetComputerProxyReservation(computer.Id);
            

            bootClientReservation.NextServer = Helpers.Utility.Between(computerReservation.NextServer);
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
