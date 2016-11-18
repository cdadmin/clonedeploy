using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Services;
using BLL.Workflows;
using Helpers;
using Models;
using Security;
using Service.Client;

/// <summary>
/// Summary description for ClientImagingController
/// </summary>
/// 

public class ClientImagingController :ApiController
{
    private readonly HttpResponseMessage _response = new HttpResponseMessage(HttpStatusCode.OK);

   

    public class ClientAuthAttribute : AuthorizeAttribute
    {
        public string Permission { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var userToken = Utility.Decode(HttpContext.Current.Request.Headers["Authorization"], "Authorization");
            if (!new Service.Client.Logic().Authorize(userToken))
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                throw new HttpResponseException(response);
            }
        }
    }

    [HttpGet]
    public HttpResponseMessage Test()
    {
        _response.Content = new StringContent("true", System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage DownloadCoreScripts(Class1 a)
    {

        var scriptPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                         Path.DirectorySeparatorChar + "clientscripts" + Path.DirectorySeparatorChar;

        if (File.Exists(scriptPath + a.scriptName))
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(scriptPath + a.scriptName, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = a.scriptName;
            return result;
        }
        else
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage CheckTaskAuth(Class2 t)
    {
        var userToken = Utility.Decode(HttpContext.Current.Request.Headers["Authorization"], "Authorization");
        _response.Content = new StringContent(new Service.Client.Logic().CheckTaskAuth(t.task, userToken), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage GetPartLayout(string imageProfileId, string hdToGet, string newHdSize, string clientHd, string taskType, string partitionPrefix, string lbs)
    {

        var partLayout = new ClientPartitionScript
        {
            profileId = Convert.ToInt32(imageProfileId),
            HdNumberToGet = Convert.ToInt32(hdToGet),
            NewHdSize = newHdSize,
            ClientHd = clientHd,
            TaskType = taskType,
            partitionPrefix = partitionPrefix,
            clientBlockSize = Convert.ToInt32(lbs)
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
    public HttpResponseMessage IsLoginRequired(Class1 a)
    {
        _response.Content = new StringContent(new Service.Client.Logic().IsLoginRequired(a.scriptName), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    public HttpResponseMessage ConsoleLogin()
    {
        var ip = Utility.Decode(HttpContext.Current.Request.Form["clientIP"], "clientIP");
        var username = Utility.Decode(HttpContext.Current.Request.Form["username"], "username");
        var password = Utility.Decode(HttpContext.Current.Request.Form["password"], "password");
        var task = Utility.Decode(HttpContext.Current.Request.Form["task"], "task");

        _response.Content = new StringContent(new Authenticate().ConsoleLogin(username, password, task, ip), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage GetMunkiBasicAuth(string profileId)
    {
        _response.Content = new StringContent(new Service.Client.Logic().GetMunkiBasicAuth(Convert.ToInt32(profileId)), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage AddComputer(string name, string mac)
    {
        _response.Content = new StringContent(new Service.Client.Logic().AddComputer(name, mac), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage AddImage(string name)
    {
        _response.Content = new StringContent(new Service.Client.Logic().AddImage(name), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage AddImageOsxEnv(string name)
    {
        _response.Content = new StringContent(new Service.Client.Logic().AddImageOsxEnv(name), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage AddImageWinPEEnv(string name)
    {
        _response.Content = new StringContent(new Service.Client.Logic().AddImageWinPEEnv(name), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage ListImages(string userId, string environment)
    {
        if (string.IsNullOrEmpty(userId))
            userId = "0";
        _response.Content = new StringContent(new Service.Client.Logic().ImageList(environment, Convert.ToInt32(userId)), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage ListImageProfiles(string imageId)
    {
        _response.Content = new StringContent(new Service.Client.Logic().ImageProfileList(Convert.ToInt32(imageId)), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage ListMulticasts(string environment)
    {
        _response.Content = new StringContent(new Service.Client.Logic().MulicastSessionList(environment), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage CheckIn(string computerMac)
    {
        _response.Content = new StringContent(new Service.Client.Logic().CheckIn(computerMac), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage DistributionPoint(string dpId, string task)
    {
        _response.Content = new StringContent(new Logic().DistributionPoint(Convert.ToInt32(dpId), task), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public void UpdateStatusInProgress(int computerId)
    {
        new Logic().ChangeStatusInProgress(computerId);
    }

    [HttpPost]
    [ClientAuthAttribute]
    public void DeleteImage(string profileId)
    {
        new Logic().DeleteImage(Convert.ToInt32(profileId));
    }

    [HttpPost]
    [ClientAuthAttribute]
    public void ErrorEmail(string computerId, string error)
    {
        new Service.Client.Logic().ErrorEmail(Convert.ToInt32(computerId), error);
    }


    [HttpPost]
    [ClientAuthAttribute]
    public void CheckOut(string computerId)
    {
        new Service.Client.Logic().CheckOut(Convert.ToInt32(computerId));
    }

    [HttpPost]
    [ClientAuthAttribute]
    public void PermanentTaskCheckOut(string computerId)
    {
        new Service.Client.Logic().PermanentTaskCheckOut(Convert.ToInt32(computerId));
    }


    [HttpPost]
    [ClientAuthAttribute]
    public void UploadLog()
    {
        var computerId = Utility.Decode(HttpContext.Current.Request.Form["computerId"], "computerId");
        var logContents = Utility.Decode(HttpContext.Current.Request.Form["logContents"], "logContents");
        var subType = Utility.Decode(HttpContext.Current.Request.Form["subType"], "subType");
        var computerMac = Utility.Decode(HttpContext.Current.Request.Form["mac"], "mac");
        new Logic().UploadLog(Convert.ToInt32(computerId), logContents, subType, computerMac);
    }

    [HttpPost]
    [ClientAuthAttribute]
    public void UpdateProgress(string computerId, string progress, string progressType)
    {
        new Service.Client.Logic().UpdateProgress(Convert.ToInt32(computerId), progress, progressType);
    }

    [HttpPost]
    [ClientAuthAttribute]
    public void UpdateProgressPartition(string computerId, string partition)
    {
        new Service.Client.Logic().UpdateProgressPartition(Convert.ToInt32(computerId), partition);
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage CheckQueue(string computerId)
    {
        _response.Content = new StringContent(new Service.Client.Logic().CheckQueue(Convert.ToInt32(computerId)), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage CheckHdRequirements(string profileId, string clientHdNumber, string newHdSize, string schemaHds, string clientLbs)
    {
        _response.Content = new StringContent(new Service.Client.Logic().CheckHdRequirements(Convert.ToInt32(profileId), Convert.ToInt32(clientHdNumber), newHdSize, schemaHds, Convert.ToInt32(clientLbs)), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage GetOriginalLvm(string profileId, string clientHd, string hdToGet, string partitionPrefix)
    {
        _response.Content = new StringContent(new Service.Client.Logic().GetOriginalLvm(Convert.ToInt32(profileId), clientHd, hdToGet, partitionPrefix), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage CheckForCancelledTask(string computerId)
    {
        _response.Content = new StringContent(new Service.Client.Logic().CheckForCancelledTask(Convert.ToInt32(computerId)), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage UpdateBcd()
    {
        var bcd = Utility.Decode(HttpContext.Current.Request.Form["bcd"], "bcd");
        var offsetBytes = Utility.Decode(HttpContext.Current.Request.Form["offsetBytes"], "offsetBytes");
        _response.Content = new StringContent(new BLL.Bcd().UpdateEntry(bcd, Convert.ToInt64(offsetBytes)), System.Text.Encoding.UTF8, "text/plain");
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

        _response.Content = new StringContent(new Authenticate().IpxeLogin(username, password, kernel, bootImage, task), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage GetCustomScript(int scriptId)
    {
        _response.Content = new StringContent(new Logic().GetCustomScript(scriptId), System.Text.Encoding.UTF8, "text/plain");
        return _response;

    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage GetSysprepTag(int tagId, string imageEnvironment)
    {
        _response.Content = new StringContent(new Logic().GetSysprepTag(tagId, imageEnvironment), System.Text.Encoding.UTF8, "text/plain");
        return _response;

    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage GetFileCopySchema(int profileId)
    {
        _response.Content = new StringContent(new Logic().GetFileCopySchema(profileId), System.Text.Encoding.UTF8, "text/plain");
        return _response;

    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage MulticastCheckOut(string portBase)
    {
        _response.Content = new StringContent(new Logic().MulticastCheckout(portBase), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage GetCustomPartitionScript(string profileId)
    {
        _response.Content = new StringContent(new Logic().GetCustomPartitionScript(Convert.ToInt32(profileId)), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    [ClientAuthAttribute]
    public HttpResponseMessage GetOnDemandArguments(string mac, string objectId, string task)
    {
        _response.Content = new StringContent(new Logic().GetOnDemandArguments(mac, Convert.ToInt32(objectId), task), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    public HttpResponseMessage GetComputerName(string mac)
    {
        _response.Content = new StringContent(BLL.Computer.GetComputerFromMac(mac).Name, System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }

    [HttpPost]
    public HttpResponseMessage GetProxyReservation(string mac)
    {
        _response.Content = new StringContent(new Logic().GetProxyReservation(mac), System.Text.Encoding.UTF8, "text/plain");
        return _response;
    }
}