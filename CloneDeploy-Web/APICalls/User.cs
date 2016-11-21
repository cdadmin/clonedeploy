using RestSharp;

namespace CloneDeploy_Web.APICalls
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class User: GenericAPI<Models.CloneDeployUser>
    {
        public User(string resource):base(resource)
        {
		
        }

        public Models.ActionResult GetForLogin(int id)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetForLogin/{1}", _resource,id);
            var response = Execute<Models.ActionResult>(_request);

            if (response.ObjectId == 0)
                response.Success = false;
            return response;

        }
    }
}