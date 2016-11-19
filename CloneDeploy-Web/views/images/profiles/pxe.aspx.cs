using System;
using BasePages;
using Helpers;

public partial class views_images_profiles_pxe : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlKernel.DataSource = Utility.GetKernels();
            ddlBootImage.DataSource = Utility.GetBootImages();       
            ddlKernel.DataBind();
            ddlBootImage.DataBind();
            ddlBootImage.SelectedValue = Settings.DefaultInit;
            ddlKernel.Text = ImageProfile.Kernel;
            ddlBootImage.Text = ImageProfile.BootImage;
            txtKernelArgs.Text = ImageProfile.KernelArguments;
        }
    }

    protected void buttonUpdatePXE_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorizationOrManagedImage(Authorizations.UpdateProfile, Image.Id);
        var imageProfile = ImageProfile;
        imageProfile.Kernel = ddlKernel.Text;
        imageProfile.BootImage = ddlBootImage.Text;
        imageProfile.KernelArguments = txtKernelArgs.Text;
        var result = BLL.ImageProfile.UpdateProfile(imageProfile);
        EndUserMessage = result.Success ? "Successfully Updated Image Profile" : result.Message;
    }
}