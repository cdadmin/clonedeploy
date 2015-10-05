using System;
using BasePages;
using Helpers;
using Models;
using Security;

namespace views.images
{
    public partial class ImageCreate : Images
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (new Authorize().IsInMembership("User"))
                Response.Redirect("~/views/dashboard/dash.aspx?access=denied");

            chkVisible.Checked = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var image = new Image
            {
                Name = txtImageName.Text,
                Os = ddlImageOS.Text,
                Description = txtImageDesc.Text,
                Protected = chkProtected.Checked ? 1 : 0,
                IsVisible = chkVisible.Checked ? 1 : 0
            };

           
            if (BllImage.ValidateImageData(image)) BllImage.AddImage(image);
            if (Message.Text.Contains("Successfully"))
                Response.Redirect("~/views/images/edit.aspx?imageid=" + image.Id);

        }
    }
}