using System;
using Models;

namespace BasePages
{
    public class Profiles : PageBaseMaster
    {
        public Image Image { get; set; }
        public BLL.Image BllImage { get; set; }
        public LinuxProfile ImageProfile { get; set; }
        public BLL.LinuxProfile BllLinuxProfile { get; set; }

        public Profiles() 
        {
            BllImage = new BLL.Image();
            BllLinuxProfile = new BLL.LinuxProfile();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Image = !string.IsNullOrEmpty(Request["imageid"]) ? BllImage.GetImage(Convert.ToInt32(Request.QueryString["imageid"])) : null;
            ImageProfile = !string.IsNullOrEmpty(Request["profileid"]) ? BllLinuxProfile.ReadProfile(Convert.ToInt32(Request.QueryString["profileid"])) : null;
        }
    }
}