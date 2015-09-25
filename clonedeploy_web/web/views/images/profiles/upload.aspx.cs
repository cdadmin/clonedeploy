using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Global;

public partial class views_images_profiles_upload : BasePages.Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkRemoveGpt.Checked = Convert.ToBoolean(ImageProfile.RemoveGPT);
            chkUpNoShrink.Checked = Convert.ToBoolean(ImageProfile.SkipShrinkVolumes);
            chkUpNoShrinkLVM.Checked = Convert.ToBoolean(ImageProfile.SkipShrinkLvm);
            chkUpDebugResize.Checked = Convert.ToBoolean(ImageProfile.DebugResize);


        }
    }

    protected void btnUpdateUpload_OnClick(object sender, EventArgs e)
    {
        var imageProfile = ImageProfile;
        imageProfile.RemoveGPT = Convert.ToInt16(chkRemoveGpt.Checked);
        imageProfile.SkipShrinkVolumes = Convert.ToInt16(chkUpNoShrink.Checked);
        imageProfile.SkipShrinkLvm = Convert.ToInt16(chkUpNoShrinkLVM.Checked);
        imageProfile.DebugResize = Convert.ToInt16(chkUpDebugResize.Checked);
        BllLinuxProfile.UpdateProfile(imageProfile);
    }
}