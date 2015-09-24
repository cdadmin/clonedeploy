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
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;
using Security;
using Group = BLL.Group;

namespace views.users
{
    public partial class CreateUser : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
         
            if (IsPostBack) return;
            if (new Authorize().IsInMembership("User"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");


            gvGroups.DataSource = new BLL.Group().SearchGroups("%");
            gvGroups.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var bllUser = new BLL.User();
            if (txtUserPwd.Text != txtUserPwdConfirm.Text)
            {
                Message.Text  = "Passwords Did Not Match";
                return;
            }

            var listGroupManagement = new List<string>();
            foreach (GridViewRow row in gvGroups.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvGroups.DataKeys[row.RowIndex];
                if (dataKey != null)
                    listGroupManagement.Add(dataKey.Value.ToString());
            }
            var user = new WdsUser
            {
                GroupManagement = string.Join(" ", listGroupManagement),
                Name = txtUserName.Text,
                Password = txtUserPwd.Text,
                Membership = ddluserMembership.Text,
                Salt = bllUser.CreateSalt(16)
            };

            if (permissions.Visible)
            {
                user.OndAccess = chkOnd.Checked ? "1" : "0";
                user.DebugAccess = chkDebug.Checked ? "1" : "0";
                user.DiagAccess = chkDiag.Checked ? "1" : "0";
            }
            else
            {
                user.OndAccess = "1";
                user.DiagAccess = "1";
                user.DebugAccess = "1";
            }
            if (bllUser.ValidateUserData(user)) bllUser.AddUser(user);

        }

        protected void ddluserMembership_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddluserMembership.Text == "User")
            {
                management.Visible = true;
                permissions.Visible = true;
            }
            else
            {
                management.Visible = false;
                permissions.Visible = false;
            }
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

            gvGroups.DataSource = new BLL.Group().SearchGroups("%");
            var dataTable = (DataTable) gvGroups.DataSource;

            if (dataTable == null) return;
            var dataView = new DataView(dataTable)
            {
                Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression)
            };
            gvGroups.DataSource = dataView;
            gvGroups.DataBind();
        }

        protected void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            var hcb = (CheckBox) gvGroups.HeaderRow.FindControl("chkSelectAll");
            ToggleCheckState(hcb.Checked);
        }

        private void ToggleCheckState(bool checkState)
        {
            foreach (GridViewRow row in gvGroups.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb != null)
                    cb.Checked = checkState;
            }
        }
    }
}