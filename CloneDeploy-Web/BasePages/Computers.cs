using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;

namespace CloneDeploy_Web.BasePages
{
    public class Computers : PageBaseMaster
    {
        public ComputerEntity Computer { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Computer = !string.IsNullOrEmpty(Request.QueryString["computerid"])
                ? Call.ComputerApi.Get(Convert.ToInt32(Request.QueryString["computerid"]))
                : null;
            if (Computer == null)
                RequiresAuthorization(AuthorizationStrings.SearchComputer);
            else
                RequiresAuthorizationOrManagedComputer(AuthorizationStrings.ReadComputer, Computer.Id);
        }
    }
}