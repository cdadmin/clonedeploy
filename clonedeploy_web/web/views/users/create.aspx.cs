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
                Password = txtUserPwd.Text,
                Membership = ddluserMembership.Text,
                Salt = Helpers.Utility.CreateSalt(64)
            };

            var result = BLL.User.AddUser(user);
            if (!result.IsValid)
                EndUserMessage = result.Message;
            else
            {
                EndUserMessage = "Successfully Created User";
                Response.Redirect("~/views/users/edit.aspx?userid=" + user.Id);
            }
        }
    }
}