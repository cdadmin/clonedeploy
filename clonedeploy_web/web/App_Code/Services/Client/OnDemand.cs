using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using Logic;
using Models;

namespace Services.Client
{
    public class OnDemand
    {
        public string GetCustomMulticastInfo(string mac, string mcTaskName)
        {
            ActiveMcTask mcTask;
            var host = new ComputerLogic().GetComputerFromMac(mac.ToLower());
  
            using (var db = new DB())
            {
                mcTask = db.ActiveMcTasks.FirstOrDefault(t => t.Name == mcTaskName);
            }

            return "portBase=" + mcTask.Port + " imgName=" + mcTask.Image + " hostName=" + host.Name;
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
            var image = new Image {Id = Convert.ToInt32(imageId)};
            image.Read();

            var host = new ComputerLogic().GetComputerFromMac(mac.ToLower());

            if (direction == "push")
            {
                if (!image.Check_Checksum())
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
            result = "imgName=" + image.Name + " hostName=" + host.Name +
                     //" hostScripts=" + "\"" + host.Scripts + "\" " + host.Args + " storage=" +
                     storage + " serverIP=" + serverIp + " xferMode=" + xferMode + " compAlg=" + compAlg +
                     " compLevel=-" + compLevel + " imageProtected=" + image.Protected;

            if (direction == "pull" && Settings.ImageTransferMode == "udp+http")
            {
                var portBase = new Port().GetPort();
                result = result + " portBase=" + portBase;
            }

            return result;
        }

        public string GetImageListing()
        {
            string result = null;
            using (var db = new DB())
            {
                var images = from i in db.Images where i.IsVisible == 1 orderby i.Name select i;
                 foreach (var image in images)
                    result += image.Id  + " " + image.Name + ",";
            }
         
            return result;
        }
    }
}