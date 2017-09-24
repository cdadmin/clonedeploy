using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_ApiCalls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
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
            Timer_Tick(sender,e);
        }

        protected void Timer_Tick(object sender, EventArgs e)
        {
            PopulateGrid();
            lblTotal.Text = new APICall().ActiveImagingTaskApi.GetAllActiveCount() + " Active Task(s)";
            UpdatePanel1.Update();
        }

        private void PopulateGrid()
        {
            gvTasks.DataSource = new APICall().ActiveImagingTaskApi.GetActiveTasks();
            gvTasks.DataBind();

            var currentUser = Session["CloneDeployUser"];
            var cu = (CloneDeployUserEntity) currentUser;
            var listOfAuditLogs = new APICall().CloneDeployUserApi.GetCurrentUserAuditLogs(1000);
            gvHistory.DataSource = listOfAuditLogs;
            gvHistory.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow)control.Parent.Parent;
                var dataKey = gvTasks.DataKeys[gvRow.RowIndex];
                if (dataKey != null)

                    new APICall().ActiveImagingTaskApi.Delete(Convert.ToInt32(dataKey.Value));
            }
            gvTasks.DataSource = new APICall().ActiveImagingTaskApi.GetActiveTasks();
            gvTasks.DataBind();
        }

        protected void LogOut_OnClick(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Response.Redirect("~/", true);
        }
    }
}