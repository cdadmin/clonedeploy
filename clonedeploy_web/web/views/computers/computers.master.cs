using System;
using System.Collections.Generic;
using System.Web.UI;
using BLL;
using Global;
using Models;
using Tasks;

namespace views.masters
{
    public partial class ComputerMaster : BasePages.MasterBaseMaster
    {
        private BasePages.Computers computerBasePage { get; set; }
        private readonly BLL.Image _bllImage = new BLL.Image();
        public Models.Computer Computer { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            computerBasePage = (Page as BasePages.Computers);
            Computer = computerBasePage.Computer;
            if (Computer == null)
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
            DisplayConfirm();
        }

        protected void btnDeploy_Click(object sender, EventArgs e)
        {
            var image = _bllImage.GetImage(Computer.Image);
            Session["direction"] = "push";
            lblTitle.Text = "Deploy " + image.Name + " To " + Computer.Name + " ?";
            DisplayConfirm();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Session["direction"] = "pull";
            lblTitle.Text = "Upload This Computer?";
            DisplayConfirm();
        }

        protected void buttonConfirm_Click(object sender, EventArgs e)
        {
            var direction = (string) (Session["direction"]);
            Session.Remove("direction");
            switch (direction)
            {
                case "delete":
                    if (computerBasePage.BllComputer.DeleteComputer(Computer.Id))
                        Response.Redirect("~/views/computers/search.aspx");
                    break;
                case "push":
                {
                    var image = _bllImage.GetImage(Computer.Image);
                    Session["imageID"] = image.Id;

                    if (_bllImage.Check_Checksum(image))
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
        }

        protected void buttonConfirmChecksum_Click(object sender, EventArgs e)
        {
            var imageId = (string) (Session["imageID"]);
            Response.Redirect("~/views/images/specs.aspx?imageid=" + imageId, false);
            Session.Remove("imageID");
        }
    }
}