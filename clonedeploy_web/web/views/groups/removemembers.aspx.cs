using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;

public partial class views_groups_removemembers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        if (Settings.DefaultHostView == "all")
            PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        var hcb = (CheckBox)gvHosts.HeaderRow.FindControl("chkSelectAll");

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
        List<Computer> listHosts = (List<Computer>)gvHosts.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listHosts = GetSortDirection(e.SortExpression) == "Asc" ? listHosts.OrderBy(h => h.Name).ToList() : listHosts.OrderByDescending(h => h.Name).ToList();
                break;
            case "Mac":
                listHosts = GetSortDirection(e.SortExpression) == "Asc" ? listHosts.OrderBy(h => h.Mac).ToList() : listHosts.OrderByDescending(h => h.Mac).ToList();
                break;
          
        }


        gvHosts.DataSource = listHosts;
        gvHosts.DataBind();
    }

    protected void PopulateGrid()
    {
        var membership = new GroupMembership();
        gvHosts.DataSource = membership.Search(Master.Group.Id,txtSearch.Text);
        gvHosts.DataBind();

        lblTotal.Text = gvHosts.Rows.Count + " Result(s) / " + membership.GetTotalCount(Master.Group.Id) + " Total Host(s)";
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

    private void ToggleCheckState(bool checkState)
    {
        foreach (GridViewRow row in gvHosts.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb != null)
                cb.Checked = checkState;
        }
    }

    protected void btnRemoveSelected_OnClick(object sender, EventArgs e)
    {
        var removedCount = 0;
        foreach (GridViewRow row in gvHosts.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvHosts.DataKeys[row.RowIndex];
            if (dataKey != null)
            {
                var membership = new GroupMembership
                {
                    ComputerId = Convert.ToInt32(dataKey.Value),
                    GroupId = Master.Group.Id
                };
                if (membership.Delete())
                    removedCount++;
            }
        }

        new Utility().Msgbox("Successfully Removed " + removedCount + " Members" );

        PopulateGrid();

    }
}