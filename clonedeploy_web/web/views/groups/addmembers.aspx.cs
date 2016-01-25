using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using BasePages;
using Helpers;
using Models;

public partial class views_groups_addmembers : Groups
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        if (Settings.DefaultComputerView == "all")
            PopulateGrid();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ChkAll(gvComputers);
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
      
        var listOfComputers = BLL.Computer.SearchComputersForUser(CloneDeployCurrentUser.Id, txtSearch.Text);
        listOfComputers.AddRange(BLL.Computer.ComputersWithoutGroup());
      
        gvComputers.DataSource = listOfComputers.GroupBy(c => c.Id).Select(g => g.First()).ToList(); ;
        gvComputers.DataBind();

        lblTotal.Text = gvComputers.Rows.Count + " Result(s) / " + BLL.Computer.TotalCount() + " Total Computer(s)";
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

   

    protected void btnAddSelected_OnClick(object sender, EventArgs e)
    {
        var memberships = (from GridViewRow row in gvComputers.Rows
            let cb = (CheckBox) row.FindControl("chkSelector")
            where cb != null && cb.Checked
            select gvComputers.DataKeys[row.RowIndex]
            into dataKey
            where dataKey != null
            select new Models.GroupMembership
            {
                ComputerId = Convert.ToInt32(dataKey.Value), GroupId = Group.Id
            }).ToList();
        EndUserMessage = BLL.GroupMembership.AddMembership(memberships) ? "Successfully Added Group Members" : "Could Not Add Group Members";
    }
}