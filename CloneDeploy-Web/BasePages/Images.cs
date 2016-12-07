using System;
using CloneDeploy_Entities;
using CloneDeploy_Web;

namespace BasePages
{
    public class Images : PageBaseMaster
    {
       public ImageEntity Image { get; set; }
        public ImageProfileEntity ImageProfile { get; set; }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Image = !string.IsNullOrEmpty(Request["imageid"]) ? Call.ImageApi.Get(Convert.ToInt32(Request.QueryString["imageid"])) : null;
            ImageProfile = !string.IsNullOrEmpty(Request["profileid"]) ? Call.ImageProfileApi.Get(Convert.ToInt32(Request.QueryString["profileid"])) : null;
            if (Image == null)
                RequiresAuthorization(Authorizations.SearchImage);
            else
                RequiresAuthorizationOrManagedImage(Authorizations.ReadImage, Image.Id);
        }
    }
}