using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasePages
{
    public class Users : PageBaseMaster
    {
        public Models.WdsUser CloneDeployUser { get; set; }
        public BLL.User BllUser { get; set; }

        public Users() 
        {
            BllUser = new BLL.User();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CloneDeployUser = !string.IsNullOrEmpty(Request["userid"]) ? BllUser.GetUser(Convert.ToInt32(Request.QueryString["userid"])) : null;
        }
    }
}