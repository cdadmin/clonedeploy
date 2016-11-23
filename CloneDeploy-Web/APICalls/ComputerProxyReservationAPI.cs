using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloneDeploy_Web.Models;
using RestSharp;

namespace CloneDeploy_Web.APICalls
{
    public class ComputerProxyReservationAPI:GenericAPI<Models.ComputerProxyReservation>
    {
        public ComputerProxyReservationAPI(string resource):base(resource)
        {
		
        }

        public ApiBoolDTO Toggle(int id, bool status)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Toggle/{1}", _resource,id);
            _request.AddParameter("status", status);
            return new ApiRequest().Execute<ApiBoolDTO>(_request);
        }
    
    }
}