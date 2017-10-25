using System;
using System.IO;
using System.Web;
using System.Web.Http;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class UploadController : ApiController
    {
      
        protected HttpContext Context;
        protected HttpResponse Response;
        protected HttpRequest Request;

        [HttpPost]
        [Authorize]
        public void UploadFile()
        {
            
            Context = HttpContext.Current;
            Request = HttpContext.Current.Request;
            Response = HttpContext.Current.Response;



            var formUpload = Request.Files.Count > 0;
            // find filename
            var xFileName = Request.Headers["X-File-Name"];
            var qqFile = Request["qqfile"];
            var formFilename = formUpload ? Request.Files[0].FileName : null;

            var baseDir = Context.Server.MapPath("~") + Path.DirectorySeparatorChar + "private" +
                          Path.DirectorySeparatorChar + "policy_uploads";

            if (Request["module"] == "software")
                baseDir = Path.Combine(baseDir, "softwaremodules");
            else
            {
                Response.Write(new FineUploaderResult(false, error: "Module Not Defined").BuildResponse());
                Response.End();
            }


            var upload = new FileUploadDTO()
            {
                Filename = xFileName ?? qqFile ?? formFilename,
                InputStream = formUpload ? Request.Files[0].InputStream : Request.InputStream,
                UploadMethod = Request["uploadMethod"],
                DestinationDirectory = Path.Combine(baseDir, Request["subDirectory"])
            };


            int someval;
            if (int.TryParse(Request["qqpartindex"], out someval))
            {
                upload.PartIndex = someval;
                upload.OriginalFilename = Request["qqfilename"];
                upload.TotalParts = int.Parse(Request["qqtotalparts"]);
                upload.PartUuid = Request["qquuid"];
                upload.FileSize = ulong.Parse(Request["qqtotalfilesize"]);
            }


            var result = new FileUploadServices(upload).Upload();

            Response.Write(result == null
                ? new FineUploaderResult(true, new {extraInformation = 12345}).BuildResponse()
                : new FineUploaderResult(false, error: result).BuildResponse());

            Response.End();

        }

      
    }
}