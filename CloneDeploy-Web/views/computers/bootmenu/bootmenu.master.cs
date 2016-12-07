using System;
using BasePages;
using CloneDeploy_Entities;

public partial class views_computers_bootmenu_bootmenu : BasePages.MasterBaseMaster
{
    private Computers computerBasePage { get; set; }
    public ComputerEntity Computer { get; set; }

    public void Page_Load(object sender, EventArgs e)
    {
        computerBasePage = (Page as Computers);
        Computer = computerBasePage.Computer;

        if (Computer == null) Response.Redirect("~/", true);
    } 
}
