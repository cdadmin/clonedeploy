using System;
using System.Collections.Generic;
using BasePages;
using BLL.Workflows;
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
                Level2.Visible = false;
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
                    if (BLL.Group.DeleteGroup(Group.Id))
                        Response.Redirect("~/views/groups/search.aspx");
                    break;
                case "unicast":
                    BLL.Group.StartGroupUnicast(Group);
                    break;
                case "multicast":
                    PageBaseMaster.EndUserMessage = new Multicast(Group).Create();
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