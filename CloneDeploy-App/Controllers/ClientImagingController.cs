using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Common;
using CloneDeploy_Entities.DTOs.FormData;
using CloneDeploy_Services;
using CloneDeploy_Services.Workflows;

namespace CloneDeploy_App.Controllers
{
    /// <summary>
    ///     Summary description for ClientImagingController
    /// </summary>
    public class ClientImagingController : ApiController
    {
        private readonly HttpResponseMessage _response = new HttpResponseMessage(HttpStatusCode.OK);

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage AddComputer(AddComputerDTO addComputerDto)
        {
            _response.Content =
                new StringContent(new ClientImagingServices().AddComputer(addComputerDto.name, addComputerDto.mac, addComputerDto.clientIdentifier),
                    Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage AddImage(NameDTO nameDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().AddImage(nameDto.name), Encoding.UTF8,
                "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage AddImageOsxEnv(NameDTO nameDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().AddImageOsxEnv(nameDto.name),
                Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage AddImageWinPEEnv(NameDTO nameDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().AddImageWinPEEnv(nameDto.name),
                Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage CheckForCancelledTask(ComputerIdDTO computerIdDto)
        {
            _response.Content =
                new StringContent(
                    new ClientImagingServices().CheckForCancelledTask(Convert.ToInt32(computerIdDto.computerId)),
                    Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage CheckHdRequirements(HdReqs hdReqs)
        {
            _response.Content =
                new StringContent(
                    new ClientImagingServices().CheckHdRequirements(Convert.ToInt32(hdReqs.profileId),
                        Convert.ToInt32(hdReqs.clientHdNumber), hdReqs.newHdSize, hdReqs.schemaHds,
                        Convert.ToInt32(hdReqs.clientLbs)), Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        public HttpResponseMessage DetermineTask(IdTypeDTO idTypeDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().DetermineTask(idTypeDto.idType, idTypeDto.id), Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage CheckIn(CheckInTaskDTO checkInTaskDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().CheckIn(checkInTaskDto.computerId),
                Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage OnDemandCheckin(OnDemandDTO onDemandDto)
        {
            _response.Content =
                new StringContent(
                    new ClientImagingServices().OnDemandCheckIn(onDemandDto.mac,
                        Convert.ToInt32(onDemandDto.objectId), onDemandDto.task,onDemandDto.userId,onDemandDto.computerId), Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public void CheckOut(ComputerIdDTO computerIdDto)
        {
            new ClientImagingServices().CheckOut(computerIdDto.computerId);
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage CheckQueue(ComputerIdDTO computerIdDto)
        {
            _response.Content =
                new StringContent(new ClientImagingServices().CheckQueue(Convert.ToInt32(computerIdDto.computerId)),
                    Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage CheckTaskAuth(TaskDTO taskDto)
        {
            var userToken = StringManipulationServices.Decode(HttpContext.Current.Request.Headers["Authorization"], "Authorization");
            _response.Content = new StringContent(new ClientImagingServices().CheckTaskAuth(taskDto.task, userToken),
                Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        public HttpResponseMessage ConsoleLogin()
        {
            var ip = StringManipulationServices.Decode(HttpContext.Current.Request.Form["clientIP"], "clientIP");
            var username = StringManipulationServices.Decode(HttpContext.Current.Request.Form["username"], "username");
            var password = StringManipulationServices.Decode(HttpContext.Current.Request.Form["password"], "password");
            var task = StringManipulationServices.Decode(HttpContext.Current.Request.Form["task"], "task");

            _response.Content =
                new StringContent(new AuthenticationServices().ConsoleLogin(username, password, task, ip), Encoding.UTF8,
                    "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public void DeleteImage(ProfileDTO profileDto)
        {
            new ClientImagingServices().DeleteImage(Convert.ToInt32(profileDto.profileId));
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage DistributionPoint(DpDTO dpDto)
        {
            _response.Content = new StringContent(
                new ClientImagingServices().DistributionPoint(dpDto.dpId, dpDto.task), Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetAllClusterDps(ComputerIdDTO computerIdDto)
        {
            _response.Content = new StringContent(
                new ClientImagingServices().GetAllClusterDps(computerIdDto.computerId), Encoding.UTF8, "text/plain");
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
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new FileStream(scriptPath + scriptDto.scriptName, FileMode.Open);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = scriptDto.scriptName;
                return result;
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [ClientAuth]
        public void ErrorEmail(ErrorEmailDTO errorEmailDto)
        {
            new ClientImagingServices().ErrorEmail(Convert.ToInt32(errorEmailDto.computerId), errorEmailDto.error);
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetCustomPartitionScript(ProfileDTO profileDto)
        {
            _response.Content =
                new StringContent(
                    new ClientImagingServices().GetCustomPartitionScript(Convert.ToInt32(profileDto.profileId)),
                    Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetCustomScript(ScriptIdDTO scriptIdDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().GetCustomScript(scriptIdDto.scriptId),
                Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetFileCopySchema(ProfileDTO profileDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().GetFileCopySchema(profileDto.profileId),
                Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpGet]
        public HttpResponseMessage GetLocalDateTime()
        {
            _response.Content = new StringContent(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Encoding.UTF8,
                "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetMunkiBasicAuth(ProfileDTO profileDto)
        {
            _response.Content =
                new StringContent(new ClientImagingServices().GetMunkiBasicAuth(Convert.ToInt32(profileDto.profileId)),
                    Encoding.UTF8, "text/plain");
            return _response;
        }

       
        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetOriginalLvm(OriginalLVM originalLvm)
        {
            _response.Content =
                new StringContent(
                    new ClientImagingServices().GetOriginalLvm(Convert.ToInt32(originalLvm.profileId),
                        originalLvm.clientHd, originalLvm.hdToGet, originalLvm.partitionPrefix), Encoding.UTF8,
                    "text/plain");
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

            _response.Content = new StringContent(partLayout.GeneratePartitionScript(), Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage GetSysprepTag(SysprepDTO sysprepDto)
        {
            _response.Content =
                new StringContent(
                    new ClientImagingServices().GetSysprepTag(sysprepDto.tagId, sysprepDto.imageEnvironment),
                    Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpGet]
        public HttpResponseMessage GetUtcDateTime()
        {
            _response.Content = new StringContent(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), Encoding.UTF8,
                "text/plain");
            return _response;
        }

        [HttpGet]
        public void IpxeBoot(string filename, string type)
        {
            if (type == "kernel")
            {
                var path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "kernels" + Path.DirectorySeparatorChar;
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "inline; filename=" + filename);
                HttpContext.Current.Response.TransmitFile(path + filename);
                HttpContext.Current.Response.End();
            }
            else
            {
                var path = SettingServices.GetSettingValue(SettingStrings.TftpPath) + "images" + Path.DirectorySeparatorChar;
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

            _response.Content =
                new StringContent(new AuthenticationServices().IpxeLogin(username, password, kernel, bootImage, task),
                    Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        public HttpResponseMessage IsLoginRequired(TaskDTO taskDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().IsLoginRequired(taskDto.task),
                Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage ListImageProfiles(ImageIdDTO imageIdDto)
        {
            _response.Content =
                new StringContent(new ClientImagingServices().ImageProfileList(Convert.ToInt32(imageIdDto.imageId)),
                    Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage ListImages(ImageListDTO imageListDto)
        {
            if (string.IsNullOrEmpty(imageListDto.userId))
                imageListDto.userId = "0";
            _response.Content =
                new StringContent(
                    new ClientImagingServices().ImageList(imageListDto.environment, Convert.ToInt32(imageListDto.userId)),
                    Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage ListMulticasts(EnvironmentDTO environmentDto)
        {
            _response.Content =
                new StringContent(new ClientImagingServices().MulicastSessionList(environmentDto.environment),
                    Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage MulticastCheckOut(PortDTO portDto)
        {
            _response.Content = new StringContent(new ClientImagingServices().MulticastCheckout(portDto.portBase),
                Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public void PermanentTaskCheckOut(ComputerIdDTO computerIdDto)
        {
            new ClientImagingServices().PermanentTaskCheckOut(computerIdDto.computerId);
        }

        [HttpGet]
        public HttpResponseMessage Test()
        {
            _response.Content = new StringContent("true", Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public HttpResponseMessage UpdateBcd()
        {
            var bcd = StringManipulationServices.Decode(HttpContext.Current.Request.Form["bcd"], "bcd");
            var offsetBytes = StringManipulationServices.Decode(HttpContext.Current.Request.Form["offsetBytes"], "offsetBytes");
            _response.Content = new StringContent(new BcdServices().UpdateEntry(bcd, Convert.ToInt64(offsetBytes)),
                Encoding.UTF8, "text/plain");
            return _response;
        }

        [HttpPost]
        [ClientAuth]
        public void UpdateProgress(ProgressDTO progressDto)
        {
            new ClientImagingServices().UpdateProgress(Convert.ToInt32(progressDto.computerId), progressDto.progress,
                progressDto.progressType);
        }

        [HttpPost]
        [ClientAuth]
        public void UpdateProgressPartition(ProgressPartitionDTO progressPartitionDto)
        {
            new ClientImagingServices().UpdateProgressPartition(Convert.ToInt32(progressPartitionDto.computerId),
                progressPartitionDto.partition);
        }

        [HttpPost]
        [ClientAuth]
        public void UpdateStatusInProgress(ComputerIdDTO computerIdDto)
        {
            new ClientImagingServices().ChangeStatusInProgress(computerIdDto.computerId);
        }


        [HttpPost]
        [ClientAuth]
        public void UploadLog()
        {
            var computerId = StringManipulationServices.Decode(HttpContext.Current.Request.Form["computerId"], "computerId");
            var logContents = StringManipulationServices.Decode(HttpContext.Current.Request.Form["logContents"], "logContents");
            var subType = StringManipulationServices.Decode(HttpContext.Current.Request.Form["subType"], "subType");
            var computerMac = StringManipulationServices.Decode(HttpContext.Current.Request.Form["mac"], "mac");
            new ClientImagingServices().UploadLog(Convert.ToInt32(computerId), logContents, subType, computerMac);
        }
    }
}