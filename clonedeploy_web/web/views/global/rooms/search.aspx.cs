using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class views_global_rooms_search : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) BindGrid();
        else
        {
            if (gvRooms.Rows[0].Cells[0].Text == "No Rooms Have Been Created")
            {
                gvRooms.Rows[0].Cells.Clear();
                gvRooms.Rows[0].Cells.Add(new TableCell());
                gvRooms.Rows[0].Cells[0].Text = "No Rooms Have Been Created";
            }
        }
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        BindGrid();

    }
    protected void BindGrid()
    {
        gvRooms.DataSource = BllRoom.SearchRooms(txtSearch.Text);
        gvRooms.DataBind();

        if (gvRooms.Rows.Count == 0)
        {
            var obj = new List<Models.Room>();
            obj.Add(new Models.Room());
            gvRooms.DataSource = obj;
            gvRooms.DataBind();

            gvRooms.Rows[0].Cells.Clear();
            gvRooms.Rows[0].Cells.Add(new TableCell());

            gvRooms.Rows[0].Cells[0].Text = "No Rooms Have Been Created";

        }
    }

    protected void Insert(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        var room = new Models.Room
        {
            Name = ((TextBox)gvRow.FindControl("txtNameAdd")).Text
        };

        BllRoom.AddRoom(room);
        BindGrid();
    }

    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gvRooms.EditIndex = e.NewEditIndex;
        BindGrid();
    }


    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow gvRow = gvRooms.Rows[e.RowIndex];
        var room = new Models.Room
        {
            Id = Convert.ToInt32(gvRooms.DataKeys[e.RowIndex].Values[0]),
            Name = ((TextBox)gvRow.FindControl("txtName")).Text

        };
        BllRoom.UpdateRoom(room);

        gvRooms.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowCancelingEdit(object sender, EventArgs e)
    {
        gvRooms.EditIndex = -1;
        BindGrid();
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BllRoom.DeleteRoom(Convert.ToInt32(gvRooms.DataKeys[e.RowIndex].Values[0]));
        BindGrid();
    }
}