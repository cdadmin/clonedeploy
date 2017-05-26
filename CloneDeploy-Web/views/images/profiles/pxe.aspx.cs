using System;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.images.profiles
{
    public partial class views_images_profiles_pxe : Images
    {
        protected void buttonUpdatePXE_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedImage(AuthorizationStrings.UpdateProfile, Image.Id);
            var imageProfile = ImageProfile;
            imageProfile.Kernel = ddlKernel.Text;
            imageProfile.BootImage = ddlBootImage.Text;
            imageProfile.KernelArguments = txtKernelArgs.Text;
            var result = Call.ImageProfileApi.Put(imageProfile.Id, imageProfile);
            EndUserMessage = result.Success ? "Successfully Updated Image Profile" : result.ErrorMessage;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlKernel.DataSource = Call.FilesystemApi.GetKernels();
                ddlBootImage.DataSource = Call.FilesystemApi.GetBootImages();
                ddlKernel.DataBind();
                ddlBootImage.DataBind();
                ddlBootImage.SelectedValue = SettingStrings.DefaultInit;
                ddlKernel.Text = ImageProfile.Kernel;
                ddlBootImage.Text = ImageProfile.BootImage;
                txtKernelArgs.Text = ImageProfile.KernelArguments;
            }
        }
    }
}