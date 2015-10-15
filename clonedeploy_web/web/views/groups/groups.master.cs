using System;
using System.Collections.Generic;
using BasePages;
using Models;

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
                Level2_Smart_Group.Visible = false;
                Level2_Standard_Group.Visible = false;
                return;
            }


            Level1.Visible = false;
            if (Group.Type == "standard")
            {
                Level2_Standard_Group.Visible = true;
                Level2_Smart_Group.Visible = false;
            }
            else
            {
                Level2_Standard_Group.Visible = false;
                Level2_Smart_Group.Visible = true;
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
                    BLL.Group.StartMulticast(Group);
                    break;

            }

            PageBaseMaster.EndUserMessage =
                "This Image Has Not Been Confirmed And Cannot Be Deployed.  <br>Confirm It Now?";


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
            lblTitle.Text = "Unicast All The Hosts In The Selected Group?";
            gvConfirm.DataSource = new List<Group> { Group };
            gvConfirm.DataBind();
            DisplayConfirm();
        }
    }
}