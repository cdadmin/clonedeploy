using System;
using BasePages;
using Helpers;

public partial class views_images_profiles_task : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkGlobalNoCore.Checked = Convert.ToBoolean(ImageProfile.SkipCore);
            chkGlobalNoClock.Checked = Convert.ToBoolean(ImageProfile.SkipClock);
            chkWebCancel.Checked = Convert.ToBoolean(ImageProfile.WebCancel);
            ddlTaskComplete.Text = ImageProfile.TaskCompletedAction;
            if (Image.Environment == "macOS") nolinuxhide.Visible = false;
        }
    }

    protected void btnUpdateTask_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorizationOrManagedImage(Authorizations.UpdateProfile, Image.Id);
        var imageProfile = ImageProfile;
        imageProfile.SkipCore = Convert.ToInt16(chkGlobalNoCore.Checked);
        imageProfile.SkipClock = Convert.ToInt16(chkGlobalNoClock.Checked);
        imageProfile.WebCancel = Convert.ToInt16(chkWebCancel.Checked);
        imageProfile.TaskCompletedAction = ddlTaskComplete.Text;
        var result = BLL.ImageProfile.UpdateProfile(imageProfile);
        EndUserMessage = result.IsValid ? "Successfully Updated Image Profile" : result.Message;
    }
}