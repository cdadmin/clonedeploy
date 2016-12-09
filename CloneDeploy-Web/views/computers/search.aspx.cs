using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

namespace views.computers
{
    public partial class Searchcomputers : Computers
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateSitesDdl(ddlSite);
                PopulateBuildingsDdl(ddlBuilding);
                PopulateRoomsDdl(ddlRoom);
                PopulateGroupsDdl(ddlGroup);
                PopulateImagesDdl(ddlImage);
                if (Settings.DefaultComputerView == "all")
                    PopulateGrid();
            }

          
        }

        protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            //Gridview is only populated with only allowed computers to view via group management
            //Don't need to worry about rechecking group management
            //RequiresAuthorization(Authorizations.DeleteComputer);
            
            var deletedCount = 0;

            foreach (GridViewRow row in gvComputers.Rows)
            {
               
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvComputers.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                if (Call.ComputerApi.Delete(Convert.ToInt32(dataKey.Value)).Success)
                    deletedCount++;
                

            }
            EndUserMessage = "Deleted " + deletedCount + " Computer(s)";
            PopulateGrid();
        }


        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();

            List<ComputerEntity> listComputers = (List<ComputerEntity>) gvComputers.DataSource;
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
            var limit = 0;
            limit = ddlLimit.Text == "All" ? Int32.MaxValue : Convert.ToInt32(ddlLimit.Text);

            
            var listOfComputers = Call.ComputerApi.GetAll(limit, txtSearch.Text);
            
            listOfComputers = listOfComputers.GroupBy(c => c.Id).Select(g => g.First()).ToList();
            if (ddlSite.SelectedValue != "-1")
                listOfComputers = listOfComputers.Where(c => c.SiteId == Convert.ToInt32(ddlSite.SelectedValue)).ToList();
            if (ddlBuilding.SelectedValue != "-1")
                listOfComputers = listOfComputers.Where(c => c.BuildingId == Convert.ToInt32(ddlBuilding.SelectedValue)).ToList();
            if (ddlRoom.SelectedValue != "-1")
                listOfComputers = listOfComputers.Where(c => c.RoomId == Convert.ToInt32(ddlRoom.SelectedValue)).ToList();
            if (ddlGroup.SelectedValue != "-1")
            {
                var groupMembers = Call.GroupApi.GetGroupMembers(Convert.ToInt32(ddlGroup.SelectedValue));

                listOfComputers =
                    (from groupMember in groupMembers
                        from computer in listOfComputers
                        where groupMember.Id == computer.Id
                        select computer).ToList();
              
               
            }
            if (ddlImage.SelectedValue != "-1")
                listOfComputers = listOfComputers.Where(c => c.Image != null).Where(a => a.Image.Id == Convert.ToInt32(ddlImage.SelectedValue)).ToList();
            gvComputers.DataSource = listOfComputers;
            
            /*Dynamic column example
            BoundField test = new BoundField();
            test.DataField = "ImageProfileId";
            test.HeaderText = "Image Profile";
            gvComputers.Columns.Add(test);
            */
            gvComputers.DataBind();
            
        
            lblTotal.Text = gvComputers.Rows.Count + " Result(s) / " + Call.ComputerApi.GetCount() + " Computer(s)";
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvComputers);
        }

        protected void ddl_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}