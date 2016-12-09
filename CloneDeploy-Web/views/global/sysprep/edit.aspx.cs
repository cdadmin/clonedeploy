using System;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_global_sysprep_edit : Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        var sysprepTag = new SysprepTagEntity()
        {
            Id = SysprepTag.Id,
            Name = txtName.Text,
            OpeningTag = txtOpenTag.Text,
            ClosingTag = txtCloseTag.Text,
            Description = txtSysprepDesc.Text,
            Contents = txtContent.Text        
        };
      
        var result = Call.SysprepTagApi.Put(sysprepTag.Id,sysprepTag);
        EndUserMessage = result.Success ? "Successfully Updated Sysprep Tag" : result.ErrorMessage;
    }

    protected void PopulateForm()
    {
        txtName.Text = SysprepTag.Name;
        txtOpenTag.Text = SysprepTag.OpeningTag;
        txtCloseTag.Text = SysprepTag.ClosingTag;
        txtSysprepDesc.Text = SysprepTag.Description;
        txtContent.Text = SysprepTag.Contents;
    }
}