using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.groups
{
    public partial class GroupMaster : MasterBaseMaster
    {
        public GroupEntity Group { get; set; }
        private Groups groupBasePage { get; set; }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            var taskType = (string) Session["taskType"];
            Session.Remove("taskType");
            switch (taskType)
            {
                case "delete":
                    groupBasePage.RequiresAuthorizationOrManagedGroup(AuthorizationStrings.DeleteGroup, Group.Id);
                    var result = groupBasePage.Call.GroupApi.Delete(Group.Id);
                    if (result.Success)
                    {
                        PageBaseMaster.EndUserMessage = "Successfully Deleted Group";
                        Response.Redirect("~/views/groups/search.aspx");
                    }
                    else
                        PageBaseMaster.EndUserMessage = result.ErrorMessage;
                    break;
                case "unicast":
                    groupBasePage.RequiresAuthorizationOrManagedGroup(AuthorizationStrings.ImageDeployTask, Group.Id);
                    var successCount = groupBasePage.Call.GroupApi.StartGroupUnicast(Group.Id);
                    PageBaseMaster.EndUserMessage = "Successfully Started " + successCount + " Tasks";
                    break;
                case "multicast":
                    groupBasePage.RequiresAuthorizationOrManagedGroup(AuthorizationStrings.ImageMulticastTask, Group.Id);
                    PageBaseMaster.EndUserMessage = groupBasePage.Call.GroupApi.StartMulticast(Group.Id);
                    break;
            }
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Delete This Group?";
            Session["taskType"] = "delete";
            gvConfirm.DataBind(); // clear gridview if deploy or upload was clicked first
            DisplayConfirm();
        }

        protected void btnMulticast_Click(object sender, EventArgs e)
        {
            Session["taskType"] = "multicast";
            lblTitle.Text = "Multicast The Selected Group?";
            gvConfirm.DataSource = new List<GroupEntity> {Group};
            gvConfirm.DataBind();
            foreach (GridViewRow row in gvConfirm.Rows)
            {
                var lbl = row.FindControl("lblImage") as Label;
                var image = groupBasePage.Call.ImageApi.Get(Group.ImageId);
                if (image != null)
                    lbl.Text = image.Name;
            }
            DisplayConfirm();
        }

        protected void btnUnicast_Click(object sender, EventArgs e)
        {
            Session["taskType"] = "unicast";
            lblTitle.Text = "Unicast All The Computers In The Selected Group?";
            gvConfirm.DataSource = new List<GroupEntity> {Group};
            gvConfirm.DataBind();
            DisplayConfirm();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            groupBasePage = Page as Groups;
            Group = groupBasePage.Group;
            if (Group == null)
            {
                Level2_Edit.Visible = false;
                btnDelete.Visible = false;
                btnMulticast.Visible = false;
                btnUnicast.Visible = false;
                return;
            }

            Level1.Visible = false;
            if (Group.Type == "standard")
            {
                addmembers.Visible = true;
                removemembers.Visible = true;
            }
            else
            {
                smart.Visible = true;
                currentmembers.Visible = true;
            }
        }
    }
}