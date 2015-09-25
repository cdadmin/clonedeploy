using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Global;

public partial class views_images_profiles_task : BasePages.Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkGlobalNoCore.Checked = Convert.ToBoolean(ImageProfile.SkipCore);
            chkGlobalNoClock.Checked = Convert.ToBoolean(ImageProfile.SkipClock);
            ddlTaskComplete.Text = ImageProfile.TaskCompletedAction;

        }
    }

    protected void btnUpdateTask_OnClick(object sender, EventArgs e)
    {
        var imageProfile = ImageProfile;
        imageProfile.SkipCore = Convert.ToInt16(chkGlobalNoCore.Checked);
        imageProfile.SkipClock = Convert.ToInt16(chkGlobalNoClock.Checked);
        imageProfile.TaskCompletedAction = ddlTaskComplete.Text;
        BllLinuxProfile.UpdateProfile(imageProfile);
    }
}