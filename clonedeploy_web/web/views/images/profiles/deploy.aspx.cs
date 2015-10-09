using System;
using BasePages;

public partial class views_images_profiles_deploy : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkDownNoExpand.Checked = Convert.ToBoolean(ImageProfile.SkipExpandVolumes);
            chkAlignBCD.Checked = Convert.ToBoolean(ImageProfile.FixBcd);
            chkRunFixBoot.Checked = Convert.ToBoolean(ImageProfile.FixBootloader);
        }

    }

    protected void btnUpdateDeploy_OnClick(object sender, EventArgs e)
    {
        var imageProfile = ImageProfile;
        imageProfile.SkipExpandVolumes = Convert.ToInt16(chkDownNoExpand.Checked);
        imageProfile.FixBcd = Convert.ToInt16(chkAlignBCD.Checked);
        imageProfile.FixBootloader = Convert.ToInt16(chkRunFixBoot.Checked);
        BLL.LinuxProfile.UpdateProfile(imageProfile);
    }
}