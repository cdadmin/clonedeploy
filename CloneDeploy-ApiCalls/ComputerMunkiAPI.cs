using System.Collections.Generic;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ComputerMunkiAPI : BaseAPI
    {
        public ComputerMunkiAPI(string resource):base(resource)
        {
		
        }

        public ActionResultDTO Post(ComputerMunkiEntity tObject)
        {
            _request.Method = Method.POST;
            _request.AddJsonBody(tObject);
            _request.Resource = string.Format("api/{0}/Post/", _resource);
            var response = new ApiRequest().Execute<ActionResultDTO>(_request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }

        public List<ComputerMunkiEntity> GetTemplateComputers(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetTemplateComputers/{1}", _resource,id);
            return new ApiRequest().Execute<List<ComputerMunkiEntity>>(_request);
        }

     

       

       
    
    }
}