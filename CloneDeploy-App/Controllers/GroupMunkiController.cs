using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class GroupMunkiController:ApiController
    {
        private readonly GroupMunkiServices _groupMunkiServices;

        public GroupMunkiController()
        {
            _groupMunkiServices = new GroupMunkiServices();
        }

        [GroupAuth(Permission = "GroupCreate")]
        public ActionResultDTO Post(GroupMunkiEntity groupMunki)
        {
            return  _groupMunkiServices.AddMunkiTemplates(groupMunki);
        }

    
    }
}