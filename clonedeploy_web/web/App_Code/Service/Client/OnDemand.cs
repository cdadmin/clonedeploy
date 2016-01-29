using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using Helpers;
using Models;
using Newtonsoft.Json;

namespace Service.Client
{
    public class OnDemand
    {
        public string ImageList(int userId = 0)
        {
            var imageList = new Services.Client.ImageList { Images = new List<string>() };

            foreach (var image in BLL.Image.GetOnDemandImageList(userId))
                imageList.Images.Add(image.Id + " " + image.Name);

            return JsonConvert.SerializeObject(imageList);
        }

        public string ImageProfileList(int imageId)
        {
            var imageProfileList = new Services.Client.ImageProfileList { ImageProfiles = new List<string>() };

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

        public string AddImage(string imageName)
        {
            var image = new Models.Image()
            {
                Name = imageName

            };
            var result = BLL.Image.AddImage(image);
            if (result.IsValid)
                result.Message = image.Id.ToString();

            return JsonConvert.SerializeObject(result);
        }

        /*
        public string GetCustomMulticastInfo(string mac, string mcTaskName)
        {
            Models.ActiveMulticastSession mcTask;
            var computer = new BLL.Computer().GetComputerFromMac(mac.ToLower());
  
            using (var db = new DB())
            {
                mcTask = db.ActiveMcTasks.FirstOrDefault(t => t.Name == mcTaskName);
            }

            return "portBase=" + mcTask.Port + " imgName=" + mcTask.Image + " computerName=" + computer.Name;
        }

        public string GetCustomMulticastSessions()
        {
            string result = null;
            var listSessions = new List<string>();
            try
            {
                using (var db = new DB())
                {
                    var mcTasks = db.ActiveMcTasks;
                    foreach (var mcTask in mcTasks)
                    {
                        int n;
                        if(int.TryParse(mcTask.Name, out n))
                            listSessions.Add(mcTask.Name);
                    }
                }
               

                if (listSessions.Count == 0)
                    result = "There Are No Active Sessions";
                else
                {
                    foreach (var session in listSessions)
                        result += session + " ";
                }
            }
            catch (Exception ex)
            {
                result = "Could Not Read Active Multicasts.  Check The Exception Log For More Info";
                Logger.Log(ex.ToString());
            }
            return result;
        }

        public string GetCustomUnicastInfo(string direction, string mac, string imageId)
        {
            var image = new BLL.Image().GetImage(Convert.ToInt32(imageId));


            var computer = new BLL.Computer().GetComputerFromMac(mac.ToLower());

            if (direction == "push")
            {
                if (!new BLL.Image().Check_Checksum(image))
                {
                    return "Client Error: This Image Has Not Been Confirmed And Cannot Be Deployed.";
                }
            }

            string storage;
            var serverIp = Settings.ServerIp;
            var xferMode = Settings.ImageTransferMode;
            var compAlg = Settings.CompressionAlgorithm;
            var compLevel = Settings.CompressionLevel;
            string result;
            if (xferMode == "smb" || xferMode == "smb+http")
                storage = Settings.SmbPath;
            else
            {
                storage = direction == "pull" ? Settings.NfsUploadPath : Settings.NfsDeployPath;
            }

            //FIX ME
            result = "imgName=" + image.Name + " computerName=" + computer.Name +
                     //" computerScripts=" + "\"" + computer.Scripts + "\" " + computer.Args + " storage=" +
                     storage + " serverIP=" + serverIp + " xferMode=" + xferMode + " compAlg=" + compAlg +
                     " compLevel=-" + compLevel + " imageProtected=" + image.Protected;

            if (direction == "pull" && Settings.ImageTransferMode == "udp+http")
            {
                var portBase = new BLL.Port().GetNextPort();
                result = result + " portBase=" + portBase;
            }

            return result;
        }
        */
      
    }
}