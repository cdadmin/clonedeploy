using System;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_global_boottemplates_edit : Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        txtName.Text = base.BootTemplate.Name;
        txtDescription.Text = base.BootTemplate.Description;
        txtContents.Text = base.BootTemplate.Contents;
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        var bootTemplate = new BootTemplateEntity()
        {
            Id = BootTemplate.Id,
            Name = txtName.Text,
            Description = txtDescription.Text,
            Contents = txtContents.Text
        };

        var result = Call.BootTemplateApi.Put(bootTemplate.Id,bootTemplate);
        EndUserMessage = !result.Success ? result.ErrorMessage : "Successfully Updated Boot Menu Template";
       
    }
}