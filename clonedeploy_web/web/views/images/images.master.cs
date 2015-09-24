using System;
using System.Activities.Statements;
using System.Web.UI;
using Global;
using Models;

namespace views.masters
{
    public partial class ImageMaster : MasterPage
    {
        public Image Image { get { return ReadProfile(); } }
        private readonly BLL.Image _bllImage = new BLL.Image();

        public void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["imageid"]))
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
            Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                true);
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            _bllImage.DeleteImage(Image);
            if (Utility.Message.Contains("Successfully"))
                Response.Redirect("~/views/images/search.aspx");
        }

        private Image ReadProfile()
        {
            return _bllImage.GetImage(Convert.ToInt32(Request.QueryString["imageid"]));
        }
    }
}