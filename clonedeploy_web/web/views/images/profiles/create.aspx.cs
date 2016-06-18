using System;
using BasePages;
using Helpers;

public partial class views_images_profiles_create : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void buttonCreateProfile_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.CreateProfile);
        var defaultProfile = BLL.ImageProfile.SeedDefaultImageProfile(Image);
        defaultProfile.ImageId = Image.Id;
        defaultProfile.Name = txtProfileName.Text;
        defaultProfile.Description = txtProfileDesc.Text;
        var result = BLL.ImageProfile.AddProfile(defaultProfile);
        if (result.IsValid)
        {
            EndUserMessage = "Successfully Created Image Profile";
            Response.Redirect("~/views/images/profiles/general.aspx?imageid=" + defaultProfile.ImageId + "&profileid=" +
                              defaultProfile.Id + "&cat=profiles");
        }
        else
        {
            EndUserMessage = result.Message;
        }
    }
}