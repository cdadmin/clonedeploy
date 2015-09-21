using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using Global;
using Logic;
using Models;

public partial class views_images_profiles_general : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtProfileName.Text = Master.ImageProfile.Name;
            txtProfileDesc.Text = Master.ImageProfile.Description;
        }

    }

    protected void buttonUpdateGeneral_OnClick(object sender, EventArgs e)
    {
        var imageProfile = Master.ImageProfile;
        imageProfile.Name = txtProfileName.Text;
        new LinuxProfileLogic().UpdateProfile(imageProfile);
        new Utility().Msgbox(Utility.Message);
    }
}