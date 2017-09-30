using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services.Workflows;

namespace CloneDeploy_App.Controllers
{
    public class WorkflowController : ApiController
    {
        [CustomAuth(Permission = "Administrator")]
        [HttpGet]
        public ApiBoolResponseDTO CancelAllImagingTasks()
        {
            return new ApiBoolResponseDTO {Value = new CancelAllImagingTasks().Execute()};
        }

        [CustomAuth(Permission = "AdminUpdate")]
        [HttpGet]
        public ApiBoolResponseDTO CopyPxeBinaries()
        {
            return new ApiBoolResponseDTO {Value = new CopyPxeBinaries().Execute()};
        }

        [CustomAuth(Permission = "AdminUpdate")]
        [HttpGet]
        public ApiBoolResponseDTO CreateClobberBootMenu(int profileId, bool promptComputerName)
        {
            new ClobberBootMenu(profileId, promptComputerName).Execute();
            return new ApiBoolResponseDTO {Value = true};
        }

        [CustomAuth(Permission = "AdminUpdate")]
        [HttpPost]
        public ApiBoolResponseDTO CreateDefaultBootMenu(BootMenuGenOptionsDTO defaultMenuOptions)
        {
            new DefaultBootMenu(defaultMenuOptions).Execute();
            return new ApiBoolResponseDTO {Value = true};
        }

        [CustomAuth(Permission = "AdminUpdate")]
        [HttpPost]
        public HttpResponseMessage GenerateLinuxIsoConfig(IsoGenOptionsDTO isoOptions)
        {
            new IsoGen(isoOptions).Generate();
            var basePath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                           Path.DirectorySeparatorChar + "client_iso" + Path.DirectorySeparatorChar;
            if (isoOptions.buildType == "ISO")
                basePath += "clientboot.iso";
            else
                basePath += "clientboot.zip";

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(basePath, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(basePath);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return result;
        }

        [CustomAuth(Permission = "AdminRead")]
        [HttpGet]
        public AppleVendorDTO GetAppleVendorString(string ip)
        {
            return new CreateAppleVendorString().Execute(ip);
        }

        [CustomAuth(Permission = "AllowOnd")]
        [HttpGet]
        public ApiStringResponseDTO StartOnDemandMulticast(int profileId, string clientCount,int clusterId)
        {
            var identity = (ClaimsPrincipal) Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();

            return new ApiStringResponseDTO
            {
                Value =
                    new Multicast(profileId, clientCount, Convert.ToInt32(userId), Request.GetClientIpAddress(),clusterId).Create()
            };
        }
    }
}