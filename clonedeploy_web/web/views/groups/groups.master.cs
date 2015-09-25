using System;
using System.Collections.Generic;
using System.Web.UI;
using Global;
using Models;
using Tasks;


namespace views.masters
{
    public partial class GroupMaster : BasePages.MasterBaseMaster
    {
        private BasePages.Groups groupBasePage { get; set; }
        public Models.Group Group { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            groupBasePage = (Page as BasePages.Groups);
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
                    if (groupBasePage.BllGroup.DeleteGroup(Group.Id))
                        Response.Redirect("~/views/groups/search.aspx");
                    break;
                case "unicast":
                    groupBasePage.BllGroup.StartGroupUnicast(Group);
                    break;
                case "multicast":
                    groupBasePage.BllGroup.StartMulticast(Group);
                    break;

            }

            lblIncorrectChecksum.Text =
                "This Image Has Not Been Confirmed And Cannot Be Deployed.  <br>Confirm It Now?";
            DisplayIncorrectChecksum();

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

        protected void OkButtonChecksum_Click(object sender, EventArgs e)
        {
            ApproveChecksumRedirect();
        }
    }
}