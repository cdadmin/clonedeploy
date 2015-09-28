using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;


namespace views.groups
{
    public partial class GroupSearch : BasePages.Groups
    {
        private readonly BLL.Group _bllGroup = new BLL.Group();
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvGroups.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvGroups.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                _bllGroup.DeleteGroup(Convert.ToInt32(dataKey.Value));
            }


            PopulateGrid();
        }

       

        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();
            List<Group> listGroups = (List<Group>)gvGroups.DataSource;
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
                    group = _bllGroup.GetGroup(Convert.ToInt32(dataKey.Value));
                if (row.Cells[4].Text == "smart")
                {
                   
                       
                
                    //FIX ME
                    //if (lbl != null) lbl.Text = @group.SearchSmartHosts(@group.Expression).Count.ToString();
                }
                else if (lbl != null)
                {
                    lbl.Text = new BLL.GroupMembership().GetGroupMemberCount(group.Id);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {        
            if (IsPostBack) return;
            PopulateGrid();
        }

        protected void PopulateGrid()
        {
           

            gvGroups.DataSource = _bllGroup.SearchGroups(txtSearch.Text);

            gvGroups.DataBind();

            foreach (GridViewRow row in gvGroups.Rows)
            {
                var group = new Models.Group();
                var lbl = row.FindControl("lblCount") as Label;
                var dataKey = gvGroups.DataKeys[row.RowIndex];
                if (dataKey != null)
                    group = _bllGroup.GetGroup(Convert.ToInt32(dataKey.Value));
                
                if (row.Cells[4].Text == "smart")
                {
                   

                    //if (lbl != null) lbl.Text = @group.SearchSmartHosts(@group.Expression).Count.ToString();
                }
                else if (lbl != null)
                {

                    lbl.Text = new BLL.GroupMembership().GetGroupMemberCount(group.Id);
                }
            }


            lblTotal.Text = gvGroups.Rows.Count + " Result(s) / " + _bllGroup.TotalCount() + " Total Group(s)";
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