using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Models;

public partial class views_images_profiles_chooser : System.Web.UI.Page
{
    public Models.LinuxProfile LinuxEnvironmentProfile { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        LinuxEnvironmentProfile = new BLL.LinuxProfile().ReadProfile(Convert.ToInt32(Request.QueryString["profileid"]));
    }
}