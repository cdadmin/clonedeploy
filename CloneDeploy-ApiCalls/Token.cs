using System;
using System.Configuration;
using System.Net;
using CloneDeploy_Entities;
using log4net;
using Newtonsoft.Json;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class TokenApi
    {
        private readonly Uri _baseUrl;
        private readonly RestClient _client = new RestClient();
        private readonly ILog _log = LogManager.GetLogger("FrontEndLog");
        private readonly RestRequest _request = new RestRequest();
        private readonly string _resource;

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
            var token = new TokenEntity();

            _client.BaseUrl = _baseUrl ?? new Uri(ConfigurationManager.AppSettings["api_base_url"]);
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}", _resource);
            _request.AddParameter("grant_type", "password");
            _request.AddParameter("userName", username);
            _request.AddParameter("password", password);

            var response = _client.Execute<TokenEntity>(_request);

            if (response == null)
            {
                _log.Debug("Could Not Complete Token Request.  The Response was empty." + _request.Resource);
                token.error_description = "Did Not Receive A Response From The Auth Server.";
                return token;
            }

            if (response.Data != null)
            {
                token = response.Data;
            }

            if (response.ErrorException != null)
            {
                _log.Debug("Error With Token API: " + response.ErrorException);
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.NotFound:
                case 0:
                    token.error_description = "Could Not Contact Token API";
                    break;
                case HttpStatusCode.BadRequest:
                    token.error_description =
                        JsonConvert.DeserializeObject<TokenEntity>(response.Content).error_description;
                    break;
                default:
                    token.error_description = "Unknown Error With Token API";
                    _log.Debug("Error With Token API: " + response.Content);
                    break;
            }

            return token;
        }
    }
}