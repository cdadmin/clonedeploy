using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class SettingController: ApiController
    {
        private readonly SettingServices _settingServices;

        public SettingController()
        {
            _settingServices = new SettingServices();
        }

        [AdminAuth(Permission = "AdminRead")]
        public SettingEntity GetSetting(string name)
        {
            return _settingServices.GetSetting(name);          
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        [HttpPost]
        public ApiBoolResponseDTO UpdateSettings(List<SettingEntity> listSettings)
        {
            return new ApiBoolResponseDTO() {Value = _settingServices.UpdateSetting(listSettings)};
            
        }
    }
}