using System;
using BasePages;

public partial class views_computers_bootmenu_bootmenu : BasePages.MasterBaseMaster
{
    private Computers computerBasePage { get; set; }
    public Computer Computer { get; set; }

    public void Page_Load(object sender, EventArgs e)
    {
        computerBasePage = (Page as Computers);
        Computer = computerBasePage.Computer;

        if (Computer == null) Response.Redirect("~/", true);
    } 
}
