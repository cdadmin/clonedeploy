using System;
using CloneDeploy_Common;
using CloneDeploy_Entities;

namespace CloneDeploy_Web.BasePages
{
    public class Images : PageBaseMaster
    {
        public ImageEntity Image { get; set; }
        public ImageProfileWithImage ImageProfile { get; set; }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Image = !string.IsNullOrEmpty(Request["imageid"])
                ? Call.ImageApi.Get(Convert.ToInt32(Request.QueryString["imageid"]))
                : null;
            ImageProfile = !string.IsNullOrEmpty(Request["profileid"])
                ? Call.ImageProfileApi.Get(Convert.ToInt32(Request.QueryString["profileid"]))
                : null;
            if (Image == null)
                RequiresAuthorization(AuthorizationStrings.SearchImage);
            else
                RequiresAuthorizationOrManagedImage(AuthorizationStrings.ReadImage, Image.Id);
        }
    }
}