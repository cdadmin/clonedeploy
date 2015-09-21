using System;
using System.Collections.Generic;
using System.Web.UI;
using Global;
using Logic;
using Models;
using Tasks;

namespace views.masters
{
    public partial class ComputerMaster : MasterPage
    {
        private readonly ComputerLogic _logic = new ComputerLogic();
        public Computer Computer { get { return ReadComputer(); } }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["hostid"]))
            {
                Level2.Visible = false;
                return;
            }

            Level1.Visible = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Delete This Host?";
            Session["direction"] = "delete";
            Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                true);
        }

        protected void btnDeploy_Click(object sender, EventArgs e)
        {
            Image image = new Image {Id = Computer.Image};
            image.Read();
            Session["direction"] = "push";
            lblTitle.Text = "Deploy " + image.Name + " To " + Computer.Name + " ?";
          
            Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                true);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Session["direction"] = "pull";
            lblTitle.Text = "Upload This Computer?";
            Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                "$(function() {  var menuTop = document.getElementById('confirmbox'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });;",
                true);
        }

        protected void buttonConfirm_Click(object sender, EventArgs e)
        {        
            var direction = (string) (Session["direction"]);
            Session.Remove("direction");
            switch (direction)
            {
                case "delete":
                    _logic.DeleteComputer(Computer.Id);
                    if (Utility.Message.Contains("Successfully"))
                        Response.Redirect("~/views/computers/search.aspx");
                    break;
                case "push":
                {
                    var image = new Image {Id = Computer.Image};
                    image.Read();
                    Session["imageID"] = image.Id;


                    if (image.Check_Checksum())
                    {
                        var unicast = new Unicast {Host = Computer, Direction = direction};
                        unicast.Create();
                    }
                    else
                    {
                        lblIncorrectChecksum.Text =
                            "This Image Has Not Been Confirmed And Cannot Be Deployed.  <br>Confirm It Now?";
                        Page.ClientScript.RegisterStartupScript(GetType(), "modalscript",
                            "$(function() {  var menuTop = document.getElementById('incorrectChecksum'),body = document.body;classie.toggle(menuTop, 'confirm-box-outer-open'); });",
                            true);
                    }
                }
                    break;
                case "pull":
                {
                    var unicast = new Unicast {Host = Computer, Direction = direction};
                    unicast.Create();
                }
                    break;
            }
            Msgbox(Utility.Message);
        }

        protected void buttonConfirmChecksum_Click(object sender, EventArgs e)
        {
            var imageId = (string) (Session["imageID"]);
            Response.Redirect("~/views/images/specs.aspx?imageid=" + imageId, false);
            Session.Remove("imageID");
        }

        public void Msgbox(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            const string msgType = "showSuccessToast";
            Page.ClientScript.RegisterStartupScript(GetType(), "msgBox",
                "$(function() { $().toastmessage('" + msgType + "', " + "\"" + message + "\"); });", true);
            Session.Remove("Message");
        }

        private Computer ReadComputer()
        {
            return _logic.GetComputer(Convert.ToInt32(Request.QueryString["hostid"]));
        }
    }
}