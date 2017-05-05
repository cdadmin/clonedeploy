using CloneDeploy_Entities;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class MunkiManifestIncludedManifestAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public MunkiManifestIncludedManifestAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public MunkiManifestIncludedManifestEntity Get(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/{1}", Resource, id);
            return _apiRequest.Execute<MunkiManifestIncludedManifestEntity>(Request);
        }
    }
}