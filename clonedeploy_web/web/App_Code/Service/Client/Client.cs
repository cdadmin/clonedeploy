using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using BLL;
using BLL.Workflows;
using Helpers;
using Newtonsoft.Json;
using Security;


namespace Service.Client
{
    [WebService(Namespace = "http://localhost/clonedeploy/Client.asmx")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Client : WebService
    {
        private bool Authorize()
        {
            var userToken = Utility.Decode(HttpContext.Current.Request.Headers["Authorization"]);
            if (Settings.ServerKey == null || userToken == null)
            {
                HttpContext.Current.Response.StatusCode = 403;
                return false;
            }
            if (userToken != Settings.ServerKey)
            {
                HttpContext.Current.Response.StatusCode = 403;
                return false;
            }
            else
            {
                return true;
            }
        }

        [WebMethod]
        public void GetPartLayout(string imageProfileId, string hdToGet, string newHdSize, string clientHd, string taskType)
        {

            var partLayout = new ClientPartitionScript
            {
                profileId = Convert.ToInt32(imageProfileId),
                HdNumberToGet = Convert.ToInt16(hdToGet),
                NewHdSize = newHdSize,
                ClientHd = clientHd,
                TaskType = taskType
            };

            HttpContext.Current.Response.Write(partLayout.GeneratePartitionScript());
        }

        [WebMethod]
        public void GetUtcDateTime()
        {
            HttpContext.Current.Response.Write(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [WebMethod]
        public void GetLocalDateTime()
        {
            HttpContext.Current.Response.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [WebMethod]
        public void IsLoginRequired(string task)
        {
            HttpContext.Current.Response.Write(new Service.Client.Global().IsLoginRequired(task));
        }

        [WebMethod]
        public void ConsoleLogin()
        {
            var ip = Utility.Decode(HttpContext.Current.Request.Form["clientIP"]);
            var username = Utility.Decode(HttpContext.Current.Request.Form["username"]);
            var password = Utility.Decode(HttpContext.Current.Request.Form["password"]);
            var task = Utility.Decode(HttpContext.Current.Request.Form["task"]);

            HttpContext.Current.Response.Write(new Authenticate().ConsoleLogin(username, password, task, ip));
        }

        [WebMethod]
        public void DownloadCoreScripts(string scriptName)
        {
            if (!Authorize()) return;
            var scriptPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                             Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar + "core" +
                             Path.DirectorySeparatorChar;

            if (File.Exists(scriptPath + scriptName))
            {
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AppendHeader("Content-Disposition",
                    "attachment; filename=" + scriptName);
                HttpContext.Current.Response.TransmitFile(scriptPath + scriptName);
                HttpContext.Current.Response.End();
            }
            else
                HttpContext.Current.Response.StatusCode = 420;
        }

        [WebMethod]
        public void AddComputer(string name, string mac, string imageId, string imageProfileId)
        {
            if (!Authorize()) return;        
            HttpContext.Current.Response.Write(new Service.Client.Global().AddComputer(name,mac,imageId,imageProfileId));
        }

        [WebMethod]
        public void AddImage(string name)
        {
            if (!Authorize()) return;      
            HttpContext.Current.Response.Write(new Service.Client.OnDemand().AddImage(name));
        }

        [WebMethod]
        public void ListImages(string userId)
        {
            HttpContext.Current.Response.Write(new Service.Client.OnDemand().ImageList(Convert.ToInt32(userId)));
        }

        [WebMethod]
        public void ListImageProfiles(string imageId)
        {
            HttpContext.Current.Response.Write(new Service.Client.OnDemand().ImageProfileList(Convert.ToInt32(imageId)));
        }

        [WebMethod]
        public void CheckIn(string computerMac)
        {
           HttpContext.Current.Response.Write(new Service.Client.Global().CheckIn(computerMac));
        }

        [WebMethod]
        public void DistributionPoint(string dpId)
        {
            HttpContext.Current.Response.Write(new Global().DistributionPoint(Convert.ToInt32(dpId)));
        }

        [WebMethod]
        public void UpdateStatusInProgress(int computerId)
        {
            new Global().ChangeStatusInProgress(computerId);

        }

        [WebMethod]
        public void DeleteImage(string profileId)
        {
            new Global().DeleteImage(Convert.ToInt32(profileId));
        }


        [WebMethod]
        public void CheckOut(string computerMac)
        {
            new Service.Client.Global().CheckOut(computerMac);
        }

        [WebMethod]
        public void UploadLog()
        {
            var computerId = Utility.Decode(HttpContext.Current.Request.Form["computerId"]);
            var logContents = Utility.Decode(HttpContext.Current.Request.Form["logContents"]);
            var subType = Utility.Decode(HttpContext.Current.Request.Form["subType"]);
            new Global().UploadLog(Convert.ToInt32(computerId), logContents, subType);
        }

        [WebMethod]
        public void UpdateProgress(string computerId, string progress, string progressType)
        {
            new Service.Client.Global().UpdateProgress(Convert.ToInt32(computerId), progress, progressType);
        }

        [WebMethod]
        public void UpdateProgressPartition(string computerId, string partition)
        {
            new Service.Client.Global().UpdateProgressPartition(Convert.ToInt32(computerId), partition);
        }

        [WebMethod]
        public void OnDemandTaskArgs(string mac, string profileId, string taskType)
        {
            HttpContext.Current.Response.Write(new Service.Client.Global().OnDemandTaskArguments(mac, Convert.ToInt32(profileId), taskType));
        }

        [WebMethod]
        public void CheckQueue(string computerId)
        {
            HttpContext.Current.Response.Write(new Service.Client.Global().CheckQueue(Convert.ToInt32(computerId)));
        }

        [WebMethod]
        public void CheckHdRequirements(string profileId, string clientHdNumber, string newHdSize, string schemaHds)
        {
            HttpContext.Current.Response.Write(new Service.Client.Global().CheckHdRequirements(Convert.ToInt32(profileId),Convert.ToInt32(clientHdNumber),newHdSize,schemaHds));
        }

        [WebMethod]
        public void GetOriginalLvm(string profileId, string clientHd, string hdToGet)
        {
            HttpContext.Current.Response.Write(new Service.Client.Global().GetOriginalLvm(Convert.ToInt32(profileId), clientHd, hdToGet));
        }

        [WebMethod]
        public void CheckForCancelledTask(string computerId)
        {
            HttpContext.Current.Response.Write(new Service.Client.Global().CheckForCancelledTask(Convert.ToInt32(computerId)));
        }

        [WebMethod]
        public void UpdateBcd()
        {
            var bcd = Utility.Decode(HttpContext.Current.Request.Form["bcd"]);
            var offsetBytes = Utility.Decode(HttpContext.Current.Request.Form["offsetBytes"]);
            HttpContext.Current.Response.Write(new BLL.Bcd().UpdateEntry(bcd, Convert.ToInt64(offsetBytes)));
        }

        [WebMethod]
        public void IpxeBoot(string filename, string type)
        {
            if (type == "kernel")
            {
                var path = Settings.TftpPath + "kernels" + Path.DirectorySeparatorChar;
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "inline; filename=" + filename);
                HttpContext.Current.Response.TransmitFile(path + filename);
                HttpContext.Current.Response.End();
            }
            else
            {
                var path = Settings.TftpPath + "images" + Path.DirectorySeparatorChar;
                HttpContext.Current.Response.ContentType = "application/x-gzip";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "inline; filename=" + filename);
                HttpContext.Current.Response.TransmitFile(path + filename);
                HttpContext.Current.Response.End();
            }
        }
        [WebMethod]
        public void IpxeLogin()
        {
            var username = HttpContext.Current.Request.Form["uname"];
            var password = HttpContext.Current.Request.Form["pwd"];
            var kernel = HttpContext.Current.Request.Form["kernel"];
            var bootImage = HttpContext.Current.Request.Form["bootImage"];
            var task = HttpContext.Current.Request.Form["task"];

            HttpContext.Current.Response.Write(new Authenticate().IpxeLogin(username, password, kernel, bootImage, task));
        }
        /*
      

     
        [WebMethod]
        public void AlignBcdToPartition()
        {
            var bcd = Utility.Decode(HttpContext.Current.Request.Form["bcd"]);
            var offsetBytes = Utility.Decode(HttpContext.Current.Request.Form["offsetBytes"]);
            HttpContext.Current.Response.Write(new Download().AlignBcdToPartition(bcd,offsetBytes));
        }

      

        private bool Authenticate()
        {
            var result = true;
            string message = null;

            if (Utility.Decode(HttpContext.Current.Request.Headers["Authorization"]) != Settings.ServerKey)
            {
                message = "Incorrect Server Key";
                Logger.Log(message);
                result = false;
            }
            if (result) return true;

            HttpContext.Current.Response.Write(message);
            HttpContext.Current.Response.StatusCode = 403;
            return false;

            //HttpContext.Current.Response.Write(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            //HttpContext.Current.Response.Write(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
        }

        

      

      

       

        [WebMethod]
        public void CreateDirectory()
        {
            var imageName = Utility.Decode(HttpContext.Current.Request.Form["imgName"]);
            var dirName = Utility.Decode(HttpContext.Current.Request.Form["dirName"]);

            if (Authenticate()) HttpContext.Current.Response.Write(new Upload().CreateDirectory(imageName, dirName));
        }

       

      

        [WebMethod]
        public void DownloadCoreScripts()
        {
            var scriptName = HttpContext.Current.Request.Form["scriptName"];

            if (!Authenticate()) return;

            var scriptPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                             Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar + "core" +
                             Path.DirectorySeparatorChar;
            if (File.Exists(scriptPath + scriptName))
            {
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AppendHeader("Content-Disposition",
                    "attachment; filename=" + scriptName);
                HttpContext.Current.Response.TransmitFile(scriptPath + scriptName);
                HttpContext.Current.Response.End();
            }
            else
                HttpContext.Current.Response.StatusCode = 420;
        }

        [WebMethod]
        public void DownloadCustomScripts()
        {
            if (!Authenticate()) return;

            var scriptName = HttpContext.Current.Request.Form["scriptName"];


            var scriptPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                             Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar;
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + scriptName);
            HttpContext.Current.Response.TransmitFile(scriptPath + scriptName);
            HttpContext.Current.Response.End();
        }

        [WebMethod]
        public void DownloadImage()
        {
            var partName = Utility.Decode(HttpContext.Current.Request.Form["partName"]);
            var imageName = Utility.Decode(HttpContext.Current.Request.Form["imgName"]);

            if (!Authenticate()) return;

            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + partName);
            HttpContext.Current.Response.TransmitFile(Settings.ImageStorePath + imageName +
                                                      Path.DirectorySeparatorChar + partName);
            HttpContext.Current.Response.End();
        }

        [WebMethod]
        public void GetFileNames()
        {
            var imageName = Utility.Decode(HttpContext.Current.Request.Form["imgName"]);
            if (Authenticate()) HttpContext.Current.Response.Write(new Download().GetFileNames(imageName));
        }

        [WebMethod]
        public void GetHdParameter(string imgName, string paramName, string hdToGet, string partNumber)
        {
            HttpContext.Current.Response.Write(new Download().GetHdParameter(imgName, hdToGet, partNumber, paramName));
        }

       

     

        [WebMethod]
        public void GetOriginalLvm(string imgName, string hdToGet, string clienthd)
        {
            HttpContext.Current.Response.Write(new Download().GetOriginalLvm(imgName, clienthd, hdToGet));
        }
        

      
        
        [WebMethod]
        public void ImageSize()
        {
            var imagename = Utility.Decode(HttpContext.Current.Request.Form["imgName"]);
            var imagesize = Utility.Decode(HttpContext.Current.Request.Form["imageSize"]);
         

            if (Authenticate()) HttpContext.Current.Response.Write(new Upload().AddNewImageSpecs(imagename, imagesize));
        }

       

        [WebMethod]
        public void IpxeBoot(string filename, string type)
        {
            if (type == "kernel")
            {
                var path = Settings.TftpPath + "kernels" + Path.DirectorySeparatorChar;
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "inline; filename=" + filename);
                HttpContext.Current.Response.TransmitFile(path + filename);
                HttpContext.Current.Response.End();
            }
            else
            {
                var path = Settings.TftpPath + "images" + Path.DirectorySeparatorChar;
                HttpContext.Current.Response.ContentType = "application/x-gzip";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "inline; filename=" + filename);
                HttpContext.Current.Response.TransmitFile(path + filename);
                HttpContext.Current.Response.End();
            }
        }

        [WebMethod]
        public void IpxeLogin()
        {
            var username = HttpContext.Current.Request.Form["uname"];
            var password = HttpContext.Current.Request.Form["pwd"];
            var kernel = HttpContext.Current.Request.Form["kernel"];
            var bootImage = HttpContext.Current.Request.Form["bootImage"];
            var task = HttpContext.Current.Request.Form["task"];

            HttpContext.Current.Response.Write(new Authenticate().IpxeLogin(username, password, kernel, bootImage, task));
        }

       

       

        [WebMethod]
        public void McCheckOut()
        {
            var portBase = Utility.Decode(HttpContext.Current.Request.Form["portBase"]);
            if (Authenticate()) HttpContext.Current.Response.Write(new Download().MulticastCheckout(portBase));
        }

        [WebMethod]
        public void McInfo()
        {
            var mcTaskName = Utility.Decode(HttpContext.Current.Request.Form["mcTaskName"]);
            var mac = Utility.Decode(HttpContext.Current.Request.Form["mac"]);

            if (Authenticate())
                HttpContext.Current.Response.Write(new OnDemand().GetCustomMulticastInfo(mac, mcTaskName));
        }

        [WebMethod(EnableSession = true)]
        public void McSessions()
        {
            if (Authenticate()) HttpContext.Current.Response.Write(new OnDemand().GetCustomMulticastSessions());
        }

        [WebMethod]
        public void ModifyKnownLayout(string clientHd, string layout)
        {
            HttpContext.Current.Response.Write(new Download().ModifyKnownLayout(layout, clientHd));
        }

      

       

     

       

       

      

      

      
         
        */
    }
}