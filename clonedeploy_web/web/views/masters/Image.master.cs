using System;
using System.Web.UI;
using Global;
using Models;

namespace views.masters
{
    public partial class ImageMaster : MasterPage
    {
        public Image Image { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["imageid"])) return;
            Image = new Image { Id = Request.QueryString["imageid"] };
            Image.Read();

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
            Image.Delete();
            if (Utility.Message.Contains("Successfully"))
                Response.Redirect("~/views/images/search.aspx");
            else
                Master.Msgbox(Utility.Message);
        }     
    }
}