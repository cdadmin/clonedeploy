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

    public Models.CloneDeployUser GetByName(string name)
    {
        _request.Method = Method.GET;
        _request.Resource = string.Format("api/{0}/GetByName/", _resource);
        _request.AddParameter("name", name);
        return Execute<Models.CloneDeployUser>(_request);
    }
}