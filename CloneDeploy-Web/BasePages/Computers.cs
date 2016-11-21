using System;
using CloneDeploy_Web.APICalls;
using Helpers;
using Models;

namespace BasePages
{
    public class Computers : PageBaseMaster
    {
        public Computer Computer { get; set; }

        protected override void OnInit(EventArgs e)
        {
            
            base.OnInit(e);
            Computer = !string.IsNullOrEmpty(Request.QueryString["computerid"])
                ? new APICall().ComputerApi.Get(Convert.ToInt32(Request.QueryString["computerid"]))
                : null;
            if (Computer == null)
                RequiresAuthorization(Authorizations.SearchComputer);
            else
                RequiresAuthorizationOrManagedComputer(Authorizations.ReadComputer, Computer.Id);
        }
    }
}