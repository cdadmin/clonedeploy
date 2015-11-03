using System;
using Models;
using Helpers;

namespace BasePages
{
    public class Images : PageBaseMaster
    {
       public Image Image { get; set; }
        public ImageProfile ImageProfile { get; set; }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Image = !string.IsNullOrEmpty(Request["imageid"]) ? BLL.Image.GetImage(Convert.ToInt32(Request.QueryString["imageid"])) : null;
            ImageProfile = !string.IsNullOrEmpty(Request["profileid"]) ? BLL.LinuxProfile.ReadProfile(Convert.ToInt32(Request.QueryString["profileid"])) : null;
            if (Image == null)
                RequiresAuthorization(Authorizations.ReadImage);
            else
                RequiresAuthorizationOrManagedImage(Authorizations.ReadImage, Image.Id);
        }
    }
}