using System;
using CloneDeploy_Entities;
using CloneDeploy_Web;

namespace BasePages
{
    public class Groups : PageBaseMaster
    {
        public GroupEntity Group { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Group = !string.IsNullOrEmpty(Request["groupid"]) ? Call.GroupApi.Get(Convert.ToInt32(Request.QueryString["groupid"])) : null;
            if (Group == null)
                RequiresAuthorization(Authorizations.SearchGroup);
            else
                RequiresAuthorizationOrManagedGroup(Authorizations.ReadGroup, Group.Id);
        }
    }
}