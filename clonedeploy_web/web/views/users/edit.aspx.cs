using System;
using BasePages;
using Helpers;

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

            var updatedUser = CloneDeployUser;
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
        
            updatedUser.Name = txtUserName.Text;
            updatedUser.Membership = ddluserMembership.Text;        
            updatedUser.Email = txtEmail.Text;
            updatedUser.Token = txtToken.Text;
            updatedUser.NotifyLockout = chkLockout.Checked ? 1 : 0;
            updatedUser.NotifyError = chkError.Checked ? 1 : 0;
            updatedUser.NotifyComplete = chkComplete.Checked ? 1 : 0;
            updatedUser.NotifyImageApproved = chkApproved.Checked ? 1 : 0;
           
            var result = BLL.User.UpdateUser(updatedUser);
            EndUserMessage = !result.IsValid ? result.Message : "Successfully Updated User";
            

        }

        protected void PopulateForm()
        {
            txtUserName.Text = CloneDeployUser.Name;
            ddluserMembership.Text = CloneDeployUser.Membership;
            txtEmail.Text = CloneDeployUser.Email;
            txtToken.Text = CloneDeployUser.Token;
            chkLockout.Checked = CloneDeployUser.NotifyLockout == 1;
            chkError.Checked = CloneDeployUser.NotifyError == 1;
            chkComplete.Checked = CloneDeployUser.NotifyComplete == 1;
            chkApproved.Checked = CloneDeployUser.NotifyImageApproved == 1;
        }


        protected void btnGenKey_OnClick(object sender, EventArgs e)
        {
            txtToken.Text = Utility.GenerateKey();
        }
    }
}