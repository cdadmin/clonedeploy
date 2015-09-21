using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using Global;

public partial class views_images_profiles_pxe : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlKernel.DataSource = Utility.GetKernels();
            ddlBootImage.DataSource = Utility.GetBootImages();
            ddlKernel.DataBind();
            ddlBootImage.DataBind();
            ddlKernel.Text = Master.ImageProfile.Kernel;
            ddlBootImage.Text = Master.ImageProfile.BootImage;
            txtKernelArgs.Text = Master.ImageProfile.KernelArguments;
        }
    }

    protected void buttonUpdatePXE_OnClick(object sender, EventArgs e)
    {
        var imageProfile = Master.ImageProfile;
        imageProfile.Kernel = ddlKernel.Text;
        imageProfile.BootImage = ddlBootImage.Text;
        imageProfile.KernelArguments = txtKernelArgs.Text;
        new LinuxProfileDataAccess().Update(imageProfile);
        new Utility().Msgbox(Utility.Message);
    }
}