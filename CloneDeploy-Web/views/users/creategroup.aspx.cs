using System;
using CloneDeploy_Entities;
using CloneDeploy_Web;

public partial class views_users_creategroup : BasePages.Users
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.Administrator);
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        var userGroup = new CloneDeployUserGroupEntity()
        {
            Name = txtGroupName.Text,
            Membership = ddlGroupMembership.Text,
            IsLdapGroup = chkldap.Checked ? 1 : 0
        };
        if (chkldap.Checked)
            userGroup.GroupLdapName = txtLdapGroupName.Text;

        var result = Call.UserGroupApi.Post(userGroup);
        if (!result.Success)
            EndUserMessage = result.ErrorMessage;
        else
        {
            EndUserMessage = "Successfully Created User Group";
            Response.Redirect("~/views/users/editgroup.aspx?groupid=" + userGroup.Id);
        }
    }

    protected void chkldap_OnCheckedChanged(object sender, EventArgs e)
    {
        divldapgroup.Visible = chkldap.Checked;
    }
}