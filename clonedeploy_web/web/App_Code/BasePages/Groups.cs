using System;
using Helpers;
using Models;

namespace BasePages
{
    public class Groups : PageBaseMaster
    {
        public Group Group { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Group = !string.IsNullOrEmpty(Request["groupid"]) ? BLL.Group.GetGroup(Convert.ToInt32(Request.QueryString["groupid"])) : null;
            RequiresAuthorization(Authorizations.ReadGroup);
        }
    }
}