using System.Web.UI;
using CloneDeploy_ApiCalls;

namespace CloneDeploy_Web.BasePages
{
    public class MasterBaseMaster : MasterPage
    {
        protected virtual void DisplayConfirm()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                true);
        }

        public static string GetSetting(string settingName)
        {
            var setting = new APICall().SettingApi.GetSetting(settingName);
            return setting != null ? setting.Value : string.Empty;
        }
    }
}