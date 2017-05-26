using System;
using CloneDeploy_Entities;
using CloneDeploy_Web.BasePages;

namespace CloneDeploy_Web.views.global.munki
{
    public partial class views_global_munki_munki : MasterBaseMaster
    {
        private Global globalBasePage { get; set; }
        public MunkiManifestTemplateEntity ManifestTemplate { get; set; }

        public void Page_Load(object sender, EventArgs e)
        {
            globalBasePage = Page as Global;
            ManifestTemplate = globalBasePage.ManifestTemplate;

            if (ManifestTemplate == null)
            {
                divMunkiDetails.Visible = false;
            }
            else
            {
                divMunki.Visible = false;
            }
        }
    }
}