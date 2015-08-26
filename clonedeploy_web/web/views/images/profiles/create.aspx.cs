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
        Master.Master.Msgbox(Utility.Message);
        Master.Master.FindControl("SubNav").Visible = false;
        var image = new Image { Id = Convert.ToInt32(Request.QueryString["imageid"]) };
        image.Read();
        var subTitle = Master.Master.FindControl("SubNavDynamic").FindControl("labelSubTitle") as Label;
        if (subTitle != null) subTitle.Text = image.Name;
       
    }
    protected void buttonCreateProfile_Click(object sender, EventArgs e)
    {
        var profile = new LinuxEnvironmentProfile()
        {
            Name = txtProfileName.Text,
            Description = txtProfileDesc.Text,
            Kernel = ddlKernel.Text,
            BootImage = ddlBootImage.Text,
            ImageId = Convert.ToInt32(Request.QueryString["imageid"])
        };

        profile.Create();

        Master.Master.Msgbox(Utility.Message);
    }
}