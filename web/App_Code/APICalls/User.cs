using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;

/// <summary>
/// Summary description for User
/// </summary>
public class User: GenericAPI<Models.CloneDeployUser>
{
	public User(string resource):base(resource)
	{
		
	}

    public Models.ActionResult GetForLogin(int id)
    {
        _request.Method = Method.GET;
        _request.Resource = string.Format("api/{0}/GetForLogin/{1}", _resource,id);
        return Execute<Models.ActionResult>(_request);
        
    }
}