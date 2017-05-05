using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class GroupPropertyAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public GroupPropertyAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public ActionResultDTO Post(GroupPropertyEntity tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public ActionResultDTO Put(int id, GroupPropertyEntity tObject)
        {
            Request.Method = Method.PUT;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Put/{1}", Resource, id);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }
    }
}