using CloneDeploy_Entities;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class MunkiManifestManagedUpdateAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public MunkiManifestManagedUpdateAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public MunkiManifestManagedUpdateEntity Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return _apiRequest.Execute<MunkiManifestManagedUpdateEntity>(Request);
        }
    }
}