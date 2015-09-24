using System;
using System.Web.UI;
using BLL;
using Global;
using Models;

namespace views.masters
{
    public partial class UserMaster : MasterPage
    {
        public WdsUser User { get; set; }
        private readonly BLL.User _bllUser = new BLL.User();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["userid"])) return;
            User = _bllUser.GetUser(Convert.ToInt32(Request.QueryString["userid"]));
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (_bllUser.GetAdminCount() == 1 && User.Membership == "Administrator")
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
            _bllUser.DeleteUser(User.Id);
            if (Utility.Message.Contains("Successfully"))
                Response.Redirect("~/views/users/search.aspx");
          
        }       
    }
}