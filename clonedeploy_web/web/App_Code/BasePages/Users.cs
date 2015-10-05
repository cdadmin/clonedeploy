using System;
using BLL;
using Models;

namespace BasePages
{
    public class Users : PageBaseMaster
    {
        public WdsUser CloneDeployUser { get; set; }
        public User BllUser { get; set; }

        public Users() 
        {
            BllUser = new User();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CloneDeployUser = !string.IsNullOrEmpty(Request["userid"]) ? BllUser.GetUser(Convert.ToInt32(Request.QueryString["userid"])) : null;
        }
    }
}