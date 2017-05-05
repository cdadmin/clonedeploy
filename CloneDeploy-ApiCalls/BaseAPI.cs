using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class BaseAPI
    {
        protected readonly RestRequest Request;
        protected readonly string Resource;

        public BaseAPI(string resource)
        {
            Request = new RestRequest();
            Resource = resource;
        }   
    }
}