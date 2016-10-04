using System;
using Helpers;

public partial class views_users_editgroup : BasePages.Users
{
   protected void Page_Load(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.Administrator);
            if (!IsPostBack) PopulateForm();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            CloneDeployUserGroup.Name = txtGroupName.Text;
            if (chkldap.Checked)
                CloneDeployUserGroup.GroupLdapName = txtLdapGroupName.Text;
          
            var result = BLL.UserGroup.UpdateUser(CloneDeployUserGroup);
            EndUserMessage = !result.IsValid ? result.Message : "Successfully Updated User Group";
            

        }

        protected void PopulateForm()
        {
            chkldap.Enabled = false;
            if (CloneDeployUserGroup.IsLdapGroup == 1)
            {
                chkldap.Checked = true;
                divldapgroup.Visible = true;
                txtLdapGroupName.Text = CloneDeployUserGroup.GroupLdapName;
            }
            ddlGroupMembership.Enabled = false;
            txtGroupName.Text = CloneDeployUserGroup.Name;
            ddlGroupMembership.Text = CloneDeployUserGroup.Membership;    
        }
    }
