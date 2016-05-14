using System;
using BasePages;
using Helpers;
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
            RequiresAuthorization(Authorizations.CreateImage);
            var image = new Image
            {
                Name = txtImageName.Text,
                Os = "",
                Environment = ddlEnvironment.Text,
                Description = txtImageDesc.Text,
                Protected = chkProtected.Checked ? 1 : 0,
                IsVisible = chkVisible.Checked ? 1 : 0,
                Enabled = 1
            };

            image.Type = ddlEnvironment.Text == "osx" ? "Block" : ddlImageType.Text;
            if (ddlEnvironment.Text == "osx")
            {
                image.OsxType = ddlOsxImageType.Text;
                if (image.OsxType == "thin")
                {
                    image.OsxThinOs = ddlThinOS.Text;
                    image.OsxThinRecovery = ddlThinRecovery.Text;
                }
            }
           
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

        protected void ddlEnvironment_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEnvironment.Text == "osx")
            {
                imageType.Visible = false;
                osxImageType.Visible = true;
            }
            else
            {
                imageType.Visible = true;
                osxImageType.Visible = false;
                thinImage.Visible = false;
            }
        }

        protected void ddlOsxImageType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOsxImageType.Text == "thin")
            {
                thinImage.Visible = true;
                ddlThinOS.DataSource = Utility.GetThinImages();
                ddlThinOS.DataBind();
                ddlThinRecovery.DataSource = Utility.GetThinImages();
                ddlThinRecovery.DataBind();
            }
            else
            {
                thinImage.Visible = false;
            }
        }
    }
}