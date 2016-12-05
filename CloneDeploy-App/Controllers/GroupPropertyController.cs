using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class GroupPropertyController : ApiController
    {
        private readonly GroupPropertyServices _groupPropertyServices;

        public GroupPropertyController()
        {
            _groupPropertyServices = new GroupPropertyServices();
        }

        [GroupAuth]
        public ActionResultDTO Post(GroupPropertyEntity groupProperty)
        {
            return _groupPropertyServices.AddGroupProperty(groupProperty);
        }

        [GroupAuth]
        public ActionResultDTO Put(int id,GroupPropertyEntity groupProperty)
        {
            groupProperty.Id = id;
            var result = _groupPropertyServices.UpdateGroupProperty(groupProperty);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

       

       
    }
}