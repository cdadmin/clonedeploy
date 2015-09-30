using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class views_global_buildings_search : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) BindGrid();
        else
        {
            if (gvBuildings.Rows[0].Cells[0].Text == "No Buildings Have Been Created")
            {
                gvBuildings.Rows[0].Cells.Clear();
                gvBuildings.Rows[0].Cells.Add(new TableCell());
                gvBuildings.Rows[0].Cells[0].Text = "No Buildings Have Been Created";
            }
        }
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        BindGrid();

    }
    protected void BindGrid()
    {
        gvBuildings.DataSource = BllBuilding.SearchBuildings(txtSearch.Text);
        gvBuildings.DataBind();

        if (gvBuildings.Rows.Count == 0)
        {
            var obj = new List<Models.Building>();
            obj.Add(new Models.Building());
            gvBuildings.DataSource = obj;
            gvBuildings.DataBind();

            gvBuildings.Rows[0].Cells.Clear();
            gvBuildings.Rows[0].Cells.Add(new TableCell());

            gvBuildings.Rows[0].Cells[0].Text = "No Buildings Have Been Created";

        }
    }

    protected void Insert(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        var building = new Models.Building
        {
            Name = ((TextBox)gvRow.FindControl("txtNameAdd")).Text
        };

        BllBuilding.AddBuilding(building);
        BindGrid();
    }

    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gvBuildings.EditIndex = e.NewEditIndex;
        BindGrid();
    }


    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow gvRow = gvBuildings.Rows[e.RowIndex];
        var building = new Models.Building
        {
            Id = Convert.ToInt32(gvBuildings.DataKeys[e.RowIndex].Values[0]),
            Name = ((TextBox)gvRow.FindControl("txtName")).Text

        };
        BllBuilding.UpdateBuilding(building);

        gvBuildings.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowCancelingEdit(object sender, EventArgs e)
    {
        gvBuildings.EditIndex = -1;
        BindGrid();
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BllBuilding.DeleteBuilding(Convert.ToInt32(gvBuildings.DataKeys[e.RowIndex].Values[0]));
        BindGrid();
    }
}