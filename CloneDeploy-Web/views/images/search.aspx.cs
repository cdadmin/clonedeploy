using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.images
{
    public partial class ImageSearch : Images
    {
        protected void btnApproveImage_OnClick(object sender, EventArgs e)
        {
            var approveCount = 0;
            foreach (GridViewRow row in gvImages.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvImages.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                var image = Call.ImageApi.Get(Convert.ToInt32(dataKey.Value));
                RequiresAuthorizationOrManagedImage(AuthorizationStrings.ApproveImage, image.Id);
                image.Approved = 1;
                if (Call.ImageApi.Put(image.Id, image).Success)
                {
                    approveCount++;
                    Call.ImageApi.SendImageApprovedMail(image.Id);
                }
            }
            EndUserMessage = "Successfully Approved " + approveCount + " Images";
            PopulateGrid();
        }

        protected void btnHds_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            var row = (GridViewRow) control.Parent.Parent;
            var gvHDs = (GridView) row.FindControl("gvHDs");
            var imageId = ((HiddenField) row.FindControl("HiddenID")).Value;
            var btn = (LinkButton) row.FindControl("btnHDs");

            if (gvHDs.Visible == false)
            {
                var td = row.FindControl("tdHds");
                td.Visible = true;
                gvHDs.Visible = true;
                var schemaRequestOptions = new ImageSchemaRequestDTO();
                schemaRequestOptions.image = Call.ImageApi.Get(Convert.ToInt32(imageId));
                schemaRequestOptions.imageProfile = null;
                schemaRequestOptions.schemaType = "deploy";

                gvHDs.DataSource = Call.ImageSchemaApi.GetHardDrives(schemaRequestOptions);
                gvHDs.DataBind();
                btn.Text = "-";
            }
            else
            {
                var td = row.FindControl("tdHds");
                td.Visible = false;
                gvHDs.Visible = false;
                btn.Text = "+";
            }

            foreach (GridViewRow hdrow in gvHDs.Rows)
            {
                var selectedHd = hdrow.RowIndex;
                var lbl = hdrow.FindControl("lblHDSize") as Label;
                if (lbl != null)
                    lbl.Text = Call.ImageApi.GetImageSizeOnServer(row.Cells[5].Text, selectedHd.ToString());
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.DeleteImage);
            var deleteCount = 0;
            foreach (GridViewRow row in gvImages.Rows)
            {
                var cb = (CheckBox) row.FindControl("chkSelector");
                if (cb == null || !cb.Checked) continue;
                var dataKey = gvImages.DataKeys[row.RowIndex];
                if (dataKey == null) continue;
                if (Call.ImageApi.Delete(Convert.ToInt32(dataKey.Value)).Success) deleteCount++;
            }
            EndUserMessage = "Successfully Deleted " + deleteCount + " Images";

            PopulateGrid();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkAll(gvImages);
        }

        protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateGrid();
            var listImages = (List<ImageWithDate>) gvImages.DataSource;
            switch (e.SortExpression)
            {
                case "Name":
                    listImages = GetSortDirection(e.SortExpression) == "Asc"
                        ? listImages.OrderBy(h => h.Name).ToList()
                        : listImages.OrderByDescending(h => h.Name).ToList();
                    break;
                case "LastUsed":
                    listImages = GetSortDirection(e.SortExpression) == "Asc"
                        ? listImages.OrderBy(h => h.LastUsed).ToList()
                        : listImages.OrderByDescending(h => h.LastUsed).ToList();
                    break;
            }
            gvImages.DataSource = listImages;
            gvImages.DataBind();
            PopulateSizes();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateGrid();
        }

        protected void PopulateGrid()
        {
            gvImages.DataSource = Call.ImageApi.Get(int.MaxValue, txtSearch.Text).OrderBy(x => x.Name).ToList();
            gvImages.DataBind();
            lblTotal.Text = gvImages.Rows.Count + " Result(s) / " + Call.ImageApi.GetCount() + " Total Image(s)";
            PopulateSizes();
        }

        protected void PopulateSizes()
        {
            foreach (GridViewRow row in gvImages.Rows)
            {
                var lbl = row.FindControl("lblSize") as Label;
                if (lbl != null) lbl.Text = Call.ImageApi.GetImageSizeOnServer(row.Cells[5].Text, "0");
            }
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}