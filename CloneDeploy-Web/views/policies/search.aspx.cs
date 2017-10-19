using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloneDeploy_Web.views.policies
{
    public partial class search : System.Web.UI.Page
    {
        protected string token { get { return Request.Cookies["cdtoken"].Value; } }
        protected string baseurl { get { return Request.Cookies["cdBaseUrl"].Value; } }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}