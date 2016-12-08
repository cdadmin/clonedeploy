using System;
using BasePages;
using CloneDeploy_Web;

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
            var updatedUser = Call.CloneDeployUserApi.Get(Convert.ToInt32(Session["UserId"]));
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

            var result = Call.CloneDeployUserApi.Put(updatedUser.Id,updatedUser);
            EndUserMessage = !result.Success ? result.ErrorMessage : "Successfully Updated User";
        }

        private void PopulateForm()
        {
            if (CloneDeployCurrentUser.IsLdapUser == 1)
            {
                chkldap.Checked = true;
                passwords.Visible = false;
                
            }
            txtEmail.Text = CloneDeployCurrentUser.Email;
            chkLockout.Checked = CloneDeployCurrentUser.NotifyLockout == 1;
            chkError.Checked = CloneDeployCurrentUser.NotifyError == 1;
            chkComplete.Checked = CloneDeployCurrentUser.NotifyComplete == 1;
            chkApproved.Checked = CloneDeployCurrentUser.NotifyImageApproved == 1;
        }
    }
}