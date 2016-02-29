using System;
using BasePages;
using Helpers;
using Models;

namespace views.masters
{
    public partial class ImageMaster : MasterBaseMaster
    {
        private Images imagesBasePage { get; set; }
        public Image Image { get; set; }

        public void Page_Load(object sender, EventArgs e)
        {
            imagesBasePage = (Page as Images);
            if (imagesBasePage != null) Image = imagesBasePage.Image;
            if (Image == null)
            {
                Level2.Visible = false;
                actions_left.Visible = false;
                return;
            }

            Level1.Visible = false;
            if (Settings.RequireImageApproval.ToLower() == "true" && Image.Approved != 1)
                approve.Visible = true;

            if (Request.QueryString["cat"] == "profiles")
            {
                Level2.Visible = false;
                btnDelete.Visible = false;
            }
            if (string.IsNullOrEmpty(Request.QueryString["profileid"]))
                Level4.Visible = false;
            else
            {
                Level3.Visible = false;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            imagesBasePage.RequiresAuthorizationOrManagedImage(Authorizations.DeleteImage, Image.Id);
            lblTitle.Text = "Delete This Image?";
            DisplayConfirm();
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            imagesBasePage.RequiresAuthorizationOrManagedImage(Authorizations.ApproveImage,Image.Id);
            Image.Approved = 1;
            PageBaseMaster.EndUserMessage = BLL.Image.UpdateImage(Image, Image.Name).IsValid
                ? "Successfully Approved Image"
                : "Could Not Approve Image";
            BLL.Image.SendImageApprovedEmail(Image.Id);
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            var result = BLL.Image.DeleteImage(Image);
            if (result.IsValid)
            {
                PageBaseMaster.EndUserMessage = "Successfully Deleted Image";
                Response.Redirect("~/views/images/search.aspx");
            }
            else
            {
                PageBaseMaster.EndUserMessage = result.Message;
            }
        }
    }
}