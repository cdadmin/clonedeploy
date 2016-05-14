using System;
using System.Web.UI;
using BasePages;

public partial class views_global_global : MasterBaseMaster
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["cat"] == "sub1")
        {
            Level1.Visible = false;
            Level3.Visible = false;
        }
        if (Request.QueryString["cat"] == "sub2")
        {
            Level1.Visible = false;
            Level2.Visible = false;
        }
    }
   
}
