using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ComputerMunkiAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public ComputerMunkiAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public List<ComputerMunkiEntity> GetTemplateComputers(int id)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/GetTemplateComputers/{1}", Resource, id);
            return _apiRequest.Execute<List<ComputerMunkiEntity>>(Request);
        }

        public ActionResultDTO Post(ComputerMunkiEntity tObject)
        {
            Request.Method = Method.POST;
            Request.AddJsonBody(tObject);
            Request.Resource = string.Format("api/{0}/Post/", Resource);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }
    }
}