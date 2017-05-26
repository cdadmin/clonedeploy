using System;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global
{
    public partial class views_global_global : MasterBaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["manifestid"] != null)
                Level1.Visible = false;
        }
    }
}