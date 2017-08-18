using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.groups
{
    public partial class views_groups_addmembers : Groups
    {
        protected void btnAddSelected_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedGroup(AuthorizationStrings.UpdateGroup, Group.Id);
            var memberships = (from GridViewRow row in gvComputers.Rows
                let cb = (CheckBox) row.FindControl("chkSelector")
                where cb != null && cb.Checked
                select gvComputers.DataKeys[row.RowIndex]
                into dataKey
                where dataKey != null
                select new GroupMembershipEntity
                {
                    ComputerId = Convert.ToInt32(dataKey.Value),
                    GroupId = Group.Id
                }).ToList();
            EndUserMessage = Call.GroupMembershipApi.Post(memberships).Success
                ? "Successfully Added Group Members"
                : "Could Not Add Group Members";
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvComputers);
        }

        protected void ddlLimit_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();
            var listComputers = (List<ComputerEntity>) gvComputers.DataSource;
            switch (e.SortExpression)
            {
                case "Name":
                    listComputers = GetSortDirection(e.SortExpression) == "Asc"
                        ? listComputers.OrderBy(h => h.Name).ToList()
                        : listComputers.OrderByDescending(h => h.Name).ToList();
                    break;
                case "Mac":
                    listComputers = GetSortDirection(e.SortExpression) == "Asc"
                        ? listComputers.OrderBy(h => h.Mac).ToList()
                        : listComputers.OrderByDescending(h => h.Mac).ToList();
                    break;
            }

            gvComputers.DataSource = listComputers;
            gvComputers.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (GetSetting(SettingStrings.DefaultComputerView) == "all")
                PopulateGrid();
        }

        protected void PopulateGrid()
        {
            var limit = 0;
            limit = ddlLimit.Text == "All" ? int.MaxValue : Convert.ToInt32(ddlLimit.Text);
            var listOfComputers = Call.ComputerApi.Get();

            //If a user is using a managed group they can also see computers without any group, to add in.
            listOfComputers.AddRange(Call.ComputerApi.GetComputersWithoutGroup(limit, txtSearch.Text));

            gvComputers.DataSource = listOfComputers.GroupBy(c => c.Id).Select(g => g.First()).ToList();
            gvComputers.DataBind();

            lblTotal.Text = gvComputers.Rows.Count + " Result(s) / " + Call.ComputerApi.GetCount() +
                            " Total Computer(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}