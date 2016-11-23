using System;
using BasePages;
using CloneDeploy_Web.Models;

public partial class views_users_acls_acls : BasePages.MasterBaseMaster
{
    private BasePages.Users userBasePage { get; set; }
    public CloneDeployUser CloneDeployUser { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        userBasePage = (Page as BasePages.Users);
        CloneDeployUser = userBasePage.CloneDeployUser;
       
        if (CloneDeployUser == null) Response.Redirect("~/", true);

        if (CloneDeployUser.Membership == "Administrator")
        {
            PageBaseMaster.EndUserMessage = "Administrators Do Not Use ACL's";
            Response.Redirect("~/views/users/edit.aspx?userid=" + CloneDeployUser.Id);
        }
    }
}
