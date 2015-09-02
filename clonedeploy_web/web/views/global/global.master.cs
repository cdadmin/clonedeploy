using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class views_global_global : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["cat"] == "sub1")
            Level1.Visible = false;
    }
    protected void OkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/views/admin/bootmenu.aspx?defaultmenu=true");
    }
}
