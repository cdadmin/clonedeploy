using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_global_boottemplates_search : Global
{
    protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.DeleteGlobal);
        foreach (GridViewRow row in gvTemplates.Rows)
        {
            var cb = (CheckBox) row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvTemplates.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            Call.BootTemplateApi.Delete(Convert.ToInt32(dataKey.Value));
        }

        PopulateGrid();
    }

    protected void chkSelectAll_OnCheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvTemplates);
    }

    protected void gvTemplates_OnSorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        var listSysprepTags = (List<BootTemplateEntity>) gvTemplates.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listSysprepTags = GetSortDirection(e.SortExpression) == "Asc"
                    ? listSysprepTags.OrderBy(s => s.Name).ToList()
                    : listSysprepTags.OrderByDescending(s => s.Name).ToList();
                break;
        }


        gvTemplates.DataSource = listSysprepTags;
        gvTemplates.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        gvTemplates.DataSource = Call.BootTemplateApi.GetAll(int.MaxValue, txtSearch.Text);
        gvTemplates.DataBind();

        lblTotal.Text = gvTemplates.Rows.Count + " Result(s) / " + Call.BootTemplateApi.GetCount() +
                        " Total Boot Template(s)";
    }

    protected void txtSearch_OnTextChanged(object sender, EventArgs e)
    {
        PopulateGrid();
    }
}