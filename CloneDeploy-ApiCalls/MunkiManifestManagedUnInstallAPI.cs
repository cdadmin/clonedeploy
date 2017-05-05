using CloneDeploy_Entities;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class MunkiManifestManagedUnInstallAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public MunkiManifestManagedUnInstallAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public MunkiManifestManagedUnInstallEntity Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return _apiRequest.Execute<MunkiManifestManagedUnInstallEntity>(Request);
        }
    }
}