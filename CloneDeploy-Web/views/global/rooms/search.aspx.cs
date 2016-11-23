using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Web.Models;
using Helpers;

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
        gvRooms.DataSource = BLL.Room.SearchRooms(txtSearch.Text);
        gvRooms.DataBind();

        if (gvRooms.Rows.Count == 0)
        {
            var obj = new List<Room>();
            obj.Add(new Room());
            gvRooms.DataSource = obj;
            gvRooms.DataBind();

            gvRooms.Rows[0].Cells.Clear();
            gvRooms.Rows[0].Cells.Add(new TableCell());

            gvRooms.Rows[0].Cells[0].Text = "No Rooms Have Been Created";

        }
    }

    protected void Insert(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.CreateGlobal);
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        var room = new Room
        {
            Name = ((TextBox)gvRow.FindControl("txtNameAdd")).Text,
            DistributionPointId = Convert.ToInt32(((DropDownList)gvRow.FindControl("ddlDpAdd")).SelectedValue)
        };

        BLL.Room.AddRoom(room);
        BindGrid();
    }

    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gvRooms.EditIndex = e.NewEditIndex;
        BindGrid();
    }


    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        GridViewRow gvRow = gvRooms.Rows[e.RowIndex];
        var room = new Room
        {
            Id = Convert.ToInt32(gvRooms.DataKeys[e.RowIndex].Values[0]),
            Name = ((TextBox)gvRow.FindControl("txtName")).Text,
            DistributionPointId = Convert.ToInt32(((DropDownList)gvRow.FindControl("ddlDp")).SelectedValue)

        };
        BLL.Room.UpdateRoom(room);

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
        RequiresAuthorization(Authorizations.DeleteGlobal);
        BLL.Room.DeleteRoom(Convert.ToInt32(gvRooms.DataKeys[e.RowIndex].Values[0]));
        BindGrid();
    }

    protected void gvRooms_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DropDownList ddlDps = null;

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ddlDps = e.Row.FindControl("ddlDpAdd") as DropDownList;
            PopulateDistributionPointsDdl(ddlDps);

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ddlDps = e.Row.FindControl("ddlDp") as DropDownList;
            if (ddlDps != null)
            {
                PopulateDistributionPointsDdl(ddlDps);
                ddlDps.SelectedValue = ((Room)(e.Row.DataItem)).DistributionPoint.ToString();
            }
        } 
    }
}