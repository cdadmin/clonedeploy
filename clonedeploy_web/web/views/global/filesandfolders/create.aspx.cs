using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class views_global_filesandfolders_create : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void txtType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        displayCheckBox.Visible = ddlType.Text == "Folder";
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        var fileFolder = new Models.FileFolder
        {
            Name = txtName.Text,
            Path = txtPath.Text,
            Type = ddlType.Text,
            ContentsOnly = Convert.ToInt16(chkContentsOnly.Checked)
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