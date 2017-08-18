using System;
using System.Web.UI;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.admin.logs
{
    public partial class logs : MasterBaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (GetSetting(SettingStrings.OperationMode) == "Cluster Secondary")
            {
                ond.Visible = false;
            }
        }
    }
}