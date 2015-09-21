using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for Message
/// </summary>
public class Message
{
    public void Show(string message)
    {
        if (string.IsNullOrEmpty(message)) return;
        const string msgType = "showSuccessToast";
        var page = HttpContext.Current.CurrentHandler as Page;

        if (page != null)
            page.ClientScript.RegisterStartupScript(GetType(), "msgBox",
                "$(function() { $().toastmessage('" + msgType + "', " + "\"" + message + "\"); });", true);
        HttpContext.Current.Session.Remove("Message");
    }
    public static string Text
    {
        get { return (string)HttpContext.Current.Session["Message"]; }
        set { HttpContext.Current.Session["Message"] = value; }
    }
}