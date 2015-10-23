using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BLL;

public partial class views_admin_scripts_search : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

            PopulateGrid();
    }

    protected void PopulateGrid()
    {
        gvScripts.DataSource = BLL.Script.SearchScripts(txtSearch.Text);
        gvScripts.DataBind();

        lblTotal.Text = gvScripts.Rows.Count + " Result(s) / " + BLL.Script.TotalCount() + " Total Script(s)";
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvScripts);
    }

    

    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        List<Models.Script> listScripts = (List<Models.Script>)gvScripts.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listScripts = GetSortDirection(e.SortExpression) == "Asc" ? listScripts.OrderBy(s => s.Name).ToList() : listScripts.OrderByDescending(s => s.Name).ToList();
                break;
            case "Priority":
                listScripts = GetSortDirection(e.SortExpression) == "Asc" ? listScripts.OrderBy(s => s.Priority).ToList() : listScripts.OrderByDescending(s => s.Priority).ToList();
                break;
         
        }


        gvScripts.DataSource = listScripts;
        gvScripts.DataBind();
    }

    protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvScripts.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvScripts.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            BLL.Script.DeleteScript(Convert.ToInt32(dataKey.Value));
        }

        PopulateGrid();
    }

}