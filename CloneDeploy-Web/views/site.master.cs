using System;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views
{
    public partial class SiteMaster : MasterBaseMaster
    {
        public void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
                Response.Redirect("~/", true);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            lblServerId.Text = GetSetting(SettingStrings.ServerIdentifier);
            if (GetSetting(SettingStrings.OperationMode) == "Cluster Secondary")
            {
                navhosts.Visible = false;
                navgroups.Visible = false;
                navimages.Visible = false;
                navglobal.Visible = false;
                navtasks.Visible = false;
            }
        }
    }
}