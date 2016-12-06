using System;
using BasePages;

public partial class views_images_profiles_Default : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtSender.Text = ImageProfile.SenderArguments;
            txtReceiver.Text = ImageProfile.ReceiverArguments;
        }
    }

    protected void buttonUpdateMulticast_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorizationOrManagedImage(Authorizations.UpdateProfile, Image.Id);
        ImageProfile.SenderArguments = txtSender.Text;
        ImageProfile.ReceiverArguments = txtReceiver.Text;
        var result = BLL.ImageProfile.UpdateProfile(ImageProfile);
        EndUserMessage = result.Success ? "Successfully Updated Image Profile" : result.Message;
    }
}