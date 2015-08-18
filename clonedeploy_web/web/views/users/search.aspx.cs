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
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;
using Security;

namespace views.users
{
    public partial class UserSearch : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Master.FindControl("SubNav").Visible = false;
            if (IsPostBack) return;
            if (!new Authorize().IsInMembership("Administrator"))
            {
                var wdsuser = new WdsUser { Name = HttpContext.Current.User.Identity.Name };
                wdsuser.Read();
                if (string.IsNullOrEmpty(wdsuser.Id)) //Fix for clicking logout button when on users page
                    Response.Redirect("~/");
                else
                    Response.Redirect("~/views/users/resetpass.aspx?userid=" + wdsuser.Id);
            }
            PopulateGrid();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvUsers.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var user = new WdsUser();
                var dataKey = gvUsers.DataKeys[row.RowIndex];
                if (dataKey != null)
                {
                    user.Id = dataKey.Value.ToString();
                    user.Membership = row.Cells[3].Text;
                }
                if (user.Membership == "Administrator")
                {
                    Utility.Message = "Administrators Must Be Changed To A Lower Level User Before They Can Be Deleted";
                    break;
                }
                user.Delete();
            }

            PopulateGrid();
            Master.Master.Msgbox(Utility.Message);
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            var hcb = (CheckBox) gvUsers.HeaderRow.FindControl("chkSelectAll");

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
            var dataTable = gvUsers.DataSource as DataTable;
            if (dataTable == null) return;
            var dataView = new DataView(dataTable)
            {
                Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression)
            };
            gvUsers.DataSource = dataView;
            gvUsers.DataBind();
        }

        protected void PopulateGrid()
        {
            var user = new WdsUser();
            gvUsers.DataSource = user.Search(txtSearch.Text);
            gvUsers.DataBind();
            lblTotal.Text = gvUsers.Rows.Count + " Result(s) / " + user.GetTotalCount() + " Total User(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void ToggleCheckState(bool checkState)
        {
            foreach (GridViewRow row in gvUsers.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb != null)
                    cb.Checked = checkState;
            }
        }
    }
}