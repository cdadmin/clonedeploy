using System;
using Helpers;

public partial class views_global_filesandfolders_edit : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void PopulateForm()
    {
        txtName.Text = base.FileFolder.Name;
        txtPath.Text = base.FileFolder.Path;
        ddlType.Text = base.FileFolder.Type;
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        var fileFolder = FileFolder;
        fileFolder.Name = txtName.Text;
        fileFolder.Path = txtPath.Text;
        fileFolder.Type = ddlType.Text;

        
        var result = BLL.FileFolder.UpdateFileFolder(FileFolder);
        EndUserMessage = !result.IsValid ? result.Message : "Successfully Updated File / Folder";

    }
}