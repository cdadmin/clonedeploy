using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;

public partial class views_admin_scripts_search : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

            PopulateGrid();
    }

    protected void PopulateGrid()
    {
        var bllScript = new BLL.Script();
        gvScripts.DataSource = bllScript.SearchScripts(txtSearch.Text);
        gvScripts.DataBind();

        lblTotal.Text = gvScripts.Rows.Count + " Result(s) / " + bllScript.TotalCount() + " Total Script(s)";
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
    
}