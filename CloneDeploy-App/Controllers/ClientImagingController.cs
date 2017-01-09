using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities.DTOs.FormData;
using CloneDeploy_Services;
using CloneDeploy_Services.Helpers;
using CloneDeploy_Services.Workflows;

namespace CloneDeploy_App.Controllers
{
    /// <summary>
    /// Summary description for ClientImagingController
    /// </summary>
    /// 

    public class ClientImagingController :ApiController
    {
        private readonly HttpResponseMessage _response = new HttpResponseMessage(HttpStatusCode.OK);

        [HttpGet]
        public HttpResponseMessage Test()
        {
            _response.Content = new StringContent("true", System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage DownloadCoreScripts(ScriptNameDTO scriptDto)
        {

            var scriptPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                             Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar;

            if (File.Exists(scriptPath + scriptDto.scriptName))
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new FileStream(scriptPath + scriptDto.scriptName, FileMode.Open);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = scriptDto.scriptName;
                return result;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage CheckTaskAuth(TaskDTO taskDto)
        {
            var userToken = Utility.Decode(HttpContext.Current.Request.Headers["Authorization"], "Authorization");
            _response.Content = new StringContent(new ClientImagingServices().CheckTaskAuth(taskDto.task, userToken), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetPartLayout(PartitionDTO partitionDto)
        {

            var partLayout = new ClientPartitionScript
            {
                profileId = Convert.ToInt32(partitionDto.imageProfileId),
                HdNumberToGet = Convert.ToInt32(partitionDto.hdToGet),
                NewHdSize = partitionDto.newHdSize,
                ClientHd = partitionDto.clientHd,
                TaskType = partitionDto.taskType,
                partitionPrefix = partitionDto.partitionPrefix,
                clientBlockSize = Convert.ToInt32(partitionDto.lbs)
            };

            _response.Content = new StringContent(partLayout.GeneratePartitionScript(), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpGet]
        public HttpResponseMessage GetUtcDateTime()
        {
            _response.Content = new StringContent(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpGet]
        public HttpResponseMessage GetLocalDateTime()
        {
            _response.Content = new StringContent(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        public HttpResponseMessage IsLoginRequired(TaskDTO taskDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().IsLoginRequired(taskDto.task), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        public HttpResponseMessage ConsoleLogin()
        {
            var ip = Utility.Decode(HttpContext.Current.Request.Form["clientIP"], "clientIP");
            var username = Utility.Decode(HttpContext.Current.Request.Form["username"], "username");
            var password = Utility.Decode(HttpContext.Current.Request.Form["password"], "password");
            var task = Utility.Decode(HttpContext.Current.Request.Form["task"], "task");

            _response.Content = new StringContent(new AuthenticationServices().ConsoleLogin(username, password, task, ip), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetMunkiBasicAuth(ProfileDTO profileDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().GetMunkiBasicAuth(Convert.ToInt32(profileDto.profileId)), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage AddComputer(AddComputerDTO addComputerDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().AddComputer(addComputerDto.name, addComputerDto.mac), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage AddImage(NameDTO nameDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().AddImage(nameDto.name), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage AddImageOsxEnv(NameDTO nameDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().AddImageOsxEnv(nameDto.name), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage AddImageWinPEEnv(NameDTO nameDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().AddImageWinPEEnv(nameDto.name), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage ListImages(ImageListDTO imageListDto)
        {
            if (string.IsNullOrEmpty(imageListDto.userId))
                imageListDto.userId = "0";
            _response.Content = new StringContent(new ClientImagingServices().ImageList(imageListDto.environment, Convert.ToInt32(imageListDto.userId)), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage ListImageProfiles(ImageIdDTO imageIdDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().ImageProfileList(Convert.ToInt32(imageIdDto.imageId)), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage ListMulticasts(EnvironmentDTO environmentDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().MulicastSessionList(environmentDto.environment), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage CheckIn(ComputerMacDTO computerMacDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().CheckIn(computerMacDto.computerMac), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage DistributionPoint(DpDTO dpDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().DistributionPoint(dpDto.serverIdentifier, dpDto.task), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public void UpdateStatusInProgress(ComputerIdDTO computerIdDto)
        {
            new ClientImagingServices().ChangeStatusInProgress(computerIdDto.computerId);
        }

        [HttpPost]
        [ClientAuth]
        public void DeleteImage(ProfileDTO profileDto)
        {
            new ClientImagingServices().DeleteImage(Convert.ToInt32(profileDto.profileId));
        }

        [HttpPost]
        [ClientAuth]
        public void ErrorEmail(ErrorEmailDTO errorEmailDto)
        {
            new ClientImagingServices().ErrorEmail(Convert.ToInt32(errorEmailDto.computerId), errorEmailDto.error);
        }


        [HttpPost]
        [ClientAuth]
        public void CheckOut(ComputerIdDTO computerIdDto)
        {
            new ClientImagingServices().CheckOut(computerIdDto.computerId);
        }

        [HttpPost]
        [ClientAuth]
        public void PermanentTaskCheckOut(ComputerIdDTO computerIdDto)
        {
            new ClientImagingServices().PermanentTaskCheckOut(computerIdDto.computerId);
        }


        [HttpPost]
        [ClientAuth]
        public void UploadLog()
        {
            var computerId = Utility.Decode(HttpContext.Current.Request.Form["computerId"], "computerId");
            var logContents = Utility.Decode(HttpContext.Current.Request.Form["logContents"], "logContents");
            var subType = Utility.Decode(HttpContext.Current.Request.Form["subType"], "subType");
            var computerMac = Utility.Decode(HttpContext.Current.Request.Form["mac"], "mac");
            new ClientImagingServices().UploadLog(Convert.ToInt32(computerId), logContents, subType, computerMac);
        }

        [HttpPost]
        [ClientAuth]
        public void UpdateProgress(ProgressDTO progressDto)
        {
            new ClientImagingServices().UpdateProgress(Convert.ToInt32(progressDto.computerId), progressDto.progress, progressDto.progressType);
        }

        [HttpPost]
        [ClientAuth]
        public void UpdateProgressPartition(ProgressPartitionDTO progressPartitionDto)
        {
            new ClientImagingServices().UpdateProgressPartition(Convert.ToInt32(progressPartitionDto.computerId), progressPartitionDto.partition);
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage CheckQueue(ComputerIdDTO computerIdDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().CheckQueue(Convert.ToInt32(computerIdDto.computerId)), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage CheckHdRequirements(HdReqs hdReqs)
        {
            _response.Content = new StringContent(new ClientImagingServices().CheckHdRequirements(Convert.ToInt32(hdReqs.profileId), Convert.ToInt32(hdReqs.clientHdNumber), hdReqs.newHdSize, hdReqs.schemaHds, Convert.ToInt32(hdReqs.clientLbs)), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetOriginalLvm(OriginalLVM originalLvm)
        {
            _response.Content = new StringContent(new ClientImagingServices().GetOriginalLvm(Convert.ToInt32(originalLvm.profileId), originalLvm.clientHd, originalLvm.hdToGet, originalLvm.partitionPrefix), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage CheckForCancelledTask(ComputerIdDTO computerIdDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().CheckForCancelledTask(Convert.ToInt32(computerIdDto.computerId)), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage UpdateBcd()
        {
            var bcd = Utility.Decode(HttpContext.Current.Request.Form["bcd"], "bcd");
            var offsetBytes = Utility.Decode(HttpContext.Current.Request.Form["offsetBytes"], "offsetBytes");
            _response.Content = new StringContent(new BcdServices().UpdateEntry(bcd, Convert.ToInt64(offsetBytes)), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpGet]
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

        [HttpPost]
        public HttpResponseMessage IpxeLogin()
        {
            var username = HttpContext.Current.Request.Form["uname"];
            var password = HttpContext.Current.Request.Form["pwd"];
            var kernel = HttpContext.Current.Request.Form["kernel"];
            var bootImage = HttpContext.Current.Request.Form["bootImage"];
            var task = HttpContext.Current.Request.Form["task"];

            _response.Content = new StringContent(new AuthenticationServices().IpxeLogin(username, password, kernel, bootImage, task), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetCustomScript(ScriptIdDTO scriptIdDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().GetCustomScript(scriptIdDto.scriptId), System.Text.Encoding.UTF8, "text/plain");
            return _response;

        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetSysprepTag(SysprepDTO sysprepDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().GetSysprepTag(sysprepDto.tagId, sysprepDto.imageEnvironment), System.Text.Encoding.UTF8, "text/plain");
            return _response;

        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetFileCopySchema(ProfileDTO profileDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().GetFileCopySchema(profileDto.profileId), System.Text.Encoding.UTF8, "text/plain");
            return _response;

        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage MulticastCheckOut(PortDTO portDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().MulticastCheckout(portDto.portBase), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetCustomPartitionScript(ProfileDTO profileDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().GetCustomPartitionScript(Convert.ToInt32(profileDto.profileId)), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetOnDemandArguments(OnDemandDTO onDemandDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().GetOnDemandArguments(onDemandDto.mac, Convert.ToInt32(onDemandDto.objectId), onDemandDto.task), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        public HttpResponseMessage GetComputerName(MacDTO macDto)
        {
            _response.Content = new StringContent(new ComputerServices().GetComputerFromMac(macDto.mac).Name, System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        public HttpResponseMessage GetProxyReservation(MacDTO macDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().GetProxyReservation(macDto.mac), System.Text.Encoding.UTF8, "text/plain");
            return _response;
        }

       
    }
}