using System.Web;
using System.Web.UI;

namespace Helpers
{
    /// <summary>
    /// Summary description for Message
    /// </summary>
    public class Message
    {
        public void Show()
        {
            if (string.IsNullOrEmpty(Message.Text)) return;
            const string msgType = "showSuccessToast";
            var page = HttpContext.Current.CurrentHandler as Page;

            if (page != null)
                page.ClientScript.RegisterStartupScript(GetType(), "msgBox",
                    "$(function() { $().toastmessage('" + msgType + "', " + "\"" + Message.Text + "\"); });", true);
            HttpContext.Current.Session.Remove("Message");
        }
        public static string Text
        {
            get { return (string)HttpContext.Current.Session["Message"]; }
            set { HttpContext.Current.Session["Message"] = value; }
        }
    }
}