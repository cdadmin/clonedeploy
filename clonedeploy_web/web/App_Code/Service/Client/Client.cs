using System;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using BLL.Workflows;
using Helpers;
using Security;


namespace Service.Client
{
    [WebService(Namespace = "http://localhost/clonedeploy/service/client.asmx")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Client : WebService
    {
        private bool Authorize()
        {
            var userToken = Utility.Decode(HttpContext.Current.Request.Headers["Authorization"],"Authorization");
            if (new Service.Client.Logic().Authorize(userToken))
                return true;
            else
            {
                HttpContext.Current.Response.StatusCode = 401;
                Logger.Log("Incorrect Token Was Provided");
                return false;
            }
        }

        [WebMethod]
        public void CheckTaskAuth(string task)
        {
            var userToken = Utility.Decode(HttpContext.Current.Request.Headers["Authorization"], "Authorization");
            HttpContext.Current.Response.Write(new Service.Client.Logic().CheckTaskAuth(task,userToken));
        }

        [WebMethod]
        public void GetPartLayout(string imageProfileId, string hdToGet, string newHdSize, string clientHd, string taskType, string partitionPrefix)
        {
            //if (!Authorize()) return;

            var partLayout = new ClientPartitionScript
            {
                profileId = Convert.ToInt32(imageProfileId),
                HdNumberToGet = Convert.ToInt32(hdToGet),
                NewHdSize = newHdSize,
                ClientHd = clientHd,
                TaskType = taskType,
                partitionPrefix = partitionPrefix
            };

            HttpContext.Current.Response.Write(partLayout.GeneratePartitionScript());
        }

        [WebMethod]
        public void GetUtcDateTime()
        {
            //No auth
            HttpContext.Current.Response.Write(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [WebMethod]
        public void GetLocalDateTime()
        {
            //No auth
            HttpContext.Current.Response.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [WebMethod]
        public void IsLoginRequired(string task)
        {
            //No auth
            HttpContext.Current.Response.Write(new Service.Client.Logic().IsLoginRequired(task));
        }

        [WebMethod]
        public void ConsoleLogin()
        {
            //No auth
            var ip = Utility.Decode(HttpContext.Current.Request.Form["clientIP"], "clientIP");
            var username = Utility.Decode(HttpContext.Current.Request.Form["username"],"username");
            var password = Utility.Decode(HttpContext.Current.Request.Form["password"],"password");
            var task = Utility.Decode(HttpContext.Current.Request.Form["task"],"task");

            HttpContext.Current.Response.Write(new Authenticate().ConsoleLogin(username, password, task, ip));
        }

        [WebMethod]
        public void DownloadCoreScripts(string scriptName)
        {
            if (!Authorize()) return;
            var scriptPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                             Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar;

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
        public void AddComputer(string name, string mac)
        {
            if (!Authorize()) return;        
            HttpContext.Current.Response.Write(new Service.Client.Logic().AddComputer(name,mac));
        }

        [WebMethod]
        public void AddImage(string name)
        {
            if (!Authorize()) return;      
            HttpContext.Current.Response.Write(new Service.Client.Logic().AddImage(name));
        }

        [WebMethod]
        public void ListImages(string userId)
        {
            if (!Authorize()) return;
            if (string.IsNullOrEmpty(userId))
                userId = "0";
            HttpContext.Current.Response.Write(new Service.Client.Logic().ImageList(Convert.ToInt32(userId)));
        }

        [WebMethod]
        public void ListImageProfiles(string imageId)
        {
            if (!Authorize()) return;
            HttpContext.Current.Response.Write(new Service.Client.Logic().ImageProfileList(Convert.ToInt32(imageId)));
        }

        [WebMethod]
        public void ListMulticasts()
        {
            if (!Authorize()) return;
            HttpContext.Current.Response.Write(new Service.Client.Logic().MulicastSessionList());
        }

        [WebMethod]
        public void CheckIn(string computerMac)
        {
            if (!Authorize()) return;
           HttpContext.Current.Response.Write(new Service.Client.Logic().CheckIn(computerMac));
        }

        [WebMethod]
        public void DistributionPoint(string dpId, string task)
        {
            if (!Authorize()) return;
            HttpContext.Current.Response.Write(new Logic().DistributionPoint(Convert.ToInt32(dpId), task));
        }

        [WebMethod]
        public void UpdateStatusInProgress(int computerId)
        {
            if (!Authorize()) return;
            new Logic().ChangeStatusInProgress(computerId);

        }

        [WebMethod]
        public void DeleteImage(string profileId)
        {
            if (!Authorize()) return;
            new Logic().DeleteImage(Convert.ToInt32(profileId));
        }

        [WebMethod]
        public void ErrorEmail(string computerId, string error)
        {
            if (!Authorize()) return;
            new Service.Client.Logic().ErrorEmail(Convert.ToInt32(computerId),error);
        }


        [WebMethod]
        public void CheckOut(string computerId)
        {
            if (!Authorize()) return;
            new Service.Client.Logic().CheckOut(Convert.ToInt32(computerId));
        }

        [WebMethod]
        public void UploadLog()
        {
            if (!Authorize()) return;
            var computerId = Utility.Decode(HttpContext.Current.Request.Form["computerId"],"computerId");
            var logContents = Utility.Decode(HttpContext.Current.Request.Form["logContents"],"logContents");
            var subType = Utility.Decode(HttpContext.Current.Request.Form["subType"],"subType");
            var computerMac = Utility.Decode(HttpContext.Current.Request.Form["mac"],"mac");
            new Logic().UploadLog(Convert.ToInt32(computerId), logContents, subType, computerMac);
        }

        [WebMethod]
        public void UpdateProgress(string computerId, string progress, string progressType)
        {
            if (!Authorize()) return;
            new Service.Client.Logic().UpdateProgress(Convert.ToInt32(computerId), progress, progressType);
        }

        [WebMethod]
        public void UpdateProgressPartition(string computerId, string partition)
        {
            if (!Authorize()) return;
            new Service.Client.Logic().UpdateProgressPartition(Convert.ToInt32(computerId), partition);
        }

        [WebMethod]
        public void CheckQueue(string computerId)
        {
            if (!Authorize()) return;
            HttpContext.Current.Response.Write(new Service.Client.Logic().CheckQueue(Convert.ToInt32(computerId)));
        }

        [WebMethod]
        public void CheckHdRequirements(string profileId, string clientHdNumber, string newHdSize, string schemaHds)
        {
            if (!Authorize()) return;
            HttpContext.Current.Response.Write(new Service.Client.Logic().CheckHdRequirements(Convert.ToInt32(profileId),Convert.ToInt32(clientHdNumber),newHdSize,schemaHds));
        }

        [WebMethod]
        public void GetOriginalLvm(string profileId, string clientHd, string hdToGet, string partitionPrefix)
        {
            if (!Authorize()) return;
            HttpContext.Current.Response.Write(new Service.Client.Logic().GetOriginalLvm(Convert.ToInt32(profileId), clientHd, hdToGet, partitionPrefix));
        }

        [WebMethod]
        public void CheckForCancelledTask(string computerId)
        {
            if (!Authorize()) return;
            HttpContext.Current.Response.Write(new Service.Client.Logic().CheckForCancelledTask(Convert.ToInt32(computerId)));
        }

        [WebMethod]
        public void UpdateBcd()
        {
            if (!Authorize()) return;
            var bcd = Utility.Decode(HttpContext.Current.Request.Form["bcd"],"bcd");
            var offsetBytes = Utility.Decode(HttpContext.Current.Request.Form["offsetBytes"],"offsetBytes");
            HttpContext.Current.Response.Write(new BLL.Bcd().UpdateEntry(bcd, Convert.ToInt64(offsetBytes)));
        }

        [WebMethod]
        public void IpxeBoot(string filename, string type)
        {
            //No auth
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
            // No Auth
            var username = HttpContext.Current.Request.Form["uname"];
            var password = HttpContext.Current.Request.Form["pwd"];
            var kernel = HttpContext.Current.Request.Form["kernel"];
            var bootImage = HttpContext.Current.Request.Form["bootImage"];
            var task = HttpContext.Current.Request.Form["task"];

            HttpContext.Current.Response.Write(new Authenticate().IpxeLogin(username, password, kernel, bootImage, task));
        }

        [WebMethod]
        public void GetCustomScript(int scriptId)
        {
            if (!Authorize()) return;
            HttpContext.Current.Response.Write(new Logic().GetCustomScript(scriptId));

        }

        [WebMethod]
        public void GetSysprepTag(int tagId)
        {
            if (!Authorize()) return;
            HttpContext.Current.Response.Write(new Logic().GetSysprepTag(tagId));

        }

        [WebMethod]
        public void GetFileCopySchema(int profileId)
        {
            if (!Authorize()) return;
            HttpContext.Current.Response.Write(new Logic().GetFileCopySchema(profileId));

        }

        [WebMethod]
        public void MulticastCheckOut(string portBase)
        {
            if (!Authorize()) return;
            HttpContext.Current.Response.Write(new Logic().MulticastCheckout(portBase));   
        }

        [WebMethod]
        public void GetCustomPartitionScript(string profileId)
        {
            if (!Authorize()) return;
            HttpContext.Current.Response.Write(new Logic().GetCustomPartitionScript(Convert.ToInt32(profileId)));
        }

        [WebMethod]
        public void GetOnDemandArguments(string mac, string objectId, string task)
        {
            if (!Authorize()) return;
            HttpContext.Current.Response.Write(new Logic().GetOnDemandArguments(mac, Convert.ToInt32(objectId),task));
        }

        [WebMethod]
        public void Test()
        {
            //No auth
            HttpContext.Current.Response.Write("true");
        }

        [WebMethod]
        public void GetComputerName(string mac)
        {
            //No auth
            HttpContext.Current.Response.Write(BLL.Computer.GetComputerFromMac(mac).Name);
        }   
    }
}