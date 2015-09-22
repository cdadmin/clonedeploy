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

public partial class views_images_profiles_create : System.Web.UI.Page
{
    private readonly Message _message = new Message();
    protected void Page_Load(object sender, EventArgs e)
    {
        _message.Show(Message.Text);   
    }

    protected void buttonCreateProfile_OnClick(object sender, EventArgs e)
    {
        var profile = new Models.LinuxProfile()
        {
            Name = txtProfileName.Text,
            Description = txtProfileDesc.Text,
            ImageId = Master.Image.Id
        };
        if (new BLL.LinuxProfile().AddProfile(profile))
            Response.Redirect("~/views/images/profiles/chooser.aspx?imageid=" + profile.ImageId + "&profileid=" + profile.Id + "&cat=profiles");


        _message.Show(Message.Text);
    }
}