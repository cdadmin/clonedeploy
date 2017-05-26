using System;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.images.profiles
{
    public partial class views_images_profiles_general : Images
    {
        protected void buttonUpdateGeneral_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedImage(AuthorizationStrings.UpdateProfile, Image.Id);
            var imageProfile = ImageProfile;
            imageProfile.Name = txtProfileName.Text;
            imageProfile.Description = txtProfileDesc.Text;
            var result = Call.ImageProfileApi.Put(imageProfile.Id, imageProfile);
            EndUserMessage = result.Success ? "Successfully Updated Image Profile" : result.ErrorMessage;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtProfileName.Text = ImageProfile.Name;
                txtProfileDesc.Text = ImageProfile.Description;
            }
        }
    }
}