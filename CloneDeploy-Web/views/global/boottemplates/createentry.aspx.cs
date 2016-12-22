using System;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_global_boottemplates_createentry : Global
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.CreateGlobal);
        var bootEntry = new BootEntryEntity
        {
            Name = txtName.Text,
            Description = txtDescription.Text,
            Content = txtContents.Text,
            Type = ddlType.Text,
            Order = txtOrder.Text,
            Active = chkActive.Checked ? 1 : 0,
            Default = chkDefault.Checked ? 1 : 0
        };

      
        var result = Call.BootEntryApi.Post(bootEntry);
        if (!result.Success)
            EndUserMessage = result.ErrorMessage;
        else
        {
            EndUserMessage = "Successfully Added Boot Menu Entry";
            Response.Redirect("~/views/global/boottemplates/editentry.aspx?cat=sub1&entryid=" + result.Id);
        }
    }
}