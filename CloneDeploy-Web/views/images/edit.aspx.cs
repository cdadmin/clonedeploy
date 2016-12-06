

using System;
using BasePages;

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
            RequiresAuthorizationOrManagedImage(Authorizations.UpdateImage, Image.Id);
            var image = Image;

            var currentName = (string) (ViewState["currentName"]);
            image.Name = txtImageName.Text;
            image.Os = "";
            image.Description = txtImageDesc.Text;
            image.Enabled = chkEnabled.Checked ? 1 : 0;
            image.Protected = chkProtected.Checked ? 1 : 0;
            image.IsVisible = chkVisible.Checked ? 1 : 0;

            var result = BLL.Image.UpdateImage(image, currentName);
            EndUserMessage = result.Success ? "Successfully Updated Image" : result.Message;
        }

        protected void PopulateForm()
        {
            ViewState["currentName"] = Image.Name;
            chkEnabled.Checked = Convert.ToBoolean(Image.Enabled);
            txtImageName.Text = Image.Name;
            txtImageDesc.Text = Image.Description;
            ddlImageType.Text = Image.Type;
            ddlEnvironment.Text = Image.Environment;

            if (Image.Environment == "winpe")
            {
                imageType.Visible = false;
            }
            else if (Image.Environment == "macOS")
            {
                osxImageType.Visible = false;
                imageType.Visible = false;
                ddlOsxImageType.Text = Image.OsxType;
                if (Image.OsxType == "thin")
                {
                    thinImage.Visible = false;
                    ddlThinOS.DataSource = Utility.GetThinImages();
                    ddlThinOS.DataBind();
                    ddlThinRecovery.DataSource = Utility.GetThinImages();
                    ddlThinRecovery.DataBind();
                    
                    ddlThinOS.Text = Image.OsxThinOs;
                    ddlThinRecovery.Text = Image.OsxThinRecovery;
                }
            }
            if (Image.Protected == 1)
                chkProtected.Checked = true;
            if (Image.IsVisible == 1)
                chkVisible.Checked = true;

            //Image types can't be changed after they are created
            ddlImageType.Enabled = false;
            ddlEnvironment.Enabled = false;
            ddlOsxImageType.Enabled = false;

        }
    }
}