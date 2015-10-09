using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BasePages;

public partial class views_computers_bootmenu_bootmenu : BasePages.MasterBaseMaster
{
    private Computers computerBasePage { get; set; }
    public Models.Computer Computer { get; set; }

    public void Page_Load(object sender, EventArgs e)
    {
        computerBasePage = (Page as Computers);
        Computer = computerBasePage.Computer;

        if (Computer == null) Response.Redirect("~/", true);
    } 
}
