using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using Helpers;
using Models;
using RestSharp;

public class GenericAPI<T> : IGenericAPI<T> where T : new()
{
    protected readonly RestRequest _request;
    protected readonly string _token;
    protected readonly string _resource;

    public GenericAPI(string resource)
    {
        _request = new RestRequest();
        _resource = resource;
        _token = HttpContext.Current.Request.Cookies["clonedeploy_token"].Value;
    }

    protected TClass Execute<TClass>(RestRequest request) where TClass : new()
    {
        var client = new RestClient();
        client.BaseUrl = new Uri(ConfigurationManager.AppSettings["api_base_url"]);

        request.AddHeader("Authorization", "bearer " + _token); // used on every request
        var response = client.Execute<TClass>(request);
    
        if (response.ErrorException != null)
        {
            const string message = "Error retrieving response: ";
            Logger.Log(message + response.ErrorException);
        }
        return response.Data;
    }

    public List<T> Get(int limit, string searchstring)
    {
        _request.Method = Method.GET;
        _request.Resource = string.Format("api/{0}/Get",_resource);
        _request.AddParameter("limit", limit);
        _request.AddParameter("searchstring", searchstring);
        return Execute<List<T>>(_request);
    }

    public T Get(int id)
    {
        _request.Method = Method.GET;
        _request.Resource = string.Format("api/{0}/Get/{1}",_resource, id);
        return Execute<T>(_request);
    }

    public ApiDTO GetCount()
    {
        _request.Method = Method.GET;
        _request.Resource = string.Format("api/{0}/GetCount", _resource);
        return Execute<ApiDTO>(_request);
    }

    public ActionResult Put(int id,T tObject)
    {
        _request.Method = Method.PUT;
        _request.AddJsonBody(tObject);
        _request.Resource = string.Format("api/{0}/Put/{1}",_resource, id);
        return Execute<ActionResult>(_request);
    }

    public ActionResult Post(T tObject)
    {
        _request.Method = Method.POST;
        _request.AddJsonBody(tObject);
        _request.Resource = string.Format("api/{0}/Post/",_resource);
        return Execute<ActionResult>(_request);
    }

    public ActionResult Delete(int id)
    {
        _request.Method = Method.DELETE;
        _request.Resource = string.Format("api/{0}/Delete/{1}",_resource, id);
        return Execute<ActionResult>(_request);
    }
}


