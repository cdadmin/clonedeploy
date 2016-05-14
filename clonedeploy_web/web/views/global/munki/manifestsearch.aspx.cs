using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_global_munki_manifestsearch : BasePages.Global
{
    public static string[] GetLogs()
    {
        var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "public" +
                      Path.DirectorySeparatorChar + "munki" + Path.DirectorySeparatorChar + "catalogs" + Path.DirectorySeparatorChar;

        var catalogs = Directory.GetFiles(logPath, "*.*");

        for (var x = 0; x < catalogs.Length; x++)
            catalogs[x] = Path.GetFileName(catalogs[x]);

        return catalogs;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        gvManifestTemplates.DataSource = BLL.MunkiManifestTemplate.SearchManifests(txtSearch.Text);
        gvManifestTemplates.DataBind();

        lblTotal.Text = gvManifestTemplates.Rows.Count + " Result(s) / " + BLL.MunkiManifestTemplate.TotalCount() + " Total Manifest Template(s)";
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvManifestTemplates);
    }



    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        List<Models.MunkiManifestTemplate> listManifestTemplates = (List<Models.MunkiManifestTemplate>)gvManifestTemplates.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listManifestTemplates = GetSortDirection(e.SortExpression) == "Asc" ? listManifestTemplates.OrderBy(s => s.Name).ToList() : listManifestTemplates.OrderByDescending(s => s.Name).ToList();
                break;
           
        }


        gvManifestTemplates.DataSource = listManifestTemplates;
        gvManifestTemplates.DataBind();
    }

    protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.DeleteGlobal);
        foreach (GridViewRow row in gvManifestTemplates.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvManifestTemplates.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            BLL.MunkiManifestTemplate.DeleteManifest(Convert.ToInt32(dataKey.Value));
        }

        PopulateGrid();
    }
}