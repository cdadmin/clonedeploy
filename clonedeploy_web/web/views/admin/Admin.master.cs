using System;
using System.Web.UI;

namespace views.masters
{
    public partial class AdminMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/views/admin/bootmenu.aspx?defaultmenu=true");
        }
    }
}