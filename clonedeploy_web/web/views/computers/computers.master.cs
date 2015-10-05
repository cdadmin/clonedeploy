using System;
using BasePages;
using BLL;
using Computer = Models.Computer;

namespace views.masters
{
    public partial class ComputerMaster : MasterBaseMaster
    {
        private Computers ComputerBasePage { get; set; }
        private readonly Image _bllImage = new Image();
        public Computer Computer { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            ComputerBasePage = (Page as Computers);
            Computer = ComputerBasePage.Computer;
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
                    ComputerBasePage.BllComputer.DeleteComputer(Computer.Id);
                        Response.Redirect("~/views/computers/search.aspx");
                    break;
                case "push":
                {
                    var image = _bllImage.GetImage(Computer.Image);
                    Session["imageID"] = image.Id;

                    if (_bllImage.Check_Checksum(image))
                    {
                        ComputerBasePage.BllComputer.StartUnicast(Computer,direction);                      
                    }
                    else
                    {
                        lblIncorrectChecksum.Text =
                            "This Image Has Not Been Confirmed And Cannot Be Deployed.  <br>Confirm It Now?";
                        DisplayIncorrectChecksum();
                    }
                }
                    break;
                case "pull":
                {
                    ComputerBasePage.BllComputer.StartUnicast(Computer, direction);
                }
                    break;
            }
        }

        protected void buttonConfirmChecksum_Click(object sender, EventArgs e)
        {
            ApproveChecksumRedirect();
        }
    }
}