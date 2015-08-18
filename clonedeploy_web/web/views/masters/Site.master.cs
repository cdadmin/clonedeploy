using System;
using System.Web;
using System.Web.UI;
using Global;

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
            Page.MaintainScrollPositionOnPostBack = true;
        }

        public void Msgbox(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            const string msgType = "showSuccessToast";
            Page.ClientScript.RegisterStartupScript(GetType(), "msgBox",
                "$(function() { $().toastmessage('" + msgType + "', " + "\"" + message + "\"); });", true);
            Session.Remove("Message");
        } 
    }
}