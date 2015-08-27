using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;

public partial class views_images_profiles_chooser : System.Web.UI.Page
{
    public LinuxEnvironmentProfile LinuxEnvironmentProfile { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        LinuxEnvironmentProfile = new LinuxEnvironmentProfile { Id = Convert.ToInt32(Request.QueryString["profileid"]) };
        LinuxEnvironmentProfile.Read();
    }
}