using System;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_users_editgroup : Users
{
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        CloneDeployUserGroup.Name = txtGroupName.Text;
        if (chkldap.Checked)
            CloneDeployUserGroup.GroupLdapName = txtLdapGroupName.Text;

        var result = Call.UserGroupApi.Put(CloneDeployUserGroup.Id, CloneDeployUserGroup);
        EndUserMessage = !result.Success ? result.ErrorMessage : "Successfully Updated User Group";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.Administrator);
        if (!IsPostBack) PopulateForm();
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