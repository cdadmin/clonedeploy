using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Helpers;

namespace views.masters
{
    public partial class SiteMaster : MasterPage
    {
        public void Page_Init(object sender, EventArgs e)
        {

            if (Settings.ForceSsL == "Yes")
            {
                if (!HttpContext.Current.Request.IsSecureConnection)
                {
                    var root = Request.Url.GetLeftPart(UriPartial.Authority);
                    root = root + Page.ResolveUrl("~/");
                    root = root.Replace("http://", "https://");
                    Response.Redirect(root);
                }
            }

            if (!Request.IsAuthenticated)
                Response.Redirect("~/", true);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            LogOut.Text = HttpContext.Current.User.Identity.Name;
            Page.MaintainScrollPositionOnPostBack = true;
        }

        protected void LogOut_OnClick(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Response.Redirect("~/", true);
        }
    }
}