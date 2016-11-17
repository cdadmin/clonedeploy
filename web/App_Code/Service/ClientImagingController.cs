using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Helpers;
using Models;

/// <summary>
/// Summary description for ClientImagingController
/// </summary>
/// 

public class ClientImagingController :ApiController
{
    private HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);

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
    [ClientAuthAttribute]
    public HttpResponseMessage Test()
    {
        resp.Content = new StringContent("True", System.Text.Encoding.UTF8, "text/plain");
        return resp;
    }

    [HttpGet]
    [ClientAuthAttribute]
    public HttpResponseMessage CheckQueue(string computerId)
    {
        resp.Content = new StringContent(new Service.Client.Logic().CheckQueue(Convert.ToInt32(computerId)),
            System.Text.Encoding.UTF8, "text/plain");
        return resp;
    }

    [HttpGet]
    [ClientAuthAttribute]
    public void DownloadCoreScripts(string scriptName)
    {
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
}