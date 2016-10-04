using System;
using Helpers;

public partial class views_global_boottemplates_createentry : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.CreateGlobal);
        var bootEntry = new Models.BootEntry
        {
            Name = txtName.Text,
            Description = txtDescription.Text,
            Content = txtContents.Text,
            Type = ddlType.Text,
            Order = txtOrder.Text,
            Active = chkActive.Checked ? 1 : 0,
            Default = chkDefault.Checked ? 1 : 0
        };

      
        var result = BLL.BootEntry.AddBootEntry(bootEntry);
        if (!result.IsValid)
            EndUserMessage = result.Message;
        else
        {
            EndUserMessage = "Successfully Added Boot Menu Entry";
            Response.Redirect("~/views/global/boottemplates/editentry.aspx?cat=sub1&entryid=" + bootEntry.Id);
        }
    }
}