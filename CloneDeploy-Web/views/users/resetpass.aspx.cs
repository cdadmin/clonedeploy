using System;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.users
{
    public partial class ResetPass : Users
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var updatedUser = Call.CloneDeployUserApi.GetSelf();
            if (!string.IsNullOrEmpty(txtUserPwd.Text))
            {
                if (txtUserPwd.Text == txtUserPwdConfirm.Text)
                {
                    updatedUser.Salt = Utility.CreateSalt(64);
                    updatedUser.Password = Utility.CreatePasswordHash(txtUserPwd.Text, updatedUser.Salt);
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

            var result = Call.CloneDeployUserApi.ChangePassword(updatedUser);
            EndUserMessage = !result.Success ? result.ErrorMessage : "Successfully Updated User";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (CloneDeployCurrentUser.Id.ToString() != (string) Session["UserId"])
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");

            PopulateForm();
        }

        private void PopulateForm()
        {
            var user = Call.CloneDeployUserApi.GetSelf();
            if (user.IsLdapUser == 1)
            {
                chkldap.Checked = true;
                passwords.Visible = false;
            }
            txtEmail.Text = user.Email;
            chkLockout.Checked = user.NotifyLockout == 1;
            chkError.Checked = user.NotifyError == 1;
            chkComplete.Checked = user.NotifyComplete == 1;
            chkApproved.Checked = user.NotifyImageApproved == 1;
        }
    }
}