using System;
using System.Collections.Generic;
using BasePages;
using BLL.Workflows;
using Helpers;
using Models;
using Tasks;

namespace views.masters
{
    public partial class GroupMaster : MasterBaseMaster
    {
        private Groups groupBasePage { get; set; }
        public Group Group { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            groupBasePage = (Page as Groups);
            Group = groupBasePage.Group;
            if (Group == null)
            {
                Level2_Edit.Visible = false;
                actions_left.Visible = false;
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

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            var taskType = (string) (Session["taskType"]);
            Session.Remove("taskType");
            switch (taskType)
            {
                case "delete":
                    groupBasePage.RequiresAuthorizationOrManagedGroup(Authorizations.DeleteGroup,Group.Id);
                    var result = BLL.Group.DeleteGroup(Group.Id);
                    if (result.IsValid)
                    {
                        PageBaseMaster.EndUserMessage = "Successfully Deleted Group";
                        Response.Redirect("~/views/groups/search.aspx");
                    }
                    else
                        PageBaseMaster.EndUserMessage = result.Message;
                    break;
                case "unicast":
                    groupBasePage.RequiresAuthorizationOrManagedGroup(Authorizations.ImageDeployTask,Group.Id);
                    var successCount = BLL.Group.StartGroupUnicast(Group,groupBasePage.CloneDeployCurrentUser.Id);
                    PageBaseMaster.EndUserMessage = "Succssfully Started " + successCount + " Tasks";
                    break;
                case "multicast":
                    groupBasePage.RequiresAuthorizationOrManagedGroup(Authorizations.ImageMulticastTask, Group.Id);
                    PageBaseMaster.EndUserMessage = new Multicast(Group,groupBasePage.CloneDeployCurrentUser.Id).Create();
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
            gvConfirm.DataSource = new List<Group> {Group};
            gvConfirm.DataBind();
            DisplayConfirm();
        }
        protected void btnUnicast_Click(object sender, EventArgs e)
        {
            Session["taskType"] = "unicast";
            lblTitle.Text = "Unicast All The Computers In The Selected Group?";
            gvConfirm.DataSource = new List<Group> { Group };
            gvConfirm.DataBind();
            DisplayConfirm();
        }
    }
}