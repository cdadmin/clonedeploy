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
    public partial class edit : Admin
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

        protected void buttonUpdateProfile_OnClick(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateAdmin);
            var netBootProfile = Call.NetBootProfileApi.Get(Convert.ToInt32(Request.QueryString["netbootid"]));

            netBootProfile.Name = txtDisplayName.Text;
            netBootProfile.Ip = txtIp.Text;

            var result = Call.NetBootProfileApi.Put(netBootProfile.Id, netBootProfile);
            if (result.Success)
            {
                //Clear all existing nbi entries for this profile
                Call.NetBootProfileApi.DeleteProfileNbiEntries(netBootProfile.Id);
                var dt = (DataTable) ViewState["nbiEntries"];
                if (dt.Rows.Count > 0)
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
                        EndUserMessage = "Successfully Updated NetBoot Profile";
                    }
                    else
                    {
                        EndUserMessage = nbiResult.ErrorMessage;
                    }
                }
            }
            else
            {
                EndUserMessage = result.ErrorMessage;
            }
            PopulateForm();
            BindGrid();
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
            if (!IsPostBack)
            {
                PopulateForm();
                BindGrid();
            }
        }

        protected void PopulateForm()
        {
            var netBootProfileId = Convert.ToInt32(Request.QueryString["netbootid"]);
            var netBootProfile = Call.NetBootProfileApi.Get(netBootProfileId);
            txtDisplayName.Text = netBootProfile.Name;
            txtIp.Text = netBootProfile.Ip;
            var nbiEntries = Call.NetBootProfileApi.GetProfileNbiEntries(netBootProfileId);
            if (nbiEntries.Count == 0) return;

            var appleVendorDto = Call.WorkflowApi.GetAppleVendorString(netBootProfile.Ip);
            if (appleVendorDto.Success)
            {
                directions.Text = appleVendorDto.Instructions;
                vendorString.Text = appleVendorDto.VendorString;
            }
            else
                EndUserMessage = appleVendorDto.ErrorMessage;

            var dt = new DataTable();
            dt.Columns.Add("ImageId");
            dt.Columns.Add("Name");
            foreach (var nbiEntry in nbiEntries)
            {
                var dataRow = dt.NewRow();
                dataRow[0] = nbiEntry.NbiId;
                dataRow[1] = nbiEntry.NbiName;
                dt.Rows.Add(dataRow);
            }
            ViewState["nbiEntries"] = dt;
        }
    }
}