using System;
using System.Web.UI;
using BasePages;
using Helpers;
using Models;

namespace views.masters
{
    public partial class UserMaster : BasePages.MasterBaseMaster
    {
        private Users UsersBasePage { get; set; }
        public WdsUser CloneDeployUser { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            UsersBasePage = (Page as Users);
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
            if (BLL.User.GetAdminCount() == 1 && CloneDeployUser.Membership == "Administrator")
            {
                //Message.Text = "There Must Be At Least One Administrator";
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
            BLL.User.DeleteUser(CloneDeployUser.Id);
                Response.Redirect("~/views/users/search.aspx");
          
        }       
    }
}