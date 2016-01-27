using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BasePages;
using Helpers;
using Models;

namespace views.computers
{
    public partial class Searchcomputers : Computers
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (Settings.DefaultComputerView == "all")
                PopulateGrid();
        }

        protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            //Gridview is only populated with only allowed computers to view via group management
            //Don't need to worry about rechecking group management
            RequiresAuthorization(Authorizations.DeleteComputer);
            var deletedCount = 0;
            foreach (GridViewRow row in gvComputers.Rows)
            {
               
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvComputers.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                
                if(BLL.Computer.DeleteComputer(BLL.Computer.GetComputer(Convert.ToInt32(dataKey.Value))).IsValid)
                    deletedCount++;

            }
            EndUserMessage = "Deleted " + deletedCount + " Computer(s)";
            PopulateGrid();
        }


        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();

            List<Computer> listComputers = (List<Computer>) gvComputers.DataSource;
            switch (e.SortExpression)
            {
                case "Name":
                    listComputers = GetSortDirection(e.SortExpression) == "Desc"
                        ? listComputers.OrderByDescending(h => h.Name).ToList()
                        : listComputers.OrderBy(h => h.Name).ToList();
                    break;
                case "Mac":
                    listComputers = GetSortDirection(e.SortExpression) == "Desc"
                        ? listComputers.OrderByDescending(h => h.Mac).ToList()
                        : listComputers.OrderBy(h => h.Mac).ToList();
                    break;
                case "Image":
                    listComputers = GetSortDirection(e.SortExpression) == "Desc"
                        ? listComputers.OrderByDescending(h => h.ImageId).ToList()
                        : listComputers.OrderBy(h => h.ImageId).ToList();
                    break;
            }


            gvComputers.DataSource = listComputers;
            gvComputers.DataBind();

        }

        protected void PopulateGrid()
        {
           
            gvComputers.DataSource = BLL.Computer.SearchComputersForUser(CloneDeployCurrentUser.Id,txtSearch.Text);
            gvComputers.DataBind();

            lblTotal.Text = gvComputers.Rows.Count + " Result(s) / " + BLL.Computer.TotalCount() + " Computer(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvComputers);
        }
    }
}