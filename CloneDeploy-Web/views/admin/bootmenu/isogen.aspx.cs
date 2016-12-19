using System;
using System.Web;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_admin_bootmenu_isogen : Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlKernel.DataSource = Call.FilesystemApi.GetKernels();      
            ddlBootImage.DataSource = Call.FilesystemApi.GetBootImages();           
            ddlKernel.DataBind();
            ddlBootImage.DataBind();
            ddlKernel.SelectedValue = Settings.DefaultKernel64;
            ddlBootImage.SelectedValue = Settings.DefaultInit;
        }
    }
    protected void btnGenerate_OnClick(object sender, EventArgs e)
    {
        var output = Call.FilesystemApi.GetServerPaths("iso","");

        var isoGenOptions = new IsoGenOptionsDTO();
        isoGenOptions.bootImage = ddlBootImage.Text;
        isoGenOptions.buildType = ddlBuildType.Text;
        isoGenOptions.kernel = ddlKernel.Text;
        isoGenOptions.arguments = txtKernelArgs.Text;

        if (
            Call.WorkflowApi.GenerateLinuxIsoConfig(isoGenOptions))
            EndUserMessage = "Complete.  Output Located At " + Utility.EscapeFilePaths(output);
        else
        {
            EndUserMessage = "Could Not Create Client ISO Files";
        }
    }
}

