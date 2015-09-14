using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;

public partial class views_images_profiles_task : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkGlobalNoCore.Checked = Master.ImageProfile.
            ddlKernel.DataSource = Utility.GetKernels();
            ddlBootImage.DataSource = Utility.GetBootImages();
            ddlKernel.DataBind();
            ddlBootImage.DataBind();
            ddlKernel.Text = Master.ImageProfile.Kernel;
            ddlBootImage.Text = Master.ImageProfile.BootImage;
            txtKernelArgs.Text = Master.ImageProfile.KernelArguments;
        }
    }

    protected void btnUpdateTask_OnClick(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}