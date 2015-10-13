using System;
using BasePages;
using BLL;
using Helpers;
using Security;

namespace views.users
{
    public partial class ResetPass : Users
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (CloneDeployCurrentUser.Id.ToString() != (string) Session["UserId"])
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var user = BLL.User.GetUser(Convert.ToInt32(Session["UserId"]));
            if (txtUserPwd.Text == txtUserPwdConfirm.Text && !string.IsNullOrEmpty(txtUserPwd.Text))
            {
                user.Password = txtUserPwd.Text;
                user.Salt = BLL.User.CreateSalt(16);
                var result = BLL.User.UpdateUser(user, true);
                EndUserMessage = !result.IsValid ? result.Message : "Successfully Changed Password";
                
            }
            else
                EndUserMessage = "Passwords Did Not Match";
        }
    }
}