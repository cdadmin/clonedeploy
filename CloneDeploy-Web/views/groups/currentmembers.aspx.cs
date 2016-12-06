using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BasePages;

public partial class views_groups_currentmembers : Groups
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        if (Settings.DefaultComputerView == "all")
            PopulateGrid();
    }

    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        PopulateGrid();
        List<Computer> listComputers = (List<Computer>)gvComputers.DataSource;
        switch (e.SortExpression)
        {
            case "Name":
                listComputers = GetSortDirection(e.SortExpression) == "Asc" ? listComputers.OrderBy(h => h.Name).ToList() : listComputers.OrderByDescending(h => h.Name).ToList();
                break;
            case "Mac":
                listComputers = GetSortDirection(e.SortExpression) == "Asc" ? listComputers.OrderBy(h => h.Mac).ToList() : listComputers.OrderByDescending(h => h.Mac).ToList();
                break;
          
        }


        gvComputers.DataSource = listComputers;
        gvComputers.DataBind();
    }

    protected void PopulateGrid()
    {

        gvComputers.DataSource = BLL.Group.GetGroupMembers(Group.Id,txtSearch.Text);
        gvComputers.DataBind();

        lblTotal.Text = gvComputers.Rows.Count + " Result(s) / " + BLL.GroupMembership.GetGroupMemberCount(Group.Id) + " Total Computer(s)";
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }
}