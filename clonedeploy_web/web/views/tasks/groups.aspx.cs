using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;

namespace views.tasks
{
    public partial class TaskMulticast : BasePages.Tasks
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateGrid();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            var groupId = Convert.ToInt32(Session["groupID"]);
            var isUnicast = Convert.ToInt32(Session["isGroupUnicast"]);
            var group = BLL.Group.GetGroup(groupId);

            if (isUnicast == 1)
            {
                RequiresAuthorizationOrManagedGroup(Authorizations.ImageDeployTask, group.Id);
                var successCount = BLL.Group.StartGroupUnicast(group, CloneDeployCurrentUser.Id);
                EndUserMessage = "Started " + successCount + " Tasks";
            }
            else
            {
                RequiresAuthorizationOrManagedGroup(Authorizations.ImageMulticastTask, group.Id);
                EndUserMessage = new BLL.Workflows.Multicast(group,CloneDeployCurrentUser.Id).Create();
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
                    var group = BLL.Group.GetGroup(Convert.ToInt32(dataKey.Value));
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
                    var group = BLL.Group.GetGroup(Convert.ToInt32(dataKey.Value));
                  
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
            var imageId = (string) (Session["imageID"]);
            Response.Redirect("~/views/images/specs.aspx?imageid=" + imageId, false);
            Session.Remove("imageID");
        }

        protected void PopulateGrid()
        {
            gvGroups.DataSource = BLL.Group.SearchGroupsForUser(CloneDeployCurrentUser.Id, txtSearch.Text);
            gvGroups.DataBind();
            lblTotal.Text = gvGroups.Rows.Count + " Result(s) / " + BLL.Group.GroupCountUser(CloneDeployCurrentUser.Id) + " Total Group(s)";
        }
    }
}