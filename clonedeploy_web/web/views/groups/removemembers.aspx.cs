using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BasePages;
using Helpers;
using Models;
using GroupMembership = BLL.GroupMembership;

public partial class views_groups_removemembers : Groups
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

        gvComputers.DataSource = BLL.Group.GetGroupMembers(Group.Id,txtSearch.Text);
        gvComputers.DataBind();

        //lblTotal.Text = gvComputers.Rows.Count + " Result(s) / " + BllGroup.GetGroupMemberCount(Group.Id) + " Total Computer(s)";
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        PopulateGrid();
    }

   

    protected void btnRemoveSelected_OnClick(object sender, EventArgs e)
    {
        var removedCount = 0;
        foreach (GridViewRow row in gvComputers.Rows)
        {
            var cb = (CheckBox)row.FindControl("chkSelector");
            if (cb == null || !cb.Checked) continue;
            var dataKey = gvComputers.DataKeys[row.RowIndex];
            if (dataKey != null)
            {
                var membership = new Models.GroupMembership
                {
                    ComputerId = Convert.ToInt32(dataKey.Value),
                    GroupId = Group.Id
                };
                if (BLL.GroupMembership.DeleteMembership(membership))
                    removedCount++;
            }
        }

        EndUserMessage = "Successfully Removed " + removedCount + " Members";

        PopulateGrid();

    }
}