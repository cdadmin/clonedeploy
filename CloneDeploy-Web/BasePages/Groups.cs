using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;

namespace CloneDeploy_Web.BasePages
{
    public class Groups : PageBaseMaster
    {
        public GroupEntity Group { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Group = !string.IsNullOrEmpty(Request["groupid"])
                ? Call.GroupApi.Get(Convert.ToInt32(Request.QueryString["groupid"]))
                : null;
            if (Group == null)
                RequiresAuthorization(AuthorizationStrings.SearchGroup);
            else
                RequiresAuthorizationOrManagedGroup(AuthorizationStrings.ReadGroup, Group.Id);
        }
    }
}