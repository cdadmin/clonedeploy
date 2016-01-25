using System;
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
                ? BLL.Computer.GetComputer(Convert.ToInt32(Request.QueryString["computerid"]))
                : null;
            if (Computer == null)
                RequiresAuthorization(Authorizations.ReadComputer);
            else
                RequiresAuthorizationOrManagedComputer(Authorizations.ReadComputer, Computer.Id);
        }
    }
}