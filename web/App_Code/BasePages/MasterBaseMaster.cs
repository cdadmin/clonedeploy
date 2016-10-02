using System.Web.UI;

namespace BasePages
{
    public class MasterBaseMaster : MasterPage
    {
        protected virtual void DisplayConfirm()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
               "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
               true);
        }

       
    }
}