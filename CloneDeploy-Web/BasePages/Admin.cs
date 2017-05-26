using System;
using CloneDeploy_Common;

namespace CloneDeploy_Web.BasePages
{
    public class Admin : PageBaseMaster
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RequiresAuthorization(AuthorizationStrings.ReadAdmin);
        }
    }
}