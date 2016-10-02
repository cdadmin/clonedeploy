using System;
using BasePages;

public partial class views_global_global : MasterBaseMaster
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["manifestid"] != null)
            Level1.Visible = false;
    }
   
}
