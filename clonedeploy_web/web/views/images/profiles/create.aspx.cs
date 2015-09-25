using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Global;
using Models;
using Image = Models.Image;

public partial class views_images_profiles_create : BasePages.Images
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void buttonCreateProfile_OnClick(object sender, EventArgs e)
    {
        var profile = new Models.LinuxProfile()
        {
            Name = txtProfileName.Text,
            Description = txtProfileDesc.Text,
            ImageId = Image.Id
        };
        if (BllLinuxProfile.AddProfile(profile))
            Response.Redirect("~/views/images/profiles/chooser.aspx?imageid=" + profile.ImageId + "&profileid=" + profile.Id + "&cat=profiles");

    }
}