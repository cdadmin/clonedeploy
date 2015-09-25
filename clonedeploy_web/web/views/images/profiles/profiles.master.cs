using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Models;
using Image = Models.Image;

public partial class views_masters_Profile : BasePages.MasterBaseMaster
{
    private BasePages.Images imageBasePage { get; set; }
    public Models.LinuxProfile ImageProfile { get; set; }
    public Models.Image Image { get; set; }

    public void Page_Load(object sender, EventArgs e)
    {
        imageBasePage = (Page as BasePages.Images);
        ImageProfile = imageBasePage.ImageProfile;
        Image = imageBasePage.Image;

        if (ImageProfile == null) return;
        if (Image == null) Response.Redirect("~/", true);
    } 
}
