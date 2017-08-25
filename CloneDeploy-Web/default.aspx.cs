using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_ApiCalls;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using Newtonsoft.Json;

namespace CloneDeploy_Web
{
    public partial class Default : Page
    {
        private void ClearSession()
        {
            Session.RemoveAll();
            Session.Abandon();

            FormsAuthentication.SignOut();
            //http://stackoverflow.com/questions/6635349/how-to-delete-cookies-in-asp-net-website
            if (HttpContext.Current != null)
            {
                var cookieCount = HttpContext.Current.Request.Cookies.Count;
                for (var i = 0; i < cookieCount; i++)
                {
                    var cookie = HttpContext.Current.Request.Cookies[i];
                    if (cookie != null)
                    {
                        var cookieName = cookie.Name;
                        var expiredCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                        HttpContext.Current.Response.Cookies.Add(expiredCookie); // overwrite it
                    }
                }

                // clear cookies server side
                HttpContext.Current.Request.Cookies.Clear();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlApplicationServers.DataSource = ApplicationServers.ServerList;
                ddlApplicationServers.DataValueField = "BaseUrl";
                ddlApplicationServers.DataTextField = "DisplayName";
                ddlApplicationServers.DataBind();

                ClearSession();

                if (Request.QueryString["session"] == "expired")
                    SessionExpired.Visible = true;
            }
            else
            {
                SessionExpired.Visible = false;
            }

            if (Request.IsAuthenticated)
            {
                Response.Redirect("~/views/dashboard/dash.aspx");
            }
        }

        protected void WebLogin_Authenticate(object sender, AuthenticateEventArgs e)
        {
            
            //ApplicationServers._baseApiUrl = ddlApplicationServers.SelectedValue;
            HttpCookie baseUrlCookie = Request.Cookies["cdBaseUrl"];
            if (baseUrlCookie == null)
            {
                baseUrlCookie = new HttpCookie("cdBaseUrl")
            {
                Value = ddlApplicationServers.SelectedValue,
                HttpOnly = true
            };
                Response.Cookies.Add(baseUrlCookie);
            }
            else
            {
                baseUrlCookie.Value = ddlApplicationServers.SelectedValue;
                Response.Cookies.Add(baseUrlCookie);
            }

            //Get token
            var token = new APICall().TokenApi.Get(WebLogin.UserName, WebLogin.Password);
            if (token == null)
            {
                lblError.Text = "Unknown API Error";
                return;
            }

            HttpCookie tokenCookie = Request.Cookies["cdtoken"];
            if (tokenCookie == null)
            {
                tokenCookie = new HttpCookie("cdtoken")
                {
                    Value = token.access_token,
                    HttpOnly = true
                };
                Response.Cookies.Add(tokenCookie);
            }
            else
            {
                tokenCookie.Value = token.access_token;
                Response.Cookies.Add(tokenCookie);
            }

            if (token.access_token != null)
            {
                //verify token is valid         
                var result = new APICall().CloneDeployUserApi.GetForLogin(WebLogin.UserName);
                if (result == null)
                {
                    lblError.Text = "Could Not Contact Application API";
                    e.Authenticated = false;
                    lblError.Visible = true;
                }
                else if (!result.Success)
                {
                    lblError.Text = result.ErrorMessage == "Forbidden"
                        ? "Token Does Not Match Requested User"
                        : result.ErrorMessage;
                    e.Authenticated = false;
                    lblError.Visible = true;
                }
                else if (result.Success)
                {
                    var cloneDeployUser = JsonConvert.DeserializeObject<CloneDeployUserEntity>(result.ObjectJson);
                    Session["CloneDeployUser"] = cloneDeployUser;
                    e.Authenticated = true;
                }
                else
                {
                    e.Authenticated = false;
                    lblError.Text = result.ErrorMessage;
                    lblError.Visible = true;
                }
            }
            else
            {
                e.Authenticated = false;
                lblError.Text = token.error_description;
                lblError.Visible = true;
            }
        }
    }
}