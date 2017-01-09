using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Entities;
using CloneDeploy_Web;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_global_sites_search : Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) BindGrid();
        else
        {
            if (gvSites.Rows[0].Cells[0].Text == "No Sites Have Been Created")
            {
                gvSites.Rows[0].Cells.Clear();
                gvSites.Rows[0].Cells.Add(new TableCell());
                gvSites.Rows[0].Cells[0].Text = "No Sites Have Been Created";
            }
        }
    }

    protected void search_Changed(object sender, EventArgs e)
    {
        BindGrid();

    }
    protected void BindGrid()
    {
        gvSites.DataSource = Call.SiteApi.GetAll(Int32.MaxValue,txtSearch.Text);
        gvSites.DataBind();

        if (gvSites.Rows.Count == 0)
        {
            var obj = new List<SiteEntity>();
            obj.Add(new SiteEntity());
            gvSites.DataSource = obj;
            gvSites.DataBind();

            gvSites.Rows[0].Cells.Clear();
            gvSites.Rows[0].Cells.Add(new TableCell()); 

            gvSites.Rows[0].Cells[0].Text = "No Sites Have Been Created";

        }
    }

    protected void Insert(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.CreateGlobal);
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        var site = new SiteEntity()
        {
            Name = ((TextBox)gvRow.FindControl("txtNameAdd")).Text,
            DistributionPointId = Convert.ToInt32(((DropDownList)gvRow.FindControl("ddlDpAdd")).SelectedValue)
        };

        Call.SiteApi.Post(site);
        BindGrid();
    }

    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gvSites.EditIndex = e.NewEditIndex;
        BindGrid();
    }


    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        GridViewRow gvRow = gvSites.Rows[e.RowIndex];
        var site = new SiteEntity()
        {
            Id = Convert.ToInt32(gvSites.DataKeys[e.RowIndex].Values[0]),
            Name = ((TextBox)gvRow.FindControl("txtName")).Text,
            DistributionPointId = Convert.ToInt32(((DropDownList)gvRow.FindControl("ddlDp")).SelectedValue)
        };
        Call.SiteApi.Put(site.Id,site);

        gvSites.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowCancelingEdit(object sender, EventArgs e)
    {
        gvSites.EditIndex = -1;
        BindGrid();
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        RequiresAuthorization(Authorizations.DeleteGlobal);
        Call.SiteApi.Delete(Convert.ToInt32(gvSites.DataKeys[e.RowIndex].Values[0]));
        BindGrid();
    }


    protected void gvSites_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DropDownList ddlDps = null;

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ddlDps = e.Row.FindControl("ddlDpAdd") as DropDownList;
            PopulateClusterGroupsDdl(ddlDps);
          
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ddlDps = e.Row.FindControl("ddlDp") as DropDownList;
            if (ddlDps != null)
            {
                PopulateClusterGroupsDdl(ddlDps);
                ddlDps.SelectedValue = ((SiteEntity)(e.Row.DataItem)).DistributionPointId.ToString();
            }
        } 
    }
}