using System.Collections.Generic;
using System.Security.Policy;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;
using CloneDeploy_Services.Helpers;


namespace CloneDeploy_App.Controllers
{
    public class SettingController: ApiController
    {
        private readonly SettingServices _settingServices;

        public SettingController()
        {
            _settingServices = new SettingServices();
        }

        [Authorize]
        public SettingEntity GetSetting(string name)
        {
            return _settingServices.GetSetting(name);          
        }

        [CustomAuth(Permission = "AdminUpdate")]
        [HttpPost]
        public ApiBoolResponseDTO UpdateSettings(List<SettingEntity> listSettings)
        {
            return new ApiBoolResponseDTO() {Value = _settingServices.UpdateSetting(listSettings)};         
        }

        [CustomAuth(Permission = "AdminUpdate")]
        [HttpGet]
        public ApiBoolResponseDTO SendEmailTest()
        {
            var mail = new Mail
            {
                Subject = "Test Message",
                Body = "Email Notifications Are Working!",
                MailTo = Settings.SmtpMailTo
            };

            mail.Send();
            return new ApiBoolResponseDTO() {Value = true};
        }
    }
}