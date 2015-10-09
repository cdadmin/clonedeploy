using System;
using Models;

namespace BasePages
{
    public class Computers : PageBaseMaster
    {
        public Computer Computer { get; set; }
 
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Computer = !string.IsNullOrEmpty(Request.QueryString["hostid"]) ? BLL.Computer.GetComputer(Convert.ToInt32(Request.QueryString["hostid"])) : null;
        }
    }
}