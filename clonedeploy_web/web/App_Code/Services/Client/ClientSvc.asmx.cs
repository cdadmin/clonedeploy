using System;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using BLL;
using BLL.ClientPartitioning;


namespace Services.Client
{
    [WebService(Namespace = "http://localhost/cruciblewds/ClientSvc.asmx")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class ClientSvc : WebService
    {
      
        /*
        [WebMethod(EnableSession = true)]
        public void AddHost(Models.Computer host)
        {
            if (!Authenticate()) return;
            new BLL.Computer().AddComputer(host);
            HttpContext.Current.Response.Write(Message.Text);
        }

        [WebMethod(EnableSession = true)]
        public void AddImage(Models.Image image)
        {
            if (!Authenticate()) return;
            new BLL.Image().AddImage(image);
            HttpContext.Current.Response.Write(image.Id + "," + Message.Text);
        }

        [WebMethod]
        public void AlignBcdToPartition()
        {
            var bcd = Utility.Decode(HttpContext.Current.Request.Form["bcd"]);
            var offsetBytes = Utility.Decode(HttpContext.Current.Request.Form["offsetBytes"]);
            HttpContext.Current.Response.Write(new Download().AlignBcdToPartition(bcd,offsetBytes));
        }

        [WebMethod]
        public void AmINext()
        {
            HttpContext.Current.Response.Write(new Download().AmINext());
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
        public void CheckIn()
        {
            var mac = Utility.Decode(HttpContext.Current.Request.Form["mac"]);
            if (Authenticate()) HttpContext.Current.Response.Write(new Global().CheckIn(mac));
        }

        [WebMethod]
        public void CheckOut()
        {
            var imgName = Utility.Decode(HttpContext.Current.Request.Form["imgName"]);
            var direction = Utility.Decode(HttpContext.Current.Request.Form["direction"]);
            var mac = Utility.Decode(HttpContext.Current.Request.Form["mac"]);

            if (Authenticate()) HttpContext.Current.Response.Write(new Global().CheckOut(mac, direction, imgName));
        }

      

        [WebMethod]
        public void ConsoleLogin()
        {
            var ip = Utility.Decode(HttpContext.Current.Request.Form["clientIP"]);
            var username = Utility.Decode(HttpContext.Current.Request.Form["username"]);
            var password = Utility.Decode(HttpContext.Current.Request.Form["password"]);
            var task = Utility.Decode(HttpContext.Current.Request.Form["task"]);
            var isWebTask = Utility.Decode(HttpContext.Current.Request.Form["isWebTask"]);

            HttpContext.Current.Response.Write(new Authenticate().ConsoleLogin(username, password, task, isWebTask, ip));
        }

        [WebMethod]
        public void CreateDirectory()
        {
            var imageName = Utility.Decode(HttpContext.Current.Request.Form["imgName"]);
            var dirName = Utility.Decode(HttpContext.Current.Request.Form["dirName"]);

            if (Authenticate()) HttpContext.Current.Response.Write(new Upload().CreateDirectory(imageName, dirName));
        }

        [WebMethod]
        public void CurrentQueuePosition()
        {
            var mac = Utility.Decode(HttpContext.Current.Request.Form["mac"]);
            HttpContext.Current.Response.Write(new Download().CurrentQueuePosition(mac));
        }

        [WebMethod]
        public void DeleteImage()
        {
            var imageName = Utility.Decode(HttpContext.Current.Request.Form["imgName"]);

            if (Authenticate()) HttpContext.Current.Response.Write(new Upload().DeleteImage(imageName));
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
        public void GetLocalDateTime()
        {
            HttpContext.Current.Response.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [WebMethod]
        public void GetMinHdSize(string imgName, string hdToGet, string newHdSize)
        {
            HttpContext.Current.Response.Write(new Download().GetMinHdSize(imgName, hdToGet, newHdSize));
        }

        [WebMethod]
        public void GetOriginalLvm(string imgName, string hdToGet, string clienthd)
        {
            HttpContext.Current.Response.Write(new Download().GetOriginalLvm(imgName, clienthd, hdToGet));
        }
        */

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
        /*
        [WebMethod]
        public void GetUtcDateTime()
        {
            HttpContext.Current.Response.Write(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }

      

        [WebMethod]
        public void ImageSize()
        {
            var imagename = Utility.Decode(HttpContext.Current.Request.Form["imgName"]);
            var imagesize = Utility.Decode(HttpContext.Current.Request.Form["imageSize"]);
         

            if (Authenticate()) HttpContext.Current.Response.Write(new Upload().AddNewImageSpecs(imagename, imagesize));
        }

        [WebMethod]
        public void InSlot()
        {
            var mac = Utility.Decode(HttpContext.Current.Request.Form["mac"]);
            if (Authenticate()) HttpContext.Current.Response.Write(new Download().InSlot(mac));
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
        public void IsLoginRequired(string task)
        {
            HttpContext.Current.Response.Write(new Global().IsLoginRequired(task));
        }

        [WebMethod]
        public void ListImages()
        {
            if (Authenticate()) HttpContext.Current.Response.Write(new OnDemand().GetImageListing());
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

        [WebMethod]
        public void QueuePosition()
        {
            var mac = Utility.Decode(HttpContext.Current.Request.Form["mac"]);
            HttpContext.Current.Response.Write(new Download().QueuePosition(mac));
        }

        [WebMethod]
        public void QueueStatus()
        {
            HttpContext.Current.Response.Write(new Download().QueueStatus());
        }

        [WebMethod]
        public void SmbCredentials()
        {
            var credential = Utility.Decode(HttpContext.Current.Request.Form["credential"]);
            if (Authenticate()) HttpContext.Current.Response.Write(new Global().GetSmbCredentials(credential));
        }

        [WebMethod]
        public void StartReceiver()
        {
            var imagePath = Utility.Decode(HttpContext.Current.Request.Form["imgPath"]);
            var port = Utility.Decode(HttpContext.Current.Request.Form["portBase"]);

            if (Authenticate()) HttpContext.Current.Response.Write(new Upload().StartReceiver(imagePath, port));
        }

        [WebMethod]
        public void UcInfo()
        {
            var direction = Utility.Decode(HttpContext.Current.Request.Form["direction"]);
            var mac = Utility.Decode(HttpContext.Current.Request.Form["mac"]);
            var imageId = Utility.Decode(HttpContext.Current.Request.Form["imageID"]);


            if (Authenticate())
                HttpContext.Current.Response.Write(new OnDemand().GetCustomUnicastInfo(direction, mac,
                    imageId));
        }

        [WebMethod(EnableSession = true)]
        public void UpdateProgress(TaskProgress progress)
        {
            progress.UpdateProgress();
        }

        [WebMethod]
        public void UpdateProgressPartition(string hostName, string partition)
        {
            var progress = new TaskProgress();
            progress.UpdateProgressPartition(hostName, partition);
        }

        [WebMethod]
        public void Upload()
        {
            var fileName = Utility.Decode(HttpContext.Current.Request.Form["fileName"]);
            var imagePath = Utility.Decode(HttpContext.Current.Request.Form["imagePath"]);
            var fileType = Utility.Decode(HttpContext.Current.Request.Form["fileType"]);
            var file = HttpContext.Current.Request.Files;

            if (Authenticate())
                HttpContext.Current.Response.Write(new Upload().UploadFile(fileName, imagePath, fileType, file));
        }*/
    }
}