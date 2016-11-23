using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Web.Models;

public partial class views_global_search : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        PopulateGrid();
    }

    protected void PopulateGrid()
    {
        gvLayout.DataSource = BLL.PartitionLayout.SearchPartitionLayouts(txtSearch.Text);
        gvLayout.DataBind();

        lblTotal.Text = gvLayout.Rows.Count + " Result(s) / " + BLL.PartitionLayout.TotalCount() + " Total Partition Layout(s)";
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        var hcb = (CheckBox)gvLayout.HeaderRow.FindControl("chkSelectAll");

        ToggleCheckState(hcb.Checked);
    }

    public string GetSortDirection(string sortExpression)
    {
        if (ViewState[sortExpression] == null)
            ViewState[sortExpression] = "Desc";
        else
            ViewState[sortExpression] = ViewState[sortExpression].ToString() == "Desc" ? "Asc" : "Desc";

        return ViewState[sortExpression].ToString();
    }

    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        List<PartitionLayout> listLayouts = (List<PartitionLayout>)gvLayout.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listLayouts = GetSortDirection(e.SortExpression) == "Asc" ? listLayouts.OrderBy(l => l.Name).ToList() : listLayouts.OrderByDescending(l => l.Name).ToList();
                break;
            case "Table":
                listLayouts = GetSortDirection(e.SortExpression) == "Asc" ? listLayouts.OrderBy(l => l.Table).ToList() : listLayouts.OrderByDescending(l => l.Table).ToList();
                break;
            case "ImageEnvironment":
                listLayouts = GetSortDirection(e.SortExpression) == "Asc" ? listLayouts.OrderBy(l => l.ImageEnvironment).ToList() : listLayouts.OrderByDescending(l => l.ImageEnvironment).ToList();
                break;

        }


        gvLayout.DataSource = listLayouts;
        gvLayout.DataBind();
    }
    private void ToggleCheckState(bool checkState)
    {
        foreach (GridViewRow row in gvLayout.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb != null)
                cb.Checked = checkState;
        }
    }

    protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvLayout.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvLayout.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            BLL.PartitionLayout.DeletePartitionLayout(Convert.ToInt32(dataKey.Value));
        }

        PopulateGrid();
    }
}