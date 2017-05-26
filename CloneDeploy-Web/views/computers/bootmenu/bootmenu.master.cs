using System;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.computers.bootmenu
{
    public partial class views_computers_bootmenu_bootmenu : MasterBaseMaster
    {
        public ComputerEntity Computer { get; set; }
        private Computers computerBasePage { get; set; }

        public void Page_Load(object sender, EventArgs e)
        {
            computerBasePage = Page as Computers;
            Computer = computerBasePage.Computer;

            if (Computer == null) Response.Redirect("~/", true);
        }
    }
}