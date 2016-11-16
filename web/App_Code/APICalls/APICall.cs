using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;

public class APICall : IAPICall
{
    public IGenericAPI<Models.Computer> ComputerApi
    {
        get { return new GenericAPI<Models.Computer>("Computer"); }
    }

    public TokenApi TokenApi
    {
        get { return new TokenApi("Token"); }
    }

    public User CloneDeployUserApi
    {
        get { return new User("User"); }
    }
}