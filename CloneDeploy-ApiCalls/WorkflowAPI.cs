using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class WorkflowAPI
    {
        private readonly RestRequest _request;     
        private readonly string _resource;

        public WorkflowAPI(string resource)
        {
            _request = new RestRequest();
            _resource = resource;
        }
    

        public bool CreateDefaultBootMenu(BootMenuGenOptionsDTO defaultMenuOptions)
        {
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/CreateDefaultBootMenu/", _resource);
            _request.AddJsonBody(defaultMenuOptions);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

        public bool GenerateLinuxIsoConfig(IsoGenOptionsDTO isoOptions)
        {
            _request.Method = Method.POST;
            _request.Resource = string.Format("api/{0}/GenerateLinuxIsoConfig/", _resource);
            _request.AddJsonBody(isoOptions);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

        public bool CreateClobberBootMenu(int profileId, bool promptComputerName)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/CreateClobberBootMenu/", _resource);
            _request.AddParameter("profileId", profileId);
            _request.AddParameter("promptComputerName", promptComputerName);
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }

        public bool CopyPxeBinaries()
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/CopyPxeBinaries/", _resource);     
            return new ApiRequest().Execute<ApiBoolResponseDTO>(_request).Value;
        }
    }
}
