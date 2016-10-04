using System;
using Helpers;

public partial class views_global_boottemplates_editentry : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        txtName.Text = base.BootEntry.Name;
        txtDescription.Text = base.BootEntry.Description;
        txtContents.Text = base.BootEntry.Content;
        ddlType.Text = BootEntry.Type;
        txtOrder.Text = BootEntry.Order;
        if (BootEntry.Active == 1) chkActive.Checked = true;
        if (BootEntry.Default == 1) chkDefault.Checked = true;
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        var bootEntry = new Models.BootEntry
        {
            Id = BootEntry.Id,
            Name = txtName.Text,
            Description = txtDescription.Text,
            Content = txtContents.Text,
            Type = ddlType.Text,
            Order = txtOrder.Text,
            Active = chkActive.Checked ? 1 : 0,
            Default = chkDefault.Checked ? 1 : 0
        };

        var result = BLL.BootEntry.UpdateBootEntry(bootEntry);
        EndUserMessage = !result.IsValid ? result.Message : "Successfully Updated Boot Menu Entry";

    }
}