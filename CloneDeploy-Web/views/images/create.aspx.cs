using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.images
{
    public partial class ImageCreate : Images
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            RequiresAuthorization(AuthorizationStrings.CreateImage);
            var image = new ImageEntity
            {
                Name = txtImageName.Text,
                Os = "",
                Environment = ddlEnvironment.Text,
                Description = txtImageDesc.Text,
                Protected = chkProtected.Checked ? 1 : 0,
                IsVisible = chkVisible.Checked ? 1 : 0,
                Enabled = 1,
                ClassificationId = Convert.ToInt32(ddlClassification.SelectedValue)
            };

            image.Type = ddlEnvironment.Text == "winpe" ? "File" : ddlImageType.Text;

            var result = Call.ImageApi.Post(image);
            if (result.Success)
            {
                EndUserMessage = "Successfully Added Image";
                Response.Redirect("~/views/images/edit.aspx?imageid=" + result.Id);
            }
            else
            {
                EndUserMessage = result.ErrorMessage;
            }
        }

        protected void ddlEnvironment_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEnvironment.Text == "winpe")
            {
                imageType.Visible = false;
            }
            else
            {
                imageType.Visible = true;
            }
        }

     

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateImageClassifications(ddlClassification);

            chkVisible.Checked = true;
        }
    }
}