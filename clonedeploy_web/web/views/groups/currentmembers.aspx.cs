using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BasePages;
using Helpers;
using Models;
using GroupMembership = BLL.GroupMembership;

public partial class views_groups_currentmembers : Groups
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        if (Settings.DefaultHostView == "all")
            PopulateGrid();
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

        gvHosts.DataSource = BLL.Group.GetGroupMembers(Group.Id,txtSearch.Text);
        gvHosts.DataBind();

        lblTotal.Text = gvHosts.Rows.Count + " Result(s) / " + BLL.GroupMembership.GetGroupMemberCount(Group.Id) + " Total Host(s)";
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }
}