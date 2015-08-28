using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;

public partial class views_images_profiles_general : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        txtProfileName.Text = Master.LinuxEnvironmentProfile.Name;
    }

    protected void buttonUpdateGeneral_OnClick(object sender, EventArgs e)
    {
        Master.LinuxEnvironmentProfile.Name = txtProfileName.Text;
        Master.LinuxEnvironmentProfile.Update();
        new Utility().Msgbox(Utility.Message);
    }
}