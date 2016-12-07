using System;
using CloneDeploy_Web;

namespace BasePages
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