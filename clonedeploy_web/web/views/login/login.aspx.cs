/*  
    CrucibleWDS A Windows Deployment Solution
    Copyright (C) 2011  Jon Dolny

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/.
 */

using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Global;
using Models;
using Security;

namespace views.login
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

            if (Request.IsAuthenticated)
                Response.Redirect("~/views/dashboard/dash.aspx");
        }

        protected void CrucibleLogin_Authenticate(object sender, AuthenticateEventArgs e)
        {
            GetIp();
            var auth = new Authenticate();

            var result = auth.GlobalLogin(CrucibleLogin.UserName, CrucibleLogin.Password, "Web");
            if ((result))
            {
                e.Authenticated = true;
            }
            else
            {
                e.Authenticated = false;
                lblError.Visible = true;
            }
        }

        private void GetIp()
        {
            string ipString;
            if (string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                ipString = Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                ipString =
                    Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(",".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries)
                        .FirstOrDefault();
            }

            IPAddress result = null;
            if (ipString != null && !IPAddress.TryParse(ipString, out result))
                result = IPAddress.None;

            if (result != null)
                Session["ip_address"] = result.ToString();
        }
    }
}