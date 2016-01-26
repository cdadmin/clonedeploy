using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_groups_multicast : BasePages.Groups
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.CreateGroup);

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
        var group = Group;

        group.ImageId = Convert.ToInt32(ddlGroupImage.SelectedValue);
        group.ImageProfileId = Convert.ToInt32(ddlGroupImage.SelectedValue) == -1
                ? -1 : Convert.ToInt32(ddlImageProfile.SelectedValue);


        var result = BLL.Group.UpdateGroup(group);
        EndUserMessage = !result.IsValid ? result.Message : "Successfully Updated Group";
    }

    protected void ddlGroupImage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateImageProfilesDdl(ddlImageProfile, Convert.ToInt32(ddlGroupImage.SelectedValue));

    }
}