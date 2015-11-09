using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using BLL.Workflows;
using Helpers;
using Tasks;
using Image = BLL.Image;

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
            var image = BLL.Image.GetImage(group.Image);

            Session["imageID"] = image.Id;
            if (BLL.Image.Check_Checksum(image))
            {
                if (isUnicast == 1)
                {
                    var count = 0;
                    foreach (var host in BLL.Group.GetGroupMembers(group.Id, ""))
                    {
                        BLL.Computer.StartUnicast(host, "push");
                        count++;
                    }
                    EndUserMessage = "Started " + count + " Tasks";
                }
                else
                {
                    var multicast = new Multicast(group);
                    multicast.Create();
                }
                Session.Remove("groupID");
                Session.Remove("isGroupUnicast");
            }
            else
            {
                lblIncorrectChecksum.Text =
                    "This Image Has Not Been Confirmed And Cannot Be Deployed.  <br>Confirm It Now?";
                ClientScript.RegisterStartupScript(GetType(), "modalscript",
                    "$(function() {  var menuTop = document.getElementById('incorrectChecksum'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                    true);
            }
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
                    lblTitle.Text = "Multicast The Selected Group?";
                    gvConfirm.DataSource = new List<Models.Group> { group };
                }
            }
            gvConfirm.DataBind();
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
                    lblTitle.Text = "Unicast All The Hosts In The Selected Group?";
                    gvConfirm.DataSource = new List<Models.Group> { group };
                }
            }
            gvConfirm.DataBind();
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
            var group = new Models.Group();

            gvGroups.DataSource = BLL.Group.SearchGroups(txtSearch.Text);

            gvGroups.DataBind();

            lblTotal.Text = gvGroups.Rows.Count + " Result(s) / " + BLL.Group.TotalCount() + " Total Group(s)";
        }
    }
}