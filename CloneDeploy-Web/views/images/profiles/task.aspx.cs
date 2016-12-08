using System;
using BasePages;
using CloneDeploy_Web;

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
        var result = Call.ImageProfileApi.Put(imageProfile.Id,imageProfile);
        EndUserMessage = result.Success ? "Successfully Updated Image Profile" : result.ErrorMessage;
    }
}