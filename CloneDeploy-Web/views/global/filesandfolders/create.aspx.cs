using System;
using CloneDeploy_Entities;
using CloneDeploy_Web;

public partial class views_global_filesandfolders_create : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.CreateGlobal);
        var fileFolder = new FileFolderEntity()
        {
            Name = txtName.Text,
            Path = txtPath.Text,
            Type = ddlType.Text
        };

        var result = Call.FileFolderApi.Post(fileFolder);
        if (!result.Success)
            EndUserMessage = result.ErrorMessage;
        else
        {
            EndUserMessage = "Successfully Added File / Folder";
            Response.Redirect("~/views/global/filesandfolders/edit.aspx?cat=sub1&fileid=" + fileFolder.Id);
        }
    }
}