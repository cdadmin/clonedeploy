using System;
using System.Net;
using System.Web;
using log4net;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ApiRequest
    {
        private readonly RestClient _client;
        private readonly ILog _log = LogManager.GetLogger("FrontEndLog");
        private readonly string _token;

        public ApiRequest()
        {
            _client = new RestClient();
            var cookie = HttpContext.Current.Request.Cookies["cdBaseUrl"];
            if (cookie != null)
                _client.BaseUrl = new Uri(cookie.Value);

            var httpCookie = HttpContext.Current.Request.Cookies["cdtoken"];
            if (httpCookie != null)
                _token = httpCookie.Value;
        }

        public ApiRequest(string token, Uri baseUrl)
        {
            _client = new RestClient();
            _client.BaseUrl = baseUrl;
            _token = token;
        }

        public TClass Execute<TClass>(RestRequest request) where TClass : new()
        {
            if (request == null)
            {
                _log.Debug("Could Not Execute API Request.  The Request was empty." + new TClass().GetType());
                return default(TClass);
            }
            request.AddHeader("Authorization", "bearer " + _token);

            var response = _client.Execute<TClass>(request);

            if (response == null)
            {
                _log.Debug("Could Not Complete API Request.  The Response was empty." + request.Resource);
                return default(TClass);
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                _log.Debug("Could Not Complete API Request.  The Response Produced An Error." + request.Resource);
                _log.Debug(response.Content);
                return default(TClass);
            }

            /*if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new HttpException();
            }*/

            if (response.ErrorException != null)
            {
                _log.Debug("Error Retrieving API Response: " + response.ErrorException);
                return default(TClass);
            }

            if (response.Data == null)
            {
                _log.Debug("Response Data Was Null For Resource: " + request.Resource);
                return default(TClass);
            }

            _log.Debug("Requesting Resource: " + request.Resource);
            return response.Data;
        }

        public byte[] ExecuteRaw(RestRequest request)
        {
            if (request == null)
            {
                _log.Debug("Could Not Execute Raw API Request.  The Request was empty.");
                return null;
            }

            request.AddHeader("Authorization", "bearer " + _token);

            var response = _client.DownloadData(request);

            if (response == null)
            {
                _log.Debug("Could Not Complete Raw API Request.  The Response was empty." + request.Resource);
                return null;
            }

            _log.Debug("Requesting Resource: " + request.Resource);
            return response;
        }
    }
}