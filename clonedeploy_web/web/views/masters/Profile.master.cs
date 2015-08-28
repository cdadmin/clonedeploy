using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;
using Image = Models.Image;

public partial class views_masters_Profile : System.Web.UI.MasterPage
{
    public LinuxEnvironmentProfile LinuxEnvironmentProfile { get; set; }
    public Image Image { get; set; }
    public void Page_Init(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["profileid"])) return;
        if (string.IsNullOrEmpty(Request["imageid"])) Response.Redirect("~/", true);

        Image = new Image {Id = Convert.ToInt32(Request.QueryString["imageid"])};
        LinuxEnvironmentProfile = new LinuxEnvironmentProfile { Id = Convert.ToInt32(Request.QueryString["profileid"]) };
        LinuxEnvironmentProfile.Read();
    }
    public void Msgbox(string message)
    {
        if (string.IsNullOrEmpty(message)) return;
        const string msgType = "showSuccessToast";
        Page.ClientScript.RegisterStartupScript(GetType(), "msgBox",
            "$(function() { $().toastmessage('" + msgType + "', " + "\"" + message + "\"); });", true);
        Session.Remove("Message");
    } 
}
