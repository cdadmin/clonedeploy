using System;
using System.Web.UI;

namespace views.dashboard
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["access"] == "denied")
                lblDenied.Text = "You Are Not Authorized For That Action";
            


        }


       
    }
}