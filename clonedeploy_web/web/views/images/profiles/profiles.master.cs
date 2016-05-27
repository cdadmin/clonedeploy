using System;
using BasePages;
using Models;

public partial class views_masters_Profile : MasterBaseMaster
{
    private Images imageBasePage { get; set; }
    public ImageProfile ImageProfile { get; set; }
    public Image Image { get; set; }

    public void Page_Load(object sender, EventArgs e)
    {
        imageBasePage = (Page as Images);
        ImageProfile = imageBasePage.ImageProfile;
        Image = imageBasePage.Image;

        if (ImageProfile == null)
        {
            osx_profile.Visible = false;
            linux_profile.Visible = false;
            return;
        }
        if (Image == null) Response.Redirect("~/", true);
        divProfiles.Visible = false;
        if (Image.Environment == "osx")
        {
            osx_profile.Visible = true;
            linux_profile.Visible = false;
        }
        else
        {
            osx_profile.Visible = false;
            linux_profile.Visible = true;
        }
    } 
}
