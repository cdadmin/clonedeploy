using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class OnlineKernelAPI : BaseAPI
    {
        public OnlineKernelAPI(string resource):base(resource)
        {
		
        }

        public List<OnlineKernel> GetAll()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetAll", _resource);
           
            return new ApiRequest().Execute<List<OnlineKernel>>(_request);
        }

        public bool Download(OnlineKernel tObject)
        {
            _request.Method = Method.POST;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Download/", _resource);
            var response = new ApiRequest().Execute<ApiBoolResponseDTO>(_request);
            return response != null && response.Value;
        }


    
    }
}
