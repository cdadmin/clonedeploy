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


namespace CloneDeploy_App.Controllers
{
    public class GroupMunkiController:ApiController
    {
        [GroupAuth(Permission = "GroupCreate")]
        public ApiBoolDTO Post(GroupMunkiEntity groupMunki)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.GroupMunki.AddMunkiTemplates(groupMunki);
           
            return apiBoolDto;
        }

    
    }
}