using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BasePages;
using BLL;
using Helpers;
using Models;
using Security;
using Group = BLL.Group;

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
            if (txtUserPwd.Text != txtUserPwdConfirm.Text)
            {
                EndUserMessage = "Passwords Did Not Match";
                return;
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
                NotifyImageApproved = chkApproved.Checked ? 1 : 0

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
    }
}