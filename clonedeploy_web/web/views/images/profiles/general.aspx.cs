using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Global;
using Models;

public partial class views_images_profiles_general : BasePages.Images
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtProfileName.Text = ImageProfile.Name;
            txtProfileDesc.Text = ImageProfile.Description;
        }

    }

    protected void buttonUpdateGeneral_OnClick(object sender, EventArgs e)
    {
        var imageProfile = ImageProfile;
        imageProfile.Name = txtProfileName.Text;
        BllLinuxProfile.UpdateProfile(imageProfile);
    }
}