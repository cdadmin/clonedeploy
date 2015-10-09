using System;
using BLL;
using Models;

namespace BasePages
{
    public class Users : PageBaseMaster
    {
        public WdsUser CloneDeployUser { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CloneDeployUser = !string.IsNullOrEmpty(Request["userid"]) ? BLL.User.GetUser(Convert.ToInt32(Request.QueryString["userid"])) : null;
        }
    }
}