using System;
using CloneDeploy_Web.Helpers;

namespace CloneDeploy_Web.BasePages
{
    public class Admin : PageBaseMaster
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RequiresAuthorization(Authorizations.ReadAdmin);
        }
    }
}