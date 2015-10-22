using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BasePages;
using BLL;

namespace views.groups
{
    public partial class GroupSearch : Groups
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (var dataKey in from GridViewRow row in gvGroups.Rows
                let cb = (CheckBox) row.FindControl("chkSelector")
                where cb != null && cb.Checked
                select gvGroups.DataKeys[row.RowIndex]
                into dataKey
                where dataKey != null
                select dataKey)
            {
                BLL.Group.DeleteGroup(Convert.ToInt32(dataKey.Value));
            }
            PopulateGrid();
        }


        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();
            List<Models.Group> listGroups = (List<Models.Group>)gvGroups.DataSource;
            switch (e.SortExpression)
            {
                case "Name":
                    listGroups = GetSortDirection(e.SortExpression) == "Asc" ? listGroups.OrderBy(g => g.Name).ToList() : listGroups.OrderByDescending(g => g.Name).ToList();
                    break;
                case "Image":
                    listGroups = GetSortDirection(e.SortExpression) == "Asc" ? listGroups.OrderBy(g => g.Image).ToList() : listGroups.OrderByDescending(g => g.Image).ToList();
                    break;
                case "Type":
                    listGroups = GetSortDirection(e.SortExpression) == "Asc" ? listGroups.OrderBy(g => g.Type).ToList() : listGroups.OrderByDescending(g => g.Type).ToList();
                    break;              
            } 

            gvGroups.DataSource = listGroups;
            gvGroups.DataBind();
            foreach (GridViewRow row in gvGroups.Rows)
            {
                var group = new Models.Group();
                var lbl = row.FindControl("lblCount") as Label;
                var dataKey = gvGroups.DataKeys[row.RowIndex];
                if (dataKey != null)
                    group = BLL.Group.GetGroup(Convert.ToInt32(dataKey.Value));              
                if (lbl != null)
                    lbl.Text = BLL.GroupMembership.GetGroupMemberCount(group.Id);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {        
            if (IsPostBack) return;
            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            gvGroups.DataSource = BLL.Group.SearchGroupsForUser(CloneDeployCurrentUser.Id,txtSearch.Text);
            gvGroups.DataBind();

            foreach (GridViewRow row in gvGroups.Rows)
            {
                var group = new Models.Group();
                var lbl = row.FindControl("lblCount") as Label;
                var dataKey = gvGroups.DataKeys[row.RowIndex];
                if (dataKey != null)
                    group = BLL.Group.GetGroup(Convert.ToInt32(dataKey.Value));
                if (lbl != null)
                    lbl.Text = BLL.GroupMembership.GetGroupMemberCount(group.Id);
                
            }


            lblTotal.Text = gvGroups.Rows.Count + " Result(s) / " + BLL.Group.TotalCount() + " Total Group(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvGroups);
        }

      
    }
}