using System;
using BasePages;
using CloneDeploy_Entities;

public partial class views_users_groupacls_groupacls : BasePages.MasterBaseMaster
{
    private BasePages.Users userBasePage { get; set; }
    public CloneDeployUserGroupEntity CloneDeployUserGroup { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        userBasePage = (Page as BasePages.Users);
        CloneDeployUserGroup = userBasePage.CloneDeployUserGroup;

        if (CloneDeployUserGroup == null) Response.Redirect("~/", true);

        if (CloneDeployUserGroup.Membership == "Administrator")
        {
            PageBaseMaster.EndUserMessage = "Administrators Do Not Use ACL's";
            Response.Redirect("~/views/users/editgroup.aspx?groupid=" + CloneDeployUserGroup.Id);
        }
    }
}
