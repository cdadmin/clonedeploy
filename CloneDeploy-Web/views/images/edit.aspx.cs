using System;
using CloneDeploy_Common;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.images
{
    public partial class ImageEdit : Images
    {
        protected void btnUpdateImage_Click(object sender, EventArgs e)
        {
            RequiresAuthorizationOrManagedImage(AuthorizationStrings.UpdateImage, Image.Id);
            var image = Image;

            var currentName = (string) ViewState["currentName"];
            image.Name = txtImageName.Text;
            image.Os = "";
            image.Description = txtImageDesc.Text;
            image.Enabled = chkEnabled.Checked ? 1 : 0;
            image.Protected = chkProtected.Checked ? 1 : 0;
            image.IsVisible = chkVisible.Checked ? 1 : 0;
            image.ClassificationId = Convert.ToInt32(ddlClassification.SelectedValue);
            var result = Call.ImageApi.Put(image.Id, image);
            EndUserMessage = result.Success ? "Successfully Updated Image" : result.ErrorMessage;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) PopulateForm();
        }

        protected void PopulateForm()
        {
            ViewState["currentName"] = Image.Name;
            PopulateImageClassifications(ddlClassification);
            chkEnabled.Checked = Convert.ToBoolean(Image.Enabled);
            txtImageName.Text = Image.Name;
            txtImageDesc.Text = Image.Description;
            ddlImageType.Text = Image.Type;
            ddlEnvironment.Text = Image.Environment;
            ddlClassification.SelectedValue = Image.ClassificationId.ToString();

            if (Image.Environment == "winpe")
            {
                imageType.Visible = false;
            }
          
            if (Image.Protected == 1)
                chkProtected.Checked = true;
            if (Image.IsVisible == 1)
                chkVisible.Checked = true;

            //Image types can't be changed after they are created
            ddlImageType.Enabled = false;
            ddlEnvironment.Enabled = false;
        }
    }
}