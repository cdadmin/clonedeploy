using System;
using Helpers;

public partial class views_global_munki_general : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtManifestName.Text = ManifestTemplate.Name;
            txtManifestDesc.Text = ManifestTemplate.Description;
            chkBoxManifest.Checked = Convert.ToBoolean(ManifestTemplate.TemplateAsManifest);
        }

    }

    protected void buttonUpdateGeneral_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);
        
        ManifestTemplate.Name = txtManifestName.Text;
        ManifestTemplate.Description = txtManifestDesc.Text;
        ManifestTemplate.TemplateAsManifest = chkBoxManifest.Checked ? 1 : 0;
        var result = BLL.MunkiManifestTemplate.UpdateManifest(ManifestTemplate);
        EndUserMessage = result.IsValid ? "Successfully Updated Manifest Template" : result.Message;
    }

}