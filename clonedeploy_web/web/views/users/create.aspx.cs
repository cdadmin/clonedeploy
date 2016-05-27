using System;
using BasePages;
using Helpers;
using Models;

namespace views.users
{
    public partial class CreateUser : Users
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.Administrator);
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

            var user = new CloneDeployUser
            {
                Name = txtUserName.Text,
                Membership = ddluserMembership.Text,
                Salt = Helpers.Utility.CreateSalt(64),
                Email = txtEmail.Text,
                Token = txtToken.Text,
                NotifyLockout = chkLockout.Checked ? 1 : 0,
                NotifyError = chkError.Checked ? 1 : 0,
                NotifyComplete = chkComplete.Checked ? 1 : 0,
                NotifyImageApproved = chkApproved.Checked ? 1 : 0,
                IsLdapUser = chkldap.Checked ? 1: 0
            };

            user.Password = Helpers.Utility.CreatePasswordHash(txtUserPwd.Text, user.Salt);
            var result = BLL.User.AddUser(user);
            if (!result.IsValid)
                EndUserMessage = result.Message;
            else
            {
                EndUserMessage = "Successfully Created User";
                Response.Redirect("~/views/users/edit.aspx?userid=" + user.Id);
            }
        }

        protected void btnGenKey_OnClick(object sender, EventArgs e)
        {
            txtToken.Text = Utility.GenerateKey();
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
    }
}