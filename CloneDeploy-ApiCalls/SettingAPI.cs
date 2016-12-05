using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_ApiCalls;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class SettingAPI : GenericAPI<SettingEntity>
    {
        public SettingAPI(string resource):base(resource)
        {
		
        }
    
      

        [AdminAuth(Permission = "AdminRead")]
        public SettingEntity GetSetting(string name)
        {
            return _settingServices.GetSetting(name);          
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public ApiBoolResponseDTO UpdateSettings(List<SettingEntity> listSettings)
        {
            return new ApiBoolResponseDTO() {Value = _settingServices.UpdateSetting(listSettings)};
            
        }
    }
}