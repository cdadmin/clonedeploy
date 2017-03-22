using System;
using System.Web.UI;
using CloneDeploy_Web.Helpers;

namespace views.masters
{
    public partial class SiteMaster : MasterPage
    {
        public void Page_Init(object sender, EventArgs e)
        {

            if (!Request.IsAuthenticated)
                Response.Redirect("~/", true);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            lblServerId.Text = Settings.ServerIdentifier;
            if (Settings.OperationMode == "Cluster Secondary")
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