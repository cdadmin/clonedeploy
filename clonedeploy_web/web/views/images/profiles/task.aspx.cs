using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;

public partial class views_images_profiles_task : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkGlobalNoCore.Checked = Convert.ToBoolean(Master.ImageProfile.SkipCore);
            chkGlobalNoClock.Checked = Convert.ToBoolean(Master.ImageProfile.SkipClock);
            ddlTaskComplete.Text = Master.ImageProfile.TaskCompletedAction;

        }
    }

    protected void btnUpdateTask_OnClick(object sender, EventArgs e)
    {
        var imageProfile = Master.ImageProfile;
        imageProfile.SkipCore = Convert.ToInt16(chkGlobalNoCore.Checked);
        imageProfile.SkipClock = Convert.ToInt16(chkGlobalNoClock.Checked);
        imageProfile.TaskCompletedAction = ddlTaskComplete.Text;
        imageProfile.Update();
        new Utility().Msgbox(Utility.Message);
    }
}