using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CloneDeploy_Common;
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
        private readonly ILog log = LogManager.GetLogger(typeof(ClientImagingServices));

        public string AddComputer(string name, string mac, string clientIdentifier)
        {
            clientIdentifier = clientIdentifier.ToUpper();
            var existingComputer = new ComputerServices().GetComputerFromClientIdentifier(clientIdentifier);
            if (existingComputer != null)
            {
                return
                    JsonConvert.SerializeObject(new ActionResultDTO
                    {
                        Success = false,
                        ErrorMessage = "A Computer With This Client Id Already Exists"
                    });
            }
            var computer = new ComputerEntity
            {
                Name = name,
                Mac = mac,
                ClientIdentifier = clientIdentifier,
                SiteId = -1,
                BuildingId = -1,
                RoomId = -1,
                ImageId = -1,
                ImageProfileId = -1,
                ClusterGroupId = -1
            };
            var result = new ComputerServices().AddComputer(computer);
            return JsonConvert.SerializeObject(result);
        }

        public string AddImage(string imageName)
        {
            var image = new ImageEntity
            {
                Name = imageName,
                Environment = "linux",
                Type = "Block",
                Enabled = 1,
                IsVisible = 1,
                Os = "",
                Description = "",
                ClassificationId = -1
                
            };
            var result = new ImageServices().AddImage(image);
            if (result.Success)
                result.Id = image.Id;

            return JsonConvert.SerializeObject(result);
        }

    

        public string AddImageWinPEEnv(string imageName)
        {
            var image = new ImageEntity
            {
                Name = imageName,
                Environment = "winpe",
                Type = "File",
                Enabled = 1,
                IsVisible = 1,
                Os = "",
                Description = "",
                ClassificationId = -1
            };
            var result = new ImageServices().AddImage(image);
            if (result.Success)
                result.Id = image.Id;

            return JsonConvert.SerializeObject(result);
        }

        public bool Authorize(string token)
        {
            if (token == SettingServices.GetSettingValue(SettingStrings.UniversalToken) &&
                !string.IsNullOrEmpty(SettingServices.GetSettingValue(SettingStrings.UniversalToken)))
                return true;

            var user = new UserServices().GetUserByToken(token);
            if (user != null)
                return true;

            return false;
        }

        public void ChangeStatusInProgress(int taskId)
        {
            var task = new ActiveImagingTaskServices().GetTask(taskId);
            task.Status = "3";
            new ActiveImagingTaskServices().UpdateActiveImagingTask(task);
        }

        public string CheckForCancelledTask(int taskId)
        {
            var task = new ActiveImagingTaskServices().GetTask(taskId);
            if (task == null)
                return "true";
            return "false";
        }

        public string CheckHdRequirements(int profileId, int clientHdNumber, string newHdSize, string imageSchemaDrives,
            int clientLbs)
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
            if (!string.IsNullOrEmpty(imageSchemaDrives))
                listSchemaDrives.AddRange(imageSchemaDrives.Split(' ').Select(hd => Convert.ToInt32(hd)));
            result.SchemaHdNumber = partitionHelper.NextActiveHardDrive(listSchemaDrives, clientHdNumber);

            if (result.SchemaHdNumber == -1)
            {
                result.IsValid = "false";
                result.Message = "No Active Hard Drive Images Were Found To Deploy.";
                return JsonConvert.SerializeObject(result);
            }

            var newHdBytes = Convert.ToInt64(newHdSize);
            var minimumSize = partitionHelper.HardDrive(result.SchemaHdNumber, newHdBytes);

            if (clientLbs != 0) //if zero should be from the winpe imaging environment
            {
                if (imageProfile.Image.Type != "File")
                {
                    if (clientLbs != imageSchema.HardDrives[result.SchemaHdNumber].Lbs)
                    {
                        log.Error("Error: The Logical Block Size Of This Hard Drive " + clientLbs +
                                  " Does Not Match The Original Image" +
                                  imageSchema.HardDrives[result.SchemaHdNumber].Lbs);

                        result.IsValid = "false";
                        result.Message = "The Logical Block Size Of This Hard Drive " + clientLbs +
                                         " Does Not Match The Original Image" +
                                         imageSchema.HardDrives[result.SchemaHdNumber].Lbs;
                        return JsonConvert.SerializeObject(result);
                    }
                }
            }

            if (minimumSize > newHdBytes)
            {
                log.Error("Error:  " + newHdBytes/1024/1024 +
                          " MB Is Less Than The Minimum Required HD Size For This Image(" +
                          minimumSize/1024/1024 + " MB)");

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

        public string CheckIn(string taskId)
        {
            var checkIn = new CheckIn();
            var computerServices = new ComputerServices();

            var task = new ActiveImagingTaskServices().GetTask(Convert.ToInt32(taskId));

            if (task == null)
            {
                checkIn.Result = "false";
                checkIn.Message = "Could Not Find Task With Id" + taskId;
                return JsonConvert.SerializeObject(checkIn);
            }

            var computer = computerServices.GetComputer(task.ComputerId);
            if (computer == null)
            {
                checkIn.Result = "false";
                checkIn.Message = "The Computer Assigned To This Task Was Not Found";
                return JsonConvert.SerializeObject(checkIn);
            }

            var imageDistributionPoint = new GetImageServer(computer, task.Type).Run();

            task.Status = "1";
            task.DpId = imageDistributionPoint;

            ImageEntity image = null;
            if (task.Type == "multicast")
            {
                var mcTask = new ActiveMulticastSessionServices().Get(task.MulticastId);
                var group = new GroupServices().GetGroupByName(mcTask.Name);
                image = new ImageServices().GetImage(group.ImageId);
            }
            else
            {
                image = new ImageServices().GetImage(computer.ImageId);
            }
            if (image.Protected == 1 && task.Type.Contains("upload"))
            {
                checkIn.Result = "false";
                checkIn.Message = "This Image Is Protected";
                return JsonConvert.SerializeObject(checkIn);
            }

            if (new ActiveImagingTaskServices().UpdateActiveImagingTask(task))
            {
                checkIn.Result = "true";

                if (image != null)
                {
                    if (image.Environment == "")
                        image.Environment = "linux";
                    checkIn.ImageEnvironment = image.Environment;
                }

                if (image.Environment == "winpe")
                    checkIn.TaskArguments = task.Arguments + "dp_id=\"" +
                                            imageDistributionPoint + "\"\r\n";
                else
                    checkIn.TaskArguments = task.Arguments + " dp_id=\"" +
                                            imageDistributionPoint + "\"";
                return JsonConvert.SerializeObject(checkIn);
            }
            checkIn.Result = "false";
            checkIn.Message = "Could Not Update Task Status";
            return JsonConvert.SerializeObject(checkIn);
        }

        public void CheckOut(int taskId, int profileId)
        {
            var activeImagingTaskServices = new ActiveImagingTaskServices();
            var task = activeImagingTaskServices.GetTask(taskId);
            if (task.Type.Contains("upload"))
            {
            }

            if (task.Type.Contains("unreg"))
                activeImagingTaskServices.DeleteUnregisteredOndTask(task.Id);
            else
                activeImagingTaskServices.DeleteActiveImagingTask(task.Id);

            if (task.Type != "multicast" && task.Type != "ondmulticast")
                activeImagingTaskServices.SendTaskCompletedEmail(task);
        }

        public string CheckQueue(int taskId)
        {
            var queueStatus = new QueueStatus();
            var activeImagingTaskServices = new ActiveImagingTaskServices();
            var thisComputerTask = activeImagingTaskServices.GetTask(taskId);
            //var computer = new ComputerServices().GetComputer(thisComputerTask.ComputerId);
            //Check if already part of the queue
            activeImagingTaskServices.CancelTimedOutTasks();
            if (thisComputerTask.Status == "2")
            {
                //Delete Any tasks that have passed the timeout value
                
                //Check if the queue is open yet
                var inUse = activeImagingTaskServices.GetCurrentQueue(thisComputerTask);
                var totalCapacity = 0;
                var dp = new DistributionPointServices().GetDistributionPoint(thisComputerTask.DpId);
                totalCapacity = dp.QueueSize;
                if (inUse < totalCapacity)
                {
                    //queue is open, is this computer next
                    var firstTaskInQueue = activeImagingTaskServices.GetNextComputerInQueue(thisComputerTask);
                    if (firstTaskInQueue.ComputerId == thisComputerTask.ComputerId)
                    {
                        ChangeStatusInProgress(taskId);
                        queueStatus.Result = "true";
                        queueStatus.Position = "0";
                        return JsonConvert.SerializeObject(queueStatus);
                    }
                    //not time for this computer yet
                    queueStatus.Result = "false";
                    queueStatus.Position = activeImagingTaskServices.GetQueuePosition(thisComputerTask);
                    return JsonConvert.SerializeObject(queueStatus);
                }
                //queue not open yet
                queueStatus.Result = "false";
                queueStatus.Position = activeImagingTaskServices.GetQueuePosition(thisComputerTask);
                return JsonConvert.SerializeObject(queueStatus);
            }
            else
            {
                //New computer checking queue for the first time

                var inUse = activeImagingTaskServices.GetCurrentQueue(thisComputerTask);
                var totalCapacity = 0;
                var dp = new DistributionPointServices().GetDistributionPoint(thisComputerTask.DpId);
                totalCapacity = dp.QueueSize;
                if (inUse < totalCapacity)
                {
                    ChangeStatusInProgress(taskId);

                    queueStatus.Result = "true";
                    queueStatus.Position = "0";
                    return JsonConvert.SerializeObject(queueStatus);
                }
                //place into queue
                var lastQueuedTask = activeImagingTaskServices.GetLastQueuedTask(thisComputerTask);
                if (lastQueuedTask == null)
                    thisComputerTask.QueuePosition = 1;
                else
                    thisComputerTask.QueuePosition = lastQueuedTask.QueuePosition + 1;
                thisComputerTask.Status = "2";
                activeImagingTaskServices.UpdateActiveImagingTask(thisComputerTask);

                queueStatus.Result = "false";
                queueStatus.Position = activeImagingTaskServices.GetQueuePosition(thisComputerTask);
                return JsonConvert.SerializeObject(queueStatus);
            }
        }

        public string CheckTaskAuth(string task, string token)
        {
            var userServices = new UserServices();
            //only check ond and debug because web tasks can't even be started if user isn't authorized
            if (task == "ond" && SettingServices.GetSettingValue(SettingStrings.OnDemand) != "Enabled")
            {
                log.Debug("A client tried to image with on demand mode, but it is not enabled on the server");
                return "false";
            }

            if (task == "clobber" && SettingServices.GetSettingValue(SettingStrings.ClobberEnabled) != "1")
            {
                log.Debug("A client tried to image with clobber mode, but it is not enabled on the server");
                return "false";
            }

            if (task == "ond" && SettingServices.GetSettingValue(SettingStrings.OnDemandRequiresLogin) == "No")
            {
                if (token == SettingServices.GetSettingValue(SettingStrings.UniversalToken) &&
                    !string.IsNullOrEmpty(SettingServices.GetSettingValue(SettingStrings.UniversalToken)))
                    return "true";
            }
            else if (task == "ond" && SettingServices.GetSettingValue(SettingStrings.OnDemandRequiresLogin) == "Yes")
            {
                var user = userServices.GetUserByToken(token);
                if (user != null)
                {
                    if (new AuthorizationServices(user.Id, AuthorizationStrings.AllowOnd).IsAuthorized())
                        return "true";
                }
            }
            else if (task == "debug" && SettingServices.GetSettingValue(SettingStrings.DebugRequiresLogin) == "No")
            {
                if (token == SettingServices.GetSettingValue(SettingStrings.UniversalToken) &&
                    !string.IsNullOrEmpty(SettingServices.GetSettingValue(SettingStrings.UniversalToken)))
                    return "true";
            }
            else if (task == "debug" && SettingServices.GetSettingValue(SettingStrings.DebugRequiresLogin) == "Yes")
            {
                var user = userServices.GetUserByToken(token);
                if (user != null)
                {
                    if (new AuthorizationServices(user.Id, AuthorizationStrings.AllowDebug).IsAuthorized())
                        return "true";
                }
            }
            else if (task == "clobber" && SettingServices.GetSettingValue(SettingStrings.ClobberRequiresLogin) == "No")
            {
                if (token == SettingServices.GetSettingValue(SettingStrings.UniversalToken) &&
                    !string.IsNullOrEmpty(SettingServices.GetSettingValue(SettingStrings.UniversalToken)))
                    return "true";
            }
            else if (task == "clobber" && SettingServices.GetSettingValue(SettingStrings.ClobberRequiresLogin) == "Yes")
            {
                var user = userServices.GetUserByToken(token);
                if (user != null)
                {
                    if (new AuthorizationServices(user.Id, AuthorizationStrings.ImageDeployTask).IsAuthorized())
                        return "true";
                }
            }

            return "false";
        }

        public string GetRegistrationSettings()
        {
            var regDto = new RegistrationDTO();
            regDto.registrationEnabled = SettingServices.GetSettingValue(SettingStrings.RegistrationEnabled);
            regDto.keepNamePrompt = SettingServices.GetSettingValue(SettingStrings.OnDemandNamePrompt);
            return JsonConvert.SerializeObject(regDto);
        }

        public void DeleteImage(int profileId)
        {
            var profile = new ImageProfileServices().ReadProfile(profileId);
            if (string.IsNullOrEmpty(profile.Image.Name)) return;
            //Remove existing custom deploy schema, it may not match newly updated image
            profile.CustomSchema = string.Empty;
            new ImageProfileServices().UpdateProfile(profile);

            var delResult = new FilesystemServices().DeleteImageFolders(profile.Image.Name);
            if (delResult)
                new FilesystemServices().CreateNewImageFolders(profile.Image.Name);
        }

        public string CheckModelMatch(string environment, string systemModel)
        {
            var modelTask = new ModelTaskDTO();
            //Check for model match
            var modelMatchProfile = new ImageProfileServices().GetModelMatch(systemModel,environment);
            if (modelMatchProfile != null)
            {
                var image = new ImageServices().GetImage(modelMatchProfile.ImageId);
                if (image != null)
                    modelTask.imageName = image.Name;
                modelTask.imageProfileId = modelMatchProfile.Id.ToString();
                modelTask.imageProfileName = modelMatchProfile.Name;
                return JsonConvert.SerializeObject(modelTask);
            }
            return JsonConvert.SerializeObject(new ModelTaskDTO());
        }

        public string DetermineTask(string idType, string id)
        {
            var determineTaskDto = new DetermineTaskDTO();
            var computerServices = new ComputerServices();
            ComputerEntity computer;
            if (idType == "mac")
            {
                //When searching for computer by mac, that means the first check of searching by cliend id
                //did not yield any results.  Fall back to mac only for legacy support, but don't include computers
                //that have a valid client id.
                computer = computerServices.GetComputerFromMac(id);
                if (computer != null)
                {
                    if (!string.IsNullOrEmpty(computer.ClientIdentifier))
                    {
                        //act like a computer wasn't found
                        computer = null;
                    }
                }
            }
            else
            {
                id = id.ToUpper();
                computer = computerServices.GetComputerFromClientIdentifier(id);
            }

            if (computer == null)
            {
                determineTaskDto.task = "ond";
                determineTaskDto.computerId = "false";
                return JsonConvert.SerializeObject(determineTaskDto);
            }

            var computerTask = computerServices.GetTaskForComputerCheckin(computer.Id);
            if (computerTask == null)
            {
                determineTaskDto.computerId = computer.Id.ToString();
                determineTaskDto.task = "ond";
            }
            else
            {
                determineTaskDto.computerId = computer.Id.ToString();
                determineTaskDto.task = computerTask.Type;
                determineTaskDto.taskId = computerTask.Id.ToString();
            }

            return JsonConvert.SerializeObject(determineTaskDto);
        }

        

        public string DistributionPoint(string dpId, string task)
        {
            var smb = new SMB();

            var dp = new DistributionPointServices().GetDistributionPoint(Convert.ToInt32(dpId));

            smb.SharePath = "//" + StringManipulationServices.PlaceHolderReplace(dp.Server) + "/" + dp.ShareName;
            smb.Domain = dp.Domain;
            smb.DisplayName = dp.DisplayName;
            smb.IsPrimary = dp.IsPrimary == 1 ? "true" : "false";
            if (task.Contains("upload"))
            {
                smb.Username = dp.RwUsername;
                smb.Password = new EncryptionServices().DecryptText(dp.RwPassword);
            }
            else
            {
                smb.Username = dp.RoUsername;
                smb.Password = new EncryptionServices().DecryptText(dp.RoPassword);
            }

            return JsonConvert.SerializeObject(smb);
        }

        public void ErrorEmail(int taskId, string error)
        {
            var activeImagingTaskServices = new ActiveImagingTaskServices();
            var task = activeImagingTaskServices.GetTask(taskId);
            activeImagingTaskServices.SendTaskErrorEmail(task, error);
        }

        public string GetAllClusterDps(int computerId)
        {
            var rnd = new Random();
            var computerServices = new ComputerServices();
            var computer = computerServices.GetComputer(computerId);

            if (SettingServices.ServerIsNotClustered)
            {
                return "single";
            }

            var clusterServices = new ClusterGroupServices();

            ClusterGroupEntity clusterGroup;

            if (computer != null)
            {
                clusterGroup = new ComputerServices().GetClusterGroup(computer.Id);
            }
            else
            {
                //on demand computer might be null
                //use default cluster group
                clusterGroup = clusterServices.GetDefaultClusterGroup();
            }

            //Something went wrong
            if (clusterGroup == null) return "false";

            var clusterDps = clusterServices.GetClusterDps(clusterGroup.Id);
            var dpList = clusterDps.Select(clusterDp => clusterDp.DistributionPointId).ToList();
            var randomDpList = new List<int>();
            try
            {
                randomDpList = dpList.OrderBy(x => rnd.Next()).ToList();
            }
            catch (Exception ex)
            {
                log.Error("Could Not Select Random Distribution Point");
                log.Error(ex.Message);
                return "false";
            }

            var result = "";
            foreach (var dpId in randomDpList)
            {
                result += dpId + " ";
            }

            return result;
        }

        public string GetCustomPartitionScript(int profileId)
        {
            return new ImageProfileServices().ReadProfile(profileId).CustomPartitionScript;
        }

        public string GetCustomScript(int scriptId)
        {
            var script = new ScriptServices().GetScript(scriptId);
            return script.Contents;
        }

        public string GetFileCopySchema(int profileId)
        {
            var fileFolderSchema = new FileFolderCopySchema {FilesAndFolders = new List<FileFolderCopy>()};
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
                         clientHd + partitionPrefix +
                         part.VolumeGroup.PhysicalVolume[part.VolumeGroup.PhysicalVolume.Length - 1] + "\r\n";
                result += "vgcreate " + part.VolumeGroup.Name + " " + clientHd + partitionPrefix +
                          part.VolumeGroup.PhysicalVolume[part.VolumeGroup.PhysicalVolume.Length - 1] + " -yf" + "\r\n";
                result += "echo \"" + part.VolumeGroup.Uuid + "\" >>/tmp/vg-" + part.VolumeGroup.Name +
                          "\r\n";
                foreach (var lv in part.VolumeGroup.LogicalVolumes.Where(lv => lv.Active))
                {
                    result += "lvcreate --yes -L " + lv.Size + "s -n " + lv.Name + " " +
                              lv.VolumeGroup + "\r\n";
                    var uuid = lv.FsType == "swap" ? lv.Uuid.Split('#')[0] : lv.Uuid;
                    result += "echo \"" + uuid + "\" >>/tmp/" + lv.VolumeGroup + "-" +
                              lv.Name + "\r\n";
                }
                result += "vgcfgbackup -f /tmp/lvm-" + part.VolumeGroup.Name + "\r\n";
            }

            return result;
        }

        public string GetSysprepTag(int tagId, string imageEnvironment)
        {
            var tag = new SysprepTagServices().GetSysprepTag(tagId);
            tag.OpeningTag = StringManipulationServices.EscapeCharacter(tag.OpeningTag, new[] {">", "<"});
            tag.ClosingTag = StringManipulationServices.EscapeCharacter(tag.ClosingTag, new[] {">", "<", "/"});
            tag.Contents = StringManipulationServices.EscapeCharacter(tag.Contents, new[] {">", "<", "/", "\""});

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

        public string ImageList(string environment, string computerId,string task, int userId = 0)
        {
            var images = new ImageServices().GetOnDemandImageList(task,userId);

            if (images.Count == 0)
            {
                var imageList = new ImageList { Images = new List<string>() };
                imageList.Images.Add(-1 + " " + "No_Images_Found");
                return JsonConvert.SerializeObject(imageList);
            }
            if (computerId == "false")
                computerId = "0";
            var filteredImages = new ComputerImageClassificationServices().FilterForOnDemandList(Convert.ToInt32(computerId), images);
            if (environment == "winpe")
            {
                filteredImages = filteredImages.Where(x => x.Environment == "winpe").ToList();
                var imageList = new List<WinPEImageList>();
                foreach (var image in filteredImages)
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
                var imageList = new ImageList { Images = new List<string>() };
                if (images.Count == 0)
                {
                    imageList.Images.Add(-1 + " " + "No_Images_Found");
                    return JsonConvert.SerializeObject(imageList);
                }


            
                if (environment == "linux")
                    filteredImages =
                        filteredImages.Where(x => x.Environment != "winpe").ToList();
                foreach (var image in filteredImages)
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
                var imageProfileList = new WinPEProfileList {ImageProfiles = new List<WinPEProfile>()};
                var profileCounter = 0;
                foreach (var imageProfile in imageServices.SearchProfiles(Convert.ToInt32(imageId)).OrderBy(x => x.Name)
                    )
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

                var profileCounter = 0;
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

        public string IsLoginRequired(string task)
        {
            switch (task)
            {
                case "ond":
                    return SettingServices.GetSettingValue(SettingStrings.OnDemandRequiresLogin);
                case "debug":
                    return SettingServices.GetSettingValue(SettingStrings.DebugRequiresLogin);
                case "register":
                    return SettingServices.GetSettingValue(SettingStrings.RegisterRequiresLogin);
                case "clobber":
                    return SettingServices.GetSettingValue(SettingStrings.ClobberRequiresLogin);
                case "deploy":
                case "permanentdeploy":
                case "upload":
                case "multicast":
                case "modelmatchdeploy":
                    return SettingServices.GetSettingValue(SettingStrings.WebTaskRequiresLogin);
              

                default:
                    return "Yes";
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
                var multicastList = new MulticastList {Multicasts = new List<string>()};

                foreach (var multicast in new ActiveMulticastSessionServices().GetOnDemandList())
                {
                    multicastList.Multicasts.Add(multicast.Port + " " + multicast.Name);
                }

                return JsonConvert.SerializeObject(multicastList);
            }
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

        public string OnDemandCheckIn(string mac, int objectId, string task, string userId, string computerId)
        {
            var checkIn = new CheckIn();
            var computerServices = new ComputerServices();

            if (userId != null) //on demand
            {
                //Check permissions
                if (task.Contains("deploy"))
                {
                    if (
                        !new AuthorizationServices(Convert.ToInt32(userId), AuthorizationStrings.ImageDeployTask)
                            .IsAuthorized())
                    {
                        checkIn.Result = "false";
                        checkIn.Message = "This User Is Not Authorized To Deploy Images";
                        return JsonConvert.SerializeObject(checkIn);
                    }
                }

                if (task.Contains("upload"))
                {
                    if (
                        !new AuthorizationServices(Convert.ToInt32(userId), AuthorizationStrings.ImageUploadTask)
                            .IsAuthorized())
                    {
                        checkIn.Result = "false";
                        checkIn.Message = "This User Is Not Authorized To Upload Images";
                        return JsonConvert.SerializeObject(checkIn);
                    }
                }

                if (task.Contains("multicast"))
                {
                    if (
                        !new AuthorizationServices(Convert.ToInt32(userId), AuthorizationStrings.ImageMulticastTask)
                            .IsAuthorized())
                    {
                        checkIn.Result = "false";
                        checkIn.Message = "This User Is Not Authorized To Multicast Images";
                        return JsonConvert.SerializeObject(checkIn);
                    }
                }
            }

            ComputerEntity computer = null;
            if (computerId != "false")
                computer = computerServices.GetComputer(Convert.ToInt32(computerId));

            ImageProfileWithImage imageProfile;

            var arguments = "";
            if (task == "deploy" || task == "upload" || task == "clobber" || task == "ondupload" || task == "onddeploy" ||
                task == "unregupload" || task == "unregdeploy" || task =="modelmatchdeploy")
            {
                imageProfile = new ImageProfileServices().ReadProfile(objectId);
                arguments = new CreateTaskArguments(computer, imageProfile, task).Execute();
            }
            else //Multicast
            {
                var multicast = new ActiveMulticastSessionServices().GetFromPort(objectId);
                imageProfile = new ImageProfileServices().ReadProfile(multicast.ImageProfileId);
                arguments = new CreateTaskArguments(computer, imageProfile, task).Execute(objectId.ToString());
            }

            var imageDistributionPoint = new GetImageServer(computer, task).Run();

            if (imageProfile.Image.Protected == 1 && (task == "upload" || task == "ondupload" || task == "unregupload"))
            {
                checkIn.Result = "false";
                checkIn.Message = "This Image Is Protected";
                return JsonConvert.SerializeObject(checkIn);
            }

            if (imageProfile.Image.Environment == "")
                imageProfile.Image.Environment = "linux";
            checkIn.ImageEnvironment = imageProfile.Image.Environment;

            if (imageProfile.Image.Environment == "winpe")
                arguments += "dp_id=\"" + imageDistributionPoint + "\"\r\n";
            else
                arguments += " dp_id=\"" + imageDistributionPoint + "\"";

            var activeTask = new ActiveImagingTaskEntity();
            activeTask.Direction = task;
            activeTask.UserId = Convert.ToInt32(userId);
            activeTask.Type = task;

            activeTask.DpId = imageDistributionPoint;
            activeTask.Status = "1";

            if (computer == null)
            {
                //Create Task for an unregistered on demand computer
                var rnd = new Random(DateTime.Now.Millisecond);
                var newComputerId = rnd.Next(-200000, -100000);

                if (imageProfile.Image.Environment == "winpe")
                    arguments += "computer_id=" + newComputerId + "\r\n";
                else
                    arguments += " computer_id=" + newComputerId;
                activeTask.ComputerId = newComputerId;
                activeTask.Arguments = mac;
            }
            else
            {
                //Create Task for a registered on demand computer
                activeTask.ComputerId = computer.Id;
                activeTask.Arguments = arguments;
            }
            new ActiveImagingTaskServices().AddActiveImagingTask(activeTask);
        
            var auditLog = new AuditLogEntity();
            switch (task)
            {
                case "ondupload":
                case "unregupload":
                case "upload":
                    auditLog.AuditType = AuditEntry.Type.OndUpload;
                    break;        
                default:
                    auditLog.AuditType = AuditEntry.Type.OndDeploy;
                    break;
            }

            try
            {
                auditLog.ObjectId = activeTask.ComputerId;
                var user = new UserServices().GetUser(activeTask.UserId);
                if (user != null)
                    auditLog.UserName = user.Name;
                auditLog.ObjectName = computer != null ? computer.Name : mac;
                auditLog.Ip = "";
                auditLog.UserId = activeTask.UserId;
                auditLog.ObjectType = "Computer";
                auditLog.ObjectJson = JsonConvert.SerializeObject(activeTask);
                new AuditLogServices().AddAuditLog(auditLog);

                auditLog.ObjectId = imageProfile.ImageId;
                auditLog.ObjectName = imageProfile.Image.Name;
                auditLog.ObjectType = "Image";
                new AuditLogServices().AddAuditLog(auditLog);

            }
            catch
            {
                //Do Nothing              
            }
          

            checkIn.Result = "true";
            checkIn.TaskArguments = arguments;
            checkIn.TaskId = activeTask.Id.ToString();
            return JsonConvert.SerializeObject(checkIn);
        }

        public void PermanentTaskCheckOut(int taskId)
        {
            var activeImagingTaskServices = new ActiveImagingTaskServices();
            var task = activeImagingTaskServices.GetTask(taskId);
            task.Status = "0";
            task.Partition = "";
            task.Completed = "";
            task.Elapsed = "";
            task.Rate = "";
            task.Remaining = "";
            activeImagingTaskServices.UpdateActiveImagingTask(task);

            activeImagingTaskServices.SendTaskCompletedEmail(task);
        }

        public string UpdateGuid(int profileId)
        {
            var imageProfile = new ImageProfileServices().ReadProfile(profileId);
            var imageServices = new ImageServices();
            var image = imageServices.GetImage(imageProfile.ImageId);
            var guid = Guid.NewGuid().ToString();
            image.LastUploadGuid = guid;
            imageServices.UpdateImage(image);
            return guid;
        }

        public void UpdateProgress(int taskId, string progress, string progressType)
        {
            var activeImagingTaskServices = new ActiveImagingTaskServices();
            if (string.IsNullOrEmpty(progress)) return;
            var task = activeImagingTaskServices.GetTask(taskId);
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

            activeImagingTaskServices.UpdateActiveImagingTask(task);
        }

        public void UpdateProgressPartition(int taskId, string partition)
        {
            var activeImagingTaskServices = new ActiveImagingTaskServices();
            var task = activeImagingTaskServices.GetTask(taskId);
            task.Partition = partition;
            task.Elapsed = "Please Wait...";
            task.Remaining = "";
            task.Completed = "";
            task.Rate = "";
            activeImagingTaskServices.UpdateActiveImagingTask(task);
        }

        public void UploadLog(string computerId, string logContents, string subType, string computerMac)
        {
            if (computerId == "false")
                computerId = "-1";
            var computerLog = new ComputerLogEntity
            {
                ComputerId = Convert.ToInt32(computerId),
                Contents = logContents,
                Type = "image",
                SubType = subType,
                Mac = computerMac
            };
            new ComputerLogServices().AddComputerLog(computerLog);
        }
    }
}