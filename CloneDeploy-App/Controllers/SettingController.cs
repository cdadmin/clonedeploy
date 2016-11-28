using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
{
    public class SettingController: ApiController
    {
        [AdminAuth(Permission = "AdminRead")]
        public IHttpActionResult Get(string name)
        {
            var result = BLL.Setting.GetSetting(name);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public ApiBoolDTO Put(List<Models.Setting> listSettings)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.Setting.UpdateSetting(listSettings);
            
            return apiBoolDto;
        }
    }
}