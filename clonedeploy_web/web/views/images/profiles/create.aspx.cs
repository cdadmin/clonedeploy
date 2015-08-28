using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;
using Image = Models.Image;

public partial class views_images_profiles_create : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new Utility().Msgbox(Utility.Message);   
    }

    protected void buttonCreateProfile_OnClick(object sender, EventArgs e)
    {
        var profile = new LinuxEnvironmentProfile()
        {
            Name = txtProfileName.Text,
            Description = txtProfileDesc.Text,
            ImageId = Master.Image.Id
        };
        if (profile.Create())
            Response.Redirect("~/views/images/profiles/chooser.aspx?imageid=" + profile.ImageId + "&profileid=" + profile.Id + "&cat=profiles");


        new Utility().Msgbox(Utility.Message);
    }
}