using System;
using Helpers;

public partial class views_global_filesandfolders_create : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.CreateGlobal);
        var fileFolder = new Models.FileFolder
        {
            Name = txtName.Text,
            Path = txtPath.Text,
            Type = ddlType.Text
        };

        var result = BLL.FileFolder.AddFileFolder(fileFolder);
        if (!result.IsValid)
            EndUserMessage = result.Message;
        else
        {
            EndUserMessage = "Successfully Added File / Folder";
            Response.Redirect("~/views/global/filesandfolders/edit.aspx?cat=sub1&fileid=" + fileFolder.Id);
        }
    }
}