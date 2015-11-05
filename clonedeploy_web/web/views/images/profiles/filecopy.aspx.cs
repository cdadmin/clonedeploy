using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BasePages;

public partial class views_images_profiles_filecopy : Images
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void PopulateProfileScripts()
    {
        var profileFiles = BLL.ImageProfileFileFolder.SearchImageProfileFileFolders(ImageProfile.Id);
        foreach (GridViewRow row in gvFile.Rows)
        {
            var enabled = (CheckBox)row.FindControl("chkEnabled");
            var dataKey = gvFile.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            foreach (var profileFile in profileFiles)
            {
                if (profileFile.FileFolderId == Convert.ToInt16(dataKey.Value))
                {

                    enabled.Checked = true;
                    var txtPriority = row.FindControl("txtPriority") as TextBox;
                    if (txtPriority != null)
                        txtPriority.Text = profileFile.Priority.ToString();
                }
            }
        }
    }

    protected void PopulateGrid()
    {
        gvFile.DataSource = BLL.FileFolder.SearchFileFolders();
        gvFile.DataBind();
        PopulateProfileScripts();
    }


    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        List<Models.FileFolder> listFilesFolders = (List<Models.FileFolder>)gvFile.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listFilesFolders = GetSortDirection(e.SortExpression) == "Asc" ? listFilesFolders.OrderBy(s => s.Name).ToList() : listFilesFolders.OrderByDescending(s => s.Name).ToList();
                break;
        }

        gvFile.DataSource = listFilesFolders;
        gvFile.DataBind();
        PopulateProfileScripts();
    }


    protected void btnUpdateFile_OnClick(object sender, EventArgs e)
    {
        BLL.ImageProfileFileFolder.DeleteImageProfileFileFolders(ImageProfile.Id);
        foreach (GridViewRow row in gvFile.Rows)
        {
            var enabled = (CheckBox)row.FindControl("chkEnabled");
            if (enabled == null) continue;
            if (!enabled.Checked) continue;
            var dataKey = gvFile.DataKeys[row.RowIndex];
            if (dataKey == null) continue;

            var profileFileFolder = new Models.ImageProfileFileFolder
            {
                FileFolderId = Convert.ToInt16(dataKey.Value),
                ProfileId = ImageProfile.Id,
            };
            var txtPriority = row.FindControl("txtPriority") as TextBox;
            if (txtPriority != null)
                if (!string.IsNullOrEmpty(txtPriority.Text))
                    profileFileFolder.Priority = Convert.ToInt32(txtPriority.Text);
            BLL.ImageProfileFileFolder.AddImageProfileFileFolder(profileFileFolder);
        }
    }
}