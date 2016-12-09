using System;
using System.Web.UI;

namespace views.masters
{
    public partial class SiteMaster : MasterPage
    {
        public void Page_Init(object sender, EventArgs e)
        {

            if (!Request.IsAuthenticated)
                Response.Redirect("~/", true);
        }

        public void Page_Load(object sender, EventArgs e)
        {
           
        }

       
    }
}