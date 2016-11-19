using System;
using BasePages;
using Helpers;

public partial class views_images_profiles_task : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkWebCancel.Checked = Convert.ToBoolean(ImageProfile.WebCancel);
            ddlTaskComplete.Text = ImageProfile.TaskCompletedAction;           
        }
    }

    protected void btnUpdateTask_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorizationOrManagedImage(Authorizations.UpdateProfile, Image.Id);
        var imageProfile = ImageProfile;
        imageProfile.WebCancel = Convert.ToInt16(chkWebCancel.Checked);
        imageProfile.TaskCompletedAction = ddlTaskComplete.Text;
        var result = BLL.ImageProfile.UpdateProfile(imageProfile);
        EndUserMessage = result.Success ? "Successfully Updated Image Profile" : result.Message;
    }
}