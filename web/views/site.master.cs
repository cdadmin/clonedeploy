using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Helpers;
using Security;

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

            //login attempt through query string
            //can't really say I recommend this but someone asked for it
            if (Request.QueryString["password"] != null)
            {            
                var auth = new Authenticate();
                var validationResult = auth.GlobalLogin(Request.QueryString["username"], Request.QueryString["password"], "Web");
                if (validationResult.Success)
                {
                    var cloneDeployUser = BLL.User.GetUser(Request.QueryString["username"]);
                    Session["CloneDeployUser"] = cloneDeployUser;
                    FormsAuthentication.SetAuthCookie(cloneDeployUser.Name, false);
                    Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path),true);
                }
            }

            if (!Request.IsAuthenticated)
                Response.Redirect("~/", true);
        }

        public void Page_Load(object sender, EventArgs e)
        {
           
        }

       
    }
}