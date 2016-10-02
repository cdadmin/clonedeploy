using System;
using BasePages;
using Helpers;

public partial class views_images_profiles_general : Images
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtProfileName.Text = ImageProfile.Name;
            txtProfileDesc.Text = ImageProfile.Description;
        }

    }

    protected void buttonUpdateGeneral_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorizationOrManagedImage(Authorizations.UpdateProfile, Image.Id);
        var imageProfile = ImageProfile;
        imageProfile.Name = txtProfileName.Text;
        imageProfile.Description = txtProfileDesc.Text;
        var result = BLL.ImageProfile.UpdateProfile(imageProfile);
        EndUserMessage = result.IsValid ? "Successfully Updated Image Profile" : result.Message;
    }
}