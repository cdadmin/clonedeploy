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

namespace CloneDeploy_App.Controllers
{
    public class GroupPropertyController : ApiController
    {
        [GroupAuth]
        public IHttpActionResult Post(GroupPropertyEntity groupProperty)
        {
            BLL.GroupProperty.AddGroupProperty(groupProperty);
            return Ok();
        }

        [GroupAuth]
        public IHttpActionResult Put(int id,GroupPropertyEntity groupProperty)
        {
            groupProperty.Id = id;
            BLL.GroupProperty.UpdateGroupProperty(groupProperty);
           return Ok();
        }

       

       
    }
}