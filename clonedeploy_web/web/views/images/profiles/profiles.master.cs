using System;
using BasePages;
using Models;

public partial class views_masters_Profile : MasterBaseMaster
{
    private Images imageBasePage { get; set; }
    public LinuxProfile ImageProfile { get; set; }
    public Image Image { get; set; }

    public void Page_Load(object sender, EventArgs e)
    {
        imageBasePage = (Page as Images);
        ImageProfile = imageBasePage.ImageProfile;
        Image = imageBasePage.Image;

        if (ImageProfile == null) return;
        if (Image == null) Response.Redirect("~/", true);
    } 
}
