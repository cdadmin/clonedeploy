using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloneDeploy_Entities;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class MunkiManifestManagedUnInstallAPI : BaseAPI
    {
        public MunkiManifestManagedUnInstallAPI(string resource):base(resource)
        {
		
        }

        public MunkiManifestManagedUnInstallEntity Get(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/Get/{1}", _resource, id);
            return new ApiRequest().Execute<MunkiManifestManagedUnInstallEntity>(_request);
        }
    
    }
}
