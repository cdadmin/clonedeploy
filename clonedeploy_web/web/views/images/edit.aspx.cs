

using System;
using System.IO;
using BasePages;
using Helpers;

namespace views.images
{
    public partial class ImageEdit : Images
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

   
        protected void btnUpdateImage_Click(object sender, EventArgs e)
        {
            var image = Image;

            var currentName = (string) (ViewState["currentName"]);
            image.Name = txtImageName.Text;
            image.Os = "";
            image.Description = txtImageDesc.Text;
            image.Protected = chkProtected.Checked ? 1 : 0;
            image.IsVisible = chkVisible.Checked ? 1 : 0;


            BLL.Image.UpdateImage(image, currentName);
        }

        protected void PopulateForm()
        {
            ViewState["currentName"] = Image.Name;

            txtImageName.Text = Image.Name;
            txtImageDesc.Text = Image.Description;
            ddlImageOS.Text = Image.Os;
            ddlImageType.Text = Image.Type;
            if (Image.Protected == 1)
                chkProtected.Checked = true;
            if (Image.IsVisible == 1)
                chkVisible.Checked = true;

           
        }
    }
}