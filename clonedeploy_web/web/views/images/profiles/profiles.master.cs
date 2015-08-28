using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;
using Image = Models.Image;

public partial class views_masters_Profile : System.Web.UI.MasterPage
{
    public Image Image { get { return Master.Image; } }
    public LinuxEnvironmentProfile ImageProfile { get { return ReadProfile(); } }

    public void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["profileid"])) return;
        if (string.IsNullOrEmpty(Request["imageid"])) Response.Redirect("~/", true);
    }

    private LinuxEnvironmentProfile ReadProfile ()
    {
        var tmpProfile = new LinuxEnvironmentProfile{Id = Convert.ToInt32(Request.QueryString["profileid"])};
        tmpProfile.Read();
        return tmpProfile;
    }
}
