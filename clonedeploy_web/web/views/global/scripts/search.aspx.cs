using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;

public partial class views_admin_scripts_search : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

            PopulateGrid();
    }

    protected void PopulateGrid()
    {
        var script = new Script();
        gvScripts.DataSource = script.Search(txtSearch.Text);
        gvScripts.DataBind();

        lblTotal.Text = gvScripts.Rows.Count + " Result(s) / " + script.GetTotalCount() + " Total Script(s)";
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        var hcb = (CheckBox)gvScripts.HeaderRow.FindControl("chkSelectAll");

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
        List<Script> listScripts = (List<Script>)gvScripts.DataSource;
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
    private void ToggleCheckState(bool checkState)
    {
        foreach (GridViewRow row in gvScripts.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb != null)
                cb.Checked = checkState;
        }
    }
}