using System.Web;
using System.Web.UI;

namespace Helpers
{
    public class Message
    {
        public static string Text
        {
            get { return (string)HttpContext.Current.Session["Message"]; }
            set { HttpContext.Current.Session["Message"] = value; }
        }
    }
}