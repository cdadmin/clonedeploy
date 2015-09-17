using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;

public partial class views_images_profiles_upload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkRemoveGpt.Checked = Convert.ToBoolean(Master.ImageProfile.RemoveGPT);
            chkUpNoShrink.Checked = Convert.ToBoolean(Master.ImageProfile.SkipShrinkVolumes);
            chkUpNoShrinkLVM.Checked = Convert.ToBoolean(Master.ImageProfile.SkipShrinkLvm);
            chkUpDebugResize.Checked = Convert.ToBoolean(Master.ImageProfile.DebugResize);


        }
    }

    protected void btnUpdateUpload_OnClick(object sender, EventArgs e)
    {
        var imageProfile = Master.ImageProfile;
        imageProfile.RemoveGPT = Convert.ToInt16(chkRemoveGpt.Checked);
        imageProfile.SkipShrinkVolumes = Convert.ToInt16(chkUpNoShrink.Checked);
        imageProfile.SkipShrinkLvm = Convert.ToInt16(chkUpNoShrinkLVM.Checked);
        imageProfile.DebugResize = Convert.ToInt16(chkUpDebugResize.Checked);
        imageProfile.Update();
        new Utility().Msgbox(Utility.Message);
    }
}