using System;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.computers.inventory
{
    public partial class views_computers_inventory_inventory : MasterBaseMaster
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