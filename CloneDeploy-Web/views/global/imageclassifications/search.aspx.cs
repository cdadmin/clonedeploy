using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.imageclassifications
{
    public partial class search : Global
    {
        protected void BindGrid()
        {
            gvImageClass.DataSource = Call.ImageClassificationApi.Get();
            gvImageClass.DataBind();

            if (gvImageClass.Rows.Count == 0)
            {
                var obj = new List<ImageClassificationEntity>();
                obj.Add(new ImageClassificationEntity());
                gvImageClass.DataSource = obj;
                gvImageClass.DataBind();

                gvImageClass.Rows[0].Cells.Clear();
                gvImageClass.Rows[0].Cells.Add(new TableCell());

                gvImageClass.Rows[0].Cells[0].Text = "No Image Classifications Have Been Created";
            }
        }

        protected void Insert(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.CreateGlobal);
            var gvRow = (GridViewRow) (sender as Control).Parent.Parent;
            var imageClass = new ImageClassificationEntity
            {
                Name = ((TextBox) gvRow.FindControl("txtNameAdd")).Text
            };

            Call.ImageClassificationApi.Post(imageClass);
            BindGrid();
        }

        protected void OnRowCancelingEdit(object sender, EventArgs e)
        {
            gvImageClass.EditIndex = -1;
            BindGrid();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.DeleteGlobal);
            Call.ImageClassificationApi.Delete(Convert.ToInt32(gvImageClass.DataKeys[e.RowIndex].Values[0]));
            BindGrid();
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvImageClass.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.UpdateGlobal);
            var gvRow = gvImageClass.Rows[e.RowIndex];
            var imageClassification = new ImageClassificationEntity
            {
                Id = Convert.ToInt32(gvImageClass.DataKeys[e.RowIndex].Values[0]),
                Name = ((TextBox) gvRow.FindControl("txtName")).Text
            };
            Call.ImageClassificationApi.Put(imageClassification.Id, imageClassification);

            gvImageClass.EditIndex = -1;
            this.BindGrid();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindGrid();
            else
            {
                if (gvImageClass.Rows[0].Cells[0].Text == "No Image Classifications Have Been Created")
                {
                    gvImageClass.Rows[0].Cells.Clear();
                    gvImageClass.Rows[0].Cells.Add(new TableCell());
                    gvImageClass.Rows[0].Cells[0].Text = "No Image Classifications Have Been Created";
                }
            }
        }

        protected void search_Changed(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}