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
                return;
            }

            Level1.Visible = false;
            if (Request.QueryString["cat"] == "profiles")
                Level2.Visible = false;
            if (string.IsNullOrEmpty(Request.QueryString["profileid"]))
                Level4.Visible = false;
            else
            {
                Level3.Visible = false;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Delete This Image?";
            DisplayConfirm();
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            imagesBasePage.BllImage.DeleteImage(Image);

                Response.Redirect("~/views/images/search.aspx");
        }
    }
}