using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BasePages;

namespace views.groups
{
    public partial class GroupSearch : Groups
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(Authorizations.DeleteGroup); 
            var deletedCount = 0;
            foreach (var dataKey in from GridViewRow row in gvGroups.Rows
                let cb = (CheckBox) row.FindControl("chkSelector")
                where cb != null && cb.Checked
                select gvGroups.DataKeys[row.RowIndex]
                into dataKey
                where dataKey != null
                select dataKey)
            {
                if (BLL.Group.DeleteGroup(Convert.ToInt32(dataKey.Value)).Success)
                    deletedCount++;
            }
            EndUserMessage = "Deleted " + deletedCount + " Group(s)";
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
                    listGroups = GetSortDirection(e.SortExpression) == "Asc" ? listGroups.OrderBy(g => g.ImageId).ToList() : listGroups.OrderByDescending(g => g.ImageId).ToList();
                    break;
                case "Type":
                    listGroups = GetSortDirection(e.SortExpression) == "Asc" ? listGroups.OrderBy(g => g.Type).ToList() : listGroups.OrderByDescending(g => g.Type).ToList();
                    break;              
            } 

            gvGroups.DataSource = listGroups;
            gvGroups.DataBind();
            foreach (GridViewRow row in gvGroups.Rows)
            {
                var group = new Group();
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
            var call = new APICall();
            gvGroups.DataSource = call.GroupApi.Get(txtSearch.Text);
            gvGroups.DataBind();

            foreach (GridViewRow row in gvGroups.Rows)
            {
                var group = new Group();
                var lbl = row.FindControl("lblCount") as Label;
                var dataKey = gvGroups.DataKeys[row.RowIndex];
                if (dataKey != null)
                    group = call.GroupApi.Get(Convert.ToInt32(dataKey.Value));
                if (lbl != null)
                    lbl.Text = call.GroupApi.GetMemberCount(group.Id).Value;
                
            }


            lblTotal.Text = gvGroups.Rows.Count + " Result(s) / " + call.GroupApi.GetCount() + " Total Group(s)";
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