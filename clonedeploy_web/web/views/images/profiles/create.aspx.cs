using System;
using BasePages;
using Models;

public partial class views_images_profiles_create : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void buttonCreateProfile_OnClick(object sender, EventArgs e)
    {
        var profile = new ImageProfile()
        {
            Name = txtProfileName.Text,
            Description = txtProfileDesc.Text,
            ImageId = Image.Id
        };
        var result = BLL.ImageProfile.AddProfile(profile);
        if (result.IsValid)
        {
            EndUserMessage = "Successfully Created Image Profile";
            Response.Redirect("~/views/images/profiles/general.aspx?imageid=" + profile.ImageId + "&profileid=" +
                              profile.Id + "&cat=profiles");
        }
        else
        {
            EndUserMessage = result.Message;
        }
    }
}