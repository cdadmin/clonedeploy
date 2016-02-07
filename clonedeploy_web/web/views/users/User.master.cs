using System;
using BasePages;
using Models;

namespace views.masters
{
    public partial class UserMaster : BasePages.MasterBaseMaster
    {
        private Users UsersBasePage { get; set; }
        public CloneDeployUser CloneDeployUser { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            UsersBasePage = (Page as Users);
            CloneDeployUser = UsersBasePage.CloneDeployUser;
            if (CloneDeployUser == null) //level 2
            {
                Level2.Visible = false;
                Level3.Visible = false;
                actions_left.Visible = false;
            }
            else
            {
                Level1.Visible = false;
                if (Request.QueryString["level"] == "3")
                    Level2.Visible = false;
            }

            if (UsersBasePage.CloneDeployCurrentUser.Membership == "User")
                Level1.Visible = false;

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (BLL.User.GetAdminCount() == 1 && CloneDeployUser.Membership == "Administrator")
            {
                PageBaseMaster.EndUserMessage = "There Must Be At Least One Administrator";
            }
            else
            {
                lblTitle.Text = "Delete This User?";
                Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                    "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                    true);
            }
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            if (BLL.User.DeleteUser(CloneDeployUser.Id))
            {
                PageBaseMaster.EndUserMessage = "Successfully Deleted User";
                Response.Redirect("~/views/users/search.aspx");
            }
            else
            {
                PageBaseMaster.EndUserMessage = "Could Not Delete User";
            }

        }       
    }
}