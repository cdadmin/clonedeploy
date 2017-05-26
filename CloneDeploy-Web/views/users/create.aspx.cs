using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.users
{
    public partial class CreateUser : Users
    {
        protected void btnGenKey_OnClick(object sender, EventArgs e)
        {
            txtToken.Text = Utility.GenerateKey();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!chkldap.Checked)
            {
                if (txtUserPwd.Text != txtUserPwdConfirm.Text)
                {
                    EndUserMessage = "Passwords Did Not Match";
                    return;
                }

                if (string.IsNullOrEmpty(txtUserPwd.Text))
                {
                    EndUserMessage = "Passwords Cannot Be Empty";
                    return;
                }
            }
            else
            {
                //Create a local random db pass, should never actually be possible to use.
                txtUserPwd.Text = new Guid().ToString();
                txtUserPwdConfirm.Text = txtUserPwd.Text;
            }

            var user = new CloneDeployUserEntity
            {
                Name = txtUserName.Text,
                Membership = ddluserMembership.Text,
                Salt = Utility.CreateSalt(64),
                Email = txtEmail.Text,
                Token = txtToken.Text,
                NotifyLockout = chkLockout.Checked ? 1 : 0,
                NotifyError = chkError.Checked ? 1 : 0,
                NotifyComplete = chkComplete.Checked ? 1 : 0,
                NotifyImageApproved = chkApproved.Checked ? 1 : 0,
                IsLdapUser = chkldap.Checked ? 1 : 0,
                UserGroupId = -1
            };

            user.Password = Utility.CreatePasswordHash(txtUserPwd.Text, user.Salt);
            var result = Call.CloneDeployUserApi.Post(user);
            if (!result.Success)
                EndUserMessage = result.ErrorMessage;
            else
            {
                EndUserMessage = "Successfully Created User";
                Response.Redirect("~/views/users/edit.aspx?userid=" + result.Id);
            }
        }

        protected void chkldap_OnCheckedChanged(object sender, EventArgs e)
        {
            if (chkldap.Checked)
                passwords.Visible = false;
            else
            {
                passwords.Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.Administrator);
            txtToken.Text = Utility.GenerateKey();
        }
    }
}