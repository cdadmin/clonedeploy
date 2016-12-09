using System;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

public partial class views_computers_bootmenu_bootmenu : MasterBaseMaster
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
