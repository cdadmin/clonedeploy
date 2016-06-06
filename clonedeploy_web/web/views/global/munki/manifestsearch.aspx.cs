using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;

public partial class views_global_munki_manifestsearch : BasePages.Global
{
    

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

    protected void btnPreview_OnClick(object sender, EventArgs e)
    {
        var control = sender as Control;
        if (control != null)
        {
            var gvRow = (GridViewRow) control.Parent.Parent;
            var dataKey = gvManifestTemplates.DataKeys[gvRow.RowIndex];
            if (dataKey != null)
            {
                var effectiveManifest = new BLL.Workflows.EffectiveMunkiTemplate().MunkiTemplate(Convert.ToInt32(dataKey.Value));
                Response.Write(Encoding.UTF8.GetString(effectiveManifest.ToArray()));
                Response.ContentType = "text/plain";
                Response.End();  
            }
        }
    }

    protected void btnApply_OnClick(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}