using System;
using System.Configuration;
using System.Web;
using log4net;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ApiRequest
    {
        private static readonly ILog Log = LogManager.GetLogger("ApplicationLog");
        private readonly string _token;
        private readonly Uri _baseUrl;

        public ApiRequest()
        {
            var httpCookie = HttpContext.Current.Request.Cookies["cdtoken"];
            if (httpCookie != null)
                _token = httpCookie.Value;
            _baseUrl = new Uri(ConfigurationManager.AppSettings["api_base_url"]);
        }

        public ApiRequest(string token, Uri baseUrl)
        {
            _token = token;
            _baseUrl = baseUrl;
        }

        public TClass Execute<TClass>(RestRequest request) where TClass : new()
        {
            var client = new RestClient();
            client.BaseUrl = _baseUrl ;
            client.Timeout = 5000;

            request.AddHeader("Authorization", "bearer " + _token);
            var response = client.Execute<TClass>(request);

            if (response.ErrorException != null)
            {
                Log.Debug("Error retrieving response: " + response.ErrorException);
                return default(TClass);
            }
            else if (response.Data == null)
            {
                //Fix me - not finished
                Log.Debug("Response was null: ");
                return default(TClass);
            }
            return response.Data;
        }

        public byte[] ExecuteRaw(RestRequest request)
        {
            var client = new RestClient();
            client.BaseUrl = _baseUrl;
            client.Timeout = 5000;

            request.AddHeader("Authorization", "bearer " + _token);
            return client.DownloadData(request);
          
        }

       
    }
}