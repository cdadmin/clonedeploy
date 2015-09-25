using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Global;

public partial class views_images_profiles_deploy : BasePages.Images
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
        BllLinuxProfile.UpdateProfile(imageProfile);
    }
}