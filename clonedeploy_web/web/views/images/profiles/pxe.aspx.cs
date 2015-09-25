using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Global;
using Helpers;

public partial class views_images_profiles_pxe : BasePages.Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlKernel.DataSource = Utility.GetKernels();
            ddlBootImage.DataSource = Utility.GetBootImages();
            ddlKernel.DataBind();
            ddlBootImage.DataBind();
            ddlKernel.Text = ImageProfile.Kernel;
            ddlBootImage.Text = ImageProfile.BootImage;
            txtKernelArgs.Text = ImageProfile.KernelArguments;
        }
    }

    protected void buttonUpdatePXE_OnClick(object sender, EventArgs e)
    {
        var imageProfile = ImageProfile;
        imageProfile.Kernel = ddlKernel.Text;
        imageProfile.BootImage = ddlBootImage.Text;
        imageProfile.KernelArguments = txtKernelArgs.Text;
        BllLinuxProfile.UpdateProfile(imageProfile);
    }
}