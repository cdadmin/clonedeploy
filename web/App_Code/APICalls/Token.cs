using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;

/// <summary>
/// Summary description for Token
/// </summary>
public class TokenApi
{
    private RestRequest _request = new RestRequest();
    private RestClient _client = new RestClient("http://localhost/clonedeploy");
    private string _resource;
    public TokenApi(string resource)
    {
        resource = _resource;
    }

    public Models.Token Get(string username, string password)
    {
        var client = new RestClient("http://localhost/clonedeploy/");
        var request = new RestRequest("Token", Method.POST);

        request.AddParameter("grant_type", "password");
        request.AddParameter("userName", username);
        request.AddParameter("password", password);

        var response = client.Execute<Models.Token>(request);
        return response.Data;

       
    }

    
}