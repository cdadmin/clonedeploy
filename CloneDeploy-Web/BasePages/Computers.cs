﻿using System;
using CloneDeploy_App.Helpers;
using CloneDeploy_Entities;

namespace BasePages
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
                RequiresAuthorization(Authorizations.SearchComputer);
            else
                RequiresAuthorizationOrManagedComputer(Authorizations.ReadComputer, Computer.Id);
        }
    }
}