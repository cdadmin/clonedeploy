using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.admin.server
{
    public partial class alternateips : Admin
    {
        protected void BindGrid()
        {
            gvIps.DataSource = Call.AlternateServerIpApi.Get();
            gvIps.DataBind();

            if (gvIps.Rows.Count == 0)
            {
                var obj = new List<AlternateServerIpEntity>();
                obj.Add(new AlternateServerIpEntity());
                gvIps.DataSource = obj;
                gvIps.DataBind();

                gvIps.Rows[0].Cells.Clear();
                gvIps.Rows[0].Cells.Add(new TableCell());

                gvIps.Rows[0].Cells[0].Text = "No Alternate Server Ips Have Been Created";
            }
        }

        protected void Insert(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateAdmin);
            var gvRow = (GridViewRow) (sender as Control).Parent.Parent;
            var alternateIp = new AlternateServerIpEntity
            {
                Ip = ((TextBox) gvRow.FindControl("txtIpAdd")).Text,
                ApiUrl = ((TextBox) gvRow.FindControl("txtApiAdd")).Text
            };

            Call.AlternateServerIpApi.Post(alternateIp);
            BindGrid();
        }

        protected void OnRowCancelingEdit(object sender, EventArgs e)
        {
            gvIps.EditIndex = -1;
            BindGrid();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateAdmin);
            Call.AlternateServerIpApi.Delete(Convert.ToInt32(gvIps.DataKeys[e.RowIndex].Values[0]));
            BindGrid();
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvIps.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateAdmin);
            var gvRow = gvIps.Rows[e.RowIndex];
            var alternateIp = new AlternateServerIpEntity
            {
                Id = Convert.ToInt32(gvIps.DataKeys[e.RowIndex].Values[0]),
                Ip = ((TextBox) gvRow.FindControl("txtIp")).Text,
                ApiUrl = ((TextBox) gvRow.FindControl("txtApi")).Text
            };
            Call.AlternateServerIpApi.Put(alternateIp.Id, alternateIp);

            gvIps.EditIndex = -1;
            this.BindGrid();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindGrid();
            else
            {
                if (gvIps.Rows[0].Cells[0].Text == "No Alternate Server Ip's Have Been Created")
                {
                    gvIps.Rows[0].Cells.Clear();
                    gvIps.Rows[0].Cells.Add(new TableCell());
                    gvIps.Rows[0].Cells[0].Text = "No Alternate Server Ip's Have Been Created";
                }
            }
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}