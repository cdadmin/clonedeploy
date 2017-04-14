using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloneDeploy_Entities;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class MunkiManifestManagedInstallAPI : BaseAPI
    {
        public MunkiManifestManagedInstallAPI(string resource):base(resource)
        {
		
        }

        public MunkiManifestManagedInstallEntity Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}", _resource, id);
            return new ApiRequest().Execute<MunkiManifestManagedInstallEntity>(_request);
        }
    
    }
}
