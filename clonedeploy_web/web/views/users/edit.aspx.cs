using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BasePages;
using BLL;
using Helpers;
using Models;
using Group = BLL.Group;

namespace views.users
{
    public partial class EditUser : Users
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.Administrator);
            if (!IsPostBack) PopulateForm();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            if (BLL.User.GetAdminCount() == 1 && ddluserMembership.Text != "Administrator" &&
                CloneDeployUser.Membership == "Administrator")
            {
                EndUserMessage = "There Must Be At Least One Administrator";
                return;
            }

            bool updatePassword = false;
            if (!string.IsNullOrEmpty(txtUserPwd.Text) && !string.IsNullOrEmpty(txtUserPwdConfirm.Text))
            {
                updatePassword = true;
                if (txtUserPwd.Text != txtUserPwdConfirm.Text)
                {
                    EndUserMessage = "Passwords Did Not Match";
                    return;
                }
            }
            var updatedUser = CloneDeployUser;
            updatedUser.Name = txtUserName.Text;
            updatedUser.Membership = ddluserMembership.Text;
            updatedUser.Password = txtUserPwd.Text;
            updatedUser.Salt = BLL.User.CreateSalt(16);

            var result = BLL.User.UpdateUser(updatedUser, updatePassword);
            EndUserMessage = !result.IsValid ? result.Message : "Successfully Updated User";
            

        }







        protected void PopulateForm()
        {
         
          
           
            txtUserName.Text = CloneDeployUser.Name;
            ddluserMembership.Text = CloneDeployUser.Membership;
           
        }

     
    }
}