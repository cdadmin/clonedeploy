using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;
using Models;
using Newtonsoft.Json;
using RestSharp;
using Security;

namespace views.login
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();

            if (Request.QueryString["session"] == "expired")
                SessionExpired.Visible = true;

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

            HttpContext.Current.Response.Cookies.Add(new System.Web.HttpCookie("clonedeploy_token")
            {
                Value = token.access_token,
                HttpOnly = true
            });
           
            if (token.access_token != null)
            {
                //verify token is valid
                var cloneDeployUser = new APICall().CloneDeployUserApi.GetByName(CrucibleLogin.UserName);
                cloneDeployUser.Salt = "";
                cloneDeployUser.Password = "";

                Session["CloneDeployUser"] = cloneDeployUser;
                e.Authenticated = true; 
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