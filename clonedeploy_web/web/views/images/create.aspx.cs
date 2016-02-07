using System;
using BasePages;
using Models;

namespace views.images
{
    public partial class ImageCreate : Images
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            

            chkVisible.Checked = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var image = new Image
            {
                Name = txtImageName.Text,
                Os = "",
                Description = txtImageDesc.Text,
                Protected = chkProtected.Checked ? 1 : 0,
                IsVisible = chkVisible.Checked ? 1 : 0,
                Type = ddlImageType.Text
            };

           
            var result = BLL.Image.AddImage(image);
            if (result.IsValid)
            {
                EndUserMessage = "Successfully Added Image";
                Response.Redirect("~/views/images/edit.aspx?imageid=" + image.Id);
            }
            else
            {
                EndUserMessage = result.Message;
            }

        }
    }
}