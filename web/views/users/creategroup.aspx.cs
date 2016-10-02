using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;
using Models;

public partial class views_users_creategroup : BasePages.Users
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.Administrator);
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        var userGroup = new CloneDeployUserGroup
        {
            Name = txtGroupName.Text,
            Membership = ddlGroupMembership.Text,
            IsLdapGroup = chkldap.Checked ? 1 : 0
        };
        if (chkldap.Checked)
            userGroup.GroupLdapName = txtLdapGroupName.Text;

        var result = BLL.UserGroup.AddUserGroup(userGroup);
        if (!result.IsValid)
            EndUserMessage = result.Message;
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