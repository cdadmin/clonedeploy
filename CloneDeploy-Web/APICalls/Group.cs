using CloneDeploy_Web.Models;
using RestSharp;

namespace CloneDeploy_Web.APICalls
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class GroupAPI: GenericAPI<Models.Group>
    {
        public GroupAPI(string resource):base(resource)
        {
		
        }

        public ApiDTO GetMemberCount(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetMemberCount/{1}", _resource,id);
            return new ApiRequest().Execute<ApiDTO>(_request);
        }
    }
}