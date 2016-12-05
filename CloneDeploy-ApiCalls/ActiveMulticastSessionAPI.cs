using System.Collections.Generic;
using CloneDeploy_Entities;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ActiveMulticastSessionAPI:GenericAPI<ActiveMulticastSessionEntity>
    {
        public ActiveMulticastSessionAPI(string resource):base(resource)
        {
		
        }



        public IEnumerable<ActiveImagingTaskEntity> GetMemberStatus(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetUnicasts/{1}", _resource,id);
            return new ApiRequest().Execute<List<ActiveImagingTaskEntity>>(_request);
        }

        public IEnumerable<ComputerEntity> GetComputers(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetComputers/{1}", _resource,id);
            return new ApiRequest().Execute<List<ComputerEntity>>(_request);
        }


        public IEnumerable<ActiveImagingTaskEntity> GetProgress(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetUnicasts/{1}", _resource,id);
            return new ApiRequest().Execute<List<ActiveImagingTaskEntity>>(_request);
           
        }

     
    }
}