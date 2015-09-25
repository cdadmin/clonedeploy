using System;
using System.Web.UI;
using BLL;
using Global;
using Helpers;
using Models;

namespace views.masters
{
    public partial class UserMaster : MasterPage
    {
        private BasePages.Users UsersBasePage { get; set; }
        public Models.WdsUser CloneDeployUser { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            UsersBasePage = (Page as BasePages.Users);
            CloneDeployUser = UsersBasePage.CloneDeployUser;
            if (CloneDeployUser == null)
            {
                Level2.Visible = false;
                return;
            }

            Level1.Visible = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (UsersBasePage.BllUser.GetAdminCount() == 1 && CloneDeployUser.Membership == "Administrator")
            {
                Message.Text = "There Must Be At Least One Administrator";
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
            UsersBasePage.BllUser.DeleteUser(CloneDeployUser.Id);
            if (Message.Text.Contains("Successfully"))
                Response.Redirect("~/views/users/search.aspx");
          
        }       
    }
}