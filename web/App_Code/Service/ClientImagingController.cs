using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

/// <summary>
/// Summary description for ClientImagingController
/// </summary>
public class ClientImagingController :ApiController
{
	public ClientImagingController()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    [HttpGet]
    public HttpResponseMessage Test()
    {
        //No auth
        var resp = new HttpResponseMessage(HttpStatusCode.OK);
        resp.Content = new StringContent("Test", System.Text.Encoding.UTF8, "text/plain");
        return resp;
    }
}