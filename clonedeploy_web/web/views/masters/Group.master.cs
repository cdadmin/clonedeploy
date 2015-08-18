using System;
using System.Collections.Generic;
using System.Web.UI;
using Global;
using Models;
using Tasks;

namespace views.masters
{
    public partial class GroupMaster : MasterPage
    {
        public Group Group { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["groupid"])) return;
            Group = new Group { Id = Request.QueryString["groupid"] };
            Group.Read();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if ((string) Session["direction"] == "delete")
            {
                Session.Remove("direction");
                Group.Delete();
                if (Utility.Message.Contains("Successfully"))
                    Response.Redirect("~/views/groups/search.aspx");
                else
                    Master.Msgbox(Utility.Message);
            }
            else
            {
                var image = new Image {Name = Group.Image};
                image.Read();
                Session["imageID"] = image.Id;
                if (image.Check_Checksum())
                {
                    var count = 0;
                    var isUnicast = Convert.ToInt32(Session["isGroupUnicast"]);
                    if (isUnicast == 1)
                    {
                        foreach (var host in Group.GroupMembers())
                        {
                            var unicast = new Unicast {Host = host, Direction = "push"};
                            unicast.Create();
                            count++;
                        }
                        Utility.Message = "Started " + count + " Tasks";
                        var history = new History
                        {
                            Event = "Unicast",
                            Type = "Group",
                            TypeId = Group.Id
                        };
                        history.CreateEvent();
                    }
                    else
                    {
                        var multicast = new Multicast {Group = Group};
                        multicast.Create();
                    }
                    Session.Remove("isGroupUnicast");
                    Master.Msgbox(Utility.Message);
                }

                else
                {
                    lblIncorrectChecksum.Text =
                        "This Image Has Not Been Confirmed And Cannot Be Deployed.  <br>Confirm It Now?";
                    Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                        "$(function() {  var menuTop = document.getElementById('incorrectChecksum'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                        true);
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Delete This Group?";
            Session["direction"] = "delete";
            gvConfirm.DataBind(); // clear gridview if deploy or upload was clicked first
            Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                true);
        }

        protected void btnMulticast_Click(object sender, EventArgs e)
        {
            Session["isGroupUnicast"] = 0;
            lblTitle.Text = "Multicast The Selected Group?";
            gvConfirm.DataSource = new List<Group> {Group};
            gvConfirm.DataBind();
            Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                "$(function() { var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                true);
        }

        protected void btnUnicast_Click(object sender, EventArgs e)
        {
            Session["isGroupUnicast"] = 1;
            lblTitle.Text = "Unicast All The Hosts In The Selected Group?";
            gvConfirm.DataSource = new List<Group> { Group };
            gvConfirm.DataBind();
            Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                "$(function() { var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                true);
        }

        protected void OkButtonChecksum_Click(object sender, EventArgs e)
        {
            var imageId = (string) (Session["imageID"]);
            Response.Redirect("~/views/images/specs.aspx?imageid=" + imageId, false);
            Session.Remove("imageID");
        }
    }
}