using System;
using BasePages;

namespace views.users
{
    public partial class ResetPass : Users
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (CloneDeployCurrentUser.Id.ToString() != (string) Session["UserId"])
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");

            PopulateForm();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var updatedUser = BLL.User.GetUser(Convert.ToInt32(Session["UserId"]));
            if (!string.IsNullOrEmpty(txtUserPwd.Text))
            {
                if (txtUserPwd.Text == txtUserPwdConfirm.Text)
                {
                    updatedUser.Salt = Helpers.Utility.CreateSalt(64);
                    updatedUser.Password = Helpers.Utility.CreatePasswordHash(txtUserPwd.Text, updatedUser.Salt);
                }
                else
                {
                    EndUserMessage = "Passwords Did Not Match";
                    return;
                }
            }

            updatedUser.Email = txtEmail.Text;
            updatedUser.NotifyLockout = chkLockout.Checked ? 1 : 0;
            updatedUser.NotifyError = chkError.Checked ? 1 : 0;
            updatedUser.NotifyComplete = chkComplete.Checked ? 1 : 0;
            updatedUser.NotifyImageApproved = chkApproved.Checked ? 1 : 0;

            var result = BLL.User.UpdateUser(updatedUser);
            EndUserMessage = !result.IsValid ? result.Message : "Successfully Updated User";
        }

        private void PopulateForm()
        {       
            txtEmail.Text = CloneDeployCurrentUser.Email;
            chkLockout.Checked = CloneDeployCurrentUser.NotifyLockout == 1;
            chkError.Checked = CloneDeployCurrentUser.NotifyError == 1;
            chkComplete.Checked = CloneDeployCurrentUser.NotifyComplete == 1;
            chkApproved.Checked = CloneDeployCurrentUser.NotifyImageApproved == 1;
        }
    }
}