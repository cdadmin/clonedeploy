using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using Models;
using Image = Models.Image;

public partial class views_masters_Profile : System.Web.UI.MasterPage
{
    public Image Image { get { return Master.Image; } }
    public LinuxProfile ImageProfile { get { return ReadProfile(); } }

    public void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["profileid"])) return;
        if (string.IsNullOrEmpty(Request["imageid"])) Response.Redirect("~/", true);
    }

    private LinuxProfile ReadProfile ()
    {
        var tmpProfile = new LinuxProfileDataAccess().Read(Convert.ToInt32(Request.QueryString["profileid"]));
        return tmpProfile;
    }
}
