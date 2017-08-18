using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;

namespace CloneDeploy_Web.BasePages
{
    public class Admin : PageBaseMaster
    {
        public BootEntryEntity BootEntry { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RequiresAuthorization(AuthorizationStrings.ReadAdmin);
            BootEntry = !string.IsNullOrEmpty(Request["entryid"])
               ? Call.BootEntryApi.Get(Convert.ToInt32(Request.QueryString["entryid"]))
               : null;
        }
    }
}