using System;
using CloneDeploy_Web.BasePages;
using CloneDeploy_Web.Helpers;

public partial class views_global_munki_general : Global
{
    protected void buttonUpdateGeneral_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.UpdateGlobal);

        ManifestTemplate.Name = txtManifestName.Text;
        ManifestTemplate.Description = txtManifestDesc.Text;

        var result = Call.MunkiManifestTemplateApi.Put(ManifestTemplate.Id, ManifestTemplate);
        EndUserMessage = result.Success ? "Successfully Updated Manifest Template" : result.ErrorMessage;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtManifestName.Text = ManifestTemplate.Name;
            txtManifestDesc.Text = ManifestTemplate.Description;
        }
    }
}