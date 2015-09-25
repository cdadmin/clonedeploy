/*  
	CrucibleWDS A Windows Deployment Solution
	Copyright (C) 2011  Jon Dolny

	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program.  If not, see <http://www.gnu.org/licenses/.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Global;
using Helpers;
using Models;

namespace views.hosts
{
    public partial class Searchhosts : BasePages.Computers
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (Settings.DefaultHostView == "all")
                PopulateGrid();
        }

        protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvHosts.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvHosts.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                BllComputer.DeleteComputer(Convert.ToInt32(dataKey.Value));
            }

            PopulateGrid();
        }


        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();
            List<Models.Computer> listHosts = (List<Models.Computer>) gvHosts.DataSource;
            switch (e.SortExpression)
            {
                case "Name":
                    listHosts = GetSortDirection(e.SortExpression) == "Asc"
                        ? listHosts.OrderBy(h => h.Name).ToList()
                        : listHosts.OrderByDescending(h => h.Name).ToList();
                    break;
                case "Mac":
                    listHosts = GetSortDirection(e.SortExpression) == "Asc"
                        ? listHosts.OrderBy(h => h.Mac).ToList()
                        : listHosts.OrderByDescending(h => h.Mac).ToList();
                    break;
                case "Image":
                    listHosts = GetSortDirection(e.SortExpression) == "Asc"
                        ? listHosts.OrderBy(h => h.Image).ToList()
                        : listHosts.OrderByDescending(h => h.Image).ToList();
                    break;
            }


            gvHosts.DataSource = listHosts;
            gvHosts.DataBind();
        }

        protected void PopulateGrid()
        {
            gvHosts.DataSource = BllComputer.SearchComputers(txtSearch.Text);
            gvHosts.DataBind();

            lblTotal.Text = gvHosts.Rows.Count + " Result(s) / " + BllComputer.TotalCount() + " Total Host(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvHosts);
        }
    }
}