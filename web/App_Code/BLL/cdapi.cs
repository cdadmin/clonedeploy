using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Helpers;
using RestSharp;
using RestSharp.Authenticators;

/// <summary>
/// Summary description for cdapi
/// </summary>

public class cdapi
{
    const string BaseUrl = "http://localhost/clonedeploy/api/";

    readonly string _token;


    public cdapi(string token)
    {
        _token = token;

    }
   

    public T Execute<T>(RestRequest request) where T : new()
    {
        var client = new RestClient();
        client.BaseUrl = new Uri(BaseUrl);
        //client.Authenticator = new HttpBasicAuthenticator(_accountSid, _secretKey);
        request.AddHeader("Authorization", "bearer " + _token); // used on every request
        var response = client.Execute<T>(request);

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            const string message = "Error retrieving response.  Check inner details for more info.";
            throw new HttpException(401, "Forbidden, you are not authorized");

        }
        /*if (response.ErrorException != null)
        {
            const string message = "Error retrieving response.  Check inner details for more info.";
            var twilioException = new ApplicationException(message, response.ErrorException);
            throw twilioException;
        }*/
        return response.Data;
    }

}
