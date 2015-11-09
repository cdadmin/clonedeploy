using System;
using BasePages;

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
        var imageProfile = ImageProfile;
        imageProfile.Name = txtProfileName.Text;
        BLL.ImageProfile.UpdateProfile(imageProfile);
    }
}