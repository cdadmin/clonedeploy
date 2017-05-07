using System;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

public partial class views_users_groupacls_groupacls : MasterBaseMaster
{
    public CloneDeployUserGroupEntity CloneDeployUserGroup { get; set; }
    private Users userBasePage { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        userBasePage = Page as Users;
        CloneDeployUserGroup = userBasePage.CloneDeployUserGroup;

        if (CloneDeployUserGroup == null) Response.Redirect("~/", true);

        if (CloneDeployUserGroup.Membership == "Administrator")
        {
            PageBaseMaster.EndUserMessage = "Administrators Do Not Use ACL's";
            Response.Redirect("~/views/users/editgroup.aspx?groupid=" + CloneDeployUserGroup.Id);
        }
    }
}