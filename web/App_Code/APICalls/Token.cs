using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Helpers;
using Newtonsoft.Json;
using RestSharp;

/// <summary>
/// Summary description for Token
/// </summary>
public class TokenApi
{
    private readonly RestRequest _request = new RestRequest();
    private readonly RestClient _client = new RestClient();
    private readonly string _resource;

    public TokenApi(string resource)
    {
        _resource = resource;
    }

    public Models.Token Get(string username, string password)
    {
        _client.BaseUrl = new Uri(ConfigurationManager.AppSettings["api_base_url"]);
        _request.Method = Method.POST;
        _request.Resource = _resource;
        _request.AddParameter("grant_type", "password");
        _request.AddParameter("userName", username);
        _request.AddParameter("password", password);

        var response = _client.Execute<Models.Token>(_request);
        var token = response.Data ?? new Models.Token();

        var user = BLL.User.GetUser(username);
        if(user != null)
        token.user_id = user.Id;

        if (response.ErrorException != null)
        {
            const string message = "Error retrieving response: ";
            Logger.Log(message + response.ErrorException);
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return response.Data;
            case HttpStatusCode.NotFound: case 0:
                token.error_description = "Could Not Contact Token API";
                return token;
            case HttpStatusCode.BadRequest:
                token.error_description = JsonConvert.DeserializeObject<Models.Token>(response.Content).error_description;
                return token;
            default:
                token.error_description = "Unknown Error With Token API";
                return token;

        }
        
    }
}