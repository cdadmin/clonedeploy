using System;
using System.Configuration;
using System.Net;
using CloneDeploy_Entities;
using log4net;
using Newtonsoft.Json;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    /// <summary>
    /// Summary description for Token
    /// </summary>
    public class TokenApi
    {
        private readonly ILog log = LogManager.GetLogger("FrontEndLog");
        private readonly RestRequest _request = new RestRequest();
        private readonly RestClient _client = new RestClient();
        private readonly string _resource;
        private readonly Uri _baseUrl;
        public TokenApi(string resource)
        {
            _resource = resource;
        }

        public TokenApi(Uri baseUrl, string resource)
        {
            _baseUrl = baseUrl;
            _resource = resource;
        }

        public TokenEntity Get(string username, string password)
        {
            _client.BaseUrl = _baseUrl ?? new Uri(ConfigurationManager.AppSettings["api_base_url"]);
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}",_resource);
            _request.AddParameter("grant_type", "password");
            _request.AddParameter("userName", username);
            _request.AddParameter("password", password);

            var response = _client.Execute<TokenEntity>(_request);
            var token = response.Data ?? new TokenEntity();

            

            if (response.ErrorException != null)
            {
                log.Debug("Error With Token API: " + response.ErrorException);
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Data;
                case HttpStatusCode.NotFound: case 0:
                    token.error_description = "Could Not Contact Token API";
                    return token;
                case HttpStatusCode.BadRequest:
                    token.error_description = JsonConvert.DeserializeObject<TokenEntity>(response.Content).error_description;
                    return token;
                default:
                    token.error_description = "Unknown Error With Token API";
                    log.Debug("Error With Token API: " + response.Content);
                    return token;

            }
        
        }
    }
}