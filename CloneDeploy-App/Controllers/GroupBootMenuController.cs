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
    public class GroupBootMenuController : ApiController
    {

        [GroupAuth(Permission = "GroupRead")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.GroupBootMenu.GetGroupBootMenu(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }


        [GroupAuth(Permission = "GroupUpdate")]
        public ActionResultEntity Put(GroupBootMenuEntity groupBootMenu)
        {
            var actionResult = new ActionResultEntity();
            actionResult.Success = BLL.GroupBootMenu.UpdateGroupBootMenu(groupBootMenu);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

       

       
    }
}