using System;
using BLL;
using Helpers;
using Models;

namespace BasePages
{
    public class Users : PageBaseMaster
    {
        public CloneDeployUser CloneDeployUser { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CloneDeployUser = !string.IsNullOrEmpty(Request["userid"]) ? BLL.User.GetUser(Convert.ToInt32(Request.QueryString["userid"])) : null;
            //Don't Check Authorization here for admin because reset pass for users won't work.
        }

    }
}