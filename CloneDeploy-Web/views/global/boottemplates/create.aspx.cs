using System;

public partial class views_global_boottemplates_create : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.CreateGlobal);
        var bootTemplate = new BootTemplate
        {
            Name = txtName.Text,
            Description = txtDescription.Text,
            Contents = txtContents.Text
        };

        var result = BLL.BootTemplate.AddBootTemplate(bootTemplate);
        if (!result.Success)
            EndUserMessage = result.Message;
        else
        {
            EndUserMessage = "Successfully Added Boot Menu Template";
            Response.Redirect("~/views/global/boottemplates/edit.aspx?cat=sub1&templateid=" + bootTemplate.Id);
        }
    }
}