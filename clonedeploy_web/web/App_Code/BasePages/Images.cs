using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasePages
{
    public class Images : PageBaseMaster
    {
       public Models.Image Image { get; set; }
        public BLL.Image BllImage { get; set; }
        public Models.LinuxProfile ImageProfile { get; set; }
        public BLL.LinuxProfile BllLinuxProfile { get; set; }

        public Images() 
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