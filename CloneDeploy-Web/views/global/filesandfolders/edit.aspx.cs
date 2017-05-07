using System;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_global_filesandfolders_edit : Global
{
    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        var fileFolder = FileFolder;
        fileFolder.Name = txtName.Text;
        fileFolder.Path = txtPath.Text;
        fileFolder.Type = ddlType.Text;


        var result = Call.FileFolderApi.Put(fileFolder.Id, fileFolder);
        EndUserMessage = !result.Success ? result.ErrorMessage : "Successfully Updated File / Folder";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        txtName.Text = FileFolder.Name;
        txtPath.Text = FileFolder.Path;
        ddlType.Text = FileFolder.Type;
    }
}