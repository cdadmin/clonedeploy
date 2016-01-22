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

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        var fileFolder = new Models.FileFolder
        {
            Name = txtName.Text,
            Path = txtPath.Text,
            Type = ddlType.Text
        };

        if (fileFolder.Path.Trim().EndsWith("/") || fileFolder.Path.Trim().EndsWith(@"\"))
        {
            char[] toRemove = { '/', '\\'};
            string trimmed = fileFolder.Path.TrimEnd(toRemove);
            fileFolder.Path = trimmed;
        }
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