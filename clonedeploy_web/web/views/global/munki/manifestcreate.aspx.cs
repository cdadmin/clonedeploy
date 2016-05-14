using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;
using Models;

public partial class views_global_munki_manifestcreate : BasePages.Global
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        RequiresAuthorization(Authorizations.CreateGlobal);
        var manifestTemplate = new MunkiManifestTemplate()
        {
            Name = txtManifestName.Text,
            Description = txtManifestDesc.Text,
            TemplateAsManifest = chkBoxManifest.Checked ? 1 : 0,
        };

        var result = BLL.MunkiManifestTemplate.AddManifest(manifestTemplate);
        if (result.IsValid)
        {
            EndUserMessage = "Successfully Created Manifest Template";
            Response.Redirect("~/views/global/munki/general.aspx?cat=sub2&manifestid=" + manifestTemplate.Id);
        }
        else
        {
            EndUserMessage = result.Message;
        }
    }
}