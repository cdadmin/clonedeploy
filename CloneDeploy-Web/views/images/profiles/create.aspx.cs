using System;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_images_profiles_create : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void buttonCreateProfile_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.CreateProfile);
        var defaultProfile = Call.ImageApi.SeedDefaultProfile(Image.Id);
        defaultProfile.ImageId = Image.Id;
        defaultProfile.Name = txtProfileName.Text;
        defaultProfile.Description = txtProfileDesc.Text;
        var result = Call.ImageProfileApi.Post(defaultProfile);
        if (result.Success)
        {
            EndUserMessage = "Successfully Created Image Profile";
            Response.Redirect("~/views/images/profiles/general.aspx?imageid=" + defaultProfile.ImageId + "&profileid=" +
                              result.Id + "&cat=profiles");
        }
        else
        {
            EndUserMessage = result.ErrorMessage;
        }
    }
}