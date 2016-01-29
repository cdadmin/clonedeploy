using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            if (CloneDeployCurrentUser.Membership != "Administrator")
            {
                Session["UserId"] = CloneDeployCurrentUser.Id.ToString();
                Response.Redirect("~/views/users/resetpass.aspx",true);
            }

            //Just In Case
            RequiresAuthorization(Authorizations.Administrator);

            if (IsPostBack) return;         
            PopulateGrid();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var deletedCount = 0;
            var adminMessage = string.Empty;
            foreach (GridViewRow row in gvUsers.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvUsers.DataKeys[row.RowIndex];
                if (dataKey != null)
                {
                    var user = BLL.User.GetUser(Convert.ToInt32(dataKey.Value));

                    if (user.Membership == "Administrator")
                    {
                        adminMessage =
                            "<br/>Administrators Must Be Changed To A User Before They Can Be Deleted";
                        break;
                    }
                    if (BLL.User.DeleteUser(user.Id))
                        deletedCount++;
                }
            }
            EndUserMessage = "Successfully Deleted " + deletedCount + " Users" + adminMessage;
            PopulateGrid();
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvUsers);
        }



        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();
            List<Models.CloneDeployUser> listUsers = (List<Models.CloneDeployUser>)gvUsers.DataSource;
            switch (e.SortExpression)
            {
                case "Name":
                    listUsers = GetSortDirection(e.SortExpression) == "Asc" ? listUsers.OrderBy(g => g.Name).ToList() : listUsers.OrderByDescending(g => g.Name).ToList();
                    break;
                case "Membership":
                    listUsers = GetSortDirection(e.SortExpression) == "Asc" ? listUsers.OrderBy(g => g.Membership).ToList() : listUsers.OrderByDescending(g => g.Membership).ToList();
                    break;
             
            }

            gvUsers.DataSource = listUsers;
            gvUsers.DataBind();          
        }

        protected void PopulateGrid()
        {   
            gvUsers.DataSource = BLL.User.SearchUsers(txtSearch.Text);
            gvUsers.DataBind();
            lblTotal.Text = gvUsers.Rows.Count + " Result(s) / " + BLL.User.TotalCount() + " Total User(s)";
        }

       

       
    }
}