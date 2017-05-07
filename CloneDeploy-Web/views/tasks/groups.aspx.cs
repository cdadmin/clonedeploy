using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

namespace views.tasks
{
    public partial class TaskMulticast : Tasks
    {
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            var groupId = Convert.ToInt32(Session["groupID"]);
            var isUnicast = Convert.ToInt32(Session["isGroupUnicast"]);

            if (isUnicast == 1)
            {
                RequiresAuthorizationOrManagedGroup(Authorizations.ImageDeployTask, groupId);
                var successCount = Call.GroupApi.StartGroupUnicast(groupId);
                EndUserMessage = "Started " + successCount + " Tasks";
            }
            else
            {
                RequiresAuthorizationOrManagedGroup(Authorizations.ImageMulticastTask, groupId);
                EndUserMessage = Call.GroupApi.StartMulticast(groupId);
            }
            Session.Remove("groupID");
            Session.Remove("isGroupUnicast");
        }

        protected void btnMulticast_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvGroups.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    var group = Call.GroupApi.Get(Convert.ToInt32(dataKey.Value));
                    Session["groupID"] = group.Id;
                    Session["isGroupUnicast"] = 0;
                    lblTitle.Text = "Start Multicast For Group " + group.Name + "?";
                }
            }

            ClientScript.RegisterStartupScript(GetType(), "modalscript",
                "$(function() { var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                true);
        }

        protected void btnUnicast_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                var gvRow = (GridViewRow) control.Parent.Parent;
                var dataKey = gvGroups.DataKeys[gvRow.RowIndex];
                if (dataKey != null)
                {
                    var group = Call.GroupApi.Get(Convert.ToInt32(dataKey.Value));

                    Session["groupID"] = group.Id;
                    Session["isGroupUnicast"] = 1;
                    lblTitle.Text = "Unicast All Computers In Group " + group.Name + "?";
                }
            }
            ClientScript.RegisterStartupScript(GetType(), "modalscript",
                "$(function() { var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                true);
        }

        protected void OkButtonChecksum_Click(object sender, EventArgs e)
        {
            var imageId = (string) Session["imageID"];
            Response.Redirect("~/views/images/specs.aspx?imageid=" + imageId, false);
            Session.Remove("imageID");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateGrid();
        }

        protected void PopulateGrid()
        {
            gvGroups.DataSource = Call.GroupApi.GetAll(int.MaxValue, txtSearch.Text);
            gvGroups.DataBind();
            lblTotal.Text = gvGroups.Rows.Count + " Result(s) / " + Call.GroupApi.GetCount() + " Total Group(s)";
        }
    }
}