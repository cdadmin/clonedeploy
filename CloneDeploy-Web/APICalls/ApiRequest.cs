using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Helpers;
using RestSharp;

namespace CloneDeploy_Web.APICalls
{
    public class ApiRequest
    {
        private readonly string _token;

        public ApiRequest()
        {
            var httpCookie = HttpContext.Current.Request.Cookies["cdtoken"];
            if (httpCookie != null)
                _token = httpCookie.Value;
        }

        public TClass Execute<TClass>(RestRequest request) where TClass : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(ConfigurationManager.AppSettings["api_base_url"]);

            request.AddHeader("Authorization", "bearer " + _token);
            var response = client.Execute<TClass>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response: ";
                Logger.Log(message + response.ErrorException);
                return default(TClass);
            }
            return response.Data;
        }
    }
}