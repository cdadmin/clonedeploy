using System;

using BasePages;
using Models;

public partial class views_global_munki_munki : MasterBaseMaster
{
    private BasePages.Global globalBasePage { get; set; }
    public MunkiManifestTemplate ManifestTemplate { get; set; }

    public void Page_Load(object sender, EventArgs e)
    {
     
            globalBasePage = (Page as Global);
            ManifestTemplate = globalBasePage.ManifestTemplate;

            if (ManifestTemplate == null)
            {
                divMunkiDetails.Visible = false;
                return;
            }
            else
            {
                divMunki.Visible = false;
            }

            
            
        
    } 
}
