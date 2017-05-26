using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.images
{
    public partial class ImageMaster : MasterBaseMaster
    {
        public ImageEntity Image { get; set; }
        private Images imagesBasePage { get; set; }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            imagesBasePage.RequiresAuthorizationOrManagedImage(AuthorizationStrings.ApproveImage, Image.Id);
            Image.Approved = 1;
            PageBaseMaster.EndUserMessage = imagesBasePage.Call.ImageApi.Put(Image.Id, Image).Success
                ? "Successfully Approved Image"
                : "Could Not Approve Image";
            imagesBasePage.Call.ImageApi.SendImageApprovedMail(Image.Id);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            imagesBasePage.RequiresAuthorizationOrManagedImage(AuthorizationStrings.DeleteImage, Image.Id);
            lblTitle.Text = "Delete This Image?";
            DisplayConfirm();
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            var result = imagesBasePage.Call.ImageApi.Delete(Image.Id);
            if (result.Success)
            {
                PageBaseMaster.EndUserMessage = "Successfully Deleted Image";
                Response.Redirect("~/views/images/search.aspx");
            }
            else
            {
                PageBaseMaster.EndUserMessage = result.ErrorMessage;
            }
        }

        public void Page_Load(object sender, EventArgs e)
        {
            imagesBasePage = Page as Images;
            if (imagesBasePage != null) Image = imagesBasePage.Image;
            if (Image == null)
            {
                Level2.Visible = false;
                btnDelete.Visible = false;
                return;
            }

            Level1.Visible = false;
            if (GetSetting(SettingStrings.RequireImageApproval).ToLower() == "true" && Image.Approved != 1)
                btnApproveImage.Visible = true;

            if (Request.QueryString["cat"] == "profiles")
            {
                btnDelete.Visible = false;
            }
        }
    }
}