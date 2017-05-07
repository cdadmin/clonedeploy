using System;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

public partial class views_users_acls_acls : MasterBaseMaster
{
    public CloneDeployUserEntity CloneDeployUser { get; set; }
    private Users userBasePage { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        userBasePage = Page as Users;
        CloneDeployUser = userBasePage.CloneDeployUser;

        if (CloneDeployUser == null) Response.Redirect("~/", true);

        if (CloneDeployUser.Membership == "Administrator")
        {
            PageBaseMaster.EndUserMessage = "Administrators Do Not Use ACL's";
            Response.Redirect("~/views/users/edit.aspx?userid=" + CloneDeployUser.Id);
        }
    }
}