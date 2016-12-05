using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_ApiCalls;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;
using RestSharp;


namespace CloneDeploy_App.Controllers
{
    public class ActiveImagingTaskAPI:GenericAPI<ActiveImagingTaskEntity>
    {
        public ActiveImagingTaskAPI(string resource):base(resource)
        {
		
        }

        public IEnumerable<ActiveImagingTaskEntity> GetUnicasts(string taskType)
        {
            _request.Method = Method.GET;
            _request.Resource = string.Format("api/{0}/GetUnicasts/", _resource);
            _request.AddParameter("tasktype", taskType);
            return new ApiRequest().Execute<IEnumerable<ActiveImagingTaskEntity>>(_request);

        }

        public IEnumerable<ActiveImagingTaskEntity> GetActiveTasks()
        {
           
        }

        public ApiStringResponseDTO GetActiveUnicastCount(string taskType)
        {
         
        }


        public ApiStringResponseDTO GetAllActiveCount()
        {
           

        }

     
    }
}