using System;
using CloneDeploy_Entities;

namespace BasePages
{
    public class Users : PageBaseMaster
    {
        public CloneDeployUserEntity CloneDeployUser { get; set; }
        public CloneDeployUserGroupEntity CloneDeployUserGroup { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CloneDeployUser = !string.IsNullOrEmpty(Request["userid"]) ? Call.CloneDeployUserApi.Get(Convert.ToInt32(Request.QueryString["userid"])) : null;
            //Don't Check Authorization here for admin because reset pass for users won't work.
            CloneDeployUserGroup = !string.IsNullOrEmpty(Request["groupid"]) ? Call.UserGroupApi.Get(Convert.ToInt32(Request.QueryString["groupid"])) : null;
        }

    }
}