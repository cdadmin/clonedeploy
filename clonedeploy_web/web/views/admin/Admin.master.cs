using System;
using System.Web.UI;

namespace views.masters
{
    public partial class AdminMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (Request.QueryString["level"])
            {
                case "2":
                    Level1.Visible = false;
                    break;
                case "3":
                    Level1.Visible = false;
                    Level2.Visible = false;
                    break;
            }        
        }
    }
}