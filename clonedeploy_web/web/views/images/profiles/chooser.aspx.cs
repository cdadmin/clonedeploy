using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using Logic;
using Models;

public partial class views_images_profiles_chooser : System.Web.UI.Page
{
    public LinuxProfile LinuxEnvironmentProfile { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        LinuxEnvironmentProfile = new LinuxProfileLogic().ReadProfile(Convert.ToInt32(Request.QueryString["profileid"]));
    }
}