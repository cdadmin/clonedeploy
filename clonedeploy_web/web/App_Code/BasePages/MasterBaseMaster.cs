using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace BasePages
{
    public class MasterBaseMaster : System.Web.UI.MasterPage
    {
        protected virtual void DisplayIncorrectChecksum()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                       "$(function() {  var menuTop = document.getElementById('incorrectChecksum'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                       true);
        }
        protected virtual void ApproveChecksumRedirect()
        {
            var imageId = (string)(Session["imageID"]);
            Session.Remove("imageID");
            Response.Redirect("~/views/images/specs.aspx?imageid=" + imageId, false);        
        } 

        protected virtual void DisplayConfirm()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
               "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
               true);
        }
    }
}