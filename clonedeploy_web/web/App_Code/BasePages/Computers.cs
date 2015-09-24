using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using BLL;
using Models;

namespace BasePages
{
    public class Computers : PageBaseMaster
    {
        public Models.Computer Computer { get; set; }
        public BLL.Computer BllComputer { get; set; }

        public Computers() 
        {
            BllComputer = new BLL.Computer();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Computer = !string.IsNullOrEmpty(Request["hostid"]) ? BllComputer.GetComputer(Convert.ToInt32(Request.QueryString["hostid"])) : null;
        }
    }
}