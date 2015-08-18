using System;
using System.Web.UI;
using Global;
using Models;

namespace views.masters
{
    public partial class UserMaster : MasterPage
    {
        public WdsUser User { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["userid"])) return;
            User = new WdsUser { Id = Request.QueryString["userid"] };
            User.Read();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (User.GetAdminCount() == 1 && User.Membership == "Administrator")
            {
                Master.Msgbox("There Must Be At Least One Administrator");
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
            User.Delete();
            if (Utility.Message.Contains("Successfully"))
                Response.Redirect("~/views/users/search.aspx");
            else
                Master.Msgbox(Utility.Message);
        }       
    }
}