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
        var profile = new LinuxProfile()
        {
            Name = txtProfileName.Text,
            Description = txtProfileDesc.Text,
            ImageId = Image.Id
        };
        BllLinuxProfile.AddProfile(profile);
            Response.Redirect("~/views/images/profiles/chooser.aspx?imageid=" + profile.ImageId + "&profileid=" + profile.Id + "&cat=profiles");

    }
}