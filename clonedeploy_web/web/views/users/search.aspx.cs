using System;
using System.Data;
using System.Web.UI.WebControls;
using BasePages;
using BLL;
using Helpers;
using Models;

namespace views.users
{
    public partial class UserSearch : Users
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        
            if (IsPostBack) return;
          
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
                    user.Id = Convert.ToInt32(dataKey.Value);
                    user.Membership = row.Cells[3].Text;
                }
                if (user.Membership == "Administrator")
                {
                    EndUserMessage = "Administrators Must Be Changed To A Lower Level User Before They Can Be Deleted";
                    break;
                }
                BLL.User.DeleteUser(user.Id);
            }

            PopulateGrid();
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
            gvUsers.DataSource = BLL.User.SearchUsers(txtSearch.Text);
            gvUsers.DataBind();
            lblTotal.Text = gvUsers.Rows.Count + " Result(s) / " + BLL.User.TotalCount() + " Total User(s)";
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