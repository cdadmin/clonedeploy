using System;
using BasePages;

namespace views.dashboard
{
    public partial class Dashboard : PageBaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!BLL.CdVersion.FirstRunCompleted())
                Response.Redirect("~/views/login/firstrun.aspx");

            if (Request.QueryString["access"] == "denied")
                lblDenied.Text = "You Are Not Authorized For That Action";
        }

      
       
    }
}