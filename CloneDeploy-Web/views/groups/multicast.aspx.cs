using System;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_groups_multicast : Groups
{
    protected void Page_Load(object sender, EventArgs e)
    {
       

        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        PopulateImagesDdl(ddlGroupImage);
        ddlGroupImage.SelectedValue = Group.ImageId.ToString();
        PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlGroupImage.SelectedValue));
        ddlImageProfile.SelectedValue = Group.ImageProfileId.ToString();
    }

    protected void Submit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorizationOrManagedGroup(Authorizations.UpdateGroup, Group.Id); 
        var group = Group;

        group.ImageId = Convert.ToInt32(ddlGroupImage.SelectedValue);
        group.ImageProfileId = Convert.ToInt32(ddlGroupImage.SelectedValue) == -1
                ? -1 : Convert.ToInt32(ddlImageProfile.SelectedValue);


        var result = Call.GroupApi.Put(group.Id,group);
        EndUserMessage = !result.Success ? result.ErrorMessage : "Successfully Updated Group";
    }

    protected void ddlGroupImage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlGroupImage.SelectedValue));

    }
}