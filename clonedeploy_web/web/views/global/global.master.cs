using System;
using System.Web.UI;

public partial class views_global_global : MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["cat"] == "sub1")
            Level1.Visible = false;
    }
   
}
