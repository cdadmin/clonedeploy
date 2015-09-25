using System;
using System.Web.UI;

namespace views.masters
{
    public partial class AdminMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["cat"] == "sub1")
                Level1.Visible = false;
        }

     
    }
}