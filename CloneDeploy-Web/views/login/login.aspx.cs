using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloneDeploy_ApiCalls;
using CloneDeploy_Web.Models;
using Helpers;
using Newtonsoft.Json;
using RestSharp;
using Security;

namespace views.login
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.RemoveAll();
                Session.Abandon();
              
                FormsAuthentication.SignOut();
                //http://stackoverflow.com/questions/6635349/how-to-delete-cookies-in-asp-net-website
                if (HttpContext.Current != null)
                {
                    int cookieCount = HttpContext.Current.Request.Cookies.Count;
                    for (var i = 0; i < cookieCount; i++)
                    {
                        var cookie = HttpContext.Current.Request.Cookies[i];
                        if (cookie != null)
                        {
                            var cookieName = cookie.Name;
                            var expiredCookie = new System.Web.HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                            HttpContext.Current.Response.Cookies.Add(expiredCookie); // overwrite it
                        }
                    }

                    // clear cookies server side
                    HttpContext.Current.Request.Cookies.Clear();
                }

                if (Request.QueryString["session"] == "expired")
                    SessionExpired.Visible = true;
            }
            else
            {
                SessionExpired.Visible = false;
            }
            
          
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

            if (Request.IsAuthenticated)
            {
                Response.Redirect("~/views/dashboard/dash.aspx");
            }
        }

        protected void CrucibleLogin_Authenticate(object sender, AuthenticateEventArgs e)
        {
          //Get token
            var token = new APICall().TokenApi.Get(CrucibleLogin.UserName, CrucibleLogin.Password);
            if (token == null)
            {
                lblError.Text = "Unknown API Error";
                return;
            }

            HttpContext.Current.Response.Cookies.Add(new System.Web.HttpCookie("cdtoken")
            {
                Value = token.access_token,
                HttpOnly = true
            });
        
            if (token.access_token != null)
            {
                //verify token is valid         
                var result = new APICall().CloneDeployUserApi.GetForLogin(token.user_id);
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
                    var cloneDeployUser = JsonConvert.DeserializeObject<CloneDeployUser>(result.ObjectJson);
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