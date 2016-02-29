using System;
using Helpers;

public partial class views_global_boottemplates_edit : BasePages.Global
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
        var bootTemplate = new Models.BootTemplate
        {
            Id = BootTemplate.Id,
            Name = txtName.Text,
            Description = txtDescription.Text,
            Contents = txtContents.Text
        };

        var result = BLL.BootTemplate.UpdateBootTemplate(bootTemplate);
        EndUserMessage = !result.IsValid ? result.Message : "Successfully Updated Boot Menu Template";
       
    }
}