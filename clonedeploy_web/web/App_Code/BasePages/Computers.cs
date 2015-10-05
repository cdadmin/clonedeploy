using System;
using Models;

namespace BasePages
{
    public class Computers : PageBaseMaster
    {
        public Computer Computer { get; set; }
        public BLL.Computer BllComputer { get; set; }

        public Computers() 
        {
            BllComputer = new BLL.Computer();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Computer = !string.IsNullOrEmpty(Request.QueryString["hostid"]) ? BllComputer.GetComputer(Convert.ToInt32(Request.QueryString["hostid"])) : null;
        }
    }
}