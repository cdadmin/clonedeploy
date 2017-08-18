using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.admin.netboot
{
    public partial class create : Admin
    {
        protected void BindGrid()
        {
            DataTable dt;
            if (ViewState["nbiEntries"] != null)
            {
                dt = (DataTable) ViewState["nbiEntries"];
                gvNetBoot.DataSource = dt;
                gvNetBoot.DataBind();
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("ImageId");
                dt.Columns.Add("Name");
                var dataRow = dt.NewRow();
                dt.Rows.Add(dataRow);
                gvNetBoot.DataSource = dt;
                gvNetBoot.DataBind();
                gvNetBoot.Rows[0].Cells.Clear();
                gvNetBoot.Rows[0].Cells.Add(new TableCell());
                gvNetBoot.Rows[0].Cells[0].Text = "";
                var emptyTable = new DataTable();
                emptyTable.Columns.Add("ImageId");
                emptyTable.Columns.Add("Name");
                emptyTable.Clear();
                ViewState["nbiEntries"] = emptyTable;
            }
        }

        protected void btnAdd1_OnClick(object sender, EventArgs e)
        {
            var gvRow = (GridViewRow) (sender as Control).Parent.Parent;
            var id = ((TextBox) gvRow.FindControl("txtIdAdd")).Text;
            var name = ((TextBox) gvRow.FindControl("txtNameAdd")).Text;
            var dt = (DataTable) ViewState["nbiEntries"];
            var dataRow = dt.NewRow();
            dataRow[0] = id;
            dataRow[1] = name;
            dt.Rows.Add(dataRow);
            ViewState["nbiEntries"] = dt;
            BindGrid();
        }

        protected void buttonAddNetBoot_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateAdmin);
            var netBootProfile = new NetBootProfileEntity();
            netBootProfile.Name = txtDisplayName.Text;
            netBootProfile.Ip = txtIp.Text;
            var result = Call.NetBootProfileApi.Post(netBootProfile);
            if (result.Success)
            {
                var nbiList = new List<NbiEntryEntity>();
                foreach (GridViewRow row in gvNetBoot.Rows)
                {
                    var nbiId = Convert.ToInt32(((Label) row.FindControl("lblImageId")).Text);
                    var nbiName = ((Label) row.FindControl("lblName")).Text;
                    nbiList.Add(new NbiEntryEntity {NbiId = nbiId, ProfileId = result.Id, NbiName = nbiName});
                }
                var nbiResult = Call.NbiEntryApi.Post(nbiList);
                if (nbiResult.Success)
                {
                    EndUserMessage = "Successfully Created NetBoot Profile";
                    Response.Redirect("~/views/admin/netboot/edit.aspx?level=2&netbootid=" + result.Id);
                }
                else
                {
                    EndUserMessage = nbiResult.ErrorMessage;
                    Response.Redirect("~/views/admin/netboot/edit.aspx?level=2&netbootid=" + result.Id);
                }
            }
            else
            {
                EndUserMessage = result.ErrorMessage;
            }
        }

        protected void OnRowCancelingEdit(object sender, EventArgs e)
        {
            gvNetBoot.EditIndex = -1;
            BindGrid();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var dt = (DataTable) ViewState["nbiEntries"];
            dt.Rows[e.RowIndex].Delete();
            gvNetBoot.DataSource = dt;
            gvNetBoot.DataBind();
            if (gvNetBoot.Rows.Count == 0)
            {
                ViewState["nbiEntries"] = null;
                BindGrid();
            }
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvNetBoot.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var gvRow = gvNetBoot.Rows[e.RowIndex];
            var dt = (DataTable) ViewState["nbiEntries"];
            dt.Rows[e.RowIndex]["ImageId"] = ((TextBox) gvRow.FindControl("txtIdEdit")).Text;
            dt.Rows[e.RowIndex]["Name"] = ((TextBox) gvRow.FindControl("txtNameEdit")).Text;
            ViewState["nbiEntries"] = dt;
            gvNetBoot.EditIndex = -1;
            BindGrid();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindGrid();
        }
    }
}