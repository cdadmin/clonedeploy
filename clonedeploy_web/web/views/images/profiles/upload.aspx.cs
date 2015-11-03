using System;
using BasePages;

public partial class views_images_profiles_upload : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkRemoveGpt.Checked = Convert.ToBoolean(ImageProfile.RemoveGPT);
            chkUpNoShrink.Checked = Convert.ToBoolean(ImageProfile.SkipShrinkVolumes);
            chkUpNoShrinkLVM.Checked = Convert.ToBoolean(ImageProfile.SkipShrinkLvm);
          


        }
    }

    protected void btnUpdateUpload_OnClick(object sender, EventArgs e)
    {
        var imageProfile = ImageProfile;
        imageProfile.RemoveGPT = Convert.ToInt16(chkRemoveGpt.Checked);
        imageProfile.SkipShrinkVolumes = Convert.ToInt16(chkUpNoShrink.Checked);
        imageProfile.SkipShrinkLvm = Convert.ToInt16(chkUpNoShrinkLVM.Checked);
       
        BLL.LinuxProfile.UpdateProfile(imageProfile);
    }
}