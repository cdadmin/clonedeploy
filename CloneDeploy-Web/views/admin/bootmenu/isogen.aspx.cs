using System;
using System.Diagnostics.Eventing.Reader;
using System.IO;
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
       
        var isoGenOptions = new IsoGenOptionsDTO();
        isoGenOptions.bootImage = ddlBootImage.Text;
        isoGenOptions.buildType = ddlBuildType.Text;
        isoGenOptions.kernel = ddlKernel.Text;
        isoGenOptions.arguments = txtKernelArgs.Text;


        var clientboot = Call.WorkflowApi.GenerateLinuxIsoConfig(isoGenOptions);


        Response.Clear();
        MemoryStream ms = new MemoryStream(clientboot);
        if (ddlBuildType.Text == "ISO")
        {
            Response.ContentType = "application/iso";
            Response.AddHeader("content-disposition", "attachment;filename=clientboot.iso");
        }
        else
        {
            Response.ContentType = "application/zip";
            Response.AddHeader("content-disposition", "attachment;filename=clientboot.zip");
        }


        Response.Buffer = true;
        ms.WriteTo(Response.OutputStream);
        Response.End();


    }
}

