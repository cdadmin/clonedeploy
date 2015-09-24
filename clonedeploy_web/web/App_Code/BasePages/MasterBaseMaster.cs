using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace BasePages
{
    public class MasterBaseMaster : System.Web.UI.MasterPage
    {

      
        protected virtual void DisplayConfirm()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
               "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
               true);
        }
    }
}