using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class views_global_filesandfolders_edit : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) PopulateForm();
    }

    protected void txtType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        displayCheckBox.Visible = ddlType.Text == "Folder";
    }

    protected void PopulateForm()
    {
        txtName.Text = base.FileFolder.Name;
        txtPath.Text = base.FileFolder.Path;
        ddlType.Text = base.FileFolder.Type;
        chkContentsOnly.Checked = Convert.ToBoolean(base.FileFolder.ContentsOnly);
        displayCheckBox.Visible = ddlType.Text == "Folder";
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        var fileFolder = FileFolder;
        fileFolder.Name = txtName.Text;
        fileFolder.Path = txtPath.Text;
        fileFolder.Type = ddlType.Text;
        fileFolder.ContentsOnly = Convert.ToInt16(chkContentsOnly.Checked);
      

        var result = BLL.FileFolder.UpdateFileFolder(FileFolder);
        EndUserMessage = !result.IsValid ? result.Message : "Successfully Updated File / Folder";

    }
}