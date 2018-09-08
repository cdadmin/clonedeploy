using System.Collections.Generic;
using CloneDeploy_Common.Enum;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using RestSharp;

namespace CloneDeploy_ApiCalls
{
    public class ImageProfileTemplateAPI : BaseAPI
    {
        private readonly ApiRequest _apiRequest;

        public ImageProfileTemplateAPI(string resource) : base(resource)
        {
            _apiRequest = new ApiRequest();
        }

        public ImageProfileTemplate Get(EnumProfileTemplate.TemplateType templateType)
        {
            Request.Method = Method.GET;
            Request.Resource = string.Format("api/{0}/Get/", Resource);
            Request.AddParameter("templateType", templateType);
            return _apiRequest.Execute<ImageProfileTemplate>(Request);
        }

        public ActionResultDTO Put(ImageProfileTemplate template)
        {
            Request.Method = Method.PUT;
            Request.AddJsonBody(template);
            Request.Resource = string.Format("api/{0}/Put/", Resource);
            var response = _apiRequest.Execute<ActionResultDTO>(Request);
            if (response.Id == 0)
                response.Success = false;
            return response;
        }
    }
}